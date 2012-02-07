using System.Collections.Generic;
using osf.web.Data;

namespace osf.web.Models
{
    public class PagedEventsModel
    {
        public List<LatestEvent> LatestEvents { get; set; }

        public int Page { get; set; }

		public int TotalPages { get; set; }
    }
}