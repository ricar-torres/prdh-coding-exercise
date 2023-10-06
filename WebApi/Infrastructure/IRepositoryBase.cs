using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure;

public interface IRepositoryBase<TEntity> where TEntity : class {
	DbSet<TEntity> DbSet { get; }
	TEntity GetById(int id);
	TEntity GetBy(Expression<Func<TEntity, bool>> predicate);
	TEntity GetBy(Expression<Func<TEntity, bool>> predicate, params string[] includePaths);
	TEntity GetBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
	IQueryable<TEntity> GetAll();
	IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
	IQueryable<TEntity> GetAll(
			Expression<Func<TEntity, bool>> predicate = null,
			Expression<Func<TEntity, object>> orderBy = null,
			bool isDesc = false,
			string[] includePaths = null);
	IQueryable<TEntity> GetAll(
			Expression<Func<TEntity, bool>> predicate = null,
			Expression<Func<TEntity, object>> orderBy = null,
			bool isDesc = false,
			params Expression<Func<TEntity, object>>[] includes);
	IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
	void Add(TEntity entity);
	Task AddAsync(TEntity entity);
	Task AddRangeAsync(IList<TEntity> entities);
	void AddRange(IList<TEntity> entities);
	void Remove(TEntity entity);
	void RemoveRange(IList<TEntity> entities);
	void Update(TEntity entity);
	void UpdateRange(IList<TEntity> entities);
}
