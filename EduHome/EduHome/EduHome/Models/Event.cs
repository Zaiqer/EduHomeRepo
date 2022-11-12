using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Name { get; set; }
        public string Clock { get; set; }
        public string City { get; set; }
        public string Image { get; set; }
    }
}
