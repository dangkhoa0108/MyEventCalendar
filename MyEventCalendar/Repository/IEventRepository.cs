using System.Collections.Generic;
using MyEventCalendar.Models;
using MyEventCalendar.Models.Calendar;

namespace MyEventCalendar.Repository
{
    public interface IEventRepository
    {
        IEnumerable<EventModel> GetAllEvent();
        EventModel GetEventById(int id);
        void AddNewEvent(EventModel item);
        void UpdateEvent(EventModel item);
        void DeleteEvent(EventModel item);
        //Calendar
        WeekForMonthViewModel GetCalendar(int month, int year);
    }
}
