using System.ComponentModel.DataAnnotations;

namespace osf.web.Models
{
    public class LatestEventModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Date { get; set; }

        public string Description { get; set; }
    }
}