using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Data.Context;
using Musical_Theatre.Data.Models;
using Musical_Theatre.Repositories.Interfaces;

namespace Musical_Theatre.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly Musical_TheatreContext context;

        public TicketRepository(Musical_TheatreContext context)
        {
            this.context = context;
        }

        public void Add(Ticket entity)
        {
            context.Tickets.Add(entity);
        }

        public IEnumerable<Ticket> GetAllWithSeatAndPerformance()
            => context.Tickets.Include(t => t.Seat).ThenInclude(s => s.Performance).ToList();

        public int Remove(Ticket entity)
        {
            context.Tickets.Remove(entity);
            return context.SaveChanges();
        }
    }
}
