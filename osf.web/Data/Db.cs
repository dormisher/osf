using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using osf.web.Models;

namespace osf.web.Data
{
    public class Db : DbContext
    {
        public List<LatestEvent> LatestEvents { get; set; }

        public Db()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<Db>());   
        }

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