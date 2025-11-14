//-----------------------------------------------------------------------
// <copyright file="MimeTypePropertyExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.EntityFrameworkCore.Tests
{
    using PosInformatique.Foundations.MediaTypes;

    public class MimeTypePropertyExtensionsTest
    {
        [Fact]
        public void IsMimeType()
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("MimeType");

            property.GetColumnType().Should().Be("MimeType");
            property.IsUnicode().Should().BeFalse();
            property.GetMaxLength().Should().Be(128);

            var converter = property.GetValueConverter();

            converter.ConvertFromProvider("application/pdf").Should().Be(MimeType.Parse("application/pdf"));
            converter.ConvertToProvider(null).Should().Be(null);
        }

        [Fact]
        public void IsMimeType_NullArgument()
        {
            var act = () =>
            {
                MimeTypePropertyExtensions.IsMimeType<object>(null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("property");
        }

        [Fact]
        public void IsMimeType_NotMimeTypeProperty()
        {
            var builder = new ModelBuilder();
            var property = builder.Entity<EntityMock>()
                .Property(e => e.Id);

            var act = () =>
            {
                property.IsMimeType();
            };

            act.Should().ThrowExactly<ArgumentException>()
                .WithMessage("The 'IsMimeType()' method must be called on 'MimeType class. (Parameter 'property')")
                .WithParameterName("property");
        }

        [Fact]
        public void ConvertFromProvider()
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("MimeType");

            var converter = property.GetValueConverter();

            converter.ConvertFromProvider("application/pdf").Should().Be(MimeTypes.Application.Pdf);
        }

        [Fact]
        public void ConvertFromProvider_Null()
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("MimeType");

            var converter = property.GetValueConverter();

            converter.ConvertFromProvider(null).Should().BeNull();
        }

        [Fact]
        public void ConvertToProvider()
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("MimeType");

            var converter = property.GetValueConverter();

            converter.ConvertToProvider(MimeTypes.Application.Pdf).Should().Be("application/pdf");
        }

        [Fact]
        public void ConvertToProvider_WithNull()
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("MimeType");

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
                    .Property(e => e.MimeType);

                property.IsMimeType().Should().BeSameAs(property);
            }
        }

        private class EntityMock
        {
            public int Id { get; set; }

            public MimeType MimeType { get; set; }
        }
    }
}