using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Musical_Theatre.Constants;
using Musical_Theatre.Models;
using Musical_Theatre.Services.Interfaces;
using MySql.Data.MySqlClient;
using System;

namespace Musical_Theatre.Controllers
{
    public class PerformancesController : Controller
    {
        private readonly IPerformanceService _performanceService;
        private readonly IHallService _hallService;
        public PerformancesController(IPerformanceService performanceService, IHallService hallService)
        {
            _performanceService = performanceService;
            _hallService = hallService;
        }

        // GET: Performances
        public IActionResult Index()
        {
            try
            {
                var performances = _performanceService.GetPerformances();
                return View(performances);
            }
            catch (ArgumentNullException exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.EmptyPerformances));
            }
            catch (MySqlException exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.AccsessingError));
            }
        }

        // GET: Performances/Details/5
        public IActionResult Details(int id)
        {
            if (_performanceService.GetPerformances() == null)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.EmptyPerformances));
            }
            if (id == null)
            {
                return RedirectToAction(nameof(HomeController.Error), "Id is null");
            }
            try
            {
                var performance = _performanceService.GetPerformanceById(id);
                return View(performance);
            }
            catch (ArgumentNullException exception)
            {

                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.EmptyPerformance));
                
            }
            catch (MySqlException exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.AccsessingError));
            }

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
        public IActionResult Create([Bind("Name,HallId,Details")] PerformanceViewModel performanceForm)
        {
            var hall = _hallService.GetHallById(performanceForm.HallId);
            if (hall != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        int entitiesWritten = _performanceService.AddPerformance(performanceForm);

                        if (entitiesWritten == 0)
                        return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.DataTransferError));

                        return RedirectToAction(nameof(Index));
                    }
                    catch (ArgumentNullException exception)
                    {
                        return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.EmptyPerformance));
                    }
                    catch (MySqlException exception)
                    {
                        return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.CreationError));
                    }

                }
            }
            ViewData["HallId"] = new SelectList(_hallService.GetHallData(), "Id", "Name", performanceForm.HallId);
            return View();
        }

        // GET: Performances/Edit/5
        public IActionResult Edit(int id)
        {
            if (_performanceService.GetPerformances() == null)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.EmptyPerformances));
            }

            try
            {
                var performance = _performanceService.GetPerformanceById(id);
                if (performance == null)
                {
                    return NotFound();
                }

                ViewData["HallId"] = new SelectList(_hallService.GetHallData(), "Id", "Name", performance.HallId);

                PerformanceViewModel performanceForm = new PerformanceViewModel();
                performanceForm.PerformanceId = performance.Id;
                performanceForm.Name = performance.Name;
                performanceForm.HallId = performance.HallId;
                performanceForm.Details = performance.Details;
                return View(performanceForm);
            }
            catch (ArgumentNullException exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.EmptyPerformance));
            }
            catch (MySqlException exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.EditingError));
            }
        }

        // POST: Performances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("PerformanceId,Name,HallId,Details")] PerformanceViewModel performanceForm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var oldPerformance = _performanceService.GetPerformanceById(id);
                    int entitiesWritten = _performanceService.EditPerformance(performanceForm, oldPerformance);

                    if (entitiesWritten == 0)
                        return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.DataTransferError));

                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentNullException exception)
                {
                    return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.EmptyPerformance));
                }
                catch (MySqlException exception)
                {
                    return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.EditingError));
                }
            }
            ViewData["HallId"] = new SelectList(_hallService.GetHallData(), "Id", "Name", performanceForm.HallId);
            return View(performanceForm);
        }

        // GET: Performances/Delete/5
        public IActionResult Delete(int id)
        {
            // TODO: remove context from controller
            if (_performanceService.GetPerformances() == null)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.EmptyPerformances));
            }
            try
            {
                var performance = _performanceService.GetPerformanceById(id);
                return View(performance);

            }
            catch (ArgumentNullException exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.EmptyPerformance));
            }
            catch (MySqlException exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.DeletionError));
            }



        }

        // POST: Performances/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_performanceService.GetPerformances() == null)
            {
                return Problem("Entity set 'Musical_TheatreContext.Performances'  is null.");
            }
            try
            {
                int entitiesWritten = _performanceService.DeletePerformance(id);
                if (entitiesWritten == 0)
                    return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.DataTransferError));

                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentNullException exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.EmptyPerformance));
            }
            catch (MySqlException exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.DeletionError));
            }


        }

    }
}
