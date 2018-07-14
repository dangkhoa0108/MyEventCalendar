using System;
using System.Web.Mvc;
using MyEventCalendar.Models;
using MyEventCalendar.Repository;

namespace MyEventCalendar.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventRepository _eventRepository;

        public EventController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        // GET: Event
        public ActionResult ShowEvent()
        {
            return View(_eventRepository.GetAllEvent());
        }

        [HttpGet]
        public PartialViewResult AddEditEvent(int? id)
        {
            var model= new EventModel();
            if (id.HasValue)
            {
                var item = _eventRepository.GetEventById(id.Value);
                model.Date = item.Date;
                model.ShortDescription = item.ShortDescription;
                model.LongDescription = item.LongDescription;
            }

            return PartialView("~/Views/Event/_AddEditEvent.cshtml", model);
        }

        [HttpPost]
        public ActionResult AddEditEvent(int? id, EventModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isNew = !id.HasValue;
                    EventModel item = isNew ? new EventModel() : _eventRepository.GetEventById(id.Value);
                    item.Date = model.Date;
                    item.LongDescription = model.LongDescription;
                    item.ShortDescription = model.ShortDescription;
                    if (isNew)
                    {
                        _eventRepository.AddNewEvent(item);
                    }
                    else
                    {
                        _eventRepository.UpdateEvent(item);
                    }
                }

                return RedirectToAction("ShowEvent");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        public PartialViewResult DeleteEvent(int id)
        {
            EventModel item = _eventRepository.GetEventById(id);
            EventModel model= new EventModel()
            {
                ShortDescription = item.ShortDescription
            };
            return PartialView("~/Views/Event/_DeleteEvent.cshtml", model);
        }

        public ActionResult DeleteEvent(int id, FormCollection form)
        {
            var item = _eventRepository.GetEventById(id);
            _eventRepository.DeleteEvent(item);
            return RedirectToAction("ShowEvent");
        }
    }
}