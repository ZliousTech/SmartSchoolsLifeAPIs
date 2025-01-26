using SmartSchoolLifeAPI.Core.Models;
using System.Web.Mvc;

namespace SmartSchoolLifeAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public string About()
        {
            ViewBag.Message = "Your application description page.";

            return PasswordEncDec.Encrypt("ibtesam");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}