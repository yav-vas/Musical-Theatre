using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Data.Context;
using Musical_Theatre.Data.Models;
using Musical_Theatre.Repositories.Interfaces;

namespace Musical_Theatre.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        public readonly Musical_TheatreContext context;
        public SeatRepository(Musical_TheatreContext context) 
        { 
           this.context= context;
        
        }
        public int Add(Seat seat)
        {
           context.Seats.Add(seat);
            return context.SaveChanges();
        }

        public List<Seat> GetAll()
        {
            return context.Seats.ToList();
        }

        public List<Seat> GetAllWithHallAndPerformance()
        {
            return context.Seats.Include(s=> s.Performance).ThenInclude(p=> p.Hall).ToList();
        }
        public List<Seat> GetAllSeatsForPerformance(int id) { 
        return context.Seats.Where(s => s.PerformanceId == id).ToList();
        }

        public Seat GetById(int id)
        {
            Seat seat = context.Seats.FirstOrDefault(s => s.Id == id);
            return seat;
        }

        public Seat GetByRowAndColumnAndPerformance(int row, int column, Performance performance)
        {
            Seat seat = context.Seats.Include(s => s.Ticket).FirstOrDefault(s => s.Performance == performance && s.Row == row && s.SeatNumber == column);
            return seat;
        }

        public int GetCount()
        {
           return context.Seats.Count();
        }

        public int RemoveRangeOfSeats(int performanceId, int row, int column)
        {
           context.Seats.RemoveRange(context.Seats.Where(s => s.Row == row && s.SeatNumber == column && s.PerformanceId == performanceId));
          return  context.SaveChanges();
        }
    }
}
