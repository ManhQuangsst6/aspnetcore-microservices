using System.Linq.Expressions;
using Contracts.Common.Interfaces;
using Contracts.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Common;

public class RepositoryBaseAsync<T,K,TContext> :IRepositoryBaseAsync<T,K,TContext> where T: EntityBase<K> where TContext:DbContext
{
    private readonly TContext  _context;
    private readonly IUnitOfWork<TContext> _unitOfWork;
    public RepositoryBaseAsync(TContext context, IUnitOfWork<TContext> unitOfWork)
    {
        _context = context ?? throw  new  ArgumentNullException(nameof(context));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException( nameof(unitOfWork));
    }
    public IQueryable<T> GetAll(bool trackChanges = false)
        =>!trackChanges? _context.Set<T>().AsNoTracking(): _context.Set<T>();

    public IQueryable<T> GetAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
    {
        var items=GetAll(trackChanges);
        items=includeProperties.Aggregate(items,(current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false)
    => !trackChanges? _context.Set<T>().Where(expression).AsNoTracking(): _context.Set<T>().Where(expression);

    public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
    {
        var items=GetByCondition(expression,trackChanges);
        items=includeProperties.Aggregate(items,(current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    public async Task<T?> GetByIdAsync(K id) => await GetByCondition(x => x.Id.Equals(id)).FirstOrDefaultAsync();
   

    public async Task<T?> GetByIdAsync(K id, params Expression<Func<T, object>>[] includeProperties)
        => await GetByCondition(x => x.Id.Equals(id),trackChanges:false,includeProperties).FirstOrDefaultAsync();

    public async Task<K> CreateAsync(T entity)
    {
       await _context.Set<T>().AddAsync(entity);
       return entity.Id;
    }

    public async Task<IList<K>> CreateListAsync(IEnumerable<T> entities)
    {
       await _context.Set<T>().AddRangeAsync(entities);
       return entities.Select(x=>x.Id).ToList();
    }

    public async Task UpdateAsync(T entities)
    {
        if(_context.Entry(entities).State==EntityState.Unchanged) return ;
        T exist = await _context.Set<T>().FirstOrDefaultAsync(x=>x.Id.Equals(entities.Id));
        _context.Entry(exist).CurrentValues.SetValues(entities);
    } 

    public async Task UpdateListAsync(IEnumerable<T> entities)=> _context.Set<T>().UpdateRange(entities);
    

    public Task DeleteAsync(T entity)
    {
       _context.Set<T>().Remove(entity);
       return Task.CompletedTask;
    }

    public Task DeleteListAsync(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
        return Task.CompletedTask;
    }

    public Task<int> SaveChangesAsync() => _unitOfWork.CommitAsync();

    public Task<IDbContextTransaction> BeginTransactionAsync()=>_context.Database.BeginTransactionAsync();
    

    public async Task EndTransactionAsync()
    {
        await SaveChangesAsync();
        await _context.Database.CommitTransactionAsync();
    }

    public Task RollbackTransactionAsync()=>_context.Database.RollbackTransactionAsync();
    
}