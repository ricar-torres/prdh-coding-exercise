using System.Linq.Expressions;

namespace WebApi.Shared.Services;

public interface IServiceBase<TEntity> where TEntity : class {
	TEntity GetById(int id);
	TEntity GetBy(Expression<Func<TEntity, bool>> predicate);
	TEntity GetBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
	IQueryable<TEntity> GetAll();
	IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
	IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null,
		Expression<Func<TEntity, object>> orderyBy = null, bool isDesc = false,
		params Expression<Func<TEntity, object>>[] includes);
	IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
	void Add(TEntity entity);
	Task AddAsync(TEntity entity);
	void AddRange(IList<TEntity> entities);
	Task AddRangeAsync(IList<TEntity> entities);
	void Remove(TEntity entity);
	void RemoveRange(IList<TEntity> entities);
	void Update(TEntity entity);
	void UpdateRange(IList<TEntity> entities);
	bool Exist(Expression<Func<TEntity, bool>> predicate);
}
