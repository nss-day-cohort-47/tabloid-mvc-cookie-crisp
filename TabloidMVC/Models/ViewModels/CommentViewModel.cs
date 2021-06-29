using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class CommentViewModel
    { 
        public List<Comment> CommentList { get; set; }
        public Comment Comment { get; set;  }
        public Post Posts { get; set; }
    }
}
