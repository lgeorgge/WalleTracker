using WalletTracker.Application.Specifications;

namespace WalletTracker.Infrastructure.Specifications;

public static class SpecEvaluator
{
    public static IQueryable<T> GetQuery<T>(
        IQueryable<T> inputQuery,
        ISpecification<T> specification
    )
    {
        var query = inputQuery;

        // Criteria
        if (specification.Criteria is not null)
        {
            query = query.Where(specification.Criteria);
        }

        // OrderBy
        if (specification.OrderBy is not null)
        {
            query = query.OrderBy(specification.OrderBy);
        }

        // OrderByDesc
        if (specification.OrderByDesending is not null)
        {
            query = query.OrderByDescending(specification.OrderByDesending);
        }

        // Pagination
        if (specification.IsPaginationEnabled)
        {
            query = query.Skip(specification.Skip).Take(specification.Take);
        }

        return query;
    }

    public static IQueryable<T> GetCountQuery<T>(
        IQueryable<T> inputQuery,
        ISpecification<T> specification
    )
    {
        var query = inputQuery;

        if (specification.Criteria is not null)
        {
            query = query.Where(specification.Criteria);
        }

        return query;
    }
}
