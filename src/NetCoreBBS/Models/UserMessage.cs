using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBBS.Models
{
    public class UserMessage
    {
        public int Id { get; set; }
        public Guid SendUserId { get; set; }
        public Guid ReceiveUserId { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreateOn { get; set; }
    }
}
