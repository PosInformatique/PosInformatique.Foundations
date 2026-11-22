//-----------------------------------------------------------------------
// <copyright file="CollectionExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Collections.Tests
{
    public class CollectionExtensionsTest
    {
        [Fact]
        public void AddRange()
        {
            var list = new List<int>() { 1, 2, 3 }.As<ICollection<int>>();
            var items = new[] { 4, 5 };

            list.AddRange(items);

            list.Should().Equal(1, 2, 3, 4, 5);
        }

        [Fact]
        public void AddRange_NullArguments()
        {
            Action nullCollection = () => ((ICollection<int>)null).AddRange(Array.Empty<int>());
            Action nullItems = () => ((ICollection<int>)new List<int>()).AddRange(null);

            nullCollection.Should().ThrowExactly<ArgumentNullException>().WithParameterName("collection");
            nullItems.Should().ThrowExactly<ArgumentNullException>().WithParameterName("items");
        }

        [Fact]
        public void AsSequence()
        {
            "The value".AsSequence().Should().Equal("The value");
            1234.AsSequence().Should().Equal(1234);
        }

        [Fact]
        public void AsSequence_CollectionIsOneElement()
        {
            var items = new[] { 1, 2, 3 };

            var result = items.AsSequence();

            result.Should().ContainSingle().Which.Should().BeSameAs(items);
        }

        [Fact]
        public void AsSequence_NullValue()
        {
            Action act = () => ((string)null).AsSequence();

            act.Should().ThrowExactly<ArgumentNullException>().WithParameterName("value");
        }

        [Fact]
        public void IndexOf()
        {
            var items = new[] { 1, 2, 3, 4, 5, 3, 1, 2 };

            items.IndexOf(i => i == 3).Should().Be(2);
        }

        [Fact]
        public void IndexOf_NotFound()
        {
            var items = new[] { 1, 2, 3, 4, 5, 3, 1, 2 };

            items.IndexOf(i => i == 99).Should().BeNull();
        }

        [Fact]
        public void IndexOf_NullArguments()
        {
            Action nullItems = () => ((IReadOnlyList<int>)null).IndexOf(_ => true);
            Action nullPredicate = () => ((IReadOnlyList<int>)Array.Empty<int>()).IndexOf(null);

            nullItems.Should().ThrowExactly<ArgumentNullException>().WithParameterName("items");
            nullPredicate.Should().ThrowExactly<ArgumentNullException>().WithParameterName("predicate");
        }

        [Fact]
        public void RemoveAll()
        {
            var items = new List<int> { 1, 2, 3, 4, 5, 3, 10, 2 }.As<ICollection<int>>();

            items.RemoveAll(i => i >= 4);

            items.Should().Equal(1, 2, 3, 3, 2);
        }

        [Fact]
        public void RemoveAll_ReadOnlyCollection()
        {
            var items = new[] { 1, 2, 3, 4, 5, 3, 10, 2 };

            items.Invoking(i => i.RemoveAll(i => i >= 4))
                .Should().ThrowExactly<ArgumentException>()
                .WithMessage("The collection is read-only. (Parameter 'collection')")
                .WithParameterName("collection");
        }

        [Fact]
        public void RemoveAll_NullArguments()
        {
            Action nullCollection = () => ((ICollection<int>)null).RemoveAll(_ => true);
            Action nullPredicate = () => ((ICollection<int>)new List<int>()).RemoveAll(null);
            Action nullPredicateOnReadOnlyCollection = () => Array.Empty<int>().RemoveAll(null);

            nullCollection.Should().ThrowExactly<ArgumentNullException>().WithParameterName("collection");
            nullPredicate.Should().ThrowExactly<ArgumentNullException>().WithParameterName("predicate");
            nullPredicateOnReadOnlyCollection.Should().ThrowExactly<ArgumentNullException>().WithParameterName("predicate");
        }

        [Fact]
        public void Swap()
        {
            var list = new[] { 1, 2, 3 };

            list.Swap(0, 2);

            list.Should().Equal(3, 2, 1);
        }

        [Fact]
        public void Swap_NullList()
        {
            Action act = () => ((IList<int>)null).Swap(0, 0);

            act.Should().ThrowExactly<ArgumentNullException>().WithParameterName("list");
        }

        [Fact]
        public void Swap_SameIndex()
        {
            var list = new[] { 1, 2, 3 };

            list.Swap(1, 1);

            list.Should().Equal(1, 2, 3);
        }

        [Fact]
        public void Swap_WithIndex1LessThanZero()
        {
            var list = new[] { 1, 2, 3 };

            list.Invoking(l => l.Swap(-1, default))
                .Should().ThrowExactly<ArgumentOutOfRangeException>()
                .WithMessage("The first index must be greater than zero. (Parameter 'index1')\r\nActual value was -1.")
                .WithParameterName("index1")
                .And.ActualValue.Should().Be(-1);
        }

        [Fact]
        public void Swap_WithIndex1GreaterThanItemsCount()
        {
            var list = new[] { 1, 2, 3 };

            list.Invoking(l => l.Swap(10, default))
                .Should().ThrowExactly<ArgumentOutOfRangeException>()
                .WithMessage("The first index must less than the number of items of the list. (Parameter 'index1')\r\nActual value was 10.")
                .WithParameterName("index1")
                .And.ActualValue.Should().Be(10);
        }

        [Fact]
        public void Swap_WithIndex2LessThanZero()
        {
            var list = new[] { 1, 2, 3 };

            list.Invoking(l => l.Swap(default, -1))
                .Should().ThrowExactly<ArgumentOutOfRangeException>()
                .WithMessage("The second index must be greater than zero. (Parameter 'index2')\r\nActual value was -1.")
                .WithParameterName("index2")
                .And.ActualValue.Should().Be(-1);
        }

        [Fact]
        public void Swap_WithIndex2GreaterThanItemsCount()
        {
            var list = new[] { 1, 2, 3 };

            list.Invoking(l => l.Swap(default, 10))
                .Should().ThrowExactly<ArgumentOutOfRangeException>()
                .WithMessage("The second index must less than the number of items of the list. (Parameter 'index2')\r\nActual value was 10.")
                .WithParameterName("index2")
                .And.ActualValue.Should().Be(10);
        }

        [Fact]
        public void ToReadOnlyCollection_ReadOnlyList()
        {
            var list = new List<int>() { 1, 2, 3 };

            var collection = list.ToReadOnlyCollection();

            list.Add(10);

            collection.Should().Equal(1, 2, 3);
            collection.Should().NotBeSameAs(list);
            list.Should().Equal(1, 2, 3, 10);
        }

        [Fact]
        public void ToReadOnlyCollection_Enumerable()
        {
            var list = 1.AsSequence()
                .Concat(2.AsSequence())
                .Concat(3.AsSequence());

            var collection = list.ToReadOnlyCollection();

            collection.Should().Equal(1, 2, 3);
            collection.Should().NotBeSameAs(list);
        }

        [Fact]
        public void ToReadOnlyCollection_NullCollection()
        {
            Action act = () => ((IEnumerable<int>)null).ToReadOnlyCollection();

            act.Should().ThrowExactly<ArgumentNullException>().WithParameterName("collection");
        }
    }
}