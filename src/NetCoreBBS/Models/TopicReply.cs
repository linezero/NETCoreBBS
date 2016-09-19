using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBBS.Models
{
    public class TopicReply
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public Guid UserId { get; set; }
        [Required]
        public string ReplyEmail { get; set; }
        [Required]
        public string ReplyContent { get; set; }
        public DateTime CreateOn { get; set; }
    }
}
