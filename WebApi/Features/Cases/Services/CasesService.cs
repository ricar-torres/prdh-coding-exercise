using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WebApi.Features.Cases.Dtos;
using WebApi.Features.Cases.Models;
using WebApi.Features.Cases.Repos;
using WebApi.Features.LabTests.Models.Dtos;
using WebApi.Shared.Services;

namespace WebApi.Features.Cases.Services;
public interface ICasesService : IServiceBase<Case> {
	Task CreateCasesFromTestResults(IList<PatientLabTestSummary> testResults);
	Task<IList<CaseSummaryDto>> GetCasesSummaryList(CasesListFilter filter = null);
}

public class CasesService : ServiceBase<Case>, ICasesService {
	readonly ICasesRepository _repo;
	public CasesService(ICasesRepository repo) : base(repo) {
		_repo = repo;
	}
	public async Task CreateCasesFromTestResults(IList<PatientLabTestSummary> testResults) {
		IList<Case> cases = new List<Case>();
		foreach (var testResult in testResults) {
			var newCase = new Case {
				PatientId = testResult.PatientId,
				EarliestPositiveOrderTestSampleCollectedDate = testResult.EarliestPositiveOrderTestSampleCollectedDate,
				EarliestPositiveOrderTestType = testResult.OrderTestType,
				OrderTestCount = testResult.OrderTestCount
			};
			cases.Add(newCase);
		}
		await _repo.AddRangeAsync(cases);
	}

	public async Task<IList<CaseSummaryDto>> GetCasesSummaryList(CasesListFilter filter = null) {
		IQueryable<Case> query = _repo.DbSet;

		if (filter?.StartDate != null)
			query = query.Where(c => c.EarliestPositiveOrderTestSampleCollectedDate >= filter.StartDate.Value.Date);

		if (filter?.EndDate != null) {
			query = query.Where(c => c.EarliestPositiveOrderTestSampleCollectedDate.Date <= filter.EndDate.Value.Date);
		}

		var groupedData = await query
						.GroupBy(c => new { Date = c.EarliestPositiveOrderTestSampleCollectedDate.Date, c.EarliestPositiveOrderTestType })
						.Select(g => new {
							Date = g.Key.Date,
							g.Key.EarliestPositiveOrderTestType,
							Count = g.Count()
						})
						.OrderByDescending(x => x.Date)
						.ToListAsync();

		var qry = groupedData
						.GroupBy(g => g.Date)
						.Select(g => new CaseSummaryDto {
							SampleCollectedDate = g.Key,
							QuantityOfCases = g.Sum(x => x.Count),
							QuantityByTestType = g.ToDictionary(x => x.EarliestPositiveOrderTestType, x => x.Count)
						}).ToList();

		if (!string.IsNullOrEmpty(filter?.Sort)) {
			var propInfo = typeof(CaseSummaryDto).GetProperty(filter.Sort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
			if (propInfo != null) {
				if (filter.Order == "asc") {
					qry = qry.OrderBy(x => propInfo.GetValue(x)).ToList();
				}
				else if (filter.Order == "desc") {
					qry = qry.OrderByDescending(x => propInfo.GetValue(x)).ToList();
				}
			}
		}
		return qry;
	}


}
