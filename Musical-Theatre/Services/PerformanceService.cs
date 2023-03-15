using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Data;
using Musical_Theatre.Data.Context;
using Musical_Theatre.Models;

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

        public async Task<Performance?> GetHallById(int? id)
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

    }
}
