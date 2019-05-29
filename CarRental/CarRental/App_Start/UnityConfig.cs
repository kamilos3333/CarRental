using CarRental.Infrastructure;
using CarRental.Infrastructure.Interface;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace CarRental
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IImageManager, ImageManager>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}