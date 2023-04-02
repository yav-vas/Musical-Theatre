using Microsoft.AspNetCore.Mvc;
using Musical_Theatre.Constants;
using Musical_Theatre.Data.Models;
using Musical_Theatre.Models;
using Musical_Theatre.Services.Interfaces;
using MySql.Data.MySqlClient;

namespace Musical_Theatre.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly IPerformanceService _performanceService;
        private readonly ISeatService _seatService;
        public TicketsController(ITicketService ticketService, IPerformanceService performanceService, ISeatService seatService)
        {
            _ticketService = ticketService;
            _performanceService = performanceService;
            _seatService = seatService;
        }

        public IActionResult Index()
        {
            try
            {
                var result = _ticketService.GetTickets();
                return View(result);
            }
            catch (ArgumentNullException exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(exception.ParamName));
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

        public IActionResult Buy(int id)
        {
            try
            {
                Performance performance = _performanceService.GetPerformanceById(id);
                if (performance == null)
                {

                }
                string name = performance.Name;

                Hall hall = performance.Hall;

                List<List<Seat>> seats = new List<List<Seat>>();

                for (int row = 1; row <= hall.Rows; row++)
                {
                    seats.Add(new List<Seat>());
                    for (int column = 1; column <= hall.Columns; column++)
                    {
                        try
                        {
                            var seat = _seatService.GetSeatByRowAndColumnAndPerformance(row, column, performance);
                            seats.ElementAt(row - 1).Add(seat);

                        }
                        catch (ArgumentNullException exception)
                        {
                            return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.WrongSeatError));
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
                }

                return View(new TicketViewModel(name, seats, hall));
            }
            catch (ArgumentNullException exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.EmptyPerformance));
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Buy(int id, [Bind("Row,SeatNumber")] TicketViewModel ticketForm)
        {
            try
            {
                int entitiesWritten = _ticketService.BuyTicket(id, ticketForm);
                if (entitiesWritten == 0)
                    return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.DataTransferError));

                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException exception)
            {
                return View(ErrorMessages.ErrorViewFilePath, new ErrorViewModel(ErrorMessages.WrongSeatError));
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
