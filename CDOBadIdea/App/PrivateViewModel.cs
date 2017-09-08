using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDOBadIdea.App
{
    public class PrivateViewModel
    {
        public IEnumerable<SSN> SocialSecurityNumbers { get; set; }
        public IEnumerable<BlogPost> Posts { get; set; }
        public IEnumerable<string> Usernames { get; set; }
    }
}
