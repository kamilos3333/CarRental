using CarRental.Models;
using CarRental.Repository;
using System;
using System.Collections.Generic;
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
        
        [ChildActionOnly]
        public ActionResult _CarSearchView()
        {

            return PartialView("_CarSearchView");
        }

        public ActionResult SearchResult(string DateB, string DateE, string Place1, string Place2)
        {
            if (ModelState.IsValid)
            {
                TempReservation tmp = new TempReservation
                {
                    DateB = DateB,
                    DateE = DateE,
                    Place1 = Place1,
                    Place2 = Place2
                };
                ViewBag.Info = tmp;
            }

            return View();
        }

        public JsonResult getCars(string id)
        {
            var car = unitOfWork.CarRepository.GetAll().Select(x => new
            {
                Model = x.Model,
                Brand = x.Brand,
                Photo = x.Photo,
            }).ToList();

            return Json(car, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getPlace(string term)
        {
            var placesList = unitOfWork.PlaceRepository.GetAll(filter: a => a.Name.StartsWith(term), orderBy: o => o.OrderBy(a => a.Name)).Select(x => x.Name).ToList();
            return Json(placesList, JsonRequestBehavior.AllowGet);
        }

    }
}