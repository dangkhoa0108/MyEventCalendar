using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyEventCalendar.Models
{
    public class EventModel
    {
        [Key]
        public int ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
    }
}