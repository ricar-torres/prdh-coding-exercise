using WebApi.Features.Cases.Models;
using WebApi.Infrastructure;

namespace WebApi.Features.Cases.Repos;
public interface ICasesRepository : IRepositoryBase<Case> {
}

public class CasesRepository : RepositoryBase<Case>, ICasesRepository {
	public CasesRepository(DataContext context) : base(context) {
	}
}
