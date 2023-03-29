using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Musical_Theatre.Data.Context;
using Musical_Theatre.Data.Models;
using Musical_Theatre.Models;
using MySql.Data.MySqlClient;
using Mysqlx.Resultset;

namespace Musical_Theatre.Services
{
    public class PerformanceService
    {
        private readonly Musical_TheatreContext _context;

        public PerformanceService(Musical_TheatreContext context)
        {
            this._context = context;
        }

        public  List<Performance>? GetPerformances()
        {
            if (_context.Performances == null)
                throw new ArgumentNullException("Entity Performances is null!");

            List<Performance> performances =  _context.Performances.Include(p => p.Hall).ToList();
            return performances;
        }

        public  Performance? GetPerformanceById(int? id)
        {
            if (id == null)
                throw new ArgumentNullException("Id is null");

            if (_context.Performances == null)
                throw new ArgumentNullException("Entity Performances is null!");

            var performance =  _context.Performances.Include(p => p.Hall).FirstOrDefault(p => p.Id == id);

            if (performance == default)
                throw new ArgumentNullException("Performance with id " + id + " not found!");

            return performance;
        }
        public Hall? GetPerformanceHall(int id)
        {
            if (id == null)
                throw new ArgumentNullException("Id is null");

            if (_context.Performances == null)
                throw new ArgumentNullException("Entity Hall is null!");

            var hall =  _context.Halls.FirstOrDefault(h => h.Id == id);

            if (hall == default)
                throw new ArgumentNullException("Hall with id " + id + " not found!");

            return hall;
        }

        public int AddPerformance(PerformanceViewModel performanceForm)
        {
            List<Performance> performances = _context.Performances.ToList();
            int performancesCount = performances.Count;
            Hall hall = _context.Halls.FirstOrDefault(h => h.Id == performanceForm.HallId);
            int rowsCount = hall.Rows;
            int columnsCount = hall.Columns;

            if (_context.Performances == null)
                throw new ArgumentNullException("Entity Performance is null!");

            if (performanceForm == null)
                throw new ArgumentNullException("Given hall is null");
            Performance performance = new Performance();
            // TODO: create a constructor
            performance.Id = performancesCount += 1;
            performance.Name = performanceForm.Name;
            performance.Hall = hall;
            performance.HallId = performanceForm.HallId;
            performance.Details = performanceForm.Details;

            _context.Performances.Add(performance);
            hall.Performances.Add(performance);
            _context.Halls.Update(hall);
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
            int entitiesWritten =  _context.SaveChanges();



            return entitiesWritten;
        }
        public  int EditPerformance(PerformanceViewModel performanceForm, Performance performance)
        {
            Hall hall = _context.Halls.FirstOrDefault(h => h.Id == performanceForm.HallId);


            if (_context.Performances == null)
                throw new ArgumentNullException("Entity Performance is null!");

            if (performanceForm == null)
                throw new ArgumentNullException("Given hall is null");
            if (performance == null)
            {
                throw new ArgumentNullException($"Performance with Id {performanceForm.PerformanceId} doesn't exist.");
            }

            performance.Hall = hall;
            performance.Details = performanceForm.Details;
            performance.HallId = performanceForm.HallId;
            performance.Name = performanceForm.Name;


            _context.Performances.Update(performance);
            int entitiesWritten =  _context.SaveChanges();


            // TODO: could return boolean
            return entitiesWritten;


        }
        public  int DeletePerformance(int id)
        {
            var performance =  GetPerformanceById(id);
            if (performance != null)
            {
                _context.Performances.Remove(performance);
            }
            int entitiesWritten =  _context.SaveChanges();

            return entitiesWritten;
        }
        public bool PerformanceExists(int id)
        {
            return (_context.Performances?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
