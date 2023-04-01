using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Data.Models;

namespace Musical_Theatre.Repositories.Interfaces
{
    public interface ISeatRepository
    {
        public int GetCount();
        public List<Seat> GetAll();
        public Seat GetByRowAndColumnAndPerformance(int row, int column, Performance performance);
        public Seat GetById(int id);
        public List<Seat> GetAllWithHallAndPerformance();
        public int Add(Seat seat);
        public int RemoveRangeOfSeats(int performanceId, int row, int column);
        public List<Seat> GetAllSeatsForPerformance(Performance performance);
    }
}
