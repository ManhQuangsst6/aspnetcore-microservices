using Contracts.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common;

public class UnitOfWork<TContext>(TContext context) : IUnitOfWork<TContext>
    where TContext : DbContext
{
    private readonly TContext _context = context;

    public Task<int> CommitAsync() => _context.SaveChangesAsync();
    

    public void Dispose()=>_context.Dispose();
}