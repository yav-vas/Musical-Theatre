using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Data.Context;
using Musical_Theatre.Data.Models;
using Musical_Theatre.Repositories;
using Musical_Theatre.Repositories.Interfaces;
using Musical_Theatre.Services;
using Musical_Theatre.Services.Interfaces;

namespace Musical_Theatre.Tests
{
    public class HallServiceTests
    {
        private Musical_TheatreContext context;
        private IHallRepository hallRepository;
        private ISeatRepository seatRepository;
        private IHallService hallService;
        private IPerformanceRepository performanceRepository;
        private ISeatService seatService;
        private ICommonRepository<Hall> commonRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<Musical_TheatreContext>().UseInMemoryDatabase("TestDb");

            context = new Musical_TheatreContext(options.Options);
            seatService = new SeatService(seatRepository);
            hallRepository = new HallRepository(context);
            seatRepository = new SeatRepository(context);
            performanceRepository = new PerformanceRepository(context);
            commonRepository = new CommonRepository<Hall>(context);

            hallService = new HallService(seatService, hallRepository, performanceRepository, commonRepository);
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
        }

        [Test]
        public void GetAllHallsReturnsAllHalls()
        {
            Hall hall = new Hall(1, "Test hall", 5, 5, DateTime.Now);
            Hall secondHall = new Hall(2, "Test halls", 7, 7, DateTime.Now);

            hallRepository.Add(hall);
            hallRepository.Add(secondHall);

            var halls = hallService.GetHalls();

            Assert.AreEqual(halls.Count(), 2);
        }
    }
}