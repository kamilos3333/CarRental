﻿using CarRental.Infrastructure;
using CarRental.Models;
using CarRental.Repository;
using CarRental.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
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
        private CostManager costManager;

        public FormController()
        {
            costManager = new CostManager(unitOfWork);
        }

        // GET: Form
        [Authorize(Roles = "Admin")]
        [Authorize]
        public ActionResult Index()
        {
            var reservs = unitOfWork.ReservFormRepository.GetAll(orderBy: a => a.OrderByDescending(x => x.Status)).ToList();
            return View(reservs);
        }
        
        public ActionResult _EditStatusForm(int id)
        {
            ReservForm reserv = unitOfWork.ReservFormRepository.GetById(id);
            var UserDetails = unitOfWork.ContactRepository.GetAll(filter: a => a.ID_Reserv == reserv.ID_Reserv).FirstOrDefault();
            ViewBag.UserDetails = UserDetails;
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
            try
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
            catch
            {
                return View("Error");
            }
        }
        
        public ActionResult _DetailsCostView(int ID, string place1, string place2, DateTime dateB, DateTime dateE)
        {
            try
            {
                Session["summaryCost"] = costManager.CostSummary(ID, place1, place2, dateB, dateE);

                return PartialView("_DetailsCostView");
            }
            catch
            {
                return View("Error");
            }
        }
        
        public ActionResult ContactDetails(string id)
        {
            if(Session["summaryCost"] != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var user = UserManager.FindById(User.Identity.GetUserId());
                    ContactDetailsViewModels viewModel = new ContactDetailsViewModels()
                    {
                        Name = user.Name,
                        Surname = user.Surname,
                        Email = user.Email,
                        Telephone = user.PhoneNumber,
                    };
                    return View(viewModel);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [ValidateInput(false)]
        public ActionResult InsertFormDb(ContactDetailsViewModels model)
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
                            Status = "Wait"
                        };
                        unitOfWork.ReservFormRepository.Insert(reservForm);
                        unitOfWork.Save();
                        InsertContactDb(reservForm.ID_Reserv, model);
                        transaction.Commit();

                        return RedirectToAction("Success", new { success = true });
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

        [ValidateInput(false)]
        public void InsertContactDb(int id, ContactDetailsViewModels model)
        {
            if (ModelState.IsValid)
            {
                ContactDetails contactModel = new ContactDetails
                {
                    ID_Reserv = id,
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    Telephone = model.Telephone
                };
                unitOfWork.ContactRepository.Insert(contactModel);
                unitOfWork.Save();
            }
            
        }
        
        public JsonResult getCars(string filtering) //get car in search result
        {
            var car = unitOfWork.CarRepository.GetAll(includeProperties: "CarClass, Transmission, CarBody", filter: a => a.Active == true && (string.IsNullOrEmpty(filtering)? true : a.CarClass.Name == filtering), orderBy: p => p.OrderBy(a => a.Model)).Select(x => new
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

        public ActionResult OrderUserList()
        {
            var userID = User.Identity.GetUserId();
            var reserv = unitOfWork.ReservFormRepository.GetAll(filter: a => a.UserId == userID).ToList();
            return View(reserv);
        }

        public int GetNumberForm()
        {
            return unitOfWork.ReservFormRepository.GetAll(filter: a => a.Status == "Wait").Count();
        }

        public ActionResult Success(string success)
        {
            if (success != null)
            {
                return View();
            }
            else return HttpNotFound();
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

    }
}