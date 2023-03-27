using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Data.Context;
using Musical_Theatre.Data.Models;
using Musical_Theatre.Models;
using Musical_Theatre.Services;
using MySql.Data.MySqlClient;

namespace Musical_Theatre.Controllers
{
    public class TicketsController : Controller
    {
        private readonly TicketService _ticketService;
        private readonly PerformanceService _performanceService;
        private readonly SeatService _seatService;
        public TicketsController(TicketService ticketService, PerformanceService performanceService, SeatService seatService) 
        {
            _ticketService = ticketService;
            _performanceService= performanceService;
            _seatService= seatService;
        }

        public IActionResult Index()
        {
            var result = _ticketService.GetTickets();
            return View(result);
        }

        public IActionResult Buy(int? id)
        {
            Performance performance = _performanceService.GetPerformanceById(id);
            string name = performance.Name;

            Hall hall = _performanceService.GetPerformanceHall(performance.HallId);

            List<List<Seat>> seats = new List<List<Seat>>();

            for (int row = 1; row <= hall.Rows; row++)
            {
                seats.Add(new List<Seat>());
                for (int column = 1; column <= hall.Columns; column++)
                {
                    var seat = _seatService.GetSeatByRowAndColumn(row, column);
                    seats.ElementAt(row - 1).Add(seat);
                }
            }

            return View(new TicketViewModel(name, seats, hall));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Buy(int? id, [Bind("Row,SeatNumber")] TicketViewModel ticketForm)
        {
           
            
                try
                {
                    int entitiesWritten =  _ticketService.BuyTicket(id, ticketForm);
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
