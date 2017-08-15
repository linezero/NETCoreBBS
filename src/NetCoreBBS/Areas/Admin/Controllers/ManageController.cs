using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NetCoreBBS.Infrastructure;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreBBS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]
    public class ManageController : Controller
    {
        private DataContext _context;
        public ManageController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var usercount = _context.Users.Count();
            var topiccount = _context.Topics.Count();
            var replycount = _context.TopicReplys.Count();
            var allstatistics = new Tuple<int, int, int>(usercount, topiccount, replycount);
            ViewBag.Statistics = allstatistics;
            var topics = _context.Topics.OrderByDescending(r => r.CreateOn).Take(10).ToList();
            return View(topics);
        }
    }
}
