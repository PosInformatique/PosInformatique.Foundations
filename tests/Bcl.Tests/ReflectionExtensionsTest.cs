//-----------------------------------------------------------------------
// <copyright file="ReflectionExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Bcl.Tests
{
    public class ReflectionExtensionsTest
    {
        [Theory]
        [InlineData(typeof(Task<int>), typeof(Task<>), true)]
        [InlineData(typeof(Task<>), typeof(Task<>), true)]
        [InlineData(typeof(List<int>), typeof(Task<>), false)]
        [InlineData(typeof(List<>), typeof(Task<>), false)]
        [InlineData(typeof(Task), typeof(Task<>), false)]
        public void IsGeneric(Type type, Type genericType, bool expectedResult)
        {
            ReflectionExtensions.IsGeneric(type, genericType).Should().Be(expectedResult);
        }

        [Fact]
        public void IsGeneric_GenericType_NotGeneric()
        {
            var act = () =>
            {
                ReflectionExtensions.IsGeneric(typeof(int), typeof(string));
            };

            act.Should().Throw<ArgumentException>()
                .WithMessage("The 'System.String' must be a generic type definition. (Parameter 'genericType')")
                .WithParameterName("genericType");
        }

        [Fact]
        public void IsGeneric_NullArguments()
        {
            Action nullType = () => ReflectionExtensions.IsGeneric(null, typeof(List<>));
            Action nullGenericType = () => ReflectionExtensions.IsGeneric(typeof(int), null);

            nullType.Should().ThrowExactly<ArgumentNullException>().WithParameterName("type");
            nullGenericType.Should().ThrowExactly<ArgumentNullException>().WithParameterName("genericType");
        }
    }
}