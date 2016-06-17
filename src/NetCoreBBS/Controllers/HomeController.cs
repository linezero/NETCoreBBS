using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NetCoreBBS.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace NetCoreBBS.Controllers
{
    public class HomeController : Controller
    {
        private DataContext _context;
        public UserManager<User> UserManager { get; }

        public HomeController(DataContext context,UserManager<User> userManager)
        {
            _context = context;
            UserManager = userManager;
        }
        public IActionResult Index([FromServices]IUserServices user)
        {
            ViewBag.Topics = _context.Topics.OrderByDescending(r=>r.CreateOn).ToList();
            ViewBag.User = user.User.Result;
            return View();
        }

        [HttpPost]
        public IActionResult Index(Topic topic)
        {
            if (ModelState.IsValid)
            {
                topic.CreateOn = DateTime.Now;
                _context.Topics.Add(topic);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult About()
        {
            ViewData["Message"] = ".NET Core 版轻论坛";
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
