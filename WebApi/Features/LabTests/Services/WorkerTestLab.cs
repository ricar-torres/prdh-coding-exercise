using System.Text.Json;
using Microsoft.Extensions.Options;
using WebApi.Features.Cases.Services;
using WebApi.Features.LabTests.Models;
using WebApi.Features.LabTests.Models.Dtos;
using WebApi.Features.LabTests.Services;
using WebApi.Models;

namespace WebApi.Features.LabTests;
public class WorkerTestLab : BackgroundService {
	readonly ILogger<WorkerTestLab> _logger;
	readonly HttpClient _client;
	readonly ILabTestsService _labTestService;
	readonly IServiceScopeFactory _scopeFactory;
	readonly AppSettings _appSettings;

	public WorkerTestLab(ILogger<WorkerTestLab> logger, IHttpClientFactory httpClientFactory,
	ILabTestsService labTestService, IServiceScopeFactory scopeFactory, IOptions<AppSettings> options) {
		_logger = logger;
		_client = httpClientFactory.CreateClient();
		_labTestService = labTestService;
		_scopeFactory = scopeFactory;
		_appSettings = options.Value;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
		//Declaration
		Uri requestUri;
		HttpResponseMessage response;
		Stream? contentStream;
		IList<LabTest>? result;
		IList<PatientLabTestSummary>? patientTestResults;

		//Initialization
		requestUri = _labTestService.BuildRequestUri();

		while (!stoppingToken.IsCancellationRequested) {
			_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
			try {
				response = await _client.GetAsync(requestUri, stoppingToken);
				if (!response.IsSuccessStatusCode) {
					_logger.LogError($"Failed to retrieve data. Status Code: {response.StatusCode}");
					continue;
				}

				contentStream = await response.Content.ReadAsStreamAsync(stoppingToken);
				if (contentStream == null) {
					_logger.LogError("Failed to read content stream.");
					continue;
				}

				result = await JsonSerializer.DeserializeAsync<IList<LabTest>>(contentStream,
					new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, stoppingToken);
				if (result == null) {
					_logger.LogError("Failed to deserialize content stream.");
					continue;
				}
				else if (!result.Any()) {
					_logger.LogError("No data retrieved.");
					continue;
				}

				patientTestResults = _labTestService.GetEarliestPositiveTestsByPatient(result);
				if (patientTestResults == null || !patientTestResults.Any()) {
					continue;
				}

				using (var scope = _scopeFactory.CreateScope()) {
					var casesService = scope.ServiceProvider.GetRequiredService<ICasesService>();
					await casesService.CreateCasesFromTestResults(patientTestResults);
				}
			}
			catch (Exception ex) {
				_logger.LogError(ex, "An error occurred while fetching data.");
			}

			await Task.Delay(_appSettings.TimeSpan, stoppingToken);
		}
	}
}
