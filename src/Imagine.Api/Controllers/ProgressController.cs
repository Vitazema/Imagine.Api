using System.Text.Json;
using Imagine.Api.Constants;
using Imagine.Api.Errors;
using Imagine.Api.Services;
using Imagine.Core.Configurations;
using Imagine.Core.Contracts;
using Imagine.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Imagine.Api.Controllers;

public class ProgressController : BaseApiController
{
    private readonly ITaskProgressService _taskProgressService;
    private readonly IOptions<WorkersSettings> _workerSettings;
    private static readonly HttpClient HttpClient = new();

    public ProgressController(ITaskProgressService taskProgressService, IOptions<WorkersSettings> workerSettings)
    {
        _taskProgressService = taskProgressService;
        _workerSettings = workerSettings;
    }
        
    [HttpGet("{taskId:guid}")]
    [Permission(ActionConstants.ReadTask)]
    public async Task<ActionResult<AiTaskDto>> GetTaskProgress(Guid taskId)
    {
        var aiTask = _taskProgressService.GetProgress(taskId);
        
        if (aiTask == null)
        {
            return NotFound(new ApiResponse(404, $"Task {taskId} not found"));
        }
        
        var workerAddress = _workerSettings.Value.StableDiffusionWorkers.FirstOrDefault(x => x.Id == aiTask.WorkerId)?.Address;
        if (workerAddress == null)
        {
            return NotFound(new ApiResponse(404, $"Task {taskId} didn't registered on :{aiTask.WorkerId} worker"));
        }
        
        var sdProgressRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"http://{workerAddress}/sdapi/v1/progress");
        var response = await HttpClient.SendAsync(sdProgressRequestMessage);
        
        if (!response.IsSuccessStatusCode)
        {
            return NotFound(new ApiResponse(404, $"Task {taskId} not found in :{aiTask.WorkerId} worker"));
        }
        
        await using var contentStream = await response.Content.ReadAsStreamAsync();
        var sdProgressResponse = await JsonSerializer.DeserializeAsync<SdProgressResponse>(contentStream);
        
        aiTask.Progress = sdProgressResponse.Progress;
        aiTask.RelativeEstimation = sdProgressResponse.EtaRelative;

        return Ok(aiTask);
    }
}
