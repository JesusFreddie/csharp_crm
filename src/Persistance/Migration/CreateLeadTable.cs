using FluentMigrator;

namespace Persistance.Migration;

[Migration(1)]
public class CreateLeadTable : FluentMigrator.Migration
{
    public override void Up()
    {
        var table = Create.Table("leads");
        table.WithColumn("id").AsGuid().NotNullable().PrimaryKey();
        table.WithColumn("name").AsString().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("leads");
    }
}