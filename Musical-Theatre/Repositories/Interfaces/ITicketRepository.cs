using Musical_Theatre.Data.Models;

namespace Musical_Theatre.Repositories.Interfaces
{
    public interface ITicketRepository
    {
        IEnumerable<Ticket> GetAllWithSeatAndPerformance();
        int Add(Ticket entity);
        int Remove(Ticket entity);
        IEnumerable<Ticket> GetAll();
    }
}
