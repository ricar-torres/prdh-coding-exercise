using Microsoft.Extensions.Options;
using WebApi.Features.LabTests.Models;
using WebApi.Features.LabTests.Models.Dtos;
using WebApi.Models;

namespace WebApi.Features.LabTests.Services;
public interface ILabTestsService {
	Uri BuildRequestUri();
	IList<PatientLabTestSummary> GetEarliestPositiveTestsByPatient(IList<LabTest> labTests);
}

public class LabTestsService : ILabTestsService {
	private const string PositiveTestResult = "Positive";
	private const string  SampleCollectedStartDate = "SampleCollectedStartDate";
	private const string  SampleCollectedEndDate = "SampleCollectedEndDate";
	readonly AppSettings _appSettings;

	public LabTestsService(IOptions<AppSettings> options) {
		_appSettings = options.Value;
	}


	public Uri BuildRequestUri() {
		var uriBuilder = new UriBuilder(_appSettings.StatisticsUrl ?? throw new ArgumentNullException(nameof(_appSettings.StatisticsUrl))) {
			Path = _appSettings.OrderTestCovid19MinimalEndpoint
		};

		DateTimeOffset createdAtStartDate = _appSettings.StartDate ?? DateTimeOffset.Now;
		DateTimeOffset createdAtEndDate = _appSettings.EndDate ?? createdAtStartDate.AddMonths(6);

		var queryParams = System.Web.HttpUtility.ParseQueryString(string.Empty);
		queryParams[SampleCollectedStartDate] = createdAtStartDate.UtcDateTime.ToString("o");
		queryParams[SampleCollectedEndDate] = createdAtEndDate.UtcDateTime.ToString("o");

		uriBuilder.Query = queryParams.ToString();

		return uriBuilder.Uri;
	}

	public IList<PatientLabTestSummary> GetEarliestPositiveTestsByPatient(IList<LabTest> labTests) {
		var positiveTestsGroupedByPatient = labTests
				.Where(test => test.OrderTestResult == PositiveTestResult)
				.GroupBy(test => test.PatientId)
				.Select(patientGroup => {
					var earliestPositiveDate = patientGroup.Min(test => test.SampleCollectedDate);
					var earliestPositiveTest = patientGroup.First(test => test.SampleCollectedDate == earliestPositiveDate);
					return new PatientLabTestSummary {
						PatientId = patientGroup.Key,
						EarliestPositiveOrderTestSampleCollectedDate = earliestPositiveDate,
						OrderTestType = earliestPositiveTest.OrderTestType,
						OrderTestCount = patientGroup.Count()
					};
				})
				.OrderBy(x => x.EarliestPositiveOrderTestSampleCollectedDate)
				.ToList();
		return positiveTestsGroupedByPatient;
	}

}
