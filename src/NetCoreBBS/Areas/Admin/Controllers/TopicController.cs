using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NetCoreBBS.Infrastructure;
using NetCoreBBS.Entities;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreBBS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]
    public class TopicController : Controller
    {
        private DataContext _context;
        public TopicController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var pagesize = 20;
            var pageindex = 1;
            if (!string.IsNullOrEmpty(Request.Query["page"]))
                pageindex = Convert.ToInt32(Request.Query["page"]);
            var topics = _context.Topics.AsQueryable();
            var count = topics.Count();
            var topiclist = topics
                .OrderByDescending(r => r.CreateOn)
                .OrderByDescending(r => r.Top)                
                .Skip(pagesize * (pageindex - 1))
                .Take(pagesize).ToList();
            ViewBag.PageIndex = pageindex;
            ViewBag.PageCount = count % pagesize == 0 ? count / pagesize : count / pagesize + 1;
            return View(topiclist);
        }

        public IActionResult Delete(int id)
        {
            var topic = _context.Topics.FirstOrDefault(r => r.Id == id);
            if (topic != null)
            {
                _context.Topics.Remove(topic);
                var replys=_context.TopicReplys.Where(r => r.TopicId == id).ToList();
                _context.TopicReplys.RemoveRange(replys);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return Content("出现异常");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSave(Topic topic)
        {
            _context.Attach(topic);
            _context.Entry(topic).Property(r => r.Title).IsModified = true;
            _context.Entry(topic).Property(r => r.Content).IsModified = true;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Update(int id, string type)
        {
            var topic = _context.Topics.FirstOrDefault(r => r.Id == id);
            if (topic != null)
            {
                switch (type)
                {
                    case "top":
                        topic.Top = 1;
                        break;
                    //case "good":
                    //    topic.Good = true;
                    //    break;
                    case "notop":
                        topic.Top = 0;
                        break;
                    //case "nogood":
                    //    topic.Good = false;
                    //    break;
                }
                _context.Update(topic);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return Content("出现异常");
        }
    }
}
