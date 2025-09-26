//-----------------------------------------------------------------------
// <copyright file="EmailAddressTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.EmailAddresses.Tests
{
    public class EmailAddressTest
    {
        [Theory]
        [InlineData(@"""Test"" <test1@test.com>", "test1@test.com", "test1", "test.com")]
        [InlineData("test1@test.com", "test1@test.com", "test1", "test.com")]
        [InlineData("TEST1@TEST.COM", "test1@test.com", "test1", "test.com")]
        public void Parse(string emailAddress, string expectedEmailAddress, string userName, string domain)
        {
            var address = EmailAddress.Parse(emailAddress);

            address.ToString().Should().Be(expectedEmailAddress);
            address.As<IFormattable>().ToString(null, null).Should().Be(expectedEmailAddress);
            address.UserName.Should().Be(userName);
            address.Domain.Should().Be(domain);
        }

        [Fact]
        public void Parse_WithNullArgument()
        {
            var act = () =>
            {
                EmailAddress.Parse(null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("s");
        }

        [Theory]
        [MemberData(nameof(EmailAddressTestData.InvalidEmailAddresses), MemberType = typeof(EmailAddressTestData))]
        public void Parse_InvalidEmailAddress(string invalidEmailAdddress)
        {
            var act = () => EmailAddress.Parse(invalidEmailAdddress);

            act.Should().ThrowExactly<FormatException>()
                .WithMessage($"'{invalidEmailAdddress}' is not a valid email address.");
        }

        [Theory]
        [InlineData(@"""Test"" <test1@test.com>", "test1@test.com", "test1", "test.com")]
        [InlineData("test1@test.com", "test1@test.com", "test1", "test.com")]
        [InlineData("TEST1@TEST.COM", "test1@test.com", "test1", "test.com")]
        public void Parse_WithFormatProvider(string emailAddress, string expectedEmailAddress, string userName, string domain)
        {
            var formatProvider = Mock.Of<IFormatProvider>(MockBehavior.Strict);

            var address = EmailAddress.Parse(emailAddress, formatProvider);

            address.ToString().Should().Be(expectedEmailAddress);
            address.As<IFormattable>().ToString(null, null).Should().Be(expectedEmailAddress);
            address.UserName.Should().Be(userName);
            address.Domain.Should().Be(domain);
        }

        [Fact]
        public void Parse_WithFormatProvider_WithNullArgument()
        {
            var act = () =>
            {
                EmailAddress.Parse(null, default);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("s");
        }

        [Theory]
        [MemberData(nameof(EmailAddressTestData.InvalidEmailAddresses), MemberType = typeof(EmailAddressTestData))]
        public void Parse_WithFormatProvider_InvalidEmailAddress(string invalidEmailAdddress)
        {
            var formatProvider = Mock.Of<IFormatProvider>(MockBehavior.Strict);

            var act = () => EmailAddress.Parse(invalidEmailAdddress, formatProvider);

            act.Should().ThrowExactly<FormatException>()
                .WithMessage($"'{invalidEmailAdddress}' is not a valid email address.");
        }

        [Theory]
        [InlineData(@"""Test"" <test1@test.com>", "test1@test.com", "test1", "test.com")]
        [InlineData("test1@test.com", "test1@test.com", "test1", "test.com")]
        [InlineData("TEST1@TEST.COM", "test1@test.com", "test1", "test.com")]
        public void TryParse(string emailAddress, string expectedEmailAddress, string userName, string domain)
        {
            var result = EmailAddress.TryParse(emailAddress, out var address);

            result.Should().BeTrue();

            address.ToString().Should().Be(expectedEmailAddress);
            address.As<IFormattable>().ToString(null, null).Should().Be(expectedEmailAddress);
            address.UserName.Should().Be(userName);
            address.Domain.Should().Be(domain);
        }

        [Theory]
        [MemberData(nameof(EmailAddressTestData.InvalidEmailAddresses), MemberType = typeof(EmailAddressTestData))]
        public void TryParse_InvalidEmailAddress(string invalidEmailAdddress)
        {
            var result = EmailAddress.TryParse(invalidEmailAdddress, out var address);

            result.Should().BeFalse();
            address.Should().BeNull();
        }

        [Theory]
        [InlineData(@"""Test"" <test1@test.com>", "test1@test.com", "test1", "test.com")]
        [InlineData("test1@test.com", "test1@test.com", "test1", "test.com")]
        [InlineData("TEST1@TEST.COM", "test1@test.com", "test1", "test.com")]
        public void TryParse_WithFormatProvider(string emailAddress, string expectedEmailAddress, string userName, string domain)
        {
            var formatProvider = Mock.Of<IFormatProvider>(MockBehavior.Strict);

            var result = EmailAddress.TryParse(emailAddress, formatProvider, out var address);

            result.Should().BeTrue();

            address.ToString().Should().Be(expectedEmailAddress);
            address.As<IFormattable>().ToString(null, null).Should().Be(expectedEmailAddress);
            address.UserName.Should().Be(userName);
            address.Domain.Should().Be(domain);
        }

        [Theory]
        [MemberData(nameof(EmailAddressTestData.InvalidEmailAddresses), MemberType = typeof(EmailAddressTestData))]
        public void TryParse_WithFormatProvider_InvalidEmailAddress(string invalidEmailAdddress)
        {
            var formatProvider = Mock.Of<IFormatProvider>(MockBehavior.Strict);

            var result = EmailAddress.TryParse(invalidEmailAdddress, formatProvider, out var address);

            result.Should().BeFalse();
            address.Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(EmailAddressTestData.ValidEmailAddresses), MemberType = typeof(EmailAddressTestData))]
        public void IsValid_Valid(string emailAddress)
        {
            EmailAddress.IsValid(emailAddress).Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(EmailAddressTestData.InvalidEmailAddresses), MemberType = typeof(EmailAddressTestData))]
        public void IsValid_Invalid(string invalidEmailAdddress)
        {
            EmailAddress.IsValid(invalidEmailAdddress).Should().BeFalse();
        }

        [Theory]
        [InlineData(@"""Test"" <test1@test.com>", @"""Test"" <test1@test.com>", true)]
        [InlineData(@"""Test"" <test1@test.com>", @"""Test"" <test2@test.com>", false)]
        [InlineData(@"""Test"" <test1@test.com>", @"""Test"" <TEST1@test.com>", true)]
        [InlineData(@"""Test"" <test1@test.com>", @"""Test"" <test1@TEST.com>", true)]
        [InlineData(@"""Test"" <test1@test.com>", @"""Test2"" <test1@test.com>", true)]
        [InlineData(@"""Test"" <test1@test.com>", null, false)]
        public void Equals_WithEmailAddress(string emailAddress1String, string emailAddress2String, bool expectedResult)
        {
            var emailAddress1 = EmailAddress.Parse(emailAddress1String);
            var emailAddress2 = emailAddress2String is not null ? EmailAddress.Parse(emailAddress2String) : null;

            emailAddress1.Equals(emailAddress2).Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(@"""Test"" <test1@test.com>", @"""Test"" <test1@test.com>", true)]
        [InlineData(@"""Test"" <test1@test.com>", @"""Test"" <test2@test.com>", false)]
        [InlineData(@"""Test"" <test1@test.com>", @"""Test"" <TEST1@test.com>", true)]
        [InlineData(@"""Test"" <test1@test.com>", @"""Test"" <test1@TEST.com>", true)]
        [InlineData(@"""Test"" <test1@test.com>", @"""Test2"" <test1@test.com>", true)]
        [InlineData(@"""Test"" <test1@test.com>", null, false)]
        public void Equals_WithObject(string emailAddress1String, string emailAddress2String, bool expectedResult)
        {
            var emailAddress1 = EmailAddress.Parse(emailAddress1String);
            var emailAddress2 = emailAddress2String is not null ? EmailAddress.Parse(emailAddress2String) : null;

            emailAddress1.Equals((object)emailAddress2).Should().Be(expectedResult);
        }

        [Fact]
        public void GetHashCode_Test()
        {
            var address = EmailAddress.Parse(@"""Test"" <test1@test.com>");

            address.GetHashCode().Should().Be("test1@test.com".GetHashCode(StringComparison.OrdinalIgnoreCase));
        }

        [Theory]
        [InlineData(@"""Test"" <test1@test.com>", @"""Test"" <test1@test.com>", true)]
        [InlineData(@"""Test"" <test1@test.com>", @"""Test"" <test2@test.com>", false)]
        [InlineData(@"""Test"" <test1@test.com>", @"""Test"" <TEST1@test.com>", true)]
        [InlineData(@"""Test"" <test1@test.com>", @"""Test"" <test1@TEST.com>", true)]
        [InlineData(@"""Test"" <test1@test.com>", @"""Test2"" <test1@test.com>", true)]
        [InlineData(@"""Test"" <test1@test.com>", null, false)]
        public void Operator_Equals(string emailAddress1String, string emailAddress2String, bool expectedResult)
        {
            var emailAddress1 = EmailAddress.Parse(emailAddress1String);
            var emailAddress2 = emailAddress2String is not null ? EmailAddress.Parse(emailAddress2String) : null;

            (emailAddress1 == emailAddress2).Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(@"""Test"" <test1@test.com>", @"""Test"" <test1@test.com>", false)]
        [InlineData(@"""Test"" <test1@test.com>", @"""Test"" <test2@test.com>", true)]
        [InlineData(@"""Test"" <test1@test.com>", @"""Test"" <TEST1@test.com>", false)]
        [InlineData(@"""Test"" <test1@test.com>", @"""Test"" <test1@TEST.com>", false)]
        [InlineData(@"""Test"" <test1@test.com>", @"""Test2"" <test1@test.com>", false)]
        [InlineData(@"""Test"" <test1@test.com>", null, true)]
        public void Operator_NotEquals(string emailAddress1String, string emailAddress2String, bool expectedResult)
        {
            var emailAddress1 = EmailAddress.Parse(emailAddress1String);
            var emailAddress2 = emailAddress2String is not null ? EmailAddress.Parse(emailAddress2String) : null;

            (emailAddress1 != emailAddress2).Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(@"""Test""<test@test.com>", @"test@test.com")]
        [InlineData(@"test@test.com", "test@test.com")]
        public void ToString_ShouldReturnValue(string emailAddress, string expectedValue)
        {
            var address = EmailAddress.Parse(emailAddress);

            address.ToString().Should().Be(expectedValue);
            address.As<IFormattable>().ToString(null, null).Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(@"""Test""<test@test.com>")]
        [InlineData(@"test@test.com")]
        public void Domain(string emailAddress)
        {
            var address = EmailAddress.Parse(emailAddress);

            address.Domain.Should().Be("test.com");
        }

        [Theory]
        [InlineData(@"""Test""<test@test.com>")]
        [InlineData(@"test@test.com")]
        public void Username(string emailAddress)
        {
            var address = EmailAddress.Parse(emailAddress);

            address.UserName.Should().Be("test");
        }

        [Theory]
        [InlineData(@"""Test""<test1@test.com>", "test1@test.com")]
        [InlineData(@"test1@test.com", "test1@test.com")]
        public void Operator_EmailAddressToString(string emailAddressString, string expectedEmailAddress)
        {
            var emailAddress = EmailAddress.Parse(emailAddressString);

            string toStringValue = emailAddress;

            toStringValue.Should().Be(expectedEmailAddress);
        }

        [Fact]
        public void Operator_EmailAddressToString_WithNullArgument()
        {
            var act = () =>
            {
                string toStringValue = (EmailAddress)null;
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("emailAddress");
        }

        [Theory]
        [InlineData(@"""Test""<test1@test.com>", "test1@test.com", "test1", "test.com")]
        [InlineData(@"test1@test.com", "test1@test.com", "test1", "test.com")]
        public void Operator_StringToEmailAddress(string emailAddressString, string expectedEmailAddress, string expectedUserName, string expectedDomain)
        {
            EmailAddress emailAddress = emailAddressString;

            emailAddress.ToString().Should().Be(expectedEmailAddress);
            emailAddress.As<IFormattable>().ToString(null, null).Should().Be(expectedEmailAddress);
            emailAddress.Domain.Should().Be(expectedDomain);
            emailAddress.UserName.Should().Be(expectedUserName);
        }

        [Fact]
        public void Operator_StringToEmailAddress_WithNullArgument()
        {
            var act = () =>
            {
                EmailAddress toStringValue = (string)null;
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("emailAddress");
        }

        [Fact]
        public void CompareTo()
        {
            EmailAddress.Parse("test1@test.com").CompareTo(EmailAddress.Parse("test2@test.com")).Should().BeLessThan(0);
            EmailAddress.Parse("test2@test.com").CompareTo(EmailAddress.Parse("test1@test.com")).Should().BeGreaterThan(0);

            EmailAddress.Parse("test1@test.com").CompareTo(EmailAddress.Parse("test1@test.com")).Should().Be(0);

            EmailAddress.Parse("test1@test.com").CompareTo(null).Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData("test1@test.com", "test2@test.com", true)]
        [InlineData("test2@test.com", "test1@test.com", false)]
        [InlineData("test1@test.com", "test1@test.com", false)]
        [InlineData(null, "test1@test.com", true)]
        [InlineData("test1@test.com", null, false)]
        [InlineData(null, null, false)]
        public void Operator_LessThan(string emailAddress1, string emailAddress2, bool expectedResult)
        {
            ((emailAddress1 is not null ? EmailAddress.Parse(emailAddress1) : null) < (emailAddress2 is not null ? EmailAddress.Parse(emailAddress2) : null)).Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("test1@test.com", "test2@test.com", true)]
        [InlineData("test2@test.com", "test1@test.com", false)]
        [InlineData("test1@test.com", "test1@test.com", true)]
        [InlineData(null, "test1@test.com", true)]
        [InlineData("test1@test.com", null, false)]
        [InlineData(null, null, true)]
        public void Operator_LessThanOrEqual(string emailAddress1, string emailAddress2, bool expectedResult)
        {
            ((emailAddress1 is not null ? EmailAddress.Parse(emailAddress1) : null) <= (emailAddress2 is not null ? EmailAddress.Parse(emailAddress2) : null)).Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("test1@test.com", "test2@test.com", false)]
        [InlineData("test2@test.com", "test1@test.com", true)]
        [InlineData("test1@test.com", "test1@test.com", false)]
        [InlineData(null, "test1@test.com", false)]
        [InlineData("test1@test.com", null, true)]
        [InlineData(null, null, false)]
        public void Operator_GreaterThan(string emailAddress1, string emailAddress2, bool expectedResult)
        {
            ((emailAddress1 is not null ? EmailAddress.Parse(emailAddress1) : null) > (emailAddress2 is not null ? EmailAddress.Parse(emailAddress2) : null)).Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("test1@test.com", "test2@test.com", false)]
        [InlineData("test2@test.com", "test1@test.com", true)]
        [InlineData("test1@test.com", "test1@test.com", true)]
        [InlineData(null, "test1@test.com", false)]
        [InlineData("test1@test.com", null, true)]
        [InlineData(null, null, true)]
        public void Operator_GreaterThanOrEqual(string emailAddress1, string emailAddress2, bool expectedResult)
        {
            ((emailAddress1 is not null ? EmailAddress.Parse(emailAddress1) : null) >= (emailAddress2 is not null ? EmailAddress.Parse(emailAddress2) : null)).Should().Be(expectedResult);
        }
    }
}
