using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        public string FullName { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public Position Position { get; set; }
        public bool IsDeactive { get; set; }
        public int PositionId { get; set; }
    }
}
