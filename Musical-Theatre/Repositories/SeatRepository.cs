﻿using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Data.Context;
using Musical_Theatre.Data.Models;
using Musical_Theatre.Repositories.Interfaces;

namespace Musical_Theatre.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        public readonly Musical_TheatreContext _context;
        public SeatRepository(Musical_TheatreContext context) 
        { 
           _context= context;
        
        }
        public int Add(Seat seat)
        {
           _context.Seats.Add(seat);
            return _context.SaveChanges();
        }

        public List<Seat> GetAll()
        {
            return _context.Seats.ToList();
        }

        public List<Seat> GetAllWithHallAndPerformance()
        {
          return  _context.Seats.Include(s=> s.Performance).ThenInclude(p=> p.Hall).ToList();
        }

        public Seat GetById(int id)
        {
            Seat seat = _context.Seats.Include(s => s.Performance).ThenInclude(p => p.Hall).FirstOrDefault(p => p.Id == id);
            return seat;
        }

        public Seat GetByRowAndColumnAndPerformance(int row, int column, Performance performance)
        {
            Seat seat = _context.Seats.Include(s => s.Ticket).FirstOrDefault(s => s.Performance == performance && s.Row == row && s.SeatNumber == column);
            return seat;
        }

        public int RemoveRangeOfSeats(int performanceId, int row, int column)
        {
           _context.Seats.RemoveRange(_context.Seats.Where(s => s.Row == row && s.SeatNumber == column && s.PerformanceId == performanceId));
          return  _context.SaveChanges();
        }
    }
}