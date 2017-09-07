using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CDOBadIdea.App;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace CDOBadIdea.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseContext _context;
        private ITempDataProvider _temp;

        public HomeController(ITempDataProvider temp, DatabaseContext context)
        {
            _context = context;
            _temp = temp;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Private()
        {
            if ((string)_temp.LoadTempData(HttpContext)["user"] == "admin")
            {
                return View((object)"Hey, nice private access!");
            }

            return new ObjectResult("You do not have access to this resource")
            {
                StatusCode = 401
            };
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (_context.ValidLogin(username, password))
            {
                _temp.SaveTempData(HttpContext, new Dictionary<string, object> { { "user", username } });
                return RedirectToAction("Private");
            }

            return new ObjectResult("Invalid Login")
            {
                StatusCode = 401
            };
        }
    }
}