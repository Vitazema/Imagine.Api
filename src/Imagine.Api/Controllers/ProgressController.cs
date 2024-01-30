using System.Text;
using System.Text.Json;
using Imagine.Api.Constants;
using Imagine.Api.Services;
using Imagine.Core.Contracts;
using Imagine.Core.Contracts.Errors;
using Imagine.Core.Interfaces;
using Imagine.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Imagine.Api.Controllers;

public class ProgressController : BaseApiController
{
    private readonly ITaskProgressService _taskProgressService;
    private readonly IAiService _aiService;
    private readonly IWorkerPool _workerPool;
    private static readonly HttpClient HttpClient = new();

    public ProgressController(ITaskProgressService taskProgressService,
        IAiService aiService, IWorkerPool workerPool)
    {
        _taskProgressService = taskProgressService;
        _aiService = aiService;
        _workerPool = workerPool;
    }

    [HttpGet("{artId:guid}")]
    [Permission(ActionConstants.ReadTask)]
    public async Task<ActionResult<AiTask>> GetTaskProgress(Guid artId)
    {
        var aiTask = _taskProgressService.GetTask(artId);

        if (aiTask == null)
        {
            return NotFound(new ApiResponse(404, $"Task {artId} not found"));
        }
        if (aiTask.Status == AiTaskStatus.Completed)
        {
            return Ok(aiTask);
        }

        var progressRequestJson = JsonSerializer.Serialize(new
        {
            id_task = artId.ToString(),
            id_live_preview = -1
        });

        var sdProgressRequestMessage =
            new HttpRequestMessage(HttpMethod.Post,
                $"{_workerPool.GetWorkerById(aiTask.WorkerId).Address}/internal/progress")
            {
                Content = new StringContent(progressRequestJson, Encoding.UTF8, "application/json")
            };
        var response = await HttpClient.SendAsync(sdProgressRequestMessage);

        if (!response.IsSuccessStatusCode)
        {
            return NotFound(new ApiResponse(404, $"Task {artId} not found in :{aiTask.WorkerId} worker"));
        }
        try
        {
            await using var contentStream = await response.Content.ReadAsStreamAsync();
            var sdProgressResponse = await JsonSerializer.DeserializeAsync<SdTaskProgressDto>(contentStream);
            if (sdProgressResponse == null)
            {
                return NotFound(new ApiResponse(404, $"Task {artId} not found in :{aiTask.WorkerId} worker"));
            }

            aiTask.Progress = sdProgressResponse.Progress;
            aiTask.RelativeEstimation = sdProgressResponse.Eta;

            return Ok(aiTask);
        }
        catch (Exception e)
        {
            return BadRequest(new ApiResponse(400, e.Message));
        }

    }

    [HttpPost("callback")]
    public async Task SdApiCallback([FromForm] SdQueueApiCallback callback)
    {
        await _aiService.StoreArtAsync(callback);
    }
}
