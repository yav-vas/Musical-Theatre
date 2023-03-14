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
    public class PerformancesController : Controller
    {
        private readonly Musical_TheatreContext _context;

        public PerformancesController(Musical_TheatreContext context)
        {
            _context = context;
        }

        // GET: Performances
        public async Task<IActionResult> Index()
        {
            var musical_TheatreContext = _context.Performances.Include(p => p.Hall);
            return View(await musical_TheatreContext.ToListAsync());
        }

        // GET: Performances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Performances == null)
            {
                return NotFound();
            }

            var performance = await _context.Performances
                .Include(p => p.Hall)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (performance == null)
            {
                return NotFound();
            }

            return View(performance);
        }

        // GET: Performances/Create
        public IActionResult Create()
        {
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Name");
            return View();
        }

        // POST: Performances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Details")] Performance performance)
        {
            Hall hall =  _context.Halls.FirstOrDefault(h=> h.Id == performance.HallId);
            if (hall!= null)
            {
                performance.Hall = hall;
                if (ModelState.IsValid)
                {
                    _context.Add(performance);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Name", performance.Hall.Name);
            return View(performance);
        }

        // GET: Performances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Performances == null)
            {
                return NotFound();
            }

            var performance = await _context.Performances.FindAsync(id);
            if (performance == null)
            {
                return NotFound();
            }
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Name", performance.HallId);
            return View(performance);
        }

        // POST: Performances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,HallId,Details")] Performance performance)
        {
            if (id != performance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(performance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerformanceExists(performance.Id))
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
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Name", performance.HallId);
            return View(performance);
        }

        // GET: Performances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Performances == null)
            {
                return NotFound();
            }

            var performance = await _context.Performances
                .Include(p => p.Hall)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (performance == null)
            {
                return NotFound();
            }

            return View(performance);
        }

        // POST: Performances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Performances == null)
            {
                return Problem("Entity set 'Musical_TheatreContext.Performances'  is null.");
            }
            var performance = await _context.Performances.FindAsync(id);
            if (performance != null)
            {
                _context.Performances.Remove(performance);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PerformanceExists(int id)
        {
          return (_context.Performances?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
