//-----------------------------------------------------------------------
// <copyright file="FirstNamePropertyExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.EntityFrameworkCore.Tests
{
    using PosInformatique.Foundations.People;

    public class FirstNamePropertyExtensionsTest
    {
        [Fact]
        public void IsFirstName()
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("FirstName");

            property.GetColumnType().Should().Be("nvarchar(50)");
            property.IsUnicode().Should().BeTrue();
            property.IsFixedLength().Should().BeFalse();
            property.GetMaxLength().Should().Be(50);
        }

        [Fact]
        public void Comparer()
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("FirstName");

            var comparer = property.GetValueComparer();

            var expression = (Func<FirstName, FirstName, bool>)comparer.EqualsExpression.Compile();

            expression("The first name A", "The first name A").Should().BeTrue();
            expression("The first name A", "The first name B").Should().BeFalse();

            expression("The first name A", null).Should().BeFalse();
            expression(null, "The first name A").Should().BeFalse();
            expression(null, null).Should().BeTrue();
        }

        [Fact]
        public void ConvertFromProvider()
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("FirstName");

            var converter = property.GetValueConverter();

            converter.ConvertFromProvider("The first name").As<FirstName>().ToString().Should().Be("The First Name");
        }

        [Fact]
        public void ConvertFromProvider_Null()
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("FirstName");

            var converter = property.GetValueConverter();

            converter.ConvertFromProvider(null).Should().BeNull();
        }

        [Fact]
        public void ConvertToProvider()
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("FirstName");

            var converter = property.GetValueConverter();

            converter.ConvertToProvider(FirstName.Create("The first name")).Should().Be("The First Name");
        }

        [Fact]
        public void ConvertToProvider_WithNull()
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("FirstName");

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
                    .Property(e => e.FirstName);

                property.IsFirstName().Should().BeSameAs(property);
            }
        }

        private class EntityMock
        {
            public int Id { get; set; }

            public FirstName FirstName { get; set; }
        }
    }
}
