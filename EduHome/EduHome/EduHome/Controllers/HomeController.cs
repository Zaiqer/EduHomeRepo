using EduHome.DAL;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Sliders = _db.Sliders.Where(x => !x.IsDeactive).ToList(),
                About = _db.About.FirstOrDefault(),
                Notice = _db.Notice.FirstOrDefault(),
                NoticeBoards = _db.NoticeBoards.ToList(),
                Courses = _db.Courses.Take(3).ToList(),
                Services = _db.Services.Where(x => !x.IsDeactive).ToList(),
                Blogs = _db.Blogs.ToList(),
                Testimonials = _db.Testimonials.Where(x => !x.IsDeactive).ToList(),
                Events = _db.Events.ToList()
            };
            return View(homeVM);
        }
        public IActionResult GlobalSearch(string key)
        {
            SearchVM searchVM = new SearchVM
            {
                Courses = _db.Courses.Where(x => x.Title.Contains(key)).OrderByDescending(x=>x.Id).Take(3).ToList(),
                Blogs = _db.Blogs.Where(x => x.Title.Contains(key)).OrderByDescending(x => x.Id).Take(3).ToList(),
                Teachers = _db.Teachers.Where(x => x.FullName.Contains(key)).OrderByDescending(x => x.Id).Take(3).ToList()
            };
            return PartialView("_GlobalSearchPartial", searchVM);
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
