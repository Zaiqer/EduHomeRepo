using EduHome.DAL;
using EduHome.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome.Controllers
{
    public class TeachersController : Controller
    {
        private readonly AppDbContext _db;
        public TeachersController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Positions = await _db.Positions.Where(p => !p.IsDeactive).ToListAsync();
            List<Teacher> teachers = await _db.Teachers.Where(t => !t.IsDeactive).ToListAsync();
            return View(teachers);
        }
        public IActionResult Detail()
        {
            return View();
        }
    }
}
