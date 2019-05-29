using CarRental.Infrastructure;
using CarRental.Infrastructure.Interface;
using CarRental.Models;
using CarRental.Repository;
using CarRental.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarRental.Controllers
{
    public class CarsController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private IImageManager imageManager;

        public CarsController(IImageManager imageManager)
        {
            this.imageManager = imageManager;
        }

        // GET: Cars
        [Authorize(Roles = "Admin")]
        [Authorize]
        public ActionResult Index()
        {
            var cars = unitOfWork.CarRepository.GetAll(includeProperties: "CarBody, CarClass, Transmission").ToList();
            return View(cars);
        }

        // GET: Cars/Details/5
        [AllowAnonymous]
        public ActionResult _Details(int id)
        {
            Car car = unitOfWork.CarRepository.GetById(id);
            return PartialView("_Details",car);
        }

        // GET: Cars/Create
        [Authorize(Roles = "Admin")]
        [Authorize]
        public ActionResult _Create()
        {
            Car car = new Car();
            var result = new EditCarViewModel();
            result.CarBodies = unitOfWork.CarBodyRepository.GetAll().ToList();
            result.CarClasses = unitOfWork.CarClassRepository.GetAll().ToList();
            result.Transmissions = unitOfWork.TransmissionRepository.GetAll().ToList();
            result.Car = car;
            
            return PartialView("_Create", result);
        }

        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection collection, EditCarViewModel model, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                string fileName = imageManager.InsertImage(Image);

                Car car = new Car()
                {
                    Model = model.Car.Model,
                    Brand = model.Car.Brand,
                    ID_Tran = model.Car.ID_Tran,
                    ID_CarBody = model.Car.ID_CarBody,
                    ID_CarClass = model.Car.ID_CarClass,
                    Photo = fileName,
                    Active = true
                };
                unitOfWork.CarRepository.Insert(car);
                unitOfWork.Save();
            }


            return View();
        }

        // GET: Cars/Edit/5
        [Authorize(Roles = "Admin")]
        [Authorize]
        public ActionResult _Edit(int id)
        {
            Car car = unitOfWork.CarRepository.GetById(id);
            ViewBag.ID_Tran = new SelectList(unitOfWork.TransmissionRepository.GetAll(), "ID_Tran", "Name", car.ID_Tran);
            ViewBag.ID_CarBody = new SelectList(unitOfWork.CarBodyRepository.GetAll(), "ID_CarBody", "Name", car.ID_CarBody);
            ViewBag.ID_CarClass = new SelectList(unitOfWork.CarClassRepository.GetAll(), "ID_CarClass", "Name", car.ID_CarClass);
            return PartialView("_Edit",car);
        }

        // POST: Cars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Car car, FormCollection collection)
        {
            unitOfWork.CarRepository.Update(car);
            unitOfWork.Save();
            
            ViewBag.ID_Tran = new SelectList(unitOfWork.TransmissionRepository.GetAll(), "ID_Tran", "Name", car.ID_Tran);
            ViewBag.ID_CarBody = new SelectList(unitOfWork.CarBodyRepository.GetAll(), "ID_CarBody", "Name", car.ID_CarBody);
            ViewBag.ID_CarClass = new SelectList(unitOfWork.CarClassRepository.GetAll(), "ID_CarClass", "Name", car.ID_CarClass);
            return RedirectToAction("Index");
        }

       
        // POST: Cars/Delete/5
        [HttpPost]
        public JsonResult Delete(int id, FormCollection collection)
        {
            try
            {
                Car car = unitOfWork.CarRepository.GetById(id);
                unitOfWork.CarRepository.Delete(id);
                unitOfWork.Save();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false, message = "Something wrong" }, JsonRequestBehavior.AllowGet);
            }
        }
        
        }

    }
