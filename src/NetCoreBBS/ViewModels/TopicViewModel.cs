using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBBS.ViewModels
{
    public class TopicViewModel
    {
        public int Id { get; set; }
        public int NodeId { get; set; }
        public string NodeName { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public string Title { get; set; }
        public int Top { get; set; }
        public Entities.TopicType Type { get; set; }
        public int ReplyCount { get; set; }
        public DateTime LastReplyTime { get; set; }
        public DateTime CreateOn { get; set; }
    }
}
