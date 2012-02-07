using System.Collections.Generic;

namespace osf.web.Models
{
    public class PagedEventsModel
    {
        public List<LatestEventModel> LatestEvents { get; set; }

        public int Page { get; set; }

		public int TotalPages { get; set; }
    }
}