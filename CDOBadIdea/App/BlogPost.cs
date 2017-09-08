using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDOBadIdea.App
{
    public class BlogPost
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Author { get; set; }
        public string Date { get; set; }
        public string Content { get; set; }
    }
}
