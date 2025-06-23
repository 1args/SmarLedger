using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace SmartLedger.Common.Applications.AppServices.Visitors;

/// <summary>
/// Represents an expression visitor that replaces a specific expression node with another.
/// </summary>
/// <remarks>
/// This visitor is useful when transforming expression trees by substituting one node with another.
/// </remarks>
/// <param name="oldValue">Expression to be replaced.</param>
/// <param name="newValue">Expression to replace with.</param>
public sealed class ReplaceExpressionVisitor(
    Expression oldValue, Expression newValue) : ExpressionVisitor
{
    /// <summary>
    /// Old value to be replaced in the expression tree.
    /// </summary>
    private readonly Expression _oldValue = oldValue ?? throw new ArgumentNullException(nameof(oldValue));

    /// <summary>
    /// New value to replace the old value in the expression tree.
    /// </summary>
    private readonly Expression _newValue = newValue ?? throw new ArgumentNullException(nameof(newValue));

    /// <summary>
    /// Visits the given expression node and replaces it if it matches the specified old value.
    /// </summary>
    /// <param name="node">Expression node to visit.</param>
    /// <returns>
    /// The new expression if the node matches the one to be replaced;
    /// otherwise, the result of the base visit operation.
    /// </returns>
    [return: NotNullIfNotNull("node")]
    public override Expression? Visit(Expression? node)
    {
        return node == _oldValue ? _newValue : base.Visit(node);
    }
}