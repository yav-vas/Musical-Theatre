using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Data.Context;
using Musical_Theatre.Data.Models;

namespace Musical_Theatre.Services
{
    public class HallService
    {
        private readonly Musical_TheatreContext _context;

        public HallService(Musical_TheatreContext context)
        {
            _context = context;
        }

        public async Task<List<Hall>?> GetHalls()
        {
            if (_context.Halls == null)
                throw new ArgumentNullException("Entity Halls is null!");

            List<Hall> halls = await _context.Halls.ToListAsync();
            return halls;
        }

        public async Task<Hall?> GetHallById(int? id)
        {
            if (id == null)
                throw new ArgumentNullException("Id is null");

            if (_context.Halls == null)
                throw new ArgumentNullException("Entity Halls is null!");

            var hall = await _context.Halls.FirstOrDefaultAsync(h => h.Id == id);

            if (hall == default)
                throw new ArgumentNullException("Hall with id " + id + " not found!");

            return hall;
        }

        // The method sets DateCreated to current date
        public async Task<int> AddHall(Hall hall)
        {
            if (_context.Halls == null)
                throw new ArgumentNullException("Entity Halls is null!");

            if (hall == null)
                throw new ArgumentNullException("Given hall is null");

            hall.DateCreated = DateTime.Now;

            await _context.Halls.AddAsync(hall);

            
            int entitiesWritten = await _context.SaveChangesAsync();

            return entitiesWritten;
        }

        // The method keeps the DateCreated to previous set date
        public async Task<int> EditHall(int? id, Hall newHall)
        {
            if (id == null)
                throw new ArgumentNullException("Id is null");

            if (_context.Halls == null)
                throw new ArgumentNullException("Entity Halls is null!");

            if (newHall == null)
                throw new ArgumentNullException("Given newHall is null");

            if (id != newHall.Id)
                throw new ArgumentException("Id mismatch");

            try
            {
   
                var currentHall = await _context.Halls.FindAsync(newHall.Id);
                {

                }
                if (currentHall == null)
                    throw new ArgumentNullException("Hall with id " + id + " not found!");
                _context.Entry(currentHall).State = EntityState.Detached;
                var performances = _context.Performances.Where(p => p.HallId == newHall.Id).ToList();
                foreach (var performance in performances) 
                {
                    if (newHall.Rows == currentHall.Rows && newHall.Columns == currentHall.Columns)
                    {
                        break;
                    }

                    int currentRowCount = currentHall.Rows;
                    int currentColumnCount = currentHall.Columns;
                    int newRowCount = newHall.Rows;
                    int newColumnCount = newHall.Columns;
                    int newColumnCountDifference = newColumnCount - currentColumnCount;
                    int newRowCountDifference = newRowCount - currentRowCount;


                    if (newColumnCountDifference < 0 && newRowCountDifference < 0)
                    {
                        for (int row = 1; row <= newRowCount; row++)
                        {
                            for (int column = currentColumnCount; column > newColumnCount; column--)
                            {
                                List<Seat> seatsToBeRemoved = _context.Seats.Where(s => s.Row == row && s.SeatNumber == column).ToList();
                                foreach (var seat in seatsToBeRemoved)
                                {
                                    _context.Seats.Remove(seat);
                                }
                            }
                        }
                        for (int row = 1; row <= Math.Abs(newRowCountDifference); row++)
                        {
                            int rowNumber = newRowCount + row;
                            List<Seat> seatsToBeRemoved = _context.Seats.Where(s => s.Row == rowNumber).ToList();
                            foreach (var seat in seatsToBeRemoved)
                            {
                                _context.Seats.Remove(seat);
                            }
                        }
                    }
                    else if (newColumnCountDifference < 0 && newRowCountDifference == 0)
                    {
                        for (int row = 1; row <= newRowCount; row++)
                        {
                            for (int column = currentColumnCount; column > newColumnCount; column--)
                            {
                                
                                List<Seat> seatsToBeRemoved = _context.Seats.Where(s=> s.SeatNumber == column).ToList();
                                foreach (var seat in seatsToBeRemoved)
                                {
                                    _context.Seats.Remove(seat);
                                }
                            }
                        }
                    }
                    else if (newRowCountDifference < 0 && newColumnCountDifference == 0)
                    {
                        for (int row = currentRowCount; row > newRowCount; row--)
                        {
                            for (int column = 1; column <= newColumnCount; column++)
                            {
                                List<Seat> seatsToBeRemoved = _context.Seats.Where(s => s.Row == row).ToList();
                                foreach (var seat in seatsToBeRemoved)
                                {
                                    _context.Seats.Remove(seat);
                                }

                            }
                        }
                    }

           
                    else if(newRowCountDifference > 0 && newColumnCountDifference > 0)   
                    {

                        for (int row = 1; row <= currentRowCount; row++)
                        {
                            for (int column = 1; column <= newColumnCountDifference; column++)
                            {
                                Seat seat = new Seat();
                                seat.Performance = performance;
                                seat.PerformanceId = performance.Id;
                                seat.SeatNumber = column + currentColumnCount;
                                seat.Row = row;
                                _context.Seats.Add(seat);
                            }
                        }
                        for (int row = 1; row <= newRowCountDifference; row++)
                        {
                            
                            for (int column = 1; column <= newColumnCount; column++)
                            {
                                Seat seat = new Seat();
                                seat.Performance = performance;
                                seat.PerformanceId = performance.Id;
                                seat.SeatNumber = column;
                                seat.Row = row + currentRowCount;
                                _context.Seats.Add(seat);
                            }
                        }
                    }
                    else if (newRowCountDifference > 0 && newColumnCountDifference == 0)
                    {

                        for (int row = 1; row <= newRowCount - currentRowCount; row++)
                        {

                            for (int column = 1; column <= currentColumnCount; column++)
                            {
                                Seat seat = new Seat();
                                seat.Performance = performance;
                                seat.PerformanceId = performance.Id;
                                seat.SeatNumber = column;
                                seat.Row = row + currentRowCount;
                                _context.Seats.Add(seat);
                            }
                        }
                    }
                    else if (newRowCountDifference == 0 && newColumnCount > 0)
                    {
                        for (int row = 1; row <= currentRowCount; row++)
                        {
                            for (int column = 1; column <= newColumnCountDifference; column++)
                            {
                                Seat seat = new Seat();
                                seat.Performance = performance;
                                seat.PerformanceId = performance.Id;
                                seat.SeatNumber = column + currentColumnCount;
                                seat.Row = row;
                                _context.Seats.Add(seat); // TODO: should be in a different seat service

                            }
                        }
                    }


                }

                newHall.DateCreated = currentHall.DateCreated;

                _context.Halls.Update(newHall);

                int entitiesWritten = await _context.SaveChangesAsync();

                return entitiesWritten;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ArgumentException("The database has been changed before the update has been applied!");
            }
        }

        public async Task<int> DeleteHall(int? id)
        {
            if (id == null)
                throw new ArgumentNullException("Id is null");

            if (_context.Halls == null)
                throw new ArgumentNullException("Entity Halls is null!");

            var hall = await _context.Halls.FindAsync(id);

            if (hall == null)
                throw new ArgumentNullException($"Hall with id {id} not found!");

            _context.Halls.Remove(hall);
            int entitiesWritten = await _context.SaveChangesAsync();

            return entitiesWritten;
        }
    }
}
