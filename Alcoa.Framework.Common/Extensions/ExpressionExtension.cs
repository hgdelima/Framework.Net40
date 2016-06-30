using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Alcoa.Framework.Common
{
    /// <summary>
    /// Class that helps build Expressions for EF queries
    /// </summary>
    public static class ExpressionExtension
    {
        /// <summary>
        /// Builds query expressions combining then with AND operator
        /// </summary>
        public static Expression<Func<T, bool>> BuildExpression<T>(this IEnumerable<Expression<Func<T, bool>>> exprList)
        {
            var body = default(Expression<Func<T, bool>>);

            if (exprList != null && exprList.Any())
            {
                var expressionList = exprList.ToList();

                //Initializes the first expression and removes from list
                body = expressionList[0];
                expressionList.RemoveAt(0);

                //If another expression exists in the list, build using AND operator
                if (expressionList.Any())
                    body = expressionList.Aggregate(body, (current, expression) => current.And(expression));
            }

            //returns complete expression body
            return body;
        }

        /// <summary>
        /// And operator between expressions
        /// </summary>
        private static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> exprLeft, Expression<Func<T, bool>> exprRight)
        {
            ParameterExpression pe = exprLeft.Parameters[0];

            var visitor = new SubstExpressionVisitor();
            visitor._subst[exprRight.Parameters[0]] = pe;

            Expression body = Expression.AndAlso(exprLeft.Body, visitor.Visit(exprRight.Body));

            return Expression.Lambda<Func<T, bool>>(body, pe);
        }

        /// <summary>
        /// Or operator between expressions
        /// </summary>
        /// <param name="exprLeft"></param>
        /// <param name="exprRight"></param>
        private static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> exprLeft, Expression<Func<T, bool>> exprRight)
        {
            ParameterExpression p = exprLeft.Parameters[0];

            var visitor = new SubstExpressionVisitor();
            visitor._subst[exprRight.Parameters[0]] = p;

            Expression body = Expression.OrElse(exprLeft.Body, visitor.Visit(exprRight.Body));
            return Expression.Lambda<Func<T, bool>>(body, p);
        }

        /// <summary>
        /// Substitute visitor for expression parameter
        /// </summary>
        private class SubstExpressionVisitor : ExpressionVisitor
        {
            public readonly Dictionary<Expression, Expression> _subst = new Dictionary<Expression, Expression>();

            protected override Expression VisitParameter(ParameterExpression node)
            {
                Expression newValue;
                return _subst.TryGetValue(node, out newValue) ? newValue : node;
            }
        }
    }
}