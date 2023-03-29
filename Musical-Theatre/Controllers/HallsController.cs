using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Services;
using MySql.Data.MySqlClient;
using Musical_Theatre.Data.Models;

namespace Musical_Theatre.Controllers
{
    public class HallsController : Controller
    {
        private readonly HallService _hallService;

        public HallsController(HallService hallService)
        {
            _hallService = hallService;
        }

        // GET: Halls
        public IActionResult Index()
        {
            try
            {
                var halls =  _hallService.GetHalls();
                return View(halls);
            }
            catch (ArgumentNullException exception)
            {
                return NotFound(exception.Message); // TODO: RedirectToAction(nameof(Error))
            }
            catch (MySqlException exception)
            {
                return NotFound(exception.Message);
            }
        }

        // GET: Halls/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound("Id is null");
            }

            try
            {
                var hall =  _hallService.GetHallById(id);
                return View(hall);
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

        // GET: Halls/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Halls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Create(Hall hall)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int entitiesWritten =  _hallService.AddHall(hall);

                    if (entitiesWritten == 0)
                        return NotFound("No entities were written to the database!");

                    return RedirectToAction(nameof(Index));
                } 
                catch (ArgumentNullException exception)
                {
                    return NotFound(exception.Message);
                }
                catch (ArgumentException exception)
                {
                    return NotFound(exception.Message);
                }
                catch (MySqlException exception)
                {
                    return NotFound(exception.Message);
                }
            }
            return View(hall);

        }

        // GET: Halls/EditHall/5
        public IActionResult Edit(int? id)
        {
            try
            {
                var hall =  _hallService.GetHallById(id);
                return View(hall);
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

        // POST: Halls/EditHall/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Edit(int id, [Bind("Id,Name,Rows,Columns")] Hall hall)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int entitiesWritten =  _hallService.EditHall(id, hall);

                    if (entitiesWritten == 0)
                        return NotFound("No entites were written to the database!");

                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException exception)
                {
                    return NotFound(exception.Message);
                }
                catch (MySqlException exception)
                {
                    return NotFound(exception.Message);
                }
            }
            return View(hall);
        }

        // GET: Halls/Delete/5
        public IActionResult Delete(int? id)
        {
            try
            {
                var hall =  _hallService.GetHallById(id);
                return View(hall);
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

        // POST: Halls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                int entitiesWritten =  _hallService.DeleteHall(id);

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
}
