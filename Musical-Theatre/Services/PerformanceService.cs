using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Musical_Theatre.Data;
using Musical_Theatre.Data.Context;
using Musical_Theatre.Models;
using MySql.Data.MySqlClient;

namespace Musical_Theatre.Services
{
    public class PerformanceService
    {
        private readonly Musical_TheatreContext _context;

        public PerformanceService(Musical_TheatreContext context)
        {
            this._context = context;
        }

        public async Task<List<Performance>?> GetPerformances()
        {
            if (_context.Performances == null)
                throw new ArgumentNullException("Entity Performances is null!");

            List<Performance> performances = await _context.Performances.ToListAsync();
            return performances;
        }

        public async Task<Performance?> GetPerformanceById(int? id)
        {
            if (id == null)
                throw new ArgumentNullException("Id is null");

            if (_context.Performances == null)
                throw new ArgumentNullException("Entity Performances is null!");

            var performance = await _context.Performances.FirstOrDefaultAsync(p => p.Id == id);

            if (performance == default)
                throw new ArgumentNullException("Performance with id " + id + " not found!");

            return performance;
        }

        public async Task<int> AddPerformance(PerformanceViewModel performanceForm)
        {
            Hall hall = _context.Halls.FirstOrDefault(h => h.Id == performanceForm.HallId);

            if (_context.Performances == null)
                throw new ArgumentNullException("Entity Performance is null!");

            if (performanceForm == null)
                throw new ArgumentNullException("Given performance is null");
            Performance performance = new Performance();
            
               performance.Name= performanceForm.Name;
               performance.Hall = hall;
               performance.HallId = performanceForm.HallId;
               performance.Details= performanceForm.Details;
            
            await _context.Performances.AddAsync(performance);

            int entitiesWritten = await _context.SaveChangesAsync();

            return entitiesWritten;
        }
        public async Task<int> EditPerformance(PerformanceViewModel performanceForm, Performance performance)
        {
            Hall hall = await _context.Halls.FirstOrDefaultAsync(h => h.Id == performanceForm.HallId);
            

            if (_context.Performances == null)
                throw new ArgumentNullException("Entity Performance is null!");

            if (performanceForm == null)
                throw new ArgumentNullException("Given performance is null");
            if (performance == null)
            {
                throw new ArgumentNullException($"Performance with Id {performanceForm.PerformanceId} doesn't exist.");
            }
          
            
                _context.Entry(performance).State = EntityState.Detached;
                performance.Id = performance.Id;
                performance.Hall = hall;
                performance.Details = performanceForm.Details;
                performance.HallId= performanceForm.HallId;
                performance.Name= performanceForm.Name;


                _context.Performances.Update(performance);
            int entitiesWritten = await _context.SaveChangesAsync();

            return entitiesWritten;


        }
        public async Task<int> DeletePerformance(int id)
        {
            var performance = await GetPerformanceById(id);
            if (performance != null)
            {
                _context.Performances.Remove(performance);
            }
            int entitiesWritten = await _context.SaveChangesAsync();

            return entitiesWritten;
        }

    }
}
