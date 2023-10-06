namespace WebApi.Features.LabTests.Models.Dtos;
public class PatientLabTestSummary {
	public Guid PatientId { get; set; }
	public DateTime EarliestPositiveOrderTestSampleCollectedDate { get; set; }
	public string OrderTestType { get; set; }
	public int OrderTestCount { get; set; }
}
