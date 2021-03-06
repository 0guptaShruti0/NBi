﻿using System;
using System.Linq;
using Moq;
using NBi.Core;
using NBi.NUnit.Query;
using NBi.Core.ResultSet;
using System.Data.SqlClient;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Core.Calculation;
using NBi.Core.Evaluate;
using System.Collections.Generic;
using NUnit.Framework;

namespace NBi.Testing.Unit.NUnit.ResultSetComparison
{
    [TestFixture]
    public class AllRowsConstraintTest
    {
        
        #region Setup & Teardown

        [SetUp]
        public void SetUp()
        {
           
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion

        [Test]
        public void Matches_SqlCommand_CallToResultSetBuilderOnce()
        {
            var resultSet = new ResultSet();
            resultSet.Load("a;b;1");
            var cmd = new SqlCommand();

            var rsbMock = new Mock<ResultSetBuilder>();
            rsbMock.Setup(engine => engine.Build(It.IsAny<object>()))
                .Returns(resultSet);
            var rsb = rsbMock.Object;

            var alias = Mock.Of<IColumnAlias>(v => v.Column == 2 && v.Name == "Value");
            var predicate = Mock.Of<IPredicateInfo>
                (
                    p => p.ColumnType==ColumnType.Numeric 
                    && p.ComparerType==ComparerType.Equal 
                    && p.Name=="Value" 
                    && p.Reference==(object)1
                );

            var factory = new PredicateFilterFactory();
            var filter = factory.Instantiate
                (
                    new List<IColumnAlias>() { alias }
                    , new List<IColumnExpression>() { }
                    , predicate
                );

            var rowCount = new AllRowsConstraint(filter) { ResultSetBuilder = rsb };
            rowCount.ResultSetBuilder = rsb;

            //Method under test
            rowCount.Matches(cmd);

            //Test conclusion            
            rsbMock.Verify(engine => engine.Build(It.IsAny<object>()), Times.Once());
        }

    }
}
