﻿using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Data;
using Musical_Theatre.Data.Context;

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

                if (currentHall == null)
                    throw new ArgumentNullException("Hall with id " + id + " not found!");
                _context.Entry(currentHall).State = EntityState.Detached;

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