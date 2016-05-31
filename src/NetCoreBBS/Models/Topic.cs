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
        public string Email { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Top { get; set; }
        public DateTime CreateOn { get; set; }
    }
}
