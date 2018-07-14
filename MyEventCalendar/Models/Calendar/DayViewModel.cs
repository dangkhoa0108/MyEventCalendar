using System;

namespace MyEventCalendar.Models.Calendar
{
    public class DayViewModel
    {
        public DateTime DateTime { get; set; }
        public string Date { get; set; }
        public string DateStr { get; set; }
        public int DtDay { get; set; }
        public int? DayColumn { get; set; }
        public bool IsDayInCurrentMonth { get; set; }
        public bool HasEvent { get; set; }
        public string ShortDescription { get; set; }
    }
}