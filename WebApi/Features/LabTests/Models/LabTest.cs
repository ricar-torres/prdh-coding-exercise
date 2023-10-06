namespace WebApi.Features.LabTests.Models;
public class LabTest {
	public Guid OrderTestId { get; set; }
	public Guid PatientId { get; set; }
	public string OrderTestType { get; set; }
	public DateTime SampleCollectedDate { get; set; }
	public string OrderTestResult { get; set; }
}

