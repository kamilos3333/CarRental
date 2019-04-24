using CarRental.Models;
using CarRental.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
        ApplicationDbContext context = new ApplicationDbContext();

        // GET: Form
        [Authorize(Roles = "Admin")]
        [Authorize]
        public ActionResult Index()
        {
            var reservs = unitOfWork.ReservFormRepository.GetAll().ToList();
            return View(reservs);
        }
        
        public ActionResult _EditStatusForm(int id)
        {
            ReservForm reserv = unitOfWork.ReservFormRepository.GetById(id);
            return PartialView("_EditStatusForm", reserv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStatusForm(ReservForm reserv, FormCollection collection)
        {
            unitOfWork.ReservFormRepository.Update(reserv);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public ActionResult _CarSearchView()
        {
            ViewBag.PlacesList = new SelectList(unitOfWork.PlaceRepository.GetAll(orderBy: p => p.OrderBy(a=>a.Name)), "Name", "Name");
            return PartialView("_CarSearchView");
        }

        public ActionResult SearchResult(DateTime DateB, DateTime DateE, TimeSpan TimeB, TimeSpan TimeE, string Place1, string Place2)
        {
            DateTime DTBegin = DateB + TimeB;
            DateTime DTEnding = DateE + TimeE;
            
            if (ModelState.IsValid)
            {
                TempReservation tmp = new TempReservation
                {
                    Place1 = Place1,
                    Place2 = Place2,
                    DateB = DTBegin,
                    DateE = DTEnding
                };
                ViewBag.Info = tmp;
            }
            return View();
        }
        
        public ActionResult _DetailsCostView(int ID, string place1, string place2, DateTime dateB, DateTime dateE)
        {
            var additionalCostPlace = unitOfWork.PlaceRepository.GetAll(filter: a => a.Name == place1).Select(x => x.AddCost).FirstOrDefault();
            var carCost = unitOfWork.CarRepository.GetAll(includeProperties: "CarClass", filter: a => a.ID_Car == ID).Select(x => x.CarClass.Cost).FirstOrDefault();
            var totalDaysCount = dateE.Date.Subtract(dateB.Date).TotalDays;
            var totalDayCost = (totalDaysCount * 85);
            var totalCost = (decimal)totalDayCost + additionalCostPlace + carCost;
            if(User.Identity.IsAuthenticated) { totalCost = totalCost - 10; }
            
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
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
                UserViewModel viewModel = new UserViewModel()
                {
                    Name = currentUser.Name,
                    Surname = currentUser.Surname,
                    Email = currentUser.Email,
                    Telephone = currentUser.PhoneNumber,
                };
                ViewBag.ModelUser = viewModel;
            }
            else
            {
                return View();
            }

            return View();
        }

        public ActionResult InsertFormDb(ContactDetails contact)
        {
            using(var transaction = unitOfWork.ReservFormRepository.dbContext.Database.BeginTransaction())
            {
                try
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
                            Status = "Waiting"
                        };
                        unitOfWork.ReservFormRepository.Insert(reservForm);
                        unitOfWork.Save();
                        InsertContactDb(reservForm.ID_Reserv, contact);
                        transaction.Commit();
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    return HttpNotFound();
                }
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
        
        public JsonResult getCars(string filtering) //get car in search result
        {
            var car = unitOfWork.CarRepository.GetAll(includeProperties: "CarClass, Transmission, CarBody", filter: a => a.Active == true && (string.IsNullOrEmpty(filtering)? true : a.CarClass.Name == filtering), orderBy: p => p.OrderBy(a => a.CarClass.Name)).Select(x => new
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
        
        public JsonResult getCarClass()
        {
            var carClass = unitOfWork.CarClassRepository.GetAll().Select(a => new {
                ID = a.ID_CarClass,
                Name = a.Name
            }).ToList();

            return Json(carClass ,JsonRequestBehavior.AllowGet);
        }
        
    }
}