using CarRental.Models;
using CarRental.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarRental.Controllers
{
    public class FormController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Form
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult _CarSearchView()
        {
            ViewBag.PlacesList = new SelectList(unitOfWork.PlaceRepository.GetAll(orderBy: p => p.OrderBy(a=>a.Name)), "Name", "Name");
            return PartialView("_CarSearchView");
        }

        public ActionResult SearchResult(DateTime DateB, DateTime DateE, string Place1, string Place2)
        {
            if (ModelState.IsValid)
            {
                TempReservation tmp = new TempReservation
                {
                    Place1 = Place1,
                    Place2 = Place2,
                    DateB = DateB,
                    DateE = DateE,
                };
                ViewBag.Info = tmp;
            }
            return View();
        }
        
        public ActionResult _DetailsCostView(int ID, string place1, string place2 ,DateTime dateB, DateTime dateE)
        {
            var additionalCostPlace = unitOfWork.PlaceRepository.GetAll(filter: a => a.Name == place1).Select(x => x.AddCost).FirstOrDefault();
            var carCost = unitOfWork.CarRepository.GetAll(includeProperties: "CarClass", filter: a => a.ID_Car == ID).Select(x => x.CarClass.Cost).FirstOrDefault();
            var totalDaysCount = dateE.Subtract(dateB).TotalDays;
            var totalDayCost = (totalDaysCount * 85);
            var totalCost = (decimal)totalDayCost + additionalCostPlace + carCost;
            
            List<SummaryCost> summaryCosts = new List<SummaryCost>
            {
                new SummaryCost()
                {
                    ID_car = ID,
                    Place1 = place1,
                    Place2 = place2,
                    DateB = dateB,
                    DateE = dateE,
                    additionalCostPlace = additionalCostPlace,
                    totalDayCost = totalDayCost,
                    carCost = carCost,
                    totalCost = totalCost
                },
            };
            Session["summaryCost"] = summaryCosts;

            return PartialView("_DetailsCostView");
        }
        
        public JsonResult getCars() //get car in search result
        {
            var car = unitOfWork.CarRepository.GetAll(includeProperties: "CarClass, Transmission, CarBody").Select(x => new
            {
                ID = x.ID_Car,
                Model = x.Model,
                Brand = x.Brand,
                Photo = x.Photo,
                CarBody = x.CarBody.Name,
                Transmission = x.Transmission.Name,
                Cost = x.CarClass.Cost,
            }).ToList();

            return Json(car, JsonRequestBehavior.AllowGet);
        }
        
        
    }
}