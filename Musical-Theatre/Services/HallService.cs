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

        public List<Hall>? GetHalls()
        {
            if (_context.Halls == null)
                throw new ArgumentNullException("Entity Halls is null!");

            List<Hall> halls =  _context.Halls.ToList();
            return halls;
        }
        public IEnumerable<Hall> GetHallData()
        {
            return _context.Halls;
        }
        public  Hall GetHallById(int? id)
        {
            if (id == null)
                throw new ArgumentNullException("Id is null");

            if (_context.Halls == null)
                throw new ArgumentNullException("Entity Halls is null!");

            var hall =  _context.Halls.FirstOrDefault(h => h.Id == id);

            if (hall == default)
                throw new ArgumentNullException("Hall with id " + id + " not found!");

            return hall;
        }

        // The method sets DateCreated to current date
        public int AddHall(Hall hall)
        {
            if (_context.Halls == null)
                throw new ArgumentNullException("Entity Halls is null!");

            if (hall == null)
                throw new ArgumentNullException("Given hall is null");

            hall.DateCreated = DateTime.Now;

             _context.Halls.Add(hall);

            
            int entitiesWritten =  _context.SaveChanges();

            return entitiesWritten;
        }

        // The method keeps the DateCreated to previous set date
        public int EditHall(int? id, Hall newHall)
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
   
                var currentHall =  _context.Halls.Find(newHall.Id);
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

                int entitiesWritten =  _context.SaveChanges();

                return entitiesWritten;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ArgumentException("The database has been changed before the update has been applied!");
            }
        }

        public int DeleteHall(int? id)
        {
            if (id == null)
                throw new ArgumentNullException("Id is null");

            if (_context.Halls == null)
                throw new ArgumentNullException("Entity Halls is null!");

            var hall =  _context.Halls.Find(id);

            if (hall == null)
                throw new ArgumentNullException($"Hall with id {id} not found!");

            _context.Halls.Remove(hall);
            int entitiesWritten = _context.SaveChanges();

            return entitiesWritten;
        }
    }
}
