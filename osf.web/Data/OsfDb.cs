using System.Data.Entity;
using System.Linq;
using osf.web.Models;

namespace osf.web.Data
{
    public class OsfDb : DbContext
    {
        public DbSet<LatestEvent> LatestEvents { get; set; }

        public OsfDb()
        {
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<OsfDb>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) { }
    }
}