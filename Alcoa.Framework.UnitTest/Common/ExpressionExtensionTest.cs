using Alcoa.Framework.Common;
using Alcoa.Framework.Domain.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Alcoa.Framework.UnitTest.Common
{
    [TestClass]
    public class ExpressionExtensionTest
    {
        [TestInitialize]
        public void ExpressionExtensionTestInitialize()
        {
        }

        [TestMethod]
        public void BuildOrExpression()
        {
            var filter = new Worker { Login = "maria" };
            var exprList = new List<Expression<Func<Worker, bool>>>
            {
                wo => wo.Login.Contains(filter.Login),
                wo => wo.NameOrDescription.Contains(filter.NameOrDescription)
            };

            var expr = exprList.BuildExpression();

            Assert.IsNotNull(expr);
        }
    }
}