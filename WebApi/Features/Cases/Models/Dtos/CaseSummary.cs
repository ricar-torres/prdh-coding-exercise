namespace WebApi.Features.Cases.Dtos;
public class CaseSummaryDto {
	public DateTime SampleCollectedDate { get; set; }
	public int QuantityOfCases { get; set; }
	public Dictionary<string, int> QuantityByTestType { get; set; }
}
