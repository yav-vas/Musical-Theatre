using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Data.Context;
using Musical_Theatre.Data.Models;
using Musical_Theatre.Models;
using Musical_Theatre.Repositories;
using Musical_Theatre.Repositories.Interfaces;

namespace Musical_Theatre.Services
{
    public class TicketService
    {
        private readonly Musical_TheatreContext _context;
        private readonly IPerformanceRepository performanceRepository;
        private readonly ITicketRepository ticketRepository;
        public TicketService(Musical_TheatreContext context, IPerformanceRepository performanceRepository, ITicketRepository ticketRepository)
        {
            this._context = context;
            this.performanceRepository = performanceRepository;
            this.ticketRepository = ticketRepository;
        }

        public List<Ticket> GetTickets()
        {
            if (_context.Tickets == null)
                throw new ArgumentNullException("Entity Tickets is null!");

            List<Ticket> tickets = _context.Tickets.Include(t => t.Seat).ThenInclude(s => s.Performance).ToList();
            return tickets;
        }

        public int BuyTicket(int id, TicketViewModel ticketForm)
        {
            Performance performance = performanceRepository.GetByIdWithHall(id);

            Ticket ticket = new Ticket();

            Seat chosenSeat = _context.Seats.FirstOrDefault(s => s.PerformanceId == id && s.SeatNumber == ticketForm.SeatNumber && s.Row == ticketForm.Row);

            ticket.Seat = chosenSeat;

            chosenSeat.Ticket = ticket;

            ticketRepository.Add(ticket);

            int entitieswritten = _context.SaveChanges();
            return entitieswritten;
        }
    }
}
