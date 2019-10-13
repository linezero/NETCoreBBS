using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreBBS.Interfaces;
using NetCoreBBS.Entities;
using Microsoft.AspNetCore.Authorization;
using NetCoreBBS.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace NetCoreBBS.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private ITopicRepository _topic;
        private ITopicReplyRepository _reply;
        private UserManager<User> UserManager;
        private IWebHostEnvironment _env;
        public UserController(ITopicRepository topic, ITopicReplyRepository reply, UserManager<User> userManager, IWebHostEnvironment env)
        {
            _topic = topic;
            _reply = reply;
            UserManager = userManager;
            _env = env;
        }
        public IActionResult Index()
        {
            var u = UserManager.GetUserAsync(User).Result;
            var topics = _topic.List(r => r.UserId == u.Id).ToList();
            var replys = _reply.List(r => r.ReplyUserId == u.Id).ToList();
            ViewBag.Topics = topics;
            ViewBag.Replys = replys;
            return View(u);
        }

        public IActionResult Edit()
        {
            return View(UserManager.GetUserAsync(User).Result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel usermodel)
        {
            var user = UserManager.GetUserAsync(User).Result;
            if (ModelState.IsValid)
            {
                if (usermodel.Avatar != null)
                {
                    var avatar = usermodel.Avatar;
                    if (avatar.Length / 1024 > 100)
                    {
                        return Content("头像文件大小超过100KB");
                    }
                    var ext = Path.GetExtension(avatar.FileName);
                    var avatarfile = user.Id + ext;
                    var avatarpath = Path.Combine(_env.WebRootPath, "images", "avatar");
                    if (!Directory.Exists(avatarpath))
                        Directory.CreateDirectory(avatarpath);
                    var filepath = Path.Combine(avatarpath, avatarfile);
                    using (FileStream fs = new FileStream(filepath, FileMode.Create))
                    {
                        avatar.CopyTo(fs);
                        fs.Flush();
                    }
                    user.Avatar = $"/images/avatar/{avatarfile}";
                }
                user.Email = usermodel.Email;
                user.Url = usermodel.Url;
                user.GitHub = usermodel.GitHub;
                user.Profile = usermodel.Profile;
                await UserManager.UpdateAsync(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }
    }
}