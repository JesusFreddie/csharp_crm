using FluentMigrator;

namespace Persistance.Migration;

[Migration(2)]
public class Lead_AddIsArchived : FluentMigrator.Migration
{
    public override void Up()
    {
        Alter.Table("leads")
            .AddColumn("is_archived").AsBoolean().NotNullable().WithDefaultValue(false);

    }

    public override void Down()
    {
        Delete.Column("is_archived").FromTable("leads");
    }
}