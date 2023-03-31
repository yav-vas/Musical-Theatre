using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Data.Context;
using Musical_Theatre.Data.Models;

namespace Musical_Theatre.Services
{
    public class SeatService
    {
        private readonly Musical_TheatreContext _context;
        public SeatService(Musical_TheatreContext context)
        {

            _context = context;

        }

        public List<Seat> GetSeats()
        {
            if (_context.Seats == null)
                throw new ArgumentNullException("Entity Seats is null!");

            List<Seat> seats = _context.Seats.Include(s => s.Performance).ThenInclude(p => p.Hall).ToList();
            return seats;
        }

        public Seat? GetSeatById(int? id)
        {
            if (id == null)
                throw new ArgumentNullException("Id is null");

            if (_context.Seats == null)
                throw new ArgumentNullException("Entity Seats is null!");

            var seat = _context.Seats.Include(s => s.Performance).ThenInclude(p => p.Hall).FirstOrDefault(p => p.Id == id);

            if (seat == default)
                throw new ArgumentNullException("Seat with id " + id + " not found!");

            return seat;
        }

        public Seat GetSeatByRowAndColumnAndPerformance(int? row, int? column, Performance? performance)
        {
            if (row == null)
                throw new ArgumentNullException("row is null");
            else if (column == null)
                throw new ArgumentNullException("column is null");
            else if (performance == null)
                throw new ArgumentNullException("performance is null");

            if (_context.Seats == null)
                throw new ArgumentNullException("Entity Seats is null!");

            var seat = _context.Seats.Include(s => s.Ticket).FirstOrDefault(s => s.Performance == performance && s.Row == row && s.SeatNumber == column);


            if (seat == default)
                throw new ArgumentNullException($"Seat with row {row} and seat number {column} in the performance {performance.Name} not found!");

            return seat;
        }

        public void AddSeatsForPerformance(Performance performance)
        {
            Hall hall = performance.Hall;
            int rowsCount = hall.Rows;
            int columnsCount = hall.Columns;

            for (int row = 1; row <= rowsCount; row++)
            {
                for (int column = 1; column <= columnsCount; column++)
                {
                    Seat seat = new Seat();
                    seat.Performance = performance;
                    seat.PerformanceId = performance.Id;
                    seat.SeatNumber = column;
                    seat.Row = row;
                    _context.Seats.Add(seat);
                }
            }
        }

        public void SetNewSeatLayout(Performance performance, int currentRows, int currentColumns, int newRows, int newColumns)
        {
            int performanceId = performance.Id;

            // First remove unneeded seats (cast the hall)
            for (int row = 1; row <= currentRows; row++)
            {
                for (int column = 1; column <= currentColumns; column++)
                {
                    if (row > newRows || column > newColumns)
                    {
                        _context.Seats.RemoveRange(_context.Seats.Where(s => s.Row == row && s.SeatNumber == column && s.PerformanceId == performanceId));
                    }
                }
            }

            // Second add new seats (fill the hall)
            for (int row = 1; row <= newRows; row++)
            {
                for (int column = 1; column <= newColumns; column++)
                {
                    if (row > currentRows || column > currentColumns)
                    {
                        Seat seat = new Seat();
                        seat.PerformanceId = performanceId;
                        seat.SeatNumber = column;
                        seat.Row = row;
                        _context.Seats.Add(seat);
                    }
                }
            }

            _context.SaveChanges();
        }
    }
}
