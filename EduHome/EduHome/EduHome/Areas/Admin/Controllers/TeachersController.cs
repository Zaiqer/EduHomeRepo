using EduHome.DAL;
using EduHome.Helpers;
using EduHome.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TeachersController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public TeachersController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            //ViewBag.Positions = await _db.Positions.ToListAsync();
            List<Teacher> teachers = await _db.Teachers.Include(x => x.Position).ToListAsync();
            return View(teachers);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Positions = await _db.Positions.ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Teacher teacher, int? posId)
        {
            ViewBag.Positions = await _db.Positions.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (posId == null)
            {
                ModelState.AddModelError("FullName", "Position Error");
                return View();
            }
            Position position = await _db.Positions.FirstOrDefaultAsync(x => x.Id == posId);
            if (position == null)
            {
                ModelState.AddModelError("FullName", "Position Error");
                return View();
            }
            if (teacher.Photo == null)
            {
                ModelState.AddModelError("Photo", "Image can not be null!");
                return View();
            }
            if (!teacher.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Please select image file!");
                return View();
            }
            if (teacher.Photo.OlderThreeMb())
            {
                ModelState.AddModelError("Photo", "Photo more than 3 MB!");
                return View();
            }
            string path = Path.Combine(_env.WebRootPath, "img", "teacher");
            teacher.Image = await teacher.Photo.SaveFileAsync(path);
            teacher.PositionId = (int)posId;
            await _db.Teachers.AddAsync(teacher);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            ViewBag.Positions = await _db.Positions.ToListAsync();
            if (id == null)
            {
                return NotFound();
            }
            Teacher dbTeacher = await _db.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (dbTeacher == null)
            {
                return BadRequest();
            }
            return View(dbTeacher);
        }
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Positions = await _db.Positions.ToListAsync();

            if (id == null)
            {
                return NotFound();
            }
            Teacher dbTeacher = await _db.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (dbTeacher == null)
            {
                return BadRequest();
            }
            return View(dbTeacher);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Teacher teacher, int? posId)
        {
            ViewBag.Positions = await _db.Positions.ToListAsync();
            if (id == null)
            {
                return NotFound();
            }
            Teacher dbTeacher = await _db.Teachers.FirstOrDefaultAsync(x => x.Id == id);
            if (dbTeacher == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(dbTeacher);
            }
            if (posId == null)
            {
                ModelState.AddModelError("FullName", "Position Error");
                return View();
            }
            Position dbPosition = await _db.Positions.FirstOrDefaultAsync(x => x.Id == posId);
            if (dbPosition == null)
            {
                ModelState.AddModelError("FullName", "Position Error");
                return View();
            }
            if (teacher.Photo != null)
            {
                if (!teacher.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please select image file!");
                    return View(dbTeacher);
                }
                if (teacher.Photo.OlderThreeMb())
                {
                    ModelState.AddModelError("Photo", "Photo more than 3 MB!");
                    return View(dbTeacher);
                }
                string path = Path.Combine(_env.WebRootPath, "img", "teacher");
                if (System.IO.File.Exists(Path.Combine(path, dbTeacher.Image)))
                {
                    System.IO.File.Delete(Path.Combine(path, dbTeacher.Image));
                }
                dbTeacher.Image = await teacher.Photo.SaveFileAsync(path);
            }
            dbTeacher.FullName = teacher.FullName;
            dbTeacher.PositionId = (int)posId;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Activation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Teacher dbTeacher = await _db.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (dbTeacher == null)
            {
                return BadRequest();
            }
            if (dbTeacher.IsDeactive)
            {
                dbTeacher.IsDeactive = false;
            }
            else
            {
                dbTeacher.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
