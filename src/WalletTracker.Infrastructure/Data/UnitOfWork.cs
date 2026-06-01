using Microsoft.EntityFrameworkCore;
using WalletTracker.Application.Interfaces;

namespace WalletTracker.Infrastructure.Data;

public class UnitOfWork(WalletDBContext dbContext) : IUnitOfWork
{
    private readonly WalletDBContext _dbContext = dbContext;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
