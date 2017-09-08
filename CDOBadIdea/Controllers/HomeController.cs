using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CDOBadIdea.App;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;

namespace CDOBadIdea.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseContext _context;
        private ITempDataProvider _temp;

        private bool IsAdmin()
        {
            return (Request.Cookies["user"] == "admin" || Request.Cookies["user"] == "jmazouri");
        }

        public HomeController(ITempDataProvider temp, DatabaseContext context)
        {
            _context = context;
            _temp = temp;
        }

        public IActionResult Index()
        {
            return View(_context.GetBlogPosts());
        }

        [HttpPost]
        [Route("/addpost")]
        public IActionResult AddBlogPost(string title, string date, string content)
        {
            if (IsAdmin())
            {
                _context.AddBlogPost(title, Request.Cookies["user"], date, content);
                return RedirectToAction("Private");
            }

            TempData["error"] = "Your account does not have access to that resource";

            return RedirectToAction("Index");
        }

        [Route("/deletepost/{id}")]
        public IActionResult DeletePost(int id)
        {
            if (IsAdmin())
            {
                _context.DeleteBlogPost(id);
                return RedirectToAction("Private");
            }

            TempData["error"] = "Your account does not have access to that resource";

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("/addssn")]
        public IActionResult AddSSN(string name, string number)
        {
            _context.AddSSN(name, number);
            return RedirectToAction("Private");
        }

        [Route("/about")]
        public IActionResult About()
        {
            return View(_context.GetUsers());
        }

        [Route("/private")]
        public IActionResult Private()
        {
            if (IsAdmin())
            {
                return View(new PrivateViewModel
                {
                    Posts = _context.GetBlogPosts(),
                    SocialSecurityNumbers = _context.GetSSNs(),
                    Usernames = _context.GetUsers()
                });
            }

            TempData["error"] = "Your account does not have access to that resource";

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("/login")]
        public IActionResult Login(string username, string password)
        {
            if (_context.ValidLogin(username, password))
            {
                Response.Cookies.Append("user", username, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(7)
                });

                return RedirectToAction("Index");
            }

            TempData["error"] = "Invalid username or password";

            return RedirectToAction("Index");
        }

        [Route("/logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("user");
            return RedirectToAction("Index");
        }
    }
}