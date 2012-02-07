using FluentMigrator;

namespace osf.web.Data.Migrations
{
    [Migration(1)]
    public class Baseline : Migration
    {
        public override void Up()
        {
            Execute.Script("1\\baseline.sql");
        }

        public override void Down() { }
    }
}