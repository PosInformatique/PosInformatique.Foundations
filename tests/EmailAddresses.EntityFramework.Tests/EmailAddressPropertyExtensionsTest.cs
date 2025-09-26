//-----------------------------------------------------------------------
// <copyright file="EmailAddressPropertyExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.EntityFrameworkCore.Tests
{
    using PosInformatique.Foundations.EmailAddresses;

    public class EmailAddressPropertyExtensionsTest
    {
        [Fact]
        public void IsEmailAddress()
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("EmailAddress");

            property.GetColumnType().Should().Be("EmailAddress");
            property.IsUnicode().Should().BeFalse();
            property.GetMaxLength().Should().Be(320);
        }

        [Fact]
        public void IsEmailAddress_NullArgument()
        {
            var act = () =>
            {
                EmailAddressPropertyExtensions.IsEmailAddress(null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("property");
        }

        [Theory]
        [InlineData("user@domain.com")]
        [InlineData("\"The user\" <user@domain.com>")]
        public void ConvertFromProvider(string emailAddress)
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("EmailAddress");

            var converter = property.GetValueConverter();

            converter.ConvertFromProvider(emailAddress).Should().Be(EmailAddress.Parse(emailAddress));
        }

        [Fact]
        public void ConvertFromProvider_Null()
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("EmailAddress");

            var converter = property.GetValueConverter();

            converter.ConvertFromProvider(null).Should().BeNull();
        }

        [Theory]
        [InlineData("user@domain.com", "user@domain.com")]
        [InlineData("\"The user\" <user@domain.com>", "user@domain.com")]
        public void ConvertToProvider(string modelEmailAddress, string providerEmailAddress)
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("EmailAddress");

            var converter = property.GetValueConverter();

            converter.ConvertToProvider(EmailAddress.Parse(modelEmailAddress)).Should().Be(providerEmailAddress);
        }

        [Fact]
        public void ConvertToProvider_WithNull()
        {
            var context = new DbContextMock();

            var entity = context.Model.FindEntityType(typeof(EntityMock));
            var property = entity.GetProperty("EmailAddress");

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
                    .Property(e => e.EmailAddress);

                property.IsEmailAddress().Should().BeSameAs(property);
            }
        }

        private class EntityMock
        {
            public int Id { get; set; }

            public EmailAddress EmailAddress { get; set; }
        }
    }
}
