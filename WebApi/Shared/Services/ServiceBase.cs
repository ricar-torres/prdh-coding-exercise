using System.Linq.Expressions;
using WebApi.Infrastructure;

namespace WebApi.Shared.Services;

public class ServiceBase<TEntity> : IServiceBase<TEntity> where TEntity : class {
	protected readonly IRepositoryBase<TEntity> _baseRepo;

	public ServiceBase(IRepositoryBase<TEntity> repo) {
		_baseRepo = repo;
	}

	public virtual IQueryable<TEntity> GetAll() => _baseRepo.GetAll();
	public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate) => _baseRepo.GetAll(predicate);
	public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, object>> orderBy = null,
			bool isDesc = false,
			params Expression<Func<TEntity, object>>[] includes) => _baseRepo.GetAll(predicate, orderBy, isDesc, includes);
	public virtual IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate) => _baseRepo.Find(predicate);
	public virtual TEntity GetById(int id) => _baseRepo.GetById(id);
	public virtual TEntity GetBy(Expression<Func<TEntity, bool>> predicate) => _baseRepo.GetBy(predicate);
	public virtual TEntity GetBy(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includes) => _baseRepo.GetBy(predicate, includes);
	public virtual void Add(TEntity entity) => _baseRepo.Add(entity);
	public async Task AddAsync(TEntity entity) => await _baseRepo.AddAsync(entity);
	public virtual void AddRange(IList<TEntity> entities) => _baseRepo.AddRange(entities);
	public virtual async Task AddRangeAsync(IList<TEntity> entities) => await _baseRepo.AddRangeAsync(entities);
	public virtual void Update(TEntity entity) => _baseRepo.Update(entity);
	public virtual void UpdateRange(IList<TEntity> entities) => _baseRepo.UpdateRange(entities);
	public virtual void Remove(TEntity entity) => _baseRepo.Remove(entity);
	public virtual void RemoveRange(IList<TEntity> entities) => _baseRepo.RemoveRange(entities);
	public bool Exist(Expression<Func<TEntity, bool>> predicate) => this.GetAll(predicate).Any();
}
