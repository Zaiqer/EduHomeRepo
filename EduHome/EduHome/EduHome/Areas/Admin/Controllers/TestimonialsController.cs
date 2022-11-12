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
    public class TestimonialsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public TestimonialsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Testimonial> testimonials = await _db.Testimonials.ToListAsync();
            return View(testimonials);
        }

        #region Detail
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Testimonial dbTestimonial = await _db.Testimonials.FirstOrDefaultAsync(x => x.Id == id);
            if (dbTestimonial == null)
            {
                return BadRequest();
            }
            return View(dbTestimonial);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Testimonial testimonial)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (testimonial.Photo == null)
            {
                ModelState.AddModelError("Photo", "Image can not be null!");
                return View();
            }
            if (!testimonial.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Please select image file!");
                return View();
            }
            if (testimonial.Photo.OlderThreeMb())
            {
                ModelState.AddModelError("Photo", "Photo more than 1 MB!");
                return View();
            }
            string path = Path.Combine(_env.WebRootPath, "img", "testimonial");
            testimonial.Image = await testimonial.Photo.SaveFileAsync(path);
            await _db.Testimonials.AddAsync(testimonial);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Testimonial dbTestimonial = await _db.Testimonials.FirstOrDefaultAsync(x => x.Id == id);
            if (dbTestimonial == null)
            {
                return BadRequest();
            }
            return View(dbTestimonial);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Testimonial testimonial)
        {
            if (id == null)
            {
                return NotFound();
            }
            Testimonial dbTestimonial = await _db.Testimonials.FirstOrDefaultAsync(x => x.Id == id);
            if (dbTestimonial == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(dbTestimonial);
            }
            if (testimonial.Photo != null)
            {
                if (!testimonial.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please select image file!");
                    return View(dbTestimonial);
                }
                if (testimonial.Photo.OlderThreeMb())
                {
                    ModelState.AddModelError("Photo", "Photo more than 3 MB!");
                    return View(dbTestimonial);
                }
                string path = Path.Combine(_env.WebRootPath, "img", "testimonial");
                if (System.IO.File.Exists(Path.Combine(path, dbTestimonial.Image)))
                {
                    System.IO.File.Delete(Path.Combine(path, dbTestimonial.Image));
                }
                dbTestimonial.Image = await testimonial.Photo.SaveFileAsync(path);
            }
            dbTestimonial.Title = testimonial.Title;
            dbTestimonial.Name = testimonial.Name;
            dbTestimonial.Position = testimonial.Position;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Activation
        public async Task<IActionResult> Activation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Testimonial dbTestimonial = await _db.Testimonials.FirstOrDefaultAsync(t => t.Id == id);
            if (dbTestimonial == null)
            {
                return BadRequest();
            }
            if (dbTestimonial.IsDeactive)
            {
                dbTestimonial.IsDeactive = false;
            }
            else
            {
                dbTestimonial.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
