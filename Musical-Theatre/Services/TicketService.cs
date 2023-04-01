using Musical_Theatre.Data.Models;
using Musical_Theatre.Models;
using Musical_Theatre.Repositories.Interfaces;
using Musical_Theatre.Services.Interfaces;

namespace Musical_Theatre.Services
{
    public class TicketService : ITicketService
    {
        private readonly IPerformanceRepository performanceRepository;
        private readonly ITicketRepository ticketRepository;
        private readonly ISeatRepository seatRepository;

        public TicketService(IPerformanceRepository performanceRepository, ITicketRepository ticketRepository, ISeatRepository seatRepository)
        {
            this.performanceRepository = performanceRepository;
            this.ticketRepository = ticketRepository;
            this.seatRepository = seatRepository;
        }

        public List<Ticket> GetTickets()
        {
            if (ticketRepository.GetAll() == null)
                throw new ArgumentNullException("Entity Tickets is null!");

            List<Ticket> tickets = ticketRepository.GetAllWithSeatAndPerformance().ToList();
            return tickets;
        }

        public int BuyTicket(int id, TicketViewModel ticketForm)
        {
            Performance performance = performanceRepository.GetByIdWithHall(id);

            Ticket ticket = new Ticket();

            Seat chosenSeat = seatRepository.GetByRowAndColumnAndPerformance(ticketForm.Row, ticketForm.SeatNumber, performance);

            if (chosenSeat == null)
            {
                throw new ArgumentException("Seat does not exist! Check the hall layout again!");
            }

            ticket.Seat = chosenSeat;

            if (chosenSeat.Ticket != null)
            {
                throw new ArgumentException("Seat already taken! Check the hall layout again!");
            }

            chosenSeat.Ticket = ticket;

            int entitieswritten = ticketRepository.Add(ticket);
            return entitieswritten;
        }
    }
}
