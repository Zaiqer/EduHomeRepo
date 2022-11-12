using EduHome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome.ViewModels
{
    public class AboutVM
    {
        public List<Teacher> Teachers { get; set; }
        public About About { get; set; }
        public List<Testimonial> Testimonials { get; set; }
    }
}
