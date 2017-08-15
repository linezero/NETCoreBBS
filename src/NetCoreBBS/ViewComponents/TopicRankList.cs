using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using NetCoreBBS.Infrastructure;
using NetCoreBBS.Entities;

namespace NetCoreBBS.ViewComponents
{
    public class TopicRankList: ViewComponent
    {
        private readonly DataContext db;
        private IMemoryCache _memoryCache;
        private string cachekey = "topicrank";

        public TopicRankList(DataContext context, IMemoryCache memoryCache)
        {
            db = context;
            _memoryCache = memoryCache;
        }

        public  IViewComponentResult Invoke(int days)
        {
            var items = new List<Topic>();
            if (!_memoryCache.TryGetValue(cachekey, out items))
            {
                items = GetRankTopics(10, days);
                _memoryCache.Set(cachekey, items, TimeSpan.FromMinutes(10));
            }
            return View(items);
        }
        /// <summary>
        /// 获取主题排行
        /// </summary>
        /// <param name="top"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        private List<Topic> GetRankTopics(int top,int days)
        {
            return db.Topics.Where(r=>r.CreateOn>DateTime.Now.AddDays(-days))
                .OrderByDescending(r=>r.ViewCount).Take(top).ToList();
        }
    }
}
