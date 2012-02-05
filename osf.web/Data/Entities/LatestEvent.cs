using System;
using System.ComponentModel.DataAnnotations;

namespace osf.web.Data
{
    public class LatestEvent
    {
        public int Id { get; set; }

        public string FileExtension { get; set; }

        [Required(ErrorMessage = "enter the title of the event")]
        public string Title { get; set; }

        [Required(ErrorMessage = "enter the date of the event")]
        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "enter description of the event")]
        public string Description { get; set; }
    }
}