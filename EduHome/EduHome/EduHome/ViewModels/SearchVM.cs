using EduHome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome.ViewModels
{
    public class SearchVM
    {
        public List<Course> Courses { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<Teacher> Teachers { get; set; }
    }
}
