using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class CommentViewModel
    { 
        public Comment Comment { get; set;  }
        public Post Posts { get; set; }
    }
}
