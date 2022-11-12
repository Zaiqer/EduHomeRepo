using EduHome.DAL;
using EduHome.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome.Controllers
{
    public class CoursesController : Controller
    {
        private readonly AppDbContext _db;
        public CoursesController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            ViewBag.CourseCount = _db.Courses.Count();
            List<Course> courses = _db.Courses.OrderByDescending(x => x.Id).ToList();
            return View(courses);
        }
        public IActionResult CoursesSearch(string key)
        {
            List<Course> courses = _db.Courses.Where(x => x.Title.Contains(key)).OrderByDescending(x => x.Id).ToList();
            return PartialView("_CoursesPartial", courses);
        }
        public IActionResult Detail()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LoadMore(int skip)
        {
            int count = _db.Courses.Count();
            if (count <= skip)
            {
                return Content("Get out!");
            }
            List<Course> courses = _db.Courses.OrderByDescending(x => x.Id).Skip(skip).Take(6).ToList();
            return PartialView("_CoursesPartial", courses); 
        }
    }
}
