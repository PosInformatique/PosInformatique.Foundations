//-----------------------------------------------------------------------
// <copyright file="LastNamePropertyExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.EntityFrameworkCore.Tests
{
    using PosInformatique.Foundations.People;

    public class LastNamePropertyExtensionsTest
    {
        [Fact]
        public void IsLastName()
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("LastName");

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
            var property = entity.GetProperty("LastName");

            var comparer = property.GetValueComparer();

            var expression = (Func<LastName, LastName, bool>)comparer.EqualsExpression.Compile();

            expression("The last name A", "The last name A").Should().BeTrue();
            expression("The last name A", "The last name B").Should().BeFalse();

            expression("The last name A", null).Should().BeFalse();
            expression(null, "The last name A").Should().BeFalse();
            expression(null, null).Should().BeTrue();
        }

        [Fact]
        public void ConvertFromProvider()
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("LastName");

            var converter = property.GetValueConverter();

            converter.ConvertFromProvider("The last name").As<LastName>().ToString().Should().Be("THE LAST NAME");
        }

        [Fact]
        public void ConvertFromProvider_Null()
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("LastName");

            var converter = property.GetValueConverter();

            converter.ConvertFromProvider(null).Should().BeNull();
        }

        [Fact]
        public void ConvertToProvider()
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("LastName");

            var converter = property.GetValueConverter();

            converter.ConvertToProvider(LastName.Create("The last name")).Should().Be("THE LAST NAME");
        }

        [Fact]
        public void ConvertToProvider_WithNull()
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("LastName");

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
                    .Property(e => e.LastName);

                property.IsLastName().Should().BeSameAs(property);
            }
        }

        private class EntityMock
        {
            public int Id { get; set; }

            public LastName LastName { get; set; }
        }
    }
}
