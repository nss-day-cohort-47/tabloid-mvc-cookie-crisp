//holds the class (mirrors database properties) that is used in methods (in tagRepository)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models
{
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
