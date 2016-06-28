using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreBBS.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreBBS.Controllers
{
    public class TopicController : Controller
    {
        private DataContext _context;
        public TopicController(DataContext context)
        {
            _context = context;
        }
        // GET: /Topic/1
        [Route("/Topic/{id}")]
        public IActionResult Index(int id)
        {
            if (id <= 0) return Redirect("/");
            var topic = _context.Topics.FirstOrDefault(r => r.Id == id);
            if (topic == null) return Redirect("/");
            var replys = _context.TopicReplys.Where(r => r.TopicId == id).ToList();
            topic.ViewCount += 1;
            _context.Update(topic);
            _context.SaveChanges();
            ViewBag.Replys = replys;
            return View(topic);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Topic/{id}")]
        public IActionResult Index(TopicReply reply)
        {
            if (ModelState.IsValid)
            {
                reply.Id = 0;
                reply.CreateOn = DateTime.Now;
                _context.TopicReplys.Add(reply);
                var topic = _context.Topics.Single(r => r.Id == reply.TopicId);
                topic.LastReplyUserId = reply.UserId;
                topic.LastReplyTime = reply.CreateOn;
                topic.ReplyCount += 1;
                _context.Update(topic);
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "Topic", new { Id = reply.TopicId });
        }
    }
}
