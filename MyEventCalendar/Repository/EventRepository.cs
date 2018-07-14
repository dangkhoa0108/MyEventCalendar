using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using MyEventCalendar.Models;
using MyEventCalendar.Models.Calendar;
using ArgumentNullException = System.ArgumentNullException;

namespace MyEventCalendar.Repository
{
    public class EventRepository:IEventRepository
    {
        private readonly EventDbContext _eventDbContext;
        private IDbSet<EventModel> _entities;
        string _errorMessage = string.Empty;

        public EventRepository(EventDbContext eventDbContext)
        {
            _eventDbContext = eventDbContext;
        }

        private IDbSet<EventModel> Entities => _entities ?? (_entities = _eventDbContext.Set<EventModel>());

        public IEnumerable<EventModel> GetAllEvent()
        {
            return Entities.ToList();
        }

        public EventModel GetEventById(int id)
        {
            return Entities.Find(id);
        }

        public void AddNewEvent(EventModel item)
        {
            try
            {
                if (item == null)
                {
                    throw new ArgumentNullException(nameof(item));
                }
                Entities.Add(item);
                _eventDbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        _errorMessage +=$"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}" + Environment.NewLine;
                    }
                }
                throw new Exception(_errorMessage, dbEx);
            }
        }

        public void UpdateEvent(EventModel item)
        {
            try
            {
                if (item == null)
                {
                    throw new ArgumentNullException(nameof(item));
                }

                _eventDbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        _errorMessage +=$"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}" + Environment.NewLine;
                    }
                }
                throw new Exception(_errorMessage, dbEx);
            }
        }

        public void DeleteEvent(EventModel item)
        {
            try
            {
                if (item == null)
                {
                    throw new ArgumentNullException(nameof(item));
                }

                Entities.Remove(item);
                _eventDbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        _errorMessage +=$"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}" + Environment.NewLine;
                    }
                }
                throw new Exception(_errorMessage, dbEx);
            }
        }

        public WeekForMonthViewModel GetCalendar(int month, int year)
        {
            var weeks = new WeekForMonthViewModel
            {
                Weeks = new List<List<DayViewModel>>(),
                CurrentMonth = month,
                CurrentYear = year
            };
            try
            {
                //Get dates.
                var dt = GetDates(year, month);
                //Get first day.
                var firstDay = dt.FirstOrDefault();
                //Get first day infor.
                var firstDateInfo = GetDateInfo(firstDay);
                //Get month in the past.
                var monthPast = dt.FirstOrDefault().AddMonths(-1);
                //Get dates in the past.
                var dtPast = GetDates(monthPast.Year, monthPast.Month, true).Take(firstDateInfo - firstDay.Day).OrderBy(i => i.Day);
                foreach (var day in dtPast)
                {
                    //Insert data for WeekForMonthViewModel.
                    weeks.Weeks.Add(new List<DayViewModel>
                    {
                        new DayViewModel
                        {
                            DateTime = day,
                            Date = day.ToShortDateString(),
                            DateStr = day.ToString("MM/dd/yyyy"),
                            DtDay = day.Day,
                            DayColumn = GetDateInfo(day),
                            IsDayInCurrentMonth = false,
                            HasEvent = CheckHasEvent(GetAllEvent(),day, out string description),
                            ShortDescription = description
                        }
                    });
                }
                // Get days in the current date.
                foreach (var day in dt)
                {
                    //Insert data for WeekForMonthViewModel.
                    weeks.Weeks.Add(new List<DayViewModel>
                    {
                        new DayViewModel
                        {
                            DateTime = day,
                            Date = day.ToShortDateString(),
                            DateStr = day.ToString("MM/dd/yyyy"),
                            DtDay = day.Day,
                            DayColumn = GetDateInfo(day),
                            IsDayInCurrentMonth = true,
                            HasEvent = CheckHasEvent(GetAllEvent(),day, out string description),
                            ShortDescription = description
                        }
                    });
                }
                //Get last day.
                var lastDay = dt.LastOrDefault();
                //Get last day infor.
                GetDateInfo(lastDay);
                //Get month in the future.
                var monthFuture = dt.FirstOrDefault().AddMonths(1);
                // Get days in the future.
                var dtFuture = GetDates(monthFuture.Year, monthFuture.Month).Take(42 - weeks.Weeks.Count);
                foreach (var day in dtFuture)
                {
                    //Insert data for WeekForMonthViewModel.
                    weeks.Weeks.Add(new List<DayViewModel>
                    {
                        new DayViewModel
                        {
                            DateTime = day,
                            Date = day.ToShortDateString(),
                            DateStr = day.ToString("MM/dd/yyyy"),
                            DtDay = day.Day,
                            DayColumn = GetDateInfo(day),
                            IsDayInCurrentMonth = false,
                            HasEvent = CheckHasEvent(GetAllEvent(),day, out string description),
                            ShortDescription = description
                        }
                    });
                }
                while (weeks.Weeks.Count < 7) // not starting from monday
                {
                    weeks.Weeks.Insert(0, null);
                }

                //Case month is 12.
                if (month == 12)
                {
                    weeks.NextMonth = 01 + "/" + (year + 1);
                    weeks.PrevMonth = month - 1 + "/" + year;
                }
                //Case month is 1.
                else if (month == 1)
                {
                    weeks.NextMonth = month + 1 + "/" + year;
                    weeks.PrevMonth = 12 + "/" + (year - 1);
                }
                //Default
                else
                {
                    weeks.NextMonth = month + 1 + "/" + year;
                    weeks.PrevMonth = month - 1 + "/" + year;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return weeks;
        }

        public bool CheckHasEvent(IEnumerable<EventModel> model, DateTime dateTime, out string description)
        {
            description = string.Empty;
            var flag = false;
            try
            {
                foreach (var item in model)
                {
                    var dateTimeEvent = item.Date;
                    if(DateTime.Compare(dateTimeEvent, dateTime)!=0) continue;
                    flag = true;
                    description = item.ShortDescription;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return flag;
        }
        #region Private's Method

        /// <summary>
        /// Get dates of month
        /// </summary>
        /// <param name="year">current year.</param>
        /// <param name="month">current month.</param>
        /// <param name="desc">order by descending.</param>
        /// <returns></returns>
        private static List<DateTime> GetDates(int year, int month, bool desc = false)
        {
            //Case order by descending.
            if (desc)
            {
                return Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
                    .Select(day => new DateTime(year, month, day)) // Map each day to a date
                    .OrderByDescending(i => i.Day)
                    .ToList();
            }
            //Case order by ascending.
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
                .Select(day => new DateTime(year, month, day)) // Map each day to a date
                .ToList();
        }

        /// <summary>
        /// Get date info.
        /// </summary>
        /// <param name="now">current date.</param>
        /// <returns>day number.</returns>
        private int GetDateInfo(DateTime now)
        {
            var dayNumber = 1;
            var dt = now.Date;
            var dayStr = Convert.ToString(dt.DayOfWeek);

            if (dayStr.ToLower() == "monday")
            {
                dayNumber = 1;
            }
            else if (dayStr.ToLower() == "tuesday")
            {
                dayNumber = 2;
            }
            else if (dayStr.ToLower() == "wednesday")
            {
                dayNumber = 3;
            }
            else if (dayStr.ToLower() == "thursday")
            {
                dayNumber = 4;
            }
            else if (dayStr.ToLower() == "friday")
            {
                dayNumber = 5;
            }
            else if (dayStr.ToLower() == "saturday")
            {
                dayNumber = 6;
            }
            else if (dayStr.ToLower() == "sunday")
            {
                dayNumber = 7;
            }
            return dayNumber;
        }

        #endregion
    }
}