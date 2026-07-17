//-----------------------------------------------------------------------
// <copyright file="MigrationBuilderExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.EntityFrameworkCore.Migrations.Tests
{
    using Microsoft.EntityFrameworkCore.Migrations.Operations;

    public class MigrationBuilderExtensionsTest
    {
        [Fact]
        public void RenameForeignKey_DefaultSchema()
        {
            var migrationBuilder = new MigrationBuilder("Microsoft.EntityFrameworkCore.SqlServer");

            migrationBuilder.RenameForeignKey("FK_Old", "FK_New");

            var operation = (SqlOperation)migrationBuilder.Operations.Single();

            operation.Sql.Should().Be("EXECUTE sp_rename 'dbo.FK_Old', 'FK_New', 'OBJECT'");
        }

        [Fact]
        public void RenameForeignKey_CustomSchema()
        {
            var migrationBuilder = new MigrationBuilder("Microsoft.EntityFrameworkCore.SqlServer");

            migrationBuilder.RenameForeignKey("FK_Old", "FK_New", "custom");

            var operation = (SqlOperation)migrationBuilder.Operations.Single();

            operation.Sql.Should().Be("EXECUTE sp_rename 'custom.FK_Old', 'FK_New', 'OBJECT'");
        }

        [Fact]
        public void RenameForeignKey_EscapesQuotes()
        {
            var migrationBuilder = new MigrationBuilder("Microsoft.EntityFrameworkCore.SqlServer");

            migrationBuilder.RenameForeignKey("FK_O'ld", "FK_N'ew", "cu'stom");

            var operation = (SqlOperation)migrationBuilder.Operations.Single();

            operation.Sql.Should().Be("EXECUTE sp_rename 'cu''stom.FK_O''ld', 'FK_N''ew', 'OBJECT'");
        }

        [Fact]
        public void RenameForeignKey_NotSqlServerProvider_ThrowsNotSupportedException()
        {
            var migrationBuilder = new MigrationBuilder("Npgsql.EntityFrameworkCore.PostgreSQL");

            migrationBuilder.Invoking(m => m.RenameForeignKey("FK_Old", "FK_New"))
                .Should().ThrowExactly<NotSupportedException>()
                .WithMessage("The 'Npgsql.EntityFrameworkCore.PostgreSQL' provider is not supported. Only the 'Microsoft.EntityFrameworkCore.SqlServer' provider is supported.");
        }

        [Fact]
        public void RenamePrimaryKey_DefaultSchema()
        {
            var migrationBuilder = new MigrationBuilder("Microsoft.EntityFrameworkCore.SqlServer");

            migrationBuilder.RenamePrimaryKey("PK_Old", "PK_New");

            var operation = (SqlOperation)migrationBuilder.Operations.Single();

            operation.Sql.Should().Be("EXECUTE sp_rename 'dbo.PK_Old', 'PK_New', 'OBJECT'");
        }

        [Fact]
        public void RenamePrimaryKey_CustomSchema()
        {
            var migrationBuilder = new MigrationBuilder("Microsoft.EntityFrameworkCore.SqlServer");

            migrationBuilder.RenamePrimaryKey("PK_Old", "PK_New", "custom");

            var operation = (SqlOperation)migrationBuilder.Operations.Single();

            operation.Sql.Should().Be("EXECUTE sp_rename 'custom.PK_Old', 'PK_New', 'OBJECT'");
        }

        [Fact]
        public void RenamePrimaryKey_EscapesQuotes()
        {
            var migrationBuilder = new MigrationBuilder("Microsoft.EntityFrameworkCore.SqlServer");

            migrationBuilder.RenamePrimaryKey("PK_O'ld", "PK_N'ew", "cu'stom");

            var operation = (SqlOperation)migrationBuilder.Operations.Single();

            operation.Sql.Should().Be("EXECUTE sp_rename 'cu''stom.PK_O''ld', 'PK_N''ew', 'OBJECT'");
        }

        [Fact]
        public void RenamePrimaryKey_NotSqlServerProvider_ThrowsNotSupportedException()
        {
            var migrationBuilder = new MigrationBuilder("Npgsql.EntityFrameworkCore.PostgreSQL");

            migrationBuilder.Invoking(m => m.RenamePrimaryKey("PK_Old", "PK_New"))
                .Should().ThrowExactly<NotSupportedException>()
                .WithMessage("The 'Npgsql.EntityFrameworkCore.PostgreSQL' provider is not supported. Only the 'Microsoft.EntityFrameworkCore.SqlServer' provider is supported.");
        }
    }
}
