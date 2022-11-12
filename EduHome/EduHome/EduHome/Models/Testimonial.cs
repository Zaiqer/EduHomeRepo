using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome.Models
{
    public class Testimonial
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Position { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public bool IsDeactive { get; set; }
    }
}
