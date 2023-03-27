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
        private readonly PerformanceService _performanceService;
        private readonly HallService _hallService;
        public PerformancesController(PerformanceService performanceService, HallService hallService)
        {
            _performanceService = performanceService;
            _hallService = hallService;
        }

        // GET: Performances
        public  IActionResult Index()
        {
            try
            {
                var performances =  _performanceService.GetPerformances();
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
        public IActionResult Details(int? id)
        {
            if (id == null || _performanceService.GetPerformances() == null)
            {
                return NotFound();
            }

            var performance =  _performanceService.GetPerformanceById(id);
            if (performance == null)
            {
                return NotFound();
            }

            return View(performance);
        }

        // GET: Performances/Create
        public IActionResult Create()
        {
            ViewData["HallId"] = new SelectList(_hallService.GetHallData(), "Id", "Name");
            return View();
        }

        // POST: Performances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Create([Bind("Name,HallId,Details")] PerformanceViewModel performanceForm)
        {
            var hall = _hallService.GetHallById(performanceForm.HallId);
            if (hall != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        int entitiesWritten =  _performanceService.AddPerformance(performanceForm);

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
            ViewData["HallId"] = new SelectList(_hallService.GetHallData(), "Id", "Name", performanceForm.HallId);
            return View();
        }

            // GET: Performances/Edit/5
            public IActionResult Edit(int? id)
        {
            if (id == null || _performanceService.GetPerformances == null)
            {
                return NotFound();
            }

            var performance =  _performanceService.GetPerformanceById(id);
            if (performance == null)
            {
                return NotFound();
            }
            ViewData["HallId"] = new SelectList(_hallService.GetHallData(), "Id", "Name", performance.HallId);

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
        public IActionResult Edit(int id, [Bind("Name,HallId,Details")] PerformanceViewModel performanceForm)
        {
            // TODO: _context must be used only in service; move getting performance in service
            var performance = _performanceService.GetPerformanceById(id);
            if (ModelState.IsValid)
            {
                try
                {
                    int entitiesWritten =  _performanceService.EditPerformance(performanceForm, performance);

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
                    if (!_performanceService.PerformanceExists(performanceForm.PerformanceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["HallId"] = new SelectList(_hallService.GetHallData(), "Id", "Name", performanceForm.HallId);
            return View(performanceForm);
        }

        // GET: Performances/Delete/5
        public IActionResult Delete(int? id)
        {
            // TODO: remove context from controller
            if (id == null || _performanceService.GetPerformances() == null)
            {
                return NotFound();
            }

            var performance =  _performanceService.GetPerformanceById(id);
            if (performance == null)
            {
                return NotFound();
            }

            return View(performance);
        }

        // POST: Performances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] // TODO: remove
        public IActionResult DeleteConfirmed(int id)
        {
            if (_performanceService.GetPerformances() == null)
            {
                return Problem("Entity set 'Musical_TheatreContext.Performances'  is null.");
            }
           int entitiesWritten =  _performanceService.DeletePerformance(id);
            if (entitiesWritten == 0) {

                return NotFound("No entites were removed from the database!");
            }
           return RedirectToAction(nameof(Index));
           
        }

    }
}
