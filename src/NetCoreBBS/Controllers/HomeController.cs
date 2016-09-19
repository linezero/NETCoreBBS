using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NetCoreBBS.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using NetCoreBBS.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NetCoreBBS.Controllers
{
    public class HomeController : Controller
    {
        private DataContext _context;
        public UserManager<User> UserManager { get; }

        public HomeController(DataContext context, UserManager<User> userManager)
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
            var count = topics.Count();
            //var q = from t in topics
            //        join n in _context.TopicNodes on t.NodeId equals n.Id into tn
            //        from n2 in tn.DefaultIfEmpty()
            //        select t;
            ViewBag.Topics = topics
                .GroupJoin(_context.TopicNodes,
                r => r.NodeId,
                n => n.Id,
                (r, n) => new { r = r, n = n })
                .SelectMany(result => result.n.DefaultIfEmpty(), (r, n) => new TopicViewModel
                {
                    Id = r.r.Id,
                    NodeId = r.r.NodeId,
                    NodeName = n == null ? "" : n.Name,
                    Email = r.r.Email,
                    Title = r.r.Title,
                    Top = r.r.Top,
                    Good = r.r.Good,
                    ReplyCount = r.r.ReplyCount,
                    LastReplyTime = r.r.LastReplyTime,
                    CreateOn = r.r.CreateOn
                })
                .OrderByDescending(r => r.CreateOn)
                .OrderByDescending(r => r.Top)
                .Skip(pagesize * (pageindex - 1))
                .Take(pagesize).ToList();
            ViewBag.PageIndex = pageindex;
            ViewBag.PageCount = count % pagesize == 0 ? count / pagesize : count / pagesize + 1;
            ViewBag.User = user.User.Result;
            var nodes= _context.TopicNodes.ToList();
            ViewBag.Nodes = nodes;
            ViewBag.NodeListItem = nodes.Where(r => r.ParentId != 0).Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name });
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
