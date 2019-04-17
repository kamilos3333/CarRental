using CarRental.Models;
using CarRental.Repository;
using Microsoft.AspNet.Identity;
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
        protected ApplicationDbContext ApplicationDbContext { get; set; }
        protected UserManager<ApplicationUser> UserManager { get; set; }

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

        public ActionResult ContactDetails(string id, UserViewModel model)
        {
            if(User.Identity.IsAuthenticated)
            {
                var user = UserManager.FindById(id);
                model.Email = user.Email;
                model.Name = user.Name;
                model.Surname = user.Surname;
                model.PhoneNumber = user.PhoneNumber;
                model.Email = user.Email;
                ViewBag.UserModel = model;
            }

            return View();
        }

        public ActionResult InsertFormDb(ContactDetails contact)
        {
            List<SummaryCost> summaryCosts = (List<SummaryCost>)Session["summaryCost"];

            foreach (SummaryCost reserv in summaryCosts)
            {
                ReservForm reservForm = new ReservForm()
                {
                    ID_Car = reserv.ID_car,
                    UserId = User.Identity.GetUserId(),
                    DateBegin = reserv.DateB,
                    EndDate = reserv.DateE,
                    place1 = reserv.Place1,
                    place2 = reserv.Place2,
                    Cost = Convert.ToInt32(reserv.totalCost),
                    PaymentMethod = "Cash",
                    Status = "Wait"
                };
                unitOfWork.ReservFormRepository.Insert(reservForm);
                unitOfWork.Save();
                InsertContactDb(reservForm.ID_Reserv, contact);
            }
            return RedirectToAction("Index", "Home");
        }

        public void InsertContactDb(int id, ContactDetails contact)
        {
            if (ModelState.IsValid)
            {
                contact.ID_Reserv = id;
            }
            unitOfWork.ContactRepository.Insert(contact);
            unitOfWork.Save();
        }
        
        public JsonResult getCars() //get car in search result
        {
            var car = unitOfWork.CarRepository.GetAll(includeProperties: "CarClass, Transmission, CarBody", filter: a => a.Active == true, orderBy: p => p.OrderBy(a => a.CarClass.Name)).Select(x => new
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