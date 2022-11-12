using EduHome.DAL;
using EduHome.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PositionsController : Controller
    {
        private readonly AppDbContext _db;
        public PositionsController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            List<Position> positions = await _db.Positions.Include(x=>x.Teachers).ToListAsync();
            return View(positions);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Position position)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool isExist = await _db.Positions.AnyAsync(p => p.Name == position.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "This Name is already exist!");
                return View();
            }
            await _db.Positions.AddAsync(position);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Position dbPosition = await _db.Positions.FirstOrDefaultAsync(p => p.Id == id);
            if (dbPosition == null)
            {
                return BadRequest();
            }
            return View(dbPosition);
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Position dbPosition = await _db.Positions.FirstOrDefaultAsync(p => p.Id == id);
            if (dbPosition == null)
            {
                return BadRequest();
            }
            return View(dbPosition);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Position position)
        {
            if (id == null)
            {
                return NotFound();
            }
            Position dbPosition = await _db.Positions.FirstOrDefaultAsync(p => p.Id == id);
            if (dbPosition == null)
            {
                return BadRequest();
            }
            bool isExist = await _db.Positions.AnyAsync(p => p.Name == position.Name && p.Id != id);
            if (isExist)
            {
                ModelState.AddModelError("Name", "This Name is already exist!");
                return View();
            }
            dbPosition.Name = position.Name;
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Position dbPosition = await _db.Positions.FirstOrDefaultAsync(p => p.Id == id);
            if (dbPosition == null)
            {
                return BadRequest();
            }
            return View(dbPosition);
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Position dbPosition = await _db.Positions.FirstOrDefaultAsync(p => p.Id == id);
            if (dbPosition == null)
            {
                return BadRequest();
            }
            dbPosition.IsDeactive = true;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Activation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Position dbPosition = await _db.Positions.FirstOrDefaultAsync(p => p.Id == id);
            if (dbPosition == null)
            {
                return BadRequest();
            }
            if (dbPosition.IsDeactive)
            {
                dbPosition.IsDeactive = false;
            }
            else
            {
                dbPosition.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
