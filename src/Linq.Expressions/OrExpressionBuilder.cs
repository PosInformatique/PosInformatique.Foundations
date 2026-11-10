//-----------------------------------------------------------------------
// <copyright file="OrExpressionBuilder.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace System.Linq.Expressions
{
    /// <summary>
    /// Builds a predicate by combining expressions using short-circuiting logical OR.
    /// </summary>
    /// <typeparam name="T">The type of the object evaluated by the predicate.</typeparam>
    public sealed class OrExpressionBuilder<T>
    {
        private readonly List<Expression<Func<T, bool>>> expressions;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrExpressionBuilder{T}"/> class.
        /// </summary>
        public OrExpressionBuilder()
        {
            this.expressions = new List<Expression<Func<T, bool>>>();
        }

        /// <summary>
        /// Adds a predicate to the expressions to combine.
        /// </summary>
        /// <param name="predicate">The predicate to add.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="predicate"/> is <see langword="null"/>.</exception>
        public void Add(Expression<Func<T, bool>> predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate);

            this.expressions.Add(predicate);
        }

        /// <summary>
        /// Builds an expression matching any of the added predicates.
        /// </summary>
        /// <returns>
        /// A predicate combining the expressions in insertion order, or a predicate returning
        /// <see langword="true"/> when no expressions have been added, so that no filtering is applied.
        /// </returns>
        public Expression<Func<T, bool>> Build()
        {
            if (this.expressions.Count == 0)
            {
                return t => true;
            }

            var parameter = this.expressions[0].Parameters[0];
            var expression = this.expressions[0].Body;

            for (var i = 1; i < this.expressions.Count; i++)
            {
                var rewrittenExpression = ExpressionParameterRewriter.Rewrite(
                    this.expressions[i].Body,
                    this.expressions[i].Parameters[0],
                    parameter);

                expression = Expression.OrElse(expression, rewrittenExpression);
            }

            return Expression.Lambda<Func<T, bool>>(expression, parameter);
        }

        private sealed class ExpressionParameterRewriter : ExpressionVisitor
        {
            private readonly ParameterExpression sourceParameter;

            private readonly ParameterExpression newParameter;

            private ExpressionParameterRewriter(ParameterExpression sourceParameter, ParameterExpression newParameter)
            {
                this.sourceParameter = sourceParameter;
                this.newParameter = newParameter;
            }

            public static Expression Rewrite(Expression expression, ParameterExpression sourceParameter, ParameterExpression newParameter)
            {
                var visitor = new ExpressionParameterRewriter(sourceParameter, newParameter);

                return visitor.Visit(expression);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node == this.sourceParameter ? this.newParameter : node;
            }
        }
    }
}