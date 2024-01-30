using System.Text;
using System.Text.Json;
using Imagine.Core.Contracts;
using Imagine.Core.Entities;
using Imagine.Core.Interfaces;

namespace Imagine.Api.Services;

public class StableDiffusionQueueApiService : IAiApiService
{
    private readonly ILogger<StableDiffusionQueueApiService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IWorkerPool _workerPool;
    private readonly ITaskProgressService _taskProgressService;
    private readonly IRepository<Art> _artsRepository;

    public StableDiffusionQueueApiService(ILogger<StableDiffusionQueueApiService> logger,
        IHttpClientFactory httpClientFactory,
        IWorkerPool workerPool, ITaskProgressService taskProgressService,
        IRepository<Art> artsRepository)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _workerPool = workerPool;
        _taskProgressService = taskProgressService;
        _artsRepository = artsRepository;
    }

    public async Task<string> EnqueueSdTaskAsync(Art art, CancellationToken cancellationToken)
    {
        var sdRequestMessage =
            new HttpRequestMessage(HttpMethod.Post,
                $"{_workerPool.GetWorkerById(art.WorkerId).Address}/agent-scheduler/v1/queue/{art.Type.ToString().ToLower()}")
            {
                Content = new StringContent(art.ArtSetting, Encoding.UTF8, "application/json")
            };

        var httpClient = _httpClientFactory.CreateClient();

        var response = await httpClient.SendAsync(sdRequestMessage, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

        var sdResponse =
            await JsonSerializer.DeserializeAsync<SdQueueResponse>(contentStream, cancellationToken: cancellationToken);

        return sdResponse.TaskId;
    }

    public async Task<SdQueueTaskResult> GetSdTaskResultAsync(Art art)
    {
        var sdProgressRequestMessage =
            new HttpRequestMessage(HttpMethod.Get,
                $"{_workerPool.GetWorkerById(art.WorkerId).Address}/agent-scheduler/v1/results/{art.Id}");
        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.SendAsync(sdProgressRequestMessage);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        await using var contentStream = await response.Content.ReadAsStreamAsync();
        var sdQueueTaskResult = await JsonSerializer.DeserializeAsync<SdQueueTaskResult>(contentStream);

        return sdQueueTaskResult;
    }
}
