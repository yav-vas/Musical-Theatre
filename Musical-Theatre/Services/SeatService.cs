using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Data.Context;
using Musical_Theatre.Data.Models;
using Musical_Theatre.Repositories.Interfaces;

namespace Musical_Theatre.Services
{
    public class SeatService
    {
        private readonly ISeatRepository seatRepository;

        public SeatService(ISeatRepository seatRepository)
        {
            this.seatRepository = seatRepository;

        }

        public List<Seat> GetSeats()
        {
            if (seatRepository.GetAll() == null)
                throw new ArgumentNullException("Entity Seats is null!");

            List<Seat> seats = seatRepository.GetAllWithHallAndPerformance();
            return seats;
        }

        public Seat? GetSeatById(int id)
        {
            if (id == null)
                throw new ArgumentNullException("Id is null");

            if (seatRepository.GetAll() == null)
                throw new ArgumentNullException("Entity Seats is null!");

            var seat = seatRepository.GetById(id);

            if (seat == default)
                throw new ArgumentNullException("Seat with id " + id + " not found!");

            return seat;
        }

        public Seat GetSeatByRowAndColumnAndPerformance(int row, int column, Performance? performance)
        {
            if (row == null)
                throw new ArgumentNullException("row is null");
            else if (column == null)
                throw new ArgumentNullException("column is null");
            else if (performance == null)
                throw new ArgumentNullException("performance is null");

            if (seatRepository.GetAll() == null)
                throw new ArgumentNullException("Entity Seats is null!");

            var seat = seatRepository.GetByRowAndColumnAndPerformance(row, column, performance);


            if (seat == default)
                throw new ArgumentNullException($"Seat with row {row} and seat number {column} in the performance {performance.Name} not found!");

            return seat;
        }

        public void AddSeatsForPerformance(Performance performance)
        {
            Hall hall = performance.Hall;
            int rowsCount = hall.Rows;
            int columnsCount = hall.Columns;
            int seatId = seatRepository.GetCount() + 1;

            for (int row = 1; row <= rowsCount; row++)
            {
                for (int column = 1; column <= columnsCount; column++)
                {
                    Seat seat = new Seat()
                    {
                        Id = seatId,
                        Row = row,
                        SeatNumber = column,
                        Performance = performance,
                        PerformanceId = performance.Id,

                    };
                    
                    seatRepository.Add(seat);
                }
            }
        }

        public void SetNewSeatLayout(Performance performance, int currentRows, int currentColumns, int newRows, int newColumns)
        {
            int performanceId = performance.Id;

            // First remove unneeded seats (cast the hall)
            for (int row = 1; row <= currentRows; row++)
            {
                for (int column = 1; column <= currentColumns; column++)
                {
                    if (row > newRows || column > newColumns)
                    {
                        seatRepository.RemoveRangeOfSeats(performanceId, row, column);

                    }
                }
            }

            // Second add new seats (fill the hall)
            for (int row = 1; row <= newRows; row++)
            {
                for (int column = 1; column <= newColumns; column++)
                {
                    if (row > currentRows || column > currentColumns)
                    {
                        Seat seat = new Seat();
                        seat.PerformanceId = performanceId;
                        seat.SeatNumber = column;
                        seat.Row = row;
                        seatRepository.Add(seat);
                    }
                }
            }

        }
    }
}
