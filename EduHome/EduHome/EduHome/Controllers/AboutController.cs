using EduHome.DAL;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _db;
        public AboutController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Positions = await _db.Positions.Where(p => !p.IsDeactive).ToListAsync();
            AboutVM aboutVM = new AboutVM
            {
                Teachers = _db.Teachers.Where(x => !x.IsDeactive).Take(4).ToList(),
                About = _db.About.FirstOrDefault(),
                Testimonials = _db.Testimonials.Where(x => !x.IsDeactive).ToList()
            };
            return View(aboutVM);
        }
    }
}
