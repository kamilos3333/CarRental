using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using CarRental.Repository;

namespace CarRental.Controllers
{
    public class HomeController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();


        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        [OutputCache(Duration = 20)]
        public ActionResult _CarList() // Display car list
        {
            var cars = unitOfWork.CarRepository.GetAll(filter: a => a.Active == true, includeProperties: "CarClass", orderBy: p => p.OrderBy(a => a.CarClass.Name)).ToList();
            return PartialView("_CarList", cars);
        }

        [ChildActionOnly]
        public ActionResult _CarSearchView()
        {

            return PartialView("_CarSearchView");
        }

        public ActionResult SearchResult(DateTime DateB, DateTime DateE)
        {
            if (ModelState.IsValid)
            {
                TempReservation tmp = new TempReservation
                {
                    DateB = DateB,
                    DateE = DateE
                };
                ViewBag.Info = tmp;
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}