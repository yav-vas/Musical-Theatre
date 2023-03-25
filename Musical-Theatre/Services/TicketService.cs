using Microsoft.AspNetCore.Mvc;
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
        public async Task<int> BuyTicket(int? id, TicketViewModel ticketForm)
        {
            Performance performance = _context.Performances.FirstOrDefault(p => p.Id == id);

            Ticket ticket = new Ticket();

            Seat chosenseat = _context.Seats.FirstOrDefault(s => s.PerformanceId == id && s.SeatNumber == ticketForm.SeatNumber && s.Row == ticketForm.Row);

            ticket.Seat = chosenseat;
            ticket.SeatId = chosenseat.Id;

            chosenseat.Ticket = ticket;

            _context.Tickets.Add(ticket);
            int entitieswritten = await _context.SaveChangesAsync();
            return entitieswritten;
        }
    }
}
