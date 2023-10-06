using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure;

public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class {
	readonly DataContext _context;
	readonly DbSet<TEntity> _dbSet;

	public DbSet<TEntity> DbSet => _dbSet;

	protected RepositoryBase(DataContext context) {
		_context = context;
		_dbSet = context.Set<TEntity>();
	}

	public virtual IQueryable<TEntity> GetAll() {
		try {
			var list = _dbSet.AsQueryable();
			return list.AsNoTracking();
		}
		catch (Exception ex) {
			Console.WriteLine($"Exception in GetAll() at Repository: {ex.Message}");
			throw;
		}
	}

	public virtual IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate) {
		try {
			return _dbSet.Where(predicate ?? (x => true)).AsQueryable().AsNoTracking();
		}
		catch (Exception ex) {
			Console.WriteLine($"Exception in Find() at Repository: {ex.Message}");
			throw;
		}
	}


	public virtual TEntity GetById(int id) {
		try {
			var entity = _dbSet.Find(id);
			return entity;
		}
		catch (Exception ex) {
			Console.WriteLine($"Exception in GetById() at Repository: {ex.Message}");
			throw;
		}
	}

	public virtual void Add(TEntity entity) {
		try {
			_dbSet.Add(entity);
			_context.SaveChanges();
		}
		catch (Exception ex) {
			Console.WriteLine($"Exception in Add() at Repository: {ex.Message}");
			throw;
		}
	}

	public async Task AddAsync(TEntity entity) {
		try {
			await _dbSet.AddAsync(entity);
			await _context.SaveChangesAsync();
		}
		catch (Exception ex) {
			Console.WriteLine($"Exception in AddAsync() at Repository: {ex.Message}");
			throw;
		}
	}

	public virtual void AddRange(IList<TEntity> entities) {
		try {
			_dbSet.AddRange(entities);
			_context.SaveChanges();
		}
		catch (Exception ex) {
			Console.WriteLine($"Exception in AddRange() at Repository: {ex.Message}");
			throw;
		}
	}

	public virtual async Task AddRangeAsync(IList<TEntity> entities) {
		try {
			await _dbSet.AddRangeAsync(entities);
			await _context.SaveChangesAsync();
		}
		catch (Exception ex) {
			Console.WriteLine($"Exception in AddRangeAsync() at Repository: {ex.Message}");
			throw;
		}
	}

	public virtual void Update(TEntity entity) {
		try {
			_dbSet.Update(entity);
			_context.SaveChanges();
		}
		catch (Exception ex) {
			Console.WriteLine($"Exception in Update() at Repository: {ex.Message}");
			throw;
		}
	}

	public virtual void UpdateRange(IList<TEntity> entities) {
		try {
			_dbSet.UpdateRange(entities);
			_context.SaveChanges();
		}
		catch (Exception ex) {
			Console.WriteLine($"Exception in UpdateRange() at Repository: {ex.Message}");
			throw;
		}
	}

	public virtual void Remove(TEntity entity) {
		try {
			_dbSet.Remove(entity);
			_context.SaveChanges();
		}
		catch (Exception ex) {
			Console.WriteLine($"Exception in Remove() at Repository: {ex.Message}");
			throw ex;
		}
	}

	public virtual void RemoveRange(IList<TEntity> entities) {
		try {
			_dbSet.RemoveRange(entities);
			_context.SaveChanges();
		}
		catch (Exception ex) {
			Console.WriteLine($"Exception in RemoveRange() at Repository: {ex.Message}");
			throw ex;
		}
	}

	public virtual TEntity GetBy(Expression<Func<TEntity, bool>> predicate) {
		try {
			return _dbSet.Where(predicate ?? (x => true)).FirstOrDefault();
		}
		catch (Exception ex) {
			Console.WriteLine($"Exception in GetBy() at Repository: {ex.Message}");
			throw ex;
		}
	}

	public virtual TEntity GetBy(Expression<Func<TEntity, bool>> predicate, params string[] includePaths) {
		try {
			return IncludeEntitiesByString(includePaths).Where(predicate ?? (x => true)).FirstOrDefault();
		}
		catch (Exception ex) {
			Console.WriteLine($"Exception in GetBy() at Repository: {ex.Message}");
			throw ex;
		}
	}

	public virtual TEntity GetBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes) {
		try {
			return IncludeEntitiesBy(includes).Where(predicate ?? (x => true)).FirstOrDefault();
		}
		catch (Exception ex) {
			Console.WriteLine($"Exception in GetBy() at Repository: {ex.Message}");
			throw ex;
		}
	}

	public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate) {
		try {
			return _dbSet.Where(predicate ?? (x => true));
		}
		catch (Exception ex) {
			Console.WriteLine($"Exception in GetAll() at Repository: {ex.Message}");
			throw ex;
		}
	}

	public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate,
	Expression<Func<TEntity, object>> orderBy = null, bool isDesc = false,
	string[] includePaths = null) {
		try {
			IQueryable<TEntity> resQry = _dbSet.AsQueryable();

			if (includePaths is object) {
				resQry = IncludeEntitiesByString(includePaths).Where(predicate ?? (x => true));
			}
			else {
				resQry = _dbSet.Where(predicate ?? (x => true));
			}

			if (orderBy is object) {
				if (isDesc)
					resQry = resQry.OrderByDescending(orderBy);
				else
					resQry = resQry.OrderBy(orderBy);
			}

			return resQry;
		}
		catch (Exception ex) {
			Console.WriteLine($"Exception in GetAll() at Repository: {ex.Message}");
			throw ex;
		}
	}

	public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, object>> orderBy = null,
			bool isDesc = false,
			Expression<Func<TEntity, object>>[] includes = null) {
		try {
			var resQry = IncludeEntitiesBy(includes).Where(predicate ?? (x => true));
			if (orderBy is object) {
				if (isDesc)
					resQry = resQry.OrderByDescending(orderBy);
				else
					resQry = resQry.OrderBy(orderBy);
			}
			return resQry;
		}
		catch (Exception ex) {
			Console.WriteLine($"Exception in GetAll() at Repository: {ex.Message}");
			throw ex;
		}
	}

	private IQueryable<TEntity> IncludeEntitiesByString(string[] includes) {
		IQueryable<TEntity> query = _dbSet;
		includes.ToList().ForEach(includePath => query = _dbSet.Include(includePath));
		return query;
	}

	private IQueryable<TEntity> IncludeEntitiesBy(Expression<Func<TEntity, object>>[] includes) {
		IQueryable<TEntity> query = _dbSet;
		return includes.Aggregate(query.AsQueryable(), (current, include) => current.Include(include));
	}
}
