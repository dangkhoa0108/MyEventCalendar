using System;
using System.Web.Mvc;
using MyEventCalendar.Repository;

namespace MyEventCalendar.Controllers
{
    public class CalendarController : Controller
    {
        private readonly IEventRepository _eventRepository;

        public CalendarController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        // GET: Calendar
        public ActionResult Index()
        {
            var model =_eventRepository.GetCalendar(DateTime.Now.Month, DateTime.Now.Year);
            return View(model);
        }

        /// <summary>
        /// Load next month or Previous month
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult LoadCalendarAjax(int month, int year)
        {
            var model = _eventRepository.GetCalendar(month, year);
            return PartialView("_Calendar", model);
        }
    }
}