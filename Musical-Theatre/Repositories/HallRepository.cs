using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Data.Context;
using Musical_Theatre.Data.Models;
using Musical_Theatre.Repositories.Interfaces;

namespace Musical_Theatre.Repositories
{
    public class HallRepository : IHallRepository
    {
        public readonly Musical_TheatreContext _context;
        public HallRepository(Musical_TheatreContext context)
        {
            _context = context;

        }
        public int GetCount()
        {
           

            List<Hall> halls = _context.Halls.ToList();
            return halls.Count;
        }
        public List<Hall> GetAll()
        {
            List<Hall> halls = _context.Halls.ToList();
            return halls;
        }
        public Hall GetById(int id) 
        {
            Hall hall = _context.Halls.FirstOrDefault(hall=> hall.Id == id);
            return hall;        
        
        }
        public IEnumerable<Hall> GetData()
        {
            return _context.Halls;
        }
        public int Add(Hall hall)
        {
            _context.Halls.Add(hall);
            return _context.SaveChanges();
        }
        public int Edit(Hall hall)
        {
            _context.Halls.Update(hall);
            return _context.SaveChanges();
        }
        public int Remove(Hall hall)
        {
            _context.Halls.Remove(hall);
            return _context.SaveChanges();
        }

        public void Detach(Hall hall)
        {
            _context.Entry(hall).State = EntityState.Detached;
        }

      
    }
    }
