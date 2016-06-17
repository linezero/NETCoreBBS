using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBBS.Models
{
    public class UserTopic
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int TopicId { get; set; }
        public DateTime CreateOn { get; set; }
    }
}
