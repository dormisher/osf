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
            var events = LatestEvents.OrderByDescending(e => e.Date).Skip((page - 1) * take).Take(take).ToList();
            var count = LatestEvents.Count();
        	int totalPages = count % take == 0 ? count / take : count / take + 1;

			if (count == 0) totalPages = 1;

            return new PagedEventsModel
                       {
                           LatestEvents = events,
						   TotalPages = totalPages,
                           Page = page
                       };
        }
    }
}