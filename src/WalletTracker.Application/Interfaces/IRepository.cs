using WalletTracker.Application.Specifications;
using WalletTracker.Domain.Common;

namespace WalletTracker.Application.Interfaces;

public interface IRepository<T>
    where T : BaseEntity
{
    // Read
    Task<IReadOnlyList<T>> ListAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default
    );

    Task<int> CountAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default
    );

    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<T?> FindFirstOrDefaultAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default
    );

    Task<T?> FindSingleOrDefaultAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default
    );

    // Write
    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    void Update(T entity);

    void Remove(T entity);
}
