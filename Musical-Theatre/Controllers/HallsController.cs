using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Data;
using Musical_Theatre.Data.Context;

namespace Musical_Theatre.Controllers
{
    public class HallsController : Controller
    {
        private readonly Musical_TheatreContext _context;

        public HallsController(Musical_TheatreContext context)
        {
            _context = context;
        }

        // GET: Halls
        public async Task<IActionResult> Index()
        {
              return _context.Halls != null ? 
                          View(await _context.Halls.ToListAsync()) :
                          Problem("Entity set 'Musical_TheatreContext.Hall'  is null.");
        }

        // GET: Halls/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Halls == null)
            {
                return NotFound();
            }

            var hall = await _context.Halls
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hall == null)
            {
                return NotFound();
            }

            return View(hall);
        }

        // GET: Halls/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Halls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Rows,Columns")] Hall hall)
        {
            if (ModelState.IsValid)
            {
                hall.DateCreated = DateTime.Now;
                _context.Add(hall);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hall);
        }

        // GET: Halls/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Halls == null)
            {
                return NotFound();
            }

            var hall = await _context.Halls.FindAsync(id);
            if (hall == null)
            {
                return NotFound();
            }
            return View(hall);
        }

        // POST: Halls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Rows,Columns")] Hall hall)
        {
            if (id != hall.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var currentHall = await _context.Halls.FindAsync(id);
                    var dateCreated = currentHall.DateCreated;

                    if (currentHall != null)
                    {
                        _context.Entry(currentHall).State = EntityState.Detached;
                    }

                    hall.DateCreated = dateCreated;
                    _context.Update(hall);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HallExists(hall.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(hall);
        }

        // GET: Halls/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Halls == null)
            {
                return NotFound();
            }

            var hall = await _context.Halls
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hall == null)
            {
                return NotFound();
            }

            return View(hall);
        }

        // POST: Halls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Halls == null)
            {
                return Problem("Entity set 'Musical_TheatreContext.Hall'  is null.");
            }
            var hall = await _context.Halls.FindAsync(id);
            if (hall != null)
            {
                _context.Halls.Remove(hall);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HallExists(int id)
        {
          return (_context.Halls?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
