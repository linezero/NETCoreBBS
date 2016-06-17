using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBBS.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public int NodeId { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// 置顶权重
        /// </summary>
        public int Top { get; set; }
        /// <summary>
        /// 精华
        /// </summary>
        public bool Good { get; set; }
        public int ViewCount { get; set; }
        public int ReplyCount { get; set; }
        public Guid LastReplyUserId { get; set; }
        public DateTime LastReplyTime { get; set; }
        public DateTime CreateOn { get; set; }
    }
}
