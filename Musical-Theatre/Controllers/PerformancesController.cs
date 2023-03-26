using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Data.Context;
using Musical_Theatre.Data.Models;
using Musical_Theatre.Models;
using Musical_Theatre.Services;
using MySql.Data.MySqlClient;

namespace Musical_Theatre.Controllers
{
    public class PerformancesController : Controller
    {
        private readonly Musical_TheatreContext _context;
        private readonly PerformanceService _performanceService;
        public PerformancesController(Musical_TheatreContext context, PerformanceService performanceService)
        {
            _context = context;
            _performanceService = performanceService;
        }

        // GET: Performances
        public async Task<IActionResult> Index()
        {
            try
            {
                var performances = await _performanceService.GetPerformances();
                return View(performances);
            }
            catch (ArgumentNullException exception)
            {
                return NotFound(exception.Message);
            }
            catch (MySqlException exception)
            {
                return NotFound(exception.Message);
            }
        }

        // GET: Performances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Performances == null)
            {
                return NotFound();
            }

            var performance = await _performanceService.GetPerformanceById(id);
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
        public async Task<IActionResult> Create([Bind("Name,HallId,Details")] PerformanceViewModel performanceForm)
        {
            Hall hall = _context.Halls.FirstOrDefault(h => h.Id == performanceForm.HallId);
            if (hall != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        int entitiesWritten = await _performanceService.AddPerformance(performanceForm);

                        if (entitiesWritten == 0)
                            return NotFound("No entities were written to the database!");

                        return RedirectToAction(nameof(Index));
                    }
                    catch (ArgumentNullException exception)
                    {
                        return NotFound(exception.Message);
                    }
                    catch (MySqlException exception)
                    {
                        return NotFound(exception.Message);
                    }

                }
            }
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Name", performanceForm.HallId);
            return View();
        }

            // GET: Performances/Edit/5
            public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Performances == null)
            {
                return NotFound();
            }

            var performance = await _performanceService.GetPerformanceById(id);
            if (performance == null)
            {
                return NotFound();
            }
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Name", performance.HallId);

            PerformanceViewModel performanceForm = new PerformanceViewModel();
            performanceForm.Name = performance.Name;
            performanceForm.HallId = performance.HallId;
            performanceForm.Details = performance.Details;
            return View(performanceForm);
        }

        // POST: Performances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,HallId,Details")] PerformanceViewModel performanceForm)
        {
            // TODO: _context must be used only in service; move getting performance in service
            Performance performance = _context.Performances.FirstOrDefault(p => p.Id == id);
            if (ModelState.IsValid)
            {
                try
                {
                    int entitiesWritten = await _performanceService.EditPerformance(performanceForm, performance);

                    if (entitiesWritten == 0)
                        return NotFound("No entites were written to the database!"); // TODO: simpler error

                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentNullException exception)
                {
                    return NotFound(exception.Message);
                }
                catch (MySqlException exception)
                {
                    return NotFound(exception.Message);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerformanceExists(performanceForm.PerformanceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Name", performanceForm.HallId);
            return View(performanceForm);
        }

        // GET: Performances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // TODO: remove context from controller
            if (id == null || _context.Performances == null)
            {
                return NotFound();
            }

            var performance = await _performanceService.GetPerformanceById(id);
            if (performance == null)
            {
                return NotFound();
            }

            return View(performance);
        }

        // POST: Performances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] // TODO: remove
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Performances == null)
            {
                return Problem("Entity set 'Musical_TheatreContext.Performances'  is null.");
            }
           int entitiesWritten = await _performanceService.DeletePerformance(id);
            if (entitiesWritten == 0) {

                return NotFound("No entites were removed from the database!");
            }
           return RedirectToAction(nameof(Index));
           
        }

        private bool PerformanceExists(int id)
        {
          return (_context.Performances?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
