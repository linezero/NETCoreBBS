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
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "Topic", new { Id = reply.TopicId });
        }

        [Route("/Topic/Node/{name}")]
        public IActionResult Node(string name)
        {
            if (string.IsNullOrEmpty(name)) return Redirect("/");
            var node = _context.TopicNodes.FirstOrDefault(r => r.NodeName == name);
            if (node == null)
                node= _context.TopicNodes.FirstOrDefault(r => r.Id == Convert.ToInt32(name));
            if (node == null) return Redirect("/");
            var pagesize = 20;
            var pageindex = 1;
            var topics = _context.Topics.AsQueryable();
            if (!string.IsNullOrEmpty(Request.Query["page"]))
                pageindex = Convert.ToInt32(Request.Query["page"]);
            if (!string.IsNullOrEmpty(Request.Query["s"]))
                topics = topics.Where(r => r.Title.Contains(Request.Query["s"]));
            var count = topics.Count();
            ViewBag.Topics = topics
                .Where(r=>r.NodeId==node.Id)
                .OrderByDescending(r => r.CreateOn)
                .OrderByDescending(r => r.Top)
                .Skip(pagesize * (pageindex - 1))
                .Take(pagesize).ToList();
            ViewBag.PageIndex = pageindex;
            ViewBag.PageCount = count % pagesize == 0 ? count / pagesize : count / pagesize + 1;
            ViewBag.Node = node;
            ViewBag.Count = count;
            return View();
        }
    }
}
