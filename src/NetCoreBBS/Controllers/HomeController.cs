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
            var topics = _context.Topics.AsQueryable();
            if (!string.IsNullOrEmpty(Request.Query["page"]))
                pageindex = Convert.ToInt32(Request.Query["page"]);
            if (!string.IsNullOrEmpty(Request.Query["s"]))
                topics = topics.Where(r => r.Title.Contains(Request.Query["s"]));
            var count= topics.Count();
            ViewBag.Topics = topics
                .OrderByDescending(r => r.CreateOn)
                .OrderByDescending(r => r.Top)                
                .Skip(pagesize*(pageindex-1))
                .Take(pagesize).ToList();
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
