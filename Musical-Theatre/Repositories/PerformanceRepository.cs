﻿using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Data.Context;
using Musical_Theatre.Data.Models;
using Musical_Theatre.Repositories.Interfaces;

namespace Musical_Theatre.Repositories
{
    public class PerformanceRepository : IPerformanceRepository
    {
        private readonly Musical_TheatreContext context;

        public PerformanceRepository(Musical_TheatreContext context)
        {
            this.context = context;
        }

        public int GetCount()
            => context.Performances.Count();

        public List<Performance> GetAll() 
            => context.Performances.ToList();

        public void Add(Performance entity)
        {
            context.Performances.Add(entity);
            context.SaveChanges();
        }

        public IEnumerable<Performance> GetAllWithHall()
            => context.Performances.Include(p => p.Hall).ToList();

        public Performance GetByIdWithHall(int id)
            => context.Performances.Include(p => p.Hall).FirstOrDefault(p => p.Id == id);

        public Performance GetPerformanceHall(int id)
            => context.Performances.Include(p => p.Hall).FirstOrDefault(p => p.HallId == id);

        public int Edit(Performance entity)
        {
            context.Performances.Update(entity);
            return context.SaveChanges();
        }

        public int Remove(Performance entity)
        {
            context.Performances.Remove(entity);
            return context.SaveChanges();
        }

        public List<Performance> GetHallPerformances(int id)
        {
           return context.Performances.Where(p => p.HallId == id).ToList();
        }
    }
}
