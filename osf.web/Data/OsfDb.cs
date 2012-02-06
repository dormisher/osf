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

        public PagedEventsModel LoadPagedEvents(int page, int take)
        {
            var events = LatestEvents.Skip(page - 1 * take).Take(take).ToList();
            var count = LatestEvents.Count();

            return new PagedEventsModel
                       {
                           LatestEvents = events,
                           Total = count,
                           Page = page
                       };
        }
    }
}