using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Data.Context;
using Musical_Theatre.Data.Models;
using Musical_Theatre.Models;

namespace Musical_Theatre.Controllers
{
    public class TicketsController : Controller
    {
        private readonly Musical_TheatreContext _context;
        public TicketsController(Musical_TheatreContext context) 
        {
            _context = context;
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
        public IActionResult Buy(int? id, [Bind("Row,SeatNumber")] TicketViewModel ticketForm)
        {
            Performance performance = _context.Performances.FirstOrDefault(p => p.Id == id);

            Ticket ticket = new Ticket();

            Seat chosenSeat = _context.Seats.FirstOrDefault(s => s.PerformanceId == id && s.SeatNumber == ticketForm.SeatNumber && s.Row == ticketForm.Row);

            ticket.Seat = chosenSeat;
            ticket.SeatId = chosenSeat.Id;

            chosenSeat.Ticket = ticket;

            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
