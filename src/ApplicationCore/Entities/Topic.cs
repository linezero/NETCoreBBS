using System;
using System.Collections.Generic;

namespace NetCoreBBS.Entities
{
    public class Topic:BaseEntity
    {
        public int NodeId { get; set; }
        public TopicNode Node { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// 置顶权重
        /// </summary>
        public int Top { get; set; }
        public TopicType Type { get; set; }
        public int ViewCount { get; set; }
        public int ReplyCount { get; set; }
        public string LastReplyUserId { get; set; }
        public User LastReplyUser { get; set; }
        public DateTime LastReplyTime { get; set; }
        public DateTime CreateOn { get; set; }
        public virtual List<TopicReply> Replys { get; set; }
    }
    public enum TopicType
    {
        Delete=0,
        Normal = 1,
        Top=2,
        Good=3,
        Hot=4
    }
}
