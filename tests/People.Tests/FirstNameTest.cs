//-----------------------------------------------------------------------
// <copyright file="FirstNameTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.People.Tests
{
    using System.Collections;

    public class FirstNameTest
    {
        [Fact]
        public void AllowedSeparators()
        {
            FirstName.AllowedSeparators.Should().Equal([' ', '-']);
        }

        [Theory]
        [MemberData(nameof(NameTestData.ValidFirstNames), MemberType = typeof(NameTestData))]
        public void Indexer(string firstName, string expectedFirstName)
        {
            FirstName.Create(firstName)[0].Should().Be(expectedFirstName[0]);
        }

        [Theory]
        [MemberData(nameof(NameTestData.ValidFirstNames), MemberType = typeof(NameTestData))]
        public void Length(string firstName, string expectedFirstName)
        {
            FirstName.Create(firstName).Length.Should().Be(expectedFirstName.Length);
            FirstName.Create(firstName).As<IReadOnlyList<char>>().Count.Should().Be(expectedFirstName.Length);
        }

        [Theory]
        [MemberData(nameof(NameTestData.ValidFirstNames), MemberType = typeof(NameTestData))]
        public void Create_Valid(string firstName, string expectedFirstName)
        {
            var result = FirstName.Create(firstName);

            result.ToString().Should().Be(expectedFirstName);
            result.As<IFormattable>().ToString(null, null).Should().Be(expectedFirstName);
        }

        [Fact]
        public void Create_Null()
        {
            var act = () =>
            {
                FirstName.Create(null);
            };

            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("firstName");
        }

        [Theory]
        [InlineData("  jean$!patrick  ")]
        [InlineData("  jean  patrick  Jr. ")]
        [InlineData("  $!  ")]
        public void Create_Invalid(string firstName)
        {
            var act = () =>
            {
                FirstName.Create(firstName);
            };

            act.Should().Throw<ArgumentException>()
                .WithMessage($"'{firstName}' is not a valid first name. (Parameter 'firstName')")
                .WithParameterName("firstName");
        }

        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("                                                            ")]
        public void Create_Empty(string firstName)
        {
            var act = () =>
            {
                FirstName.Create(firstName);
            };

            act.Should().Throw<ArgumentException>()
                .WithMessage($"The first name cannot be empty. (Parameter 'firstName')")
                .WithParameterName("firstName");
        }

        [Fact]
        public void Create_ExceedMaxLength()
        {
            var act = () =>
            {
                FirstName.Create(string.Concat(Enumerable.Repeat("A", 51)));
            };

            act.Should().Throw<ArgumentException>()
                .WithMessage($"The first name cannot exceed more than 50 characters. (Parameter 'firstName')")
                .WithParameterName("firstName");
        }

        [Theory]
        [MemberData(nameof(NameTestData.ValidFirstNames), MemberType = typeof(NameTestData))]
        public void Parse_Valid(string firstName, string expectedFirstName)
        {
            var formatProvider = Mock.Of<IFormatProvider>(MockBehavior.Strict);

            var result = CallParse<FirstName>(firstName, formatProvider);

            result.ToString().Should().Be(expectedFirstName);
            result.As<IFormattable>().ToString(null, null).Should().Be(expectedFirstName);
        }

        [Fact]
        public void Parse_Null()
        {
            var formatProvider = Mock.Of<IFormatProvider>(MockBehavior.Strict);

            var act = () =>
            {
                CallParse<FirstName>(null, formatProvider);
            };

            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("firstName");
        }

        [Theory]
        [InlineData("  jean$!patrick  ")]
        [InlineData("  jean  patrick  Jr. ")]
        [InlineData("  $!  ")]
        public void Parse_Invalid(string firstName)
        {
            var formatProvider = Mock.Of<IFormatProvider>(MockBehavior.Strict);

            var act = () =>
            {
                CallParse<FirstName>(firstName, formatProvider);
            };

            act.Should().Throw<ArgumentException>()
                .WithMessage($"'{firstName}' is not a valid first name. (Parameter 'firstName')")
                .WithParameterName("firstName");
        }

        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("                                                            ")]
        public void Parse_Empty(string firstName)
        {
            var formatProvider = Mock.Of<IFormatProvider>(MockBehavior.Strict);

            var act = () =>
            {
                CallParse<FirstName>(firstName, formatProvider);
            };

            act.Should().Throw<ArgumentException>()
                .WithMessage($"The first name cannot be empty. (Parameter 'firstName')")
                .WithParameterName("firstName");
        }

        [Fact]
        public void Parse_ExceedMaxLength()
        {
            var formatProvider = Mock.Of<IFormatProvider>(MockBehavior.Strict);

            var act = () =>
            {
                CallParse<FirstName>(string.Concat(Enumerable.Repeat("A", 51)), formatProvider);
            };

            act.Should().Throw<ArgumentException>()
                .WithMessage($"The first name cannot exceed more than 50 characters. (Parameter 'firstName')")
                .WithParameterName("firstName");
        }

        [Theory]
        [MemberData(nameof(NameTestData.ValidFirstNames), MemberType = typeof(NameTestData))]
        public void TryCreate_Valid(string firstName, string expectedFirstName)
        {
            FirstName.TryCreate(firstName, out var result).Should().BeTrue();

            result.ToString().Should().Be(expectedFirstName);
            result.As<IFormattable>().ToString(null, null).Should().Be(expectedFirstName);
        }

        [Theory]
        [MemberData(nameof(NameTestData.InvalidFirstNames), MemberType = typeof(NameTestData))]
        public void TryCreate_Invalid(string firstName)
        {
            FirstName.TryCreate(firstName, out var result).Should().BeFalse();

            result.As<object>().Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(NameTestData.ValidFirstNames), MemberType = typeof(NameTestData))]
        public void TryParse_Valid(string firstName, string expectedFirstName)
        {
            var formatProvider = Mock.Of<IFormatProvider>(MockBehavior.Strict);

            CallTryParse<FirstName>(firstName, formatProvider, out var result).Should().BeTrue();

            result.ToString().Should().Be(expectedFirstName);
            result.As<IFormattable>().ToString(null, null).Should().Be(expectedFirstName);
        }

        [Theory]
        [MemberData(nameof(NameTestData.InvalidFirstNames), MemberType = typeof(NameTestData))]
        public void TryParse_Invalid(string firstName)
        {
            var formatProvider = Mock.Of<IFormatProvider>(MockBehavior.Strict);

            CallTryParse<FirstName>(firstName, formatProvider, out var result).Should().BeFalse();

            result.As<object>().Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(NameTestData.ValidFirstNames), MemberType = typeof(NameTestData))]
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
        public void IsValid_Valid(string firstName, string _)
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
        {
            FirstName.IsValid(firstName).Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(NameTestData.InvalidFirstNames), MemberType = typeof(NameTestData))]
        public void IsValid_Invalid(string firstName)
        {
            FirstName.IsValid(firstName).Should().BeFalse();
        }

        [Fact]
        public void GetEnumerator()
        {
            var firstName = FirstName.Create("Jean");

            var enumerator = firstName.GetEnumerator();

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be('J');

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be('e');

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be('a');

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be('n');

            enumerator.MoveNext().Should().BeFalse();
            enumerator.MoveNext().Should().BeFalse();
        }

        [Fact]
        public void GetEnumerator_NonGeneric()
        {
            var firstName = FirstName.Create("Jean");

            var enumerator = firstName.As<IEnumerable>().GetEnumerator();

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be('J');

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be('e');

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be('a');

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be('n');
        }

        [Theory]
        [InlineData("Jean", "Jean", true)]
        [InlineData("Jean", "Other", false)]
        public void Equals_Typed(string firstName1, string firstName2, bool result)
        {
            var f1 = FirstName.Create(firstName1);
            var f2 = FirstName.Create(firstName2);

            f1.Equals(f2).Should().Be(result);
        }

        [Fact]
        public void Equals_Typed_Null()
        {
            FirstName.Create("Jean").Equals(null).Should().BeFalse();
        }

        [Theory]
        [InlineData("Jean", "Jean", true)]
        [InlineData("Jean", "Other", false)]
        public void Equals_Object(string firstName1, string firstName2, bool result)
        {
            var f1 = FirstName.Create(firstName1);
            var f2 = FirstName.Create(firstName2);

            f1.Equals((object)f2).Should().Be(result);
        }

        [Fact]
        public void Equals_Object_Null()
        {
            FirstName.Create("Jean").Equals((object)null).Should().BeFalse();
        }

        [Theory]
        [InlineData("Jean", "Jean", true)]
        [InlineData("Jean", "Other", false)]
        public void Equals_Operator(string firstName1, string firstName2, bool result)
        {
            var f1 = FirstName.Create(firstName1);
            var f2 = FirstName.Create(firstName2);

            (f1 == f2).Should().Be(result);
        }

        [Theory]
        [InlineData("Jean", "Jean", false)]
        [InlineData("Jean", "Other", true)]
        public void NotEquals_Operator(string firstName1, string firstName2, bool result)
        {
            var f1 = FirstName.Create(firstName1);
            var f2 = FirstName.Create(firstName2);

            (f1 != f2).Should().Be(result);
        }

        [Fact]
        public void GetHashCode_Test()
        {
            FirstName.Create("Jean").GetHashCode().Should().Be("Jean".GetHashCode(StringComparison.Ordinal));
            FirstName.Create("Jean").GetHashCode().Should().NotBe("Autre".GetHashCode(StringComparison.Ordinal));
        }

        [Fact]
        public void ImplicitOperator_FirstNameToString()
        {
            var firstName = FirstName.Create("The first name");

            string stringValue = firstName;

            stringValue.Should().Be("The First Name");
        }

        [Fact]
        public void ImplicitOperator_FirstNameToString_WithNullArgument()
        {
            FirstName firstName = null;

            var act = () =>
            {
                string _ = firstName;
            };

            act.Should()
                .ThrowExactly<ArgumentNullException>()
                .WithParameterName("firstName");
        }

        [Fact]
        public void ImplicitOperator_StringToFirstName()
        {
            FirstName firstName = "The first name";

            firstName.ToString().Should().Be("The First Name");
            firstName.As<IFormattable>().ToString(null, null).Should().Be("The First Name");
        }

        [Fact]
        public void ImplicitOperator_StringToFirstName_WithNullArgument()
        {
            string firstName = null;

            var act = () =>
            {
                FirstName _ = firstName;
            };

            act.Should()
                .ThrowExactly<ArgumentNullException>()
                .WithParameterName("firstName");
        }

        [Fact]
        public void CompareTo()
        {
            FirstName.Create("First name A").CompareTo(FirstName.Create("First name B")).Should().BeLessThan(0);
            FirstName.Create("First name B").CompareTo(FirstName.Create("First name A")).Should().BeGreaterThan(0);

            FirstName.Create("First name A").CompareTo(FirstName.Create("First name A")).Should().Be(0);

            FirstName.Create("First name A").CompareTo(FirstName.Create("First nâme A")).Should().BeLessThan(0);
            FirstName.Create("First nâme A").CompareTo(FirstName.Create("First name A")).Should().BeGreaterThan(0);

            FirstName.Create("First name B").CompareTo(FirstName.Create("First nâme A")).Should().BeLessThan(0);
            FirstName.Create("First nâme A").CompareTo(FirstName.Create("First name B")).Should().BeGreaterThan(0);

            FirstName.Create("First name A").CompareTo(null).Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData("First name A", "First name B", true)]
        [InlineData("First name B", "First name A", false)]
        [InlineData("First name A", "First name A", false)]
        [InlineData("First name A", "First nâme A", true)]
        [InlineData("First nâme A", "First name A", false)]
        [InlineData("First name B", "First nâme A", true)]
        [InlineData("First nâme B", "First name A", false)]
        [InlineData(null, "First name A", true)]
        [InlineData("First name A", null, false)]
        [InlineData(null, null, false)]
        public void Operator_LessThan(string firstName1, string firstName2, bool result)
        {
            ((firstName1 is not null ? FirstName.Create(firstName1) : null) < (firstName2 is not null ? FirstName.Create(firstName2) : null)).Should().Be(result);
        }

        [Theory]
        [InlineData("First name A", "First name B", true)]
        [InlineData("First name B", "First name A", false)]
        [InlineData("First name A", "First name A", true)]
        [InlineData("First name A", "First nâme A", true)]
        [InlineData("First nâme A", "First name A", false)]
        [InlineData("First name B", "First nâme A", true)]
        [InlineData("First nâme B", "First name A", false)]
        [InlineData(null, "First name A", true)]
        [InlineData("First name A", null, false)]
        [InlineData(null, null, true)]
        public void Operator_LessThanOrEqual(string firstName1, string firstName2, bool result)
        {
            ((firstName1 is not null ? FirstName.Create(firstName1) : null) <= (firstName2 is not null ? FirstName.Create(firstName2) : null)).Should().Be(result);
        }

        [Theory]
        [InlineData("First name A", "First name B", false)]
        [InlineData("First name B", "First name A", true)]
        [InlineData("First name A", "First name A", false)]
        [InlineData("First name A", "First nâme A", false)]
        [InlineData("First nâme A", "First name A", true)]
        [InlineData("First name B", "First nâme A", false)]
        [InlineData("First nâme B", "First name A", true)]
        [InlineData(null, "First name A", false)]
        [InlineData("First name A", null, true)]
        [InlineData(null, null, false)]
        public void Operator_GreaterThan(string firstName1, string firstName2, bool result)
        {
            ((firstName1 is not null ? FirstName.Create(firstName1) : null) > (firstName2 is not null ? FirstName.Create(firstName2) : null)).Should().Be(result);
        }

        [Theory]
        [InlineData("First name A", "First name B", false)]
        [InlineData("First name B", "First name A", true)]
        [InlineData("First name A", "First name A", true)]
        [InlineData("First name A", "First nâme A", false)]
        [InlineData("First nâme A", "First name A", true)]
        [InlineData("First name B", "First nâme A", false)]
        [InlineData("First nâme B", "First name A", true)]
        [InlineData(null, "First name A", false)]
        [InlineData("First name A", null, true)]
        [InlineData(null, null, true)]
        public void Operator_GreaterThanOrEqual(string firstName1, string firstName2, bool result)
        {
            ((firstName1 is not null ? FirstName.Create(firstName1) : null) >= (firstName2 is not null ? FirstName.Create(firstName2) : null)).Should().Be(result);
        }

        private static T CallParse<T>(string s, IFormatProvider formatProvider)
            where T : IParsable<T>
        {
            return T.Parse(s, formatProvider);
        }

        private static bool CallTryParse<T>(string s, IFormatProvider formatProvider, out T result)
            where T : IParsable<T>
        {
            return T.TryParse(s, formatProvider, out result);
        }
    }
}
