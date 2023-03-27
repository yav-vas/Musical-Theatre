using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Data.Context;
using Musical_Theatre.Data.Models;
using Musical_Theatre.Models;

namespace Musical_Theatre.Services
{
    public class TicketService
    {
        private readonly Musical_TheatreContext _context;
        public TicketService(Musical_TheatreContext context)
        {
            this._context = context;
        }
        public int BuyTicket(int? id, TicketViewModel ticketForm)
        {
            Performance performance = _context.Performances.FirstOrDefault(p => p.Id == id);

            Ticket ticket = new Ticket();

            Seat chosenseat = _context.Seats.FirstOrDefault(s => s.PerformanceId == id && s.SeatNumber == ticketForm.SeatNumber && s.Row == ticketForm.Row);

            ticket.Seat = chosenseat;
            ticket.SeatId = chosenseat.Id;

            chosenseat.Ticket = ticket;

            _context.Tickets.Add(ticket);
            int entitieswritten = _context.SaveChanges();
            return entitieswritten;
        }
        public List<Ticket> GetTickets()
        {
            if (_context.Tickets == null)
                throw new ArgumentNullException("Entity Tickets is null!");

            List<Ticket> tickets = _context.Tickets.Include(t => t.Seat).ThenInclude(s=> s.Performance).ToList();
            return tickets;
        }
    }
}
