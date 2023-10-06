using Microsoft.EntityFrameworkCore;
using WebApi.Shared;
using WebApi.Features.Cases.Repos;
using WebApi.Features.Cases.Services;
using WebApi.Features.Cases.Models;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Dtos;
using WebApi.Features.Cases.Dtos;

namespace WebApi.Features.Cases.Definition;
public sealed class CasesDefinition : BaseFeatureDefinition {
	readonly ApiBaseUrl baseUrl = new("Cases");

	public override void DefineModels(ModelBuilder modelBuilder) {
		modelBuilder.ApplyConfiguration(new CaseEntityConfiguration());
	}

	public override void DefineServices(IServiceCollection services) {
		services.AddScoped<ICasesRepository, CasesRepository>();
		services.AddScoped<ICasesService, CasesService>();
	}

	public override void DefineEndpoints(IEndpointRouteBuilder endpoints) {
		endpoints.MapGet(baseUrl + "list", GetList);
	}


	#region "ENDPOINTS METHODS"

	private async Task<IResult> GetList(ICasesService service, HttpContext httpContext,
	[FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] string sort,
	[FromQuery] string order, [FromQuery] int? pageNumber, [FromQuery] int? pageSize) {
		const int defaultPageSize = 10;
		const int maxPageSize = 100;

		if (pageNumber <= 0 || pageSize <= 0 || pageSize > maxPageSize) {
			return Results.BadRequest("Invalid pagination parameters.");
		}
		var result = await service.GetCasesSummaryList(new CasesListFilter {
			StartDate = startDate.HasValue ? startDate.Value.Date : startDate,
			EndDate = endDate.HasValue ? endDate.Value.Date : endDate,
			Sort = sort,
			Order = order
		});

		if (result == null) return Results.NotFound("No cases found.");

		var paginatedList = PaginatedList<CaseSummaryDto>.Create(result, pageNumber ?? 1, pageSize ?? defaultPageSize);

		return Results.Ok(new {
			paginatedList.HasNextPage,
			paginatedList.HasPreviousPage,
			paginatedList.PageIndex,
			paginatedList.TotalPages,
			paginatedList.Count,
			paginatedList.Capacity,
			data = paginatedList
		});
	}

	#endregion
}
