using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreBBS.Entities;
using NetCoreBBS.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace NetCoreBBS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]
    public class NodeController : Controller
    {
        private DataContext _context;
        public NodeController(DataContext context)
        {
            _context = context;
        }
        // GET: Node
        public ActionResult Index()
        {
            var nodes = _context.TopicNodes.ToList();
            return View(nodes);
        }

        // GET: Node/Create
        public ActionResult Create(int parentid)
        {
            ViewBag.ParentId = parentid;
            return View();
        }

        // POST: Node/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TopicNode node)
        {
            try
            {
                node.CreateOn = DateTime.Now;
                _context.Add(node);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Node/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Node/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Node/Delete/5
        public ActionResult Delete(int id)
        {
            var topicnode = _context.TopicNodes.SingleOrDefault(m => m.Id == id);            
            if (topicnode == null)
            {
                return NotFound();
            }
            var childnodes = _context.TopicNodes.Any(r => r.ParentId == id);
            if (childnodes) return Content("存在子节点");
            _context.TopicNodes.Remove(topicnode);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}