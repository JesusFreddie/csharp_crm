using FluentMigrator;

namespace Persistance.Migration;

[Migration(1)]
public class Lead_Initial : FluentMigrator.Migration
{
    public override void Up()
    {
        var table = Create.Table("leads");
        table.WithColumn("id").AsGuid().NotNullable().PrimaryKey();
        table.WithColumn("name").AsString().NotNullable();
        table.WithColumn("description").AsString().Nullable();
        table.WithColumn("created_at").AsDateTime().NotNullable().WithDefaultValue(DateTime.UtcNow);
        table.WithColumn("updated_at").AsDateTime().Nullable().WithDefaultValue(DateTime.UtcNow);
        table.WithColumn("deleted_at").AsDateTime().Nullable();
    }

    public override void Down()
    {
        Delete.Table("leads");
    }
}