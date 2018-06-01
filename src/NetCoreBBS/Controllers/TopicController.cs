using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreBBS.Infrastructure;
using NetCoreBBS.Entities;
using NetCoreBBS.Interfaces;
using NetCoreBBS.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreBBS.Controllers
{
    public class TopicController : Controller
    {
        private ITopicRepository _topic;
        private IRepository<TopicNode> _node;
        private ITopicReplyRepository _reply;
        public TopicController(ITopicRepository topic, IRepository<TopicNode> node, ITopicReplyRepository reply)
        {
            _topic = topic;
            _node = node;
            _reply = reply;
        }
        // GET: /Topic/1
        [Route("/Topic/{id}")]
        public IActionResult Index(int id)
        {
            if (id <= 0) return Redirect("/");
            var topic = _topic.GetById(id);
            if (topic == null) return Redirect("/");
            var replys = _reply.List(r => r.TopicId == id).ToList();
            topic.ViewCount += 1;
            _topic.Edit(topic);
            ViewBag.Replys = replys;
            return View(topic);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Topic/{id}")]
        public IActionResult Index([Bind("TopicId,ReplyUserId,ReplyEmail,ReplyContent")]TopicReply reply)
        {
            if (ModelState.IsValid&&!string.IsNullOrEmpty(reply.ReplyContent))
            {
                reply.CreateOn = DateTime.Now;
                _reply.Add(reply);
                var topic = _topic.GetById(reply.TopicId);
                topic.LastReplyUserId = reply.ReplyUserId;
                topic.LastReplyTime = reply.CreateOn;
                topic.ReplyCount += 1;
                _topic.Edit(topic);
            }
            return RedirectToAction("Index", "Topic", new { Id = reply.TopicId });
        }

        [Route("/Topic/Node/{name}")]
        public IActionResult Node(string name)
        {
            if (string.IsNullOrEmpty(name)) return Redirect("/");
            var node = _node.List(r => r.NodeName == name).FirstOrDefault();
            if (node == null)
                node= _node.GetById(Convert.ToInt32(name));
            if (node == null) return Redirect("/");
            var pagesize = 20;
            var pageindex = 1;
            Page<Topic> result;
            if (!string.IsNullOrEmpty(Request.Query["page"]))
                pageindex = Convert.ToInt32(Request.Query["page"]);
            if (!string.IsNullOrEmpty(Request.Query["s"]))
                result = _topic.PageList(r => r.NodeId == node.Id && r.Title.Contains(Request.Query["s"]),pagesize,pageindex);
            else
                result = _topic.PageList(r => r.NodeId == node.Id, pagesize, pageindex);
            ViewBag.Topics = result.List.Select(r => new TopicViewModel
            {
                Id = r.Id,
                NodeId = r.Node.Id,
                NodeName = r.Node.Name,
                UserName = r.User.UserName,
                Avatar=r.User.Avatar,
                Title = r.Title,
                Top = r.Top,
                Type = r.Type,
                ReplyCount = r.ReplyCount,
                LastReplyTime = r.LastReplyTime,
                CreateOn = r.CreateOn
            }).ToList();
            ViewBag.PageIndex = pageindex;
            ViewBag.PageCount = result.GetPageCount();
            ViewBag.Node = node;
            ViewBag.Count = result.Total;
            return View();
        }
    }
}
