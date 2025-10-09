//-----------------------------------------------------------------------
// <copyright file="LastNameTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.People.Tests
{
    using System.Collections;

    public class LastNameTest
    {
        [Fact]
        public void AllowedSeparators()
        {
            LastName.AllowedSeparators.Should().Equal([' ', '-']);
        }

        [Theory]
        [MemberData(nameof(NameTestData.ValidLastNames), MemberType = typeof(NameTestData))]
        public void Indexer(string lastName, string expectedLastName)
        {
            LastName.Create(lastName)[0].Should().Be(expectedLastName[0]);
        }

        [Theory]
        [MemberData(nameof(NameTestData.ValidLastNames), MemberType = typeof(NameTestData))]
        public void Length(string lastName, string expectedLastName)
        {
            LastName.Create(lastName).Length.Should().Be(expectedLastName.Length);
            LastName.Create(lastName).As<IReadOnlyList<char>>().Count.Should().Be(expectedLastName.Length);
        }

        [Theory]
        [MemberData(nameof(NameTestData.ValidLastNames), MemberType = typeof(NameTestData))]
        public void Create_Valid(string lastName, string expectedLastName)
        {
            var result = LastName.Create(lastName);

            result.ToString().Should().Be(expectedLastName);
            result.As<IFormattable>().ToString(null, null).Should().Be(expectedLastName);
        }

        [Fact]
        public void Create_Null()
        {
            var act = () =>
            {
                LastName.Create(null);
            };

            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("lastName");
        }

        [Theory]
        [InlineData("$$Dupont")]
        [InlineData("Du@$+Pont")]
        [InlineData("Du-pont.")]
        public void Create_Invalid(string lastName)
        {
            var act = () =>
            {
                LastName.Create(lastName);
            };

            act.Should().Throw<ArgumentException>()
                .WithMessage($"'{lastName}' is not a valid last name. (Parameter 'lastName')")
                .WithParameterName("lastName");
        }

        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("                                                            ")]
        public void Create_Empty(string lastName)
        {
            var act = () =>
            {
                LastName.Create(lastName);
            };

            act.Should().Throw<ArgumentException>()
                .WithMessage($"The last name cannot be empty. (Parameter 'lastName')")
                .WithParameterName("lastName");
        }

        [Fact]
        public void Create_ExceedMaxLength()
        {
            var act = () =>
            {
                LastName.Create(string.Concat(Enumerable.Repeat("A", 51)));
            };

            act.Should().Throw<ArgumentException>()
                .WithMessage($"The last name cannot exceed more than 50 characters. (Parameter 'lastName')")
                .WithParameterName("lastName");
        }

        [Theory]
        [MemberData(nameof(NameTestData.ValidLastNames), MemberType = typeof(NameTestData))]
        public void Parse_Valid(string lastName, string expectedLastName)
        {
            var formatProvider = Mock.Of<IFormatProvider>(MockBehavior.Strict);

            var result = CallParse<LastName>(lastName, formatProvider);

            result.ToString().Should().Be(expectedLastName);
            result.As<IFormattable>().ToString(null, null).Should().Be(expectedLastName);
        }

        [Fact]
        public void Parse_Null()
        {
            var formatProvider = Mock.Of<IFormatProvider>(MockBehavior.Strict);

            var act = () =>
            {
                CallParse<LastName>(null, formatProvider);
            };

            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("lastName");
        }

        [Theory]
        [InlineData("$$Dupont")]
        [InlineData("Du@$+Pont")]
        [InlineData("Du-pont.")]
        public void Parse_Invalid(string lastName)
        {
            var formatProvider = Mock.Of<IFormatProvider>(MockBehavior.Strict);

            var act = () =>
            {
                CallParse<LastName>(lastName, formatProvider);
            };

            act.Should().Throw<ArgumentException>()
                .WithMessage($"'{lastName}' is not a valid last name. (Parameter 'lastName')")
                .WithParameterName("lastName");
        }

        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("                                                            ")]
        public void Parse_Empty(string lastName)
        {
            var formatProvider = Mock.Of<IFormatProvider>(MockBehavior.Strict);

            var act = () =>
            {
                CallParse<LastName>(lastName, formatProvider);
            };

            act.Should().Throw<ArgumentException>()
                .WithMessage($"The last name cannot be empty. (Parameter 'lastName')")
                .WithParameterName("lastName");
        }

        [Fact]
        public void Parse_ExceedMaxLength()
        {
            var formatProvider = Mock.Of<IFormatProvider>(MockBehavior.Strict);

            var act = () =>
            {
                CallParse<LastName>(string.Concat(Enumerable.Repeat("A", 51)), formatProvider);
            };

            act.Should().Throw<ArgumentException>()
                .WithMessage($"The last name cannot exceed more than 50 characters. (Parameter 'lastName')")
                .WithParameterName("lastName");
        }

        [Theory]
        [MemberData(nameof(NameTestData.ValidLastNames), MemberType = typeof(NameTestData))]
        public void TryCreate_Valid(string lastName, string expectedLastName)
        {
            LastName.TryCreate(lastName, out var result).Should().BeTrue();

            result.ToString().Should().Be(expectedLastName);
            result.As<IFormattable>().ToString(null, null).Should().Be(expectedLastName);
        }

        [Theory]
        [MemberData(nameof(NameTestData.InvalidLastNames), MemberType = typeof(NameTestData))]
        public void TryCreate_Invalid(string lastName)
        {
            LastName.TryCreate(lastName, out var result).Should().BeFalse();

            result.As<object>().Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(NameTestData.ValidLastNames), MemberType = typeof(NameTestData))]
        public void TryParse_Valid(string lastName, string expectedLastName)
        {
            var formatProvider = Mock.Of<IFormatProvider>(MockBehavior.Strict);

            CallTryParse<LastName>(lastName, formatProvider, out var result).Should().BeTrue();

            result.ToString().Should().Be(expectedLastName);
            result.As<IFormattable>().ToString(null, null).Should().Be(expectedLastName);
        }

        [Theory]
        [MemberData(nameof(NameTestData.InvalidFirstNames), MemberType = typeof(NameTestData))]
        public void TryParse_Invalid(string lastName)
        {
            var formatProvider = Mock.Of<IFormatProvider>(MockBehavior.Strict);

            CallTryParse<LastName>(lastName, formatProvider, out var result).Should().BeFalse();

            result.As<object>().Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(NameTestData.ValidLastNames), MemberType = typeof(NameTestData))]
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
        public void IsValid_Valid(string lastName, string _)
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
        {
            LastName.IsValid(lastName).Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(NameTestData.InvalidLastNames), MemberType = typeof(NameTestData))]
        public void IsValid_Invalid(string lastName)
        {
            LastName.IsValid(lastName).Should().BeFalse();
        }

        [Fact]
        public void GetEnumerator()
        {
            var lastName = LastName.Create("DUPONT");

            var enumerator = lastName.GetEnumerator();

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be('D');

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be('U');

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be('P');

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be('O');

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be('N');

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be('T');
        }

        [Fact]
        public void GetEnumerator_NonGeneric()
        {
            var lastName = LastName.Create("DUPONT");

            var enumerator = lastName.As<IEnumerable>().GetEnumerator();

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be('D');

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be('U');

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be('P');

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be('O');

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be('N');

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be('T');
        }

        [Theory]
        [InlineData("DUPONT", "DUPONT", true)]
        [InlineData("DUPONT", "OTHER", false)]
        public void Equals_Typed(string lastName1, string lastName2, bool result)
        {
            var f1 = LastName.Create(lastName1);
            var f2 = LastName.Create(lastName2);

            f1.Equals(f2).Should().Be(result);
        }

        [Fact]
        public void Equals_Typed_Null()
        {
            LastName.Create("Jean").Equals(null).Should().BeFalse();
        }

        [Theory]
        [InlineData("DUPONT", "DUPONT", true)]
        [InlineData("DUPONT", "OTHER", false)]
        public void Equals_Object(string lastName1, string lastName2, bool result)
        {
            var f1 = LastName.Create(lastName1);
            var f2 = LastName.Create(lastName2);

            f1.Equals((object)f2).Should().Be(result);
        }

        [Fact]
        public void Equals_Object_Null()
        {
            LastName.Create("Jean").Equals((object)null).Should().BeFalse();
        }

        [Theory]
        [InlineData("DUPONT", "DUPONT", true)]
        [InlineData("DUPONT", "OTHER", false)]
        public void Equals_Operator(string lastName1, string lastName2, bool result)
        {
            var f1 = LastName.Create(lastName1);
            var f2 = LastName.Create(lastName2);

            (f1 == f2).Should().Be(result);
        }

        [Theory]
        [InlineData("DUPONT", "DUPONT", false)]
        [InlineData("DUPONT", "OTHER", true)]
        public void NotEquals_Operator(string lastName1, string lastName2, bool result)
        {
            var f1 = LastName.Create(lastName1);
            var f2 = LastName.Create(lastName2);

            (f1 != f2).Should().Be(result);
        }

        [Fact]
        public void GetHashCode_Test()
        {
            LastName.Create("DUPONT").GetHashCode().Should().Be("DUPONT".GetHashCode(StringComparison.Ordinal));
            LastName.Create("DUPONT").GetHashCode().Should().NotBe("Other".GetHashCode(StringComparison.Ordinal));
        }

        [Fact]
        public void ImplicitOperator_LastNameToString()
        {
            var lastName = LastName.Create("The last name");

            string stringValue = lastName;

            stringValue.Should().Be("THE LAST NAME");
        }

        [Fact]
        public void ImplicitOperator_LastNameToString_WithNullArgument()
        {
            LastName lastName = null;

            var act = () =>
            {
                string _ = lastName;
            };

            act.Should()
                .ThrowExactly<ArgumentNullException>()
                .WithParameterName("lastName");
        }

        [Fact]
        public void ImplicitOperator_StringToLastName()
        {
            LastName lastName = "The last name";

            lastName.ToString().Should().Be("THE LAST NAME");
            lastName.As<IFormattable>().ToString(null, null).Should().Be("THE LAST NAME");
        }

        [Fact]
        public void ImplicitOperator_StringToLastName_WithNullArgument()
        {
            string lastName = null;

            var act = () =>
            {
                LastName _ = lastName;
            };

            act.Should()
                .ThrowExactly<ArgumentNullException>()
                .WithParameterName("lastName");
        }

        [Fact]
        public void CompareTo()
        {
            LastName.Create("Last name A").CompareTo(LastName.Create("Last name B")).Should().BeLessThan(0);
            LastName.Create("Last name B").CompareTo(LastName.Create("Last name A")).Should().BeGreaterThan(0);

            LastName.Create("Last name A").CompareTo(LastName.Create("Last name A")).Should().Be(0);

            LastName.Create("Last name A").CompareTo(LastName.Create("Last nâme A")).Should().BeLessThan(0);
            LastName.Create("Last nâme A").CompareTo(LastName.Create("Last name A")).Should().BeGreaterThan(0);

            LastName.Create("Last name B").CompareTo(LastName.Create("Last nâme A")).Should().BeLessThan(0);
            LastName.Create("Last nâme A").CompareTo(LastName.Create("Last name B")).Should().BeGreaterThan(0);

            LastName.Create("Last name A").CompareTo(null).Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData("Last name A", "Last name B", true)]
        [InlineData("Last name B", "Last name A", false)]
        [InlineData("Last name A", "Last name A", false)]
        [InlineData("Last name A", "Last nâme A", true)]
        [InlineData("Last nâme A", "Last name A", false)]
        [InlineData("Last name B", "Last nâme A", true)]
        [InlineData("Last nâme B", "Last name A", false)]
        [InlineData(null, "Last name", true)]
        [InlineData("Last name", null, false)]
        [InlineData(null, null, false)]
        public void Operator_LessThan(string lastName1, string lastName2, bool result)
        {
            ((lastName1 is not null ? LastName.Create(lastName1) : null) < (lastName2 is not null ? LastName.Create(lastName2) : null)).Should().Be(result);
        }

        [Theory]
        [InlineData("Last name A", "Last name B", true)]
        [InlineData("Last name B", "Last name A", false)]
        [InlineData("Last name A", "Last name A", true)]
        [InlineData("Last name A", "Last nâme A", true)]
        [InlineData("Last nâme A", "Last name A", false)]
        [InlineData("Last name B", "Last nâme A", true)]
        [InlineData("Last nâme B", "Last name A", false)]
        [InlineData(null, "Last name", true)]
        [InlineData("Last name", null, false)]
        [InlineData(null, null, true)]
        public void Operator_LessThanOrEqual(string lastName1, string lastName2, bool result)
        {
            ((lastName1 is not null ? LastName.Create(lastName1) : null) <= (lastName2 is not null ? LastName.Create(lastName2) : null)).Should().Be(result);
        }

        [Theory]
        [InlineData("Last name A", "Last name B", false)]
        [InlineData("Last name B", "Last name A", true)]
        [InlineData("Last name A", "Last name A", false)]
        [InlineData("Last name A", "Last nâme A", false)]
        [InlineData("Last nâme A", "Last name A", true)]
        [InlineData("Last name B", "Last nâme A", false)]
        [InlineData("Last nâme B", "Last name A", true)]
        [InlineData(null, "Last name", false)]
        [InlineData("Last name", null, true)]
        [InlineData(null, null, false)]
        public void Operator_GreaterThan(string lastName1, string lastName2, bool result)
        {
            ((lastName1 is not null ? LastName.Create(lastName1) : null) > (lastName2 is not null ? LastName.Create(lastName2) : null)).Should().Be(result);
        }

        [Theory]
        [InlineData("Last name A", "Last name B", false)]
        [InlineData("Last name B", "Last name A", true)]
        [InlineData("Last name A", "Last name A", true)]
        [InlineData("Last name A", "Last nâme A", false)]
        [InlineData("Last nâme A", "Last name A", true)]
        [InlineData("Last name B", "Last nâme A", false)]
        [InlineData("Last nâme B", "Last name A", true)]
        [InlineData(null, "Last name", false)]
        [InlineData("Last name", null, true)]
        [InlineData(null, null, true)]
        public void Operator_GreaterThanOrEqual(string lastName1, string lastName2, bool result)
        {
            ((lastName1 is not null ? LastName.Create(lastName1) : null) >= (lastName2 is not null ? LastName.Create(lastName2) : null)).Should().Be(result);
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
