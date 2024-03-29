﻿using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Musical_Theatre.Data.Models;
using Musical_Theatre.Services.Interfaces;
using Musical_Theatre.Models;
using Musical_Theatre.Constants;

namespace Musical_Theatre.Controllers
{
    public class HallsController : Controller
    {
        private readonly IHallService _hallService;

        public HallsController(IHallService hallService)
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
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.EmptyHalls));
            }
            catch (MySqlException exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.AccsessingError));
            }
            catch (Exception exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.UnknownError));
            }
        }

        // GET: Halls/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel("Id is null"));
            }

            try
            {
                var hall =  _hallService.GetHallById((int)id);
                return View(hall);
            }
            catch (ArgumentNullException exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.EmptyHall));
            }
            catch (MySqlException exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.AccsessingError));
            }
            catch (Exception exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.UnknownError));
            }
        }

        // GET: Halls/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Hall hall)
        {
            if (hall == null)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel("Emtpy hall given!"));
            }

            try
            {
                if (ModelState.IsValid)
                {
                    int entitiesWritten = _hallService.AddHall(hall);

                    if (entitiesWritten == 0)
                        return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.DataTransferError));

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (ArgumentNullException exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.EmptyHall));
            }
            catch (MySqlException exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.CreationError));
            }
            catch (Exception exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.UnknownError));
            }

            return View(hall);
        }

        // GET: Halls/EditHall/5
        public IActionResult Edit(int id)
        {
            try
            {
                var hall =  _hallService.GetHallById(id);
                return View(hall);
            }
            catch (ArgumentNullException exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.EmptyHall));
            }
            catch (MySqlException exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.EditingError));
            }
            catch (Exception exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.UnknownError));
            }
        }

        [HttpPost]
        // TODO: validate id with validation property
        public  IActionResult Edit(int id, [Bind("Id,Name,Rows,Columns")] Hall hall)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int entitiesWritten =  _hallService.EditHall(id, hall);

                    if (entitiesWritten == 0)
                        return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.DataTransferError));

                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException exception)
                {
                    return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.EmptyHall));
                }
                catch (MySqlException exception)
                {
                    return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.EditingError));
                }
                catch (Exception exception)
                {
                    return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.UnknownError));
                }
            }
            return View(hall);
        }

        // GET: Halls/Delete/5
        public IActionResult Delete(int id)
        {
            try
            {
                var hall =  _hallService.GetHallById(id);
                return View(hall);
            }
            catch (ArgumentNullException exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.EmptyHall));
            }
            catch (MySqlException exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.DeletionError));
            }
            catch (Exception exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.UnknownError));
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
                    return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.DataTransferError));

                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentNullException exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.EmptyHall));
            }
            catch (MySqlException exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.DeletionError));
            }
            catch (Exception exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.UnknownError));
            }
        }
    }
}
