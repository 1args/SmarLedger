using SmartLedger.Common.Applications.AppServices.Visitors;
using System.Linq.Expressions;
using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Common.Applications.AppServices.Specifications.Abstractions;

namespace SmartLedger.Common.Applications.AppServices.Extensions;

/// <summary>
/// Extension for applying and combining specifications in a LINQ-compatible way.
/// </summary>
public static class SpecificationExtensions
{
    /// <summary>
    /// Filters the query using the criteria defined in the provided specification.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity.</typeparam>
    /// <param name="query">Query to which filtering will be applied.</param>
    /// <param name="specification">Specification containing the filtering logic.</param>
    /// <returns>Filtered <see cref="IQueryable{T}"/> based on the specification's criteria.</returns>
    public static IQueryable<TEntity> Where<TEntity>(
        this IQueryable<TEntity> query,
        ISpecification<TEntity> specification)
        where TEntity : class
    {
        return query.Where(specification.Criteria);
    }

    /// <summary>
    /// Combines two specifications using a logical AND operation, returning a new specification.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity.</typeparam>
    /// <param name="currentSpecification">Current specification.</param>
    /// <param name="otherSpecification">Other specification to combine with.</param>
    /// <returns>New <see cref="ISpecification{TEntity}"/> representing the logical conjunction of both.</returns>
    public static ISpecification<TEntity> And<TEntity>(
        this ISpecification<TEntity> currentSpecification,
        ISpecification<TEntity> otherSpecification)
        where TEntity : class
    {
        return new Specification<TEntity>(currentSpecification.Criteria
            .AndAlso(otherSpecification.Criteria));
    }

    /// <summary>
    /// Combines two predicate expressions using a logical AND, while normalizing their parameters.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity.</typeparam>
    /// <param name="currentSpecification">Current predicate expression.</param>
    /// <param name="otherSpecification">Other predicate expression.</param>
    /// <returns>
    /// Expression that represents the logical AND of the two input expressions.
    /// </returns>
    /// <remarks>
    /// Since expressions may have different parameter instances, a visitor is used to replace both with a common one.
    /// </remarks>
    private static Expression<Func<TEntity, bool>> AndAlso<TEntity>(
        this Expression<Func<TEntity, bool>> currentSpecification,
        Expression<Func<TEntity, bool>> otherSpecification)
    {
        var parameter = Expression.Parameter(typeof(TEntity));

        var currentVisitor = new ReplaceExpressionVisitor(currentSpecification.Parameters[0], parameter);
        var currentBody = currentVisitor.Visit(currentSpecification.Body);

        var otherVisitor = new ReplaceExpressionVisitor(otherSpecification.Parameters[0], parameter);
        var otherBody = otherVisitor.Visit(otherSpecification.Body);

        return Expression.Lambda<Func<TEntity, bool>>(
            Expression.AndAlso(currentBody, otherBody),
            parameter);
    }
}