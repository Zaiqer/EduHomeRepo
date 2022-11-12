using EduHome.DAL;
using EduHome.Helpers;
using EduHome.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    public class SlidersController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public SlidersController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _db.Sliders.ToListAsync();
            return View(sliders);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Slider dbSlider = await _db.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (dbSlider == null)
            {
                return BadRequest();
            }
            return View(dbSlider);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (slider.Photo == null)
            {
                ModelState.AddModelError("Photo", "Image can not be null!");
                return View();
            }
            if (!slider.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Please select image file!");
                return View();
            }
            if (slider.Photo.OlderThreeMb())
            {
                ModelState.AddModelError("Photo", "Photo more than 1 MB!");
                return View();
            }
            string path = Path.Combine(_env.WebRootPath, "img", "slider");
            slider.Image = await slider.Photo.SaveFileAsync(path);
            await _db.Sliders.AddAsync(slider);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Slider dbSlider = await _db.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (dbSlider == null)
            {
                return BadRequest();
            }
            return View(dbSlider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Slider slider)
        {
            if (id == null)
            {
                return NotFound();
            }
            Slider dbSlider = await _db.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (dbSlider == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(dbSlider);
            }
            if (slider.Photo!=null)
            {
                if (!slider.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please select image file!");
                    return View(dbSlider);
                }
                if (slider.Photo.OlderThreeMb())
                {
                    ModelState.AddModelError("Photo", "Photo more than 3 MB!");
                    return View(dbSlider);
                }
                string path = Path.Combine(_env.WebRootPath, "img", "slider");
                if (System.IO.File.Exists(Path.Combine(path, dbSlider.Image)))
                {
                    System.IO.File.Delete(Path.Combine(path, dbSlider.Image));
                }
                dbSlider.Image = await slider.Photo.SaveFileAsync(path);
            }
            dbSlider.Title = slider.Title;
            dbSlider.SubTitle = slider.SubTitle;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Activation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Slider dbSlider = await _db.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (dbSlider == null)
            {
                return BadRequest();
            }
            if (dbSlider.IsDeactive)
            {
                dbSlider.IsDeactive = false;
            }
            else
            {
                dbSlider.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
