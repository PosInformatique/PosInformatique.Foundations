//-----------------------------------------------------------------------
// <copyright file="EnumToBooleanConverterTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.EntityFrameworkCore.Tests
{
    public class EnumToBooleanConverterTest
    {
        public enum BooleanEnumTest
        {
            False = 0,
            True = 1,
        }

        public enum MissingZeroEnumTest
        {
            One = 1,
            Two = 2,
        }

        public enum MissingOneEnumTest
        {
            Zero = 0,
            Two = 2,
        }

        public enum TooManyValuesEnumTest
        {
            Zero = 0,
            One = 1,
            Two = 2,
        }

        public enum SingleValueEnumTest
        {
            Zero = 0,
        }

        public enum DuplicatedValuesEnumTest
        {
            Zero = 0,
            AlsoZero = 0,
            One = 1,
        }

        public enum EmptyEnumTest
        {
        }

        [Fact]
        public void Instance()
        {
            EnumToBooleanConverter<BooleanEnumTest>.Instance.Should().NotBeNull();
            EnumToBooleanConverter<BooleanEnumTest>.Instance.Should().BeSameAs(EnumToBooleanConverter<BooleanEnumTest>.Instance);
        }

        [Theory]
        [InlineData(BooleanEnumTest.False, false)]
        [InlineData(BooleanEnumTest.True, true)]
        public void Convert(BooleanEnumTest enumValue, bool boolValue)
        {
            var converter = EnumToBooleanConverter<BooleanEnumTest>.Instance;

            converter.ConvertToProvider(enumValue).Should().Be(boolValue);
            converter.ConvertFromProvider(boolValue).Should().Be(enumValue);
        }

        [Fact]
        public void Instance_WithDuplicatedValues_DoesNotThrow()
        {
            var act = () => _ = EnumToBooleanConverter<DuplicatedValuesEnumTest>.Instance;

            act.Should().NotThrow();
        }

        [Fact]
        public void Instance_WithEmptyEnum_ThrowsArgumentException()
        {
            var act = () => _ = EnumToBooleanConverter<EmptyEnumTest>.Instance;

            act.Should().Throw<TypeInitializationException>()
                .WithInnerException<ArgumentException>()
                .WithMessage($"The enumeration '{typeof(EmptyEnumTest).FullName}' must only contain two distinct values that can be represented as 0 or 1. (Parameter '{nameof(EmptyEnumTest)}')")
                .WithParameterName("EmptyEnumTest");
        }

        [Fact]
        public void Instance_WithSingleValue_ThrowsArgumentException()
        {
            var act = () => _ = EnumToBooleanConverter<SingleValueEnumTest>.Instance;

            act.Should().Throw<TypeInitializationException>()
                .WithInnerException<ArgumentException>()
                .WithMessage($"The enumeration '{typeof(SingleValueEnumTest).FullName}' must only contain two distinct values that can be represented as 0 or 1. (Parameter '{nameof(SingleValueEnumTest)}')")
                .WithParameterName("SingleValueEnumTest");
        }

        [Fact]
        public void Instance_WithTooManyValues_ThrowsArgumentException()
        {
            var act = () => _ = EnumToBooleanConverter<TooManyValuesEnumTest>.Instance;

            act.Should().Throw<TypeInitializationException>()
                .WithInnerException<ArgumentException>()
                .WithMessage($"The enumeration '{typeof(TooManyValuesEnumTest).FullName}' must only contain two distinct values that can be represented as 0 or 1. (Parameter '{nameof(TooManyValuesEnumTest)}')")
                .WithParameterName("TooManyValuesEnumTest");
        }

        [Fact]
        public void Instance_WithMissingZeroValue_ThrowsArgumentException()
        {
            var act = () => _ = EnumToBooleanConverter<MissingZeroEnumTest>.Instance;

            act.Should().Throw<TypeInitializationException>()
                .WithInnerException<ArgumentException>()
                .WithMessage($"The enumeration '{typeof(MissingZeroEnumTest).FullName}' must contain a value that can be represented as 0 (False). (Parameter '{nameof(MissingZeroEnumTest)}')")
                .WithParameterName("MissingZeroEnumTest");
        }

        [Fact]
        public void Instance_WithMissingOneValue_ThrowsArgumentException()
        {
            var act = () => _ = EnumToBooleanConverter<MissingOneEnumTest>.Instance;

            act.Should().Throw<TypeInitializationException>()
                .WithInnerException<ArgumentException>()
                .WithMessage($"The enumeration '{typeof(MissingOneEnumTest).FullName}' must contain a value that can be represented as 1 (True). (Parameter '{nameof(MissingOneEnumTest)}')")
                .WithParameterName("MissingOneEnumTest");
        }
    }
}