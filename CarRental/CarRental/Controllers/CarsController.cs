using CarRental.Models;
using CarRental.Repository;
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
            ViewBag.ID_Tran = new SelectList(unitOfWork.TransmissionRepository.GetAll(), "ID_Tran", "Name");
            ViewBag.ID_CarBody = new SelectList(unitOfWork.CarBodyRepository.GetAll(), "ID_CarBody", "Name");
            ViewBag.ID_CarClass = new SelectList(unitOfWork.CarClassRepository.GetAll(), "ID_CarClass", "Name");
            return PartialView("_Create");
        }

        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection, HttpPostedFileBase upload, Car car)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    if (Path.GetExtension(upload.FileName).ToLower() == ".jpg" || Path.GetExtension(upload.FileName).ToLower() == ".png")
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName);
                        var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                        upload.SaveAs(path);

                        car.Model = car.Model.ToUpper();
                        car.Photo = fileName;
                        car.Active = true;
                        unitOfWork.CarRepository.Insert(car);
                        unitOfWork.Save();
                        return RedirectToAction("Index");
                    }
                }
             }

            ViewBag.ID_Tran = new SelectList(unitOfWork.TransmissionRepository.GetAll(), "ID_Tran", "Name", car.ID_Tran);
            ViewBag.ID_CarBody = new SelectList(unitOfWork.CarBodyRepository.GetAll(), "ID_CarBody", "Name", car.ID_CarBody);
            ViewBag.ID_CarClass = new SelectList(unitOfWork.CarClassRepository.GetAll(), "ID_CarClass", "Name", car.ID_CarClass);
            return View(car);
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
