using Microsoft.EntityFrameworkCore;

namespace Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Model builder extensions for the database context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Uses the snake case naming convention for tables, columns, keys, foreign keys and indexes.
    ///     Table names are also pluralized.
    /// </summary>
    /// <param name="builder">The model builder for the database context.</param>
    public static void UseSnakeCaseNamingConvention(this ModelBuilder builder)
    {
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            // Owned types (e.g. VehicleLocation) share the table of their owner, so they keep its table name
            // and their primary key keeps sharing the owner's "id" column (renaming it breaks table splitting).
            if (!entity.IsOwned())
            {
                var tableName = entity.GetTableName();
                if (!string.IsNullOrEmpty(tableName)) entity.SetTableName(tableName.ToPlural().ToSnakeCase());
            }

            foreach (var property in entity.GetProperties())
            {
                if (entity.IsOwned() && property.IsPrimaryKey()) continue;
                property.SetColumnName(property.GetColumnName().ToSnakeCase());
            }

            foreach (var key in entity.GetKeys())
            {
                var keyName = key.GetName();
                if (!string.IsNullOrEmpty(keyName)) key.SetName(keyName.ToSnakeCase());
            }

            foreach (var foreignKey in entity.GetForeignKeys())
            {
                var foreignKeyName = foreignKey.GetConstraintName();
                if (!string.IsNullOrEmpty(foreignKeyName)) foreignKey.SetConstraintName(foreignKeyName.ToSnakeCase());
            }

            foreach (var index in entity.GetIndexes())
            {
                var indexDatabaseName = index.GetDatabaseName();
                if (!string.IsNullOrEmpty(indexDatabaseName)) index.SetDatabaseName(indexDatabaseName.ToSnakeCase());
            }
        }
    }
}
