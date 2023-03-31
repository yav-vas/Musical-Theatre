using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Data.Context;
using Musical_Theatre.Data.Models;

namespace Musical_Theatre.Services
{
    public class HallService
    {
        private readonly Musical_TheatreContext _context;
        private readonly SeatService seatService;

        public HallService(Musical_TheatreContext context, SeatService seatService)
        {
            _context = context;
            this.seatService = seatService;
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

            var currentHall =  _context.Halls.Find(newHall.Id);
                
            if (currentHall == null)
                throw new ArgumentNullException("Hall with id " + id + " not found!");

            _context.Entry(currentHall).State = EntityState.Detached;

            var performances = _context.Performances.Where(p => p.HallId == newHall.Id).ToList();
            foreach (var performance in performances) 
            {
                int currentRowCount = currentHall.Rows;
                int currentColumnCount = currentHall.Columns;
                int newRowCount = newHall.Rows;
                int newColumnCount = newHall.Columns;

                seatService.SetNewSeatLayout(performance, currentRowCount, currentColumnCount, newRowCount, newColumnCount);
            }

            newHall.DateCreated = currentHall.DateCreated;

            _context.Halls.Update(newHall);

            int entitiesWritten =  _context.SaveChanges();

            return entitiesWritten;
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
