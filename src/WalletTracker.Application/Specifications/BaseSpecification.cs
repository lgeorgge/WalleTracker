using System.Linq.Expressions;

namespace WalletTracker.Application.Specifications;

public abstract class BaseSpecification<T> : ISpecification<T>
    where T : class
{
    public Expression<Func<T, bool>>? Criteria { get; protected set; }

    public Expression<Func<T, object>>? OrderBy { get; protected set; }

    public Expression<Func<T, object>>? OrderByDesending { get; protected set; }
}
