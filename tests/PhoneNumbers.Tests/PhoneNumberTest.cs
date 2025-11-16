//-----------------------------------------------------------------------
// <copyright file="PhoneNumberTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.PhoneNumbers.Tests
{
    public class PhoneNumberTest
    {
        [Theory]
        [MemberData(nameof(PhoneNumberTestData.ValidPhoneNumbers), MemberType = typeof(PhoneNumberTestData))]
        public void Parse(string phoneNumber, string expectedPhoneNumber)
        {
            var number = PhoneNumber.Parse(phoneNumber);

            number.ToString().Should().Be(expectedPhoneNumber);
            number.As<IFormattable>().ToString(null, null).Should().Be(expectedPhoneNumber);
        }

        [Theory]
        [MemberData(nameof(PhoneNumberTestData.ValidPhoneNumbers), MemberType = typeof(PhoneNumberTestData))]
        [InlineData("0102030405", "+33102030405")]
        public void Parse_WithDefaultRegion(string phoneNumber, string expectedPhoneNumber)
        {
            var number = PhoneNumber.Parse(phoneNumber, "FR");

            number.ToString().Should().Be(expectedPhoneNumber);
            number.As<IFormattable>().ToString(null, null).Should().Be(expectedPhoneNumber);
        }

        [Theory]
        [MemberData(nameof(PhoneNumberTestData.InvalidPhoneNumbers), MemberType = typeof(PhoneNumberTestData))]
        public void Parse_InvalidPhoneNumber(string invalidPhoneNumber)
        {
            new Action(() => PhoneNumber.Parse(invalidPhoneNumber))
                .Should().ThrowExactly<FormatException>()
                .WithMessage($"The specified phone number '{invalidPhoneNumber}' is not a valid E164 phone number.");
        }

        [Theory]
        [InlineData("invalid phone number")]
        public void Parse_InvalidPhoneNumberWithInnerException(string invalidPhoneNumber)
        {
            new Action(() => PhoneNumber.Parse(invalidPhoneNumber))
                .Should().ThrowExactly<FormatException>()
                .WithMessage($"The specified phone number '{invalidPhoneNumber}' is not a valid E164 phone number.")
                .WithInnerExceptionExactly<global::PhoneNumbers.NumberParseException>()
                .WithMessage("The string supplied did not seem to be a phone number.");
        }

        [Fact]
        public void Parse_WithNullArgument()
        {
            var act = () =>
            {
                PhoneNumber.Parse(null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("s");
        }

        [Theory]
        [MemberData(nameof(PhoneNumberTestData.ValidPhoneNumbers), MemberType = typeof(PhoneNumberTestData))]
        public void Parse_IParsable(string phoneNumber, string expectedPhoneNumber)
        {
            var number = CallParse<PhoneNumber>(phoneNumber, null);

            number.ToString().Should().Be(expectedPhoneNumber);
            number.As<IFormattable>().ToString(null, null).Should().Be(expectedPhoneNumber);
        }

        [Theory]
        [MemberData(nameof(PhoneNumberTestData.InvalidPhoneNumbers), MemberType = typeof(PhoneNumberTestData))]
        public void Parse_IParsable_InvalidPhoneNumber(string invalidPhoneNumber)
        {
            new Action(() => CallParse<PhoneNumber>(invalidPhoneNumber, null))
                .Should().ThrowExactly<FormatException>()
                .WithMessage($"The specified phone number '{invalidPhoneNumber}' is not a valid E164 phone number.");
        }

        [Theory]
        [InlineData("invalid phone number")]
        public void Parse_IParsable_InvalidPhoneNumberWithInnerException(string invalidPhoneNumber)
        {
            new Action(() => CallParse<PhoneNumber>(invalidPhoneNumber, null))
                .Should().ThrowExactly<FormatException>()
                .WithMessage($"The specified phone number '{invalidPhoneNumber}' is not a valid E164 phone number.")
                .WithInnerExceptionExactly<global::PhoneNumbers.NumberParseException>()
                .WithMessage("The string supplied did not seem to be a phone number.");
        }

        [Fact]
        public void Parse_IParsable_WithNullArgument()
        {
            var act = () =>
            {
                CallParse<PhoneNumber>(null, default);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("s");
        }

        [Theory]
        [MemberData(nameof(PhoneNumberTestData.ValidPhoneNumbers), MemberType = typeof(PhoneNumberTestData))]
        public void TryParse(string phoneNumber, string expectedValue)
        {
            var result = PhoneNumber.TryParse(phoneNumber, out var number);

            result.Should().BeTrue();
            number.ToString().Should().Be(expectedValue);
            number.As<IFormattable>().ToString(null, null).Should().Be(expectedValue);
        }

        [Theory]
        [MemberData(nameof(PhoneNumberTestData.ValidPhoneNumbers), MemberType = typeof(PhoneNumberTestData))]
        [InlineData("0102030405", "+33102030405")]
        public void TryParse_WithDefaultRegion(string phoneNumber, string expectedPhoneNumber)
        {
            var result = PhoneNumber.TryParse(phoneNumber, out var number, "FR");

            result.Should().BeTrue();
            number.ToString().Should().Be(expectedPhoneNumber);
            number.As<IFormattable>().ToString(null, null).Should().Be(expectedPhoneNumber);
        }

        [Theory]
        [MemberData(nameof(PhoneNumberTestData.InvalidPhoneNumbers), MemberType = typeof(PhoneNumberTestData))]
        public void TryParse_InvalidPhoneNumber(string invalidPhoneNumber)
        {
            var result = PhoneNumber.TryParse(invalidPhoneNumber, out var number);

            result.Should().BeFalse();
            number.Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(PhoneNumberTestData.ValidPhoneNumbers), MemberType = typeof(PhoneNumberTestData))]
        public void TryParse_IParsable(string phoneNumber, string expectedValue)
        {
            var result = CallTryParse<PhoneNumber>(phoneNumber, null, out var number);

            result.Should().BeTrue();
            number.ToString().Should().Be(expectedValue);
            number.As<IFormattable>().ToString(null, null).Should().Be(expectedValue);
        }

        [Theory]
        [MemberData(nameof(PhoneNumberTestData.InvalidPhoneNumbers), MemberType = typeof(PhoneNumberTestData))]
        public void TryParse_IParsable_InvalidPhoneNumber(string invalidPhoneNumber)
        {
            var result = CallTryParse<PhoneNumber>(invalidPhoneNumber, null, out var number);

            result.Should().BeFalse();
            number.Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(PhoneNumberTestData.ValidPhoneNumbers), MemberType = typeof(PhoneNumberTestData))]
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
        public void IsValid_Valid(string phoneNumber, string _)
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
#pragma warning restore IDE0079 // Remove unnecessary suppression
        {
            PhoneNumber.IsValid(phoneNumber).Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(PhoneNumberTestData.InvalidPhoneNumbers), MemberType = typeof(PhoneNumberTestData))]
        public void IsValid_Invalid(string invalidPhoneNumber)
        {
            PhoneNumber.IsValid(invalidPhoneNumber).Should().BeFalse();
        }

        [Fact]
        public void Equals_WithPhoneNumber()
        {
            var number1 = PhoneNumber.Parse("+33111111111");
            var number2 = PhoneNumber.Parse("+33333333333");
            var number3 = PhoneNumber.Parse("+33111111111");

            number1.Equals(number2).Should().BeFalse();
            number1.Equals(null).Should().BeFalse();
            number1.Equals(number3).Should().BeTrue();
        }

        [Fact]
        public void Equals_WithObject()
        {
            var number1 = PhoneNumber.Parse("+33111111111");
            var number2 = PhoneNumber.Parse("+33333333333");
            var number3 = PhoneNumber.Parse("+33111111111");

            number1.Equals((object)number2).Should().BeFalse();
            number1.Equals((object)number3).Should().BeTrue();

            object stringValue = "The string";
            number1.Equals(stringValue).Should().BeFalse();
        }

        [Fact]
        public void GetHashCode_Test()
        {
            var number = PhoneNumber.Parse("+33111111111");
            var wrappedTypeInstance = global::PhoneNumbers.PhoneNumberUtil.GetInstance().Parse("+33111111111", "FR");
            number.GetHashCode().Should().Be(wrappedTypeInstance.GetHashCode());
        }

        [Fact]
        public void Operator_Equals()
        {
            var number1 = PhoneNumber.Parse("+33111111111");
            var number2 = PhoneNumber.Parse("+33333333333");
            var number3 = PhoneNumber.Parse("+33111111111");

            (number1 == number2).Should().BeFalse();
            (number1 == number3).Should().BeTrue();
        }

        [Fact]
        public void Operator_NotEquals()
        {
            var number1 = PhoneNumber.Parse("+33111111111");
            var number2 = PhoneNumber.Parse("+33333333333");
            var number3 = PhoneNumber.Parse("+33111111111");

            (number1 != number2).Should().BeTrue();
            (number1 != number3).Should().BeFalse();
        }

        [Fact]
        public void ToString_ShouldReturnValue()
        {
            var number = PhoneNumber.Parse("+33111111111");

            number.ToString().Should().Be("+33111111111");
            number.As<IFormattable>().ToString(null, null).Should().Be("+33111111111");
        }

        [Fact]
        public void ToInternationalString()
        {
            var number = PhoneNumber.Parse("+33102030405");

            number.ToInternationalString().Should().Be("+33 1 02 03 04 05");
        }

        [Fact]
        public void ToNationalString()
        {
            var number = PhoneNumber.Parse("+33102030405");

            number.ToNationalString().Should().Be("01 02 03 04 05");
        }

        [Fact]
        public void ImplicitOperator_PhoneNumberToString()
        {
            var phoneNumber = PhoneNumber.Parse("+ 33 1 22 33 44 55");

            string stringValue = phoneNumber;

            stringValue.Should().Be("+33122334455");
        }

        [Fact]
        public void ImplicitOperator_PhoneNumberToString_WithNullArgument()
        {
            var act = () =>
            {
                string _ = (PhoneNumber)null;
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("phoneNumber");
        }

        [Fact]
        public void ImplicitOperator_StringToPhoneNumber()
        {
            PhoneNumber phoneNumber = "+ 33 1 22 33 44 55";

            phoneNumber.ToString().Should().Be("+33122334455");
            phoneNumber.As<IFormattable>().ToString(null, null).Should().Be("+33122334455");
        }

        [Fact]
        public void ImplicitOperator_StringToPhoneNumber_WithNullArgument()
        {
            var act = () =>
            {
                PhoneNumber _ = (string)null;
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("phoneNumber");
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