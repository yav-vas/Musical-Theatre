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
        private readonly Musical_TheatreContext _context;
        private readonly TicketService _ticketService;
        public TicketsController(Musical_TheatreContext context, TicketService ticketService) 
        {
            _context = context;
            _ticketService = ticketService;
        }

        public IActionResult Index()
        {
            var result = _context.Tickets.Include(t => t.Seat).ThenInclude(s => s.Performance);
            return View(result);
        }

        public IActionResult Buy(int? id)
        {
            Performance performance = _context.Performances.FirstOrDefault(p => p.Id == id);
            string name = performance.Name;

            Hall hall = _context.Performances.Include(p => p.Hall).FirstOrDefault(p => p.Id == id).Hall;

            List<List<Seat>> seats = new List<List<Seat>>();

            for (int i = 1; i <= hall.Rows; i++)
            {
                seats.Add(new List<Seat>());
                for (int j = 1; j <= hall.Columns; j++)
                {
                    var seat = _context.Seats.Include(s => s.Ticket).FirstOrDefault(s => s.PerformanceId == performance.Id && s.Row == i && s.SeatNumber == j);
                    seats.ElementAt(i - 1).Add(seat);
                }
            }

            return View(new TicketViewModel(name, seats, hall));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(int? id, [Bind("Row,SeatNumber")] TicketViewModel ticketForm)
        {
           
            
                try
                {
                    int entitiesWritten = await _ticketService.BuyTicket(id, ticketForm);
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
