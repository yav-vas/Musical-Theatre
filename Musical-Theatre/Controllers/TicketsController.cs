using Microsoft.AspNetCore.Mvc;
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
            var result = _ticketService.GetTickets();
            return View(result);
        }

        public IActionResult Buy(int id)
        {
            Performance performance = _performanceService.GetPerformanceById(id);
            string name = performance.Name;

            Hall hall = performance.Hall;

            List<List<Seat>> seats = new List<List<Seat>>();

            for (int row = 1; row <= hall.Rows; row++)
            {
                seats.Add(new List<Seat>());
                for (int column = 1; column <= hall.Columns; column++)
                {
                    var seat = _seatService.GetSeatByRowAndColumnAndPerformance(row, column, performance);
                    seats.ElementAt(row - 1).Add(seat);
                }
            }

            return View(new TicketViewModel(name, seats, hall));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Buy(int id, [Bind("Row,SeatNumber")] TicketViewModel ticketForm)
        {
            try
            {
                int entitiesWritten = _ticketService.BuyTicket(id, ticketForm);
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
