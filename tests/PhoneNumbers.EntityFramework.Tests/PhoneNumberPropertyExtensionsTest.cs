//-----------------------------------------------------------------------
// <copyright file="PhoneNumberPropertyExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.EntityFrameworkCore.Tests
{
    using PosInformatique.Foundations.PhoneNumbers;

    public class PhoneNumberPropertyExtensionsTest
    {
        [Fact]
        public void IsPhoneNumber()
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("PhoneNumber");

            property.GetColumnType().Should().Be("PhoneNumber");
            property.IsUnicode().Should().BeFalse();
            property.GetMaxLength().Should().Be(16);
        }

        [Fact]
        public void IsPhoneNumber_NullArgument()
        {
            var act = () =>
            {
                PhoneNumberPropertyExtensions.IsPhoneNumber<object>(null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("property");
        }

        [Fact]
        public void IsPhoneNumber_NotPhoneNumberProperty()
        {
            var builder = new ModelBuilder();
            var property = builder.Entity<EntityMock>()
                .Property(e => e.Id);

            var act = () =>
            {
                property.IsPhoneNumber();
            };

            act.Should().ThrowExactly<ArgumentException>()
                .WithMessage("The 'IsPhoneNumber()' method must be called on 'PhoneNumber class. (Parameter 'property')")
                .WithParameterName("property");
        }

        [Fact]
        public void ConvertFromProvider()
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("PhoneNumber");

            var converter = property.GetValueConverter();

            converter.ConvertFromProvider("+33111111111").Should().Be(PhoneNumber.Parse("+33111111111"));
        }

        [Fact]
        public void ConvertFromProvider_Null()
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("PhoneNumber");

            var converter = property.GetValueConverter();

            converter.ConvertFromProvider(null).Should().BeNull();
        }

        [Fact]
        public void ConvertToProvider()
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("PhoneNumber");

            var converter = property.GetValueConverter();

            converter.ConvertToProvider(PhoneNumber.Parse("+33111111111")).Should().Be("+33111111111");
        }

        [Fact]
        public void ConvertToProvider_WithNull()
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("PhoneNumber");

            var converter = property.GetValueConverter();

            converter.ConvertToProvider(null).Should().BeNull();
        }

        private class DbContextMock : DbContext
        {
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                base.OnConfiguring(optionsBuilder);

                optionsBuilder.UseSqlServer();
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                var property = modelBuilder.Entity<EntityMock>()
                    .Property(e => e.PhoneNumber);

                property.IsPhoneNumber().Should().BeSameAs(property);
            }
        }

        private class EntityMock
        {
            public int Id { get; set; }

            public PhoneNumber PhoneNumber { get; set; }
        }
    }
}