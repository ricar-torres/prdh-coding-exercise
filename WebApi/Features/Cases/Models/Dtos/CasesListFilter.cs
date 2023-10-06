namespace WebApi.Features.Cases.Dtos;
public class CasesListFilter {
	public DateTime? StartDate { get; set; }
	public DateTime? EndDate { get; set; }
	public string Order { get; set; }
	public string Sort { get; set; }
}
