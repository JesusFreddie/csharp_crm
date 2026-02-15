using FluentMigrator.Runner.VersionTableInfo;

namespace Migrator;

public class CustomVersionTable : IVersionTableMetaData
{
    public bool OwnsSchema => true;
    public string SchemaName => null;
    public string TableName => "version_info";
    public string ColumnName => "version";
    public string DescriptionColumnName => "description";
    public string UniqueIndexName => "uc_version";
    public string AppliedOnColumnName => "applied_on";
    public bool CreateWithPrimaryKey => false;
}