//-----------------------------------------------------------------------
// <copyright file="MigrationBuilderExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.EntityFrameworkCore.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations.Operations;
    using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

    /// <summary>
    /// Provides extension methods on the <see cref="MigrationBuilder"/> to rename foreign key and primary key constraints.
    /// </summary>
    public static class MigrationBuilderExtensions
    {
        private const string SqlServerProvider = "Microsoft.EntityFrameworkCore.SqlServer";

        /// <summary>
        /// Renames an existing foreign key constraint.
        /// </summary>
        /// <param name="migrationBuilder">The <see cref="MigrationBuilder"/> used to build the migration.</param>
        /// <param name="name">The current name of the foreign key constraint.</param>
        /// <param name="newName">The new name of the foreign key constraint.</param>
        /// <param name="schema">The schema which contains the foreign key constraint. Defaults to <c>"dbo"</c>.</param>
        /// <returns>The <see cref="OperationBuilder{TOperation}"/> for the <see cref="SqlOperation"/> that was added.</returns>
        /// <exception cref="NotSupportedException">The active provider of the <paramref name="migrationBuilder"/> is not SQL Server.</exception>
        public static OperationBuilder<SqlOperation> RenameForeignKey(this MigrationBuilder migrationBuilder, string name, string newName, string schema = "dbo")
        {
            EnsureSqlServerProvider(migrationBuilder);

            return migrationBuilder.Sql($"EXECUTE sp_rename '{Escape(schema)}.{Escape(name)}', '{Escape(newName)}', 'OBJECT'");
        }

        /// <summary>
        /// Renames an existing primary key constraint.
        /// </summary>
        /// <param name="migrationBuilder">The <see cref="MigrationBuilder"/> used to build the migration.</param>
        /// <param name="name">The current name of the primary key constraint.</param>
        /// <param name="newName">The new name of the primary key constraint.</param>
        /// <param name="schema">The schema which contains the primary key constraint. Defaults to <c>"dbo"</c>.</param>
        /// <returns>The <see cref="OperationBuilder{TOperation}"/> for the <see cref="SqlOperation"/> that was added.</returns>
        /// <exception cref="NotSupportedException">The active provider of the <paramref name="migrationBuilder"/> is not SQL Server.</exception>
#pragma warning disable S4144 // Methods should not have identical implementations
        public static OperationBuilder<SqlOperation> RenamePrimaryKey(this MigrationBuilder migrationBuilder, string name, string newName, string schema = "dbo")
#pragma warning restore S4144 // Methods should not have identical implementations
        {
            EnsureSqlServerProvider(migrationBuilder);

            return migrationBuilder.Sql($"EXECUTE sp_rename '{Escape(schema)}.{Escape(name)}', '{Escape(newName)}', 'OBJECT'");
        }

        private static void EnsureSqlServerProvider(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder.ActiveProvider != SqlServerProvider)
            {
                throw new NotSupportedException($"The '{migrationBuilder.ActiveProvider}' provider is not supported. Only the '{SqlServerProvider}' provider is supported.");
            }
        }

        private static string Escape(string value)
        {
            return value.Replace("'", "''");
        }
    }
}