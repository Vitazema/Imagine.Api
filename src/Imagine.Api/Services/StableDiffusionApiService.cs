using System.Text;
using System.Text.Json;
using Imagine.Core.Configurations;
using Imagine.Core.Contracts;
using Imagine.Core.Entities;
using Microsoft.Extensions.Options;

namespace Imagine.Api.Services;

public class StableDiffusionApiService : IAiApiService
{
    private readonly ILogger<StableDiffusionApiService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptions<WorkersSettings> _workerSettings;
    private readonly ITaskProgressService _taskProgressService;

    public StableDiffusionApiService(ILogger<StableDiffusionApiService> logger, IHttpClientFactory httpClientFactory,
        IOptions<WorkersSettings> workerSettings, ITaskProgressService taskProgressService)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _workerSettings = workerSettings;
        _taskProgressService = taskProgressService;
    }

    public async Task<SdResponse> RequestAsync(Art art, CancellationToken cancellationToken)
    {
        // todo: inject into art
        var aiType = "txt2img";
        // todo: make multiple requests to different workers
        var workerSetting = _workerSettings.Value.StableDiffusionWorkers.FirstOrDefault();

        var sdRequestMessage =
            new HttpRequestMessage(HttpMethod.Post, $"http://{workerSetting.Address}/sdapi/v1/{aiType}")
            {
                Content = new StringContent(JsonSerializer.Serialize(new
                {
                    prompt = "Hello, world!",
                    steps = 50,
                    sampler_index = "DDIM"
                }), Encoding.UTF8, "application/json")
            };
        
        _taskProgressService.UpdateProgress(art.TaskId, new AiTaskDto()
        {
            TaskId = art.TaskId,
            Progress = 0,
            Status = AiTaskStatus.Running,
            WorkerId = workerSetting.Id
        });
        
        var httpClient = _httpClientFactory.CreateClient();

        var response = await httpClient.SendAsync(sdRequestMessage, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            
        var sdResponse = await JsonSerializer.DeserializeAsync<SdResponse>(contentStream, cancellationToken: cancellationToken);
        
        _taskProgressService.UpdateProgress(art.TaskId, new AiTaskDto()
        {
            TaskId = art.TaskId,
            Progress = 100,
            Status = AiTaskStatus.Completed,
            WorkerId = workerSetting.Id
        });
        
        return sdResponse;
    }
}