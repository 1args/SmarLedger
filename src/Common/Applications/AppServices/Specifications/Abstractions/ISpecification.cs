using System.Linq.Expressions;

namespace SmartLedger.Common.Applications.AppServices.Specifications.Abstractions;

/// <summary>
/// Interface that defines the specification.
/// </summary>
/// <typeparam name="TEntity">Entity to which the specification is applied.</typeparam>
public interface ISpecification<TEntity>
    where TEntity : class
{
    /// <summary>Criteria for the specification as an expression.</summary>
    Expression<Func<TEntity, bool>> Criteria { get; }
}