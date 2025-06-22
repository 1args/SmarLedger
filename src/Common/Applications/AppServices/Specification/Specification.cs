using System.Linq.Expressions;
using SmartLedger.Common.Applications.AppServices.Specification.Abstractions;

namespace SmartLedger.Common.Applications.AppServices.Specification;

/// <summary>
/// Base class that implements the specification.
/// </summary>
/// <typeparam name="TEntity">Entity to which the specification is applied.</typeparam>
/// <param name="criteria">Filtering criterion as a lambda expression.</param>
public class Specification<TEntity>(
    Expression<Func<TEntity, bool>> criteria) : ISpecification<TEntity>
    where TEntity : class
{
    /// <inheritdoc />
    public Expression<Func<TEntity, bool>> Criteria => criteria;
}