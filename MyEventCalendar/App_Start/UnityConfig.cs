using System.Web.Mvc;
using MyEventCalendar.Repository;
using Unity;
using Unity.Mvc5;

namespace MyEventCalendar
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<IEventRepository, EventRepository>();
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}