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
            string name = _context.Performances.FirstOrDefault(p => p.Id == id)?.Name;
            return View(new TicketViewModel(name));
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
