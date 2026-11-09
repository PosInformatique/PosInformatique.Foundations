//-----------------------------------------------------------------------
// <copyright file="DateTimePropertyExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.EntityFrameworkCore.Tests
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class DateTimePropertyExtensionsTest
    {
        [Fact]
        public void AssumeUtc_ArgumentNullException()
        {
            var act = () => DateTimePropertyExtensions.AssumeUtc((PropertyBuilder<DateTime>)null);

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("property");
        }

        [Fact]
        public void AssumeUtc_Nullable_ArgumentNullException()
        {
            var act = () => DateTimePropertyExtensions.AssumeUtc((PropertyBuilder<DateTime?>)null);

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("property");
        }

        [Theory]
        [InlineData(DateTimeKind.Unspecified)]
        [InlineData(DateTimeKind.Local)]
        public void AssumeUtc(DateTimeKind kind)
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("DateTime");

            var converter = property.GetValueConverter();
            converter.ConvertToProvider(new DateTime(2023, 1, 2, 3, 4, 5, 666, kind)).As<DateTime>().Should().Be(new DateTime(2023, 1, 2, 3, 4, 5, 666)).And.BeIn(DateTimeKind.Utc);
            converter.ConvertFromProvider(new DateTime(2023, 1, 2, 3, 4, 5, 666, kind)).As<DateTime>().Should().Be(new DateTime(2023, 1, 2, 3, 4, 5, 666)).And.BeIn(DateTimeKind.Utc);
        }

        [Theory]
        [InlineData(DateTimeKind.Unspecified)]
        [InlineData(DateTimeKind.Local)]
        public void AssumeUtc_Nullable(DateTimeKind kind)
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("NullableDateTime");

            var converter = property.GetValueConverter();
            converter.ConvertToProvider(new DateTime(2023, 1, 2, 3, 4, 5, 666, kind)).As<DateTime>().Should().Be(new DateTime(2023, 1, 2, 3, 4, 5, 666)).And.BeIn(DateTimeKind.Utc);
            converter.ConvertFromProvider(new DateTime(2023, 1, 2, 3, 4, 5, 666, kind)).As<DateTime>().Should().Be(new DateTime(2023, 1, 2, 3, 4, 5, 666)).And.BeIn(DateTimeKind.Utc);
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
                    .Property(e => e.DateTime);

                property.AssumeUtc().Should().BeSameAs(property);

                var nullableProperty = modelBuilder.Entity<EntityMock>()
                    .Property(e => e.NullableDateTime);

                nullableProperty.AssumeUtc().Should().BeSameAs(nullableProperty);
            }
        }

        private class EntityMock
        {
            public int Id { get; set; }

            public DateTime DateTime { get; set; }

            public DateTime? NullableDateTime { get; set; }
        }
    }
}