using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NetCoreBBS.Infrastructure;

namespace NetCoreBBS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]
    public class UserController : Controller
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }
        // GET: User
        public ActionResult Index()
        {
            var pagesize = 20;
            var pageindex = 1;
            if (!string.IsNullOrEmpty(Request.Query["page"]))
                pageindex = Convert.ToInt32(Request.Query["page"]);
            var users = _context.Users.AsQueryable();
            var count = users.Count();
            var userlist = users
                .OrderByDescending(r => r.CreateOn)
                .Skip(pagesize * (pageindex - 1))
                .Take(pagesize).ToList();
            ViewBag.PageIndex = pageindex;
            ViewBag.PageCount = count % pagesize == 0 ? count / pagesize : count / pagesize + 1;
            return View(userlist);
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // POST: User/Edit/5
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

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}