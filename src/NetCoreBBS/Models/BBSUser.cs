using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace NetCoreBBS.Models
{
    public class BBSUser: IdentityUser
    {
        public string Avatar { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime LastTime { get; set; }
    }
}
