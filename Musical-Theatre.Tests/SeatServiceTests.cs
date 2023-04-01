using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Data.Context;
using Musical_Theatre.Data.Models;
using Musical_Theatre.Models;
using Musical_Theatre.Repositories.Interfaces;
using Musical_Theatre.Repositories;
using Musical_Theatre.Services.Interfaces;
using Musical_Theatre.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Musical_Theatre.Tests
{
    public class SeatServiceTests
    {
        private Musical_TheatreContext context;
        private IHallRepository hallRepository;
        private ISeatRepository seatRepository;
        private IHallService hallService;
        private IPerformanceRepository performanceRepository;
        private IPerformanceService performanceService;
        private ISeatService seatService;
        private ICommonRepository<Hall> commonHallRepository;
        private ICommonRepository<Performance> commonPerformanceRepository;
        private Hall hall;
        private Hall biggerHall;
        private Performance performance;
        private PerformanceViewModel performanceViewModel;
        private Seat seat;
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<Musical_TheatreContext>().UseInMemoryDatabase("TestDb");

            context = new Musical_TheatreContext(options.Options);
            seatRepository = new SeatRepository(context);
            seatService = new SeatService(seatRepository);
            hallRepository = new HallRepository(context);
            performanceRepository = new PerformanceRepository(context);
            commonHallRepository = new CommonRepository<Hall>(context);
            commonPerformanceRepository = new CommonRepository<Performance>(context);

            hallService = new HallService(seatService, hallRepository, performanceRepository, commonHallRepository);
            performanceService = new PerformanceService(performanceRepository, seatService, hallRepository, commonPerformanceRepository);

            hall = new Hall(1, "Test Hall", 5, 5, DateTime.Now);
            biggerHall = new Hall(1, "Test halls", 7, 7, DateTime.Now);
            performance = new Performance(1, "Test Performance", 1, "Details");
            performanceViewModel = new PerformanceViewModel("Test Performance", 1, "Details");
            seat = new Seat(performance, 1, 1);
        }
        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
        }
        [Test]
        public void GetSeatByIdReturnsSeat()
        {
            seatRepository.Add(seat);
            var chosenSeat = seatRepository.GetById(seat.Id);
            Assert.AreEqual(seat, chosenSeat);
        }
        [Test]
        public void GetSeatByRowColumnAndPerformanceReturnsSeat()
        {
            seatRepository.Add(seat);
            var chosenSeat = seatRepository.GetByRowAndColumnAndPerformance(1, 1, performance);
            Assert.IsNotNull(chosenSeat);
            Assert.AreEqual(chosenSeat, seat);
        }
        [Test]
        public void GetSeatsReturnsSeats()
        {
            seatRepository.Add(seat);
            List<Seat> seats = seatRepository.GetAll();
            Assert.AreEqual(seats.Count, 1);
        }
        [Test]
        public void AddSeatsAfterHallUpdateReturnsSeats()
        {
            hallService.AddHall(hall);
            performanceService.AddPerformance(performanceViewModel);
            List<Seat> seats = seatRepository.GetAllSeatsForPerformance(1);
            hallService.EditHall(1, biggerHall);
            List<Seat> newSeats = seatRepository.GetAllSeatsForPerformance(1);
            Assert.AreNotEqual(newSeats.Count, seats.Count);
        }
        [Test]
        public void RemoveSeatsAfterHallUpdateReturnsSeats()
        {
            hallService.AddHall(biggerHall);
            performanceService.AddPerformance(performanceViewModel);
            List<Seat> seats = seatRepository.GetAllSeatsForPerformance(1);
            hallService.EditHall(1, hall);
            List<Seat> newSeats = seatRepository.GetAllSeatsForPerformance(1);
            Assert.AreNotEqual(newSeats.Count, seats.Count);
        }
        [Test]
        public void WrongSeatIdThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                seatService.GetSeatById(1);
            });
        }
        [Test]
        public void WrongRowThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                seatService.GetSeatByRowAndColumnAndPerformance(2, seat.SeatNumber, performance);
            });
        }
        [Test]
        public void WrongColumnThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                seatService.GetSeatByRowAndColumnAndPerformance(seat.Row, 2, performance);
            });
        }
        [Test]
        public void WrongOrNullPerformanceThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                seatService.GetSeatByRowAndColumnAndPerformance(seat.Row, seat.SeatNumber, null);
            });
        }
    }
}
