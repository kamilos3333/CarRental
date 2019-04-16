using CarRental.Repository;
using System.Linq;
using System.Web.Mvc;

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
            var cars = unitOfWork.CarRepository.GetAll(filter: a => a.Active == true, includeProperties: "CarClass, CarBody", orderBy: p => p.OrderBy(a => a.CarClass.Name)).ToList();
            return PartialView("_CarList", cars);
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