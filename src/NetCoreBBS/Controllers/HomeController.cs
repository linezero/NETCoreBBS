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
            var pagesize = 20;
            var pageindex = 1;
            if (!string.IsNullOrEmpty(Request.Query["page"]))
                pageindex = Convert.ToInt32(Request.Query["page"]);
            var count= _context.Topics.Count();
            ViewBag.Topics = _context.Topics.Skip(pagesize*(pageindex-1))
                .Take(pagesize).OrderByDescending(r=>r.CreateOn).ToList();
            ViewBag.PageIndex = pageindex;
            ViewBag.PageCount = count % pagesize == 0 ? count / pagesize : count / pagesize + 1;
            ViewBag.User = user.User.Result;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
