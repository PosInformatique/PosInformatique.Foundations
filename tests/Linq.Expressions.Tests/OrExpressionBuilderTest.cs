//-----------------------------------------------------------------------
// <copyright file="OrExpressionBuilderTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Linq.Expressions.Tests
{
    using System.Linq.Expressions;

    public class OrExpressionBuilderTest
    {
        [Fact]
        public void Build()
        {
            var entities = new EntityTest[]
            {
                new EntityTest() { Name = "A" },
                new EntityTest() { Name = "B" },
                new EntityTest() { Name = "C" },
                new EntityTest() { Name = "D" },
                new EntityTest() { Name = "E" },
            };

            var expressionBuilder = new OrExpressionBuilder<EntityTest>();
            expressionBuilder.Add(e => e.Name == "B");
            expressionBuilder.Add(e => e.Name == "C");
            expressionBuilder.Add(e => e.Name == "E");

            var expression = expressionBuilder.Build();

            var result = entities.AsQueryable().Where(expression).ToArray();

            result.Should().Equal(entities[1], entities[2], entities[4]);
        }

        [Fact]
        public void Build_OneExpression()
        {
            var entities = new EntityTest[]
            {
                new EntityTest() { Name = "A" },
                new EntityTest() { Name = "B" },
                new EntityTest() { Name = "C" },
                new EntityTest() { Name = "D" },
                new EntityTest() { Name = "E" },
            };

            var expressionBuilder = new OrExpressionBuilder<EntityTest>();
            expressionBuilder.Add(e => e.Name == "C");

            var expression = expressionBuilder.Build();

            var result = entities.AsQueryable().Where(expression).ToArray();

            result.Should().Equal(entities[2]);
        }

        [Fact]
        public void Build_NoExpression()
        {
            var entities = new EntityTest[]
            {
                new EntityTest() { Name = "A" },
                new EntityTest() { Name = "B" },
                new EntityTest() { Name = "C" },
                new EntityTest() { Name = "D" },
            };

            var expressionBuilder = new OrExpressionBuilder<EntityTest>();

            var expression = expressionBuilder.Build();

            var result = entities.AsQueryable().Where(expression).ToArray();

            result.Should().Equal(entities);
        }

        [Fact]
        public void Add_WithPredicateNull()
        {
            var expressionBuilder = new OrExpressionBuilder<EntityTest>();

            var act = () => expressionBuilder.Add(null);

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("predicate");
        }

        [Fact]
        public void Build_NoMatchingExpression()
        {
            var expressionBuilder = new OrExpressionBuilder<EntityTest>();
            expressionBuilder.Add(e => e.Name == "B");
            expressionBuilder.Add(e => e.Name == "C");

            var predicate = expressionBuilder.Build().Compile();

            predicate(new EntityTest() { Name = "A" }).Should().BeFalse();
        }

        [Fact]
        public void Build_ShortCircuits()
        {
            var expressionBuilder = new OrExpressionBuilder<EntityTest>();
            expressionBuilder.Add(e => e.Name == null);
            expressionBuilder.Add(e => e.Name.Length > 0);

            var expression = expressionBuilder.Build();
            var predicate = expression.Compile();

            expression.Body.NodeType.Should().Be(ExpressionType.OrElse);
            predicate(new EntityTest()).Should().BeTrue();
            predicate(new EntityTest() { Name = "A" }).Should().BeTrue();
            predicate(new EntityTest() { Name = string.Empty }).Should().BeFalse();
        }

        [Fact]
        public void Build_WithNestedLambda()
        {
            var entities = new EntityTest[]
            {
                new EntityTest() { Name = "A" },
                new EntityTest() { Name = "B" },
                new EntityTest() { Name = "C" },
            };

            var names = new[] { "B", "D" };
            var expressionBuilder = new OrExpressionBuilder<EntityTest>();
            expressionBuilder.Add(e => e.Name == "A");
            expressionBuilder.Add(e => names.Any(name => name == e.Name));

            var expression = expressionBuilder.Build();

            var result = entities.AsQueryable().Where(expression).ToArray();

            result.Should().Equal(entities[0], entities[1]);
        }

        [Fact]
        public void Build_WithNestedLambdaOfSameType()
        {
            var entities = new EntityTest[]
            {
                new EntityTest() { Name = "A" },
                new EntityTest() { Name = "B" },
                new EntityTest() { Name = "C" },
            };

            var matches = new[] { new EntityTest() { Name = "B" } };
            var expressionBuilder = new OrExpressionBuilder<EntityTest>();
            expressionBuilder.Add(e => e.Name == "A");
            expressionBuilder.Add(e => matches.Any(other => other.Name == e.Name));

            var expression = expressionBuilder.Build();

            var result = entities.AsQueryable().Where(expression).ToArray();

            result.Should().Equal(entities[0], entities[1]);
        }

        [Fact]
        public void Build_CalledTwice()
        {
            var expressionBuilder = new OrExpressionBuilder<EntityTest>();
            expressionBuilder.Add(e => e.Name == "B");
            expressionBuilder.Add(e => e.Name == "C");

            var firstPredicate = expressionBuilder.Build().Compile();
            var secondPredicate = expressionBuilder.Build().Compile();

            var entity = new EntityTest() { Name = "C" };
            firstPredicate(entity).Should().BeTrue();
            secondPredicate(entity).Should().BeTrue();
        }

        [Fact]
        public void Build_AfterAddingExpression()
        {
            var expressionBuilder = new OrExpressionBuilder<EntityTest>();
            expressionBuilder.Add(e => e.Name == "B");

            var firstPredicate = expressionBuilder.Build().Compile();

            expressionBuilder.Add(e => e.Name == "C");

            var secondPredicate = expressionBuilder.Build().Compile();

            var entity = new EntityTest() { Name = "C" };
            firstPredicate(entity).Should().BeFalse();
            secondPredicate(entity).Should().BeTrue();
        }

        private sealed class EntityTest
        {
            public string Name { get; set; }
        }
    }
}