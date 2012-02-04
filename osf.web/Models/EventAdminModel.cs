using osf.web.Data;

namespace osf.web.Models
{
    public class EventAdminModel
    {
        public EventAdminModel()
        {
            Event = new LatestEvent();
        }

        public LatestEvent Event { get; set; }

        public PagedEventsModel Events { get; set; }
    }
}