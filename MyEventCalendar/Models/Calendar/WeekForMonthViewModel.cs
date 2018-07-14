using System.Collections.Generic;

namespace MyEventCalendar.Models.Calendar
{
    public class WeekForMonthViewModel
    {
        public WeekForMonthViewModel()
        {
            Weeks = new List<List<DayViewModel>>();
        }
        public List<List<DayViewModel>> Weeks { get; set; }
        public int CurrentMonth { get; set; }
        public int CurrentYear { get; set; }
        public string NextMonth { get; set; }
        public string PrevMonth { get; set; }
        public string EventTitle { get; set; }
        public string EventDescription { get; set; }
    }
}