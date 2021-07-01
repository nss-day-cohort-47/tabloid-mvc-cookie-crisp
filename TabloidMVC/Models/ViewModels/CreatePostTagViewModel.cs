using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class CreatePostTagViewModel
    {
        public List<Tag> TagList { get; set; }

        //********************add int labeled tag id ???
        public Post Post { get; set; }
        //allows user to view full list of tags when adding to single post
    }
}
