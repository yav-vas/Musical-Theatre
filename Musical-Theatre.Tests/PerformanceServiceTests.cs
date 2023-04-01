using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Data.Context;
using Musical_Theatre.Data.Models;
using Musical_Theatre.Models;
using Musical_Theatre.Repositories;
using Musical_Theatre.Repositories.Interfaces;
using Musical_Theatre.Services;
using Musical_Theatre.Services.Interfaces;

namespace Musical_Theatre.Tests
{
    public class PerformanceServiceTests
    {
        private Musical_TheatreContext context;
        private IPerformanceService performanceService;
        private IPerformanceRepository performanceRepository;
        private ISeatService seatService;
        private ISeatRepository seatRepository;
        private IHallService hallService;
        private IHallRepository hallRepository;
        private ICommonRepository<Performance> commonPerformanceRepository;
        private ICommonRepository<Hall> commonHallRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<Musical_TheatreContext>().UseInMemoryDatabase("TestDb");

            context = new Musical_TheatreContext(options.Options);

            commonPerformanceRepository = new CommonRepository<Performance>(context);
            commonHallRepository = new CommonRepository<Hall>(context);
            seatRepository = new SeatRepository(context);
            seatService = new SeatService(seatRepository);
            hallRepository = new HallRepository(context);
            hallService = new HallService(seatService, hallRepository, performanceRepository, commonHallRepository);
            performanceRepository = new PerformanceRepository(context);
            performanceService = new PerformanceService(performanceRepository, seatService, hallRepository, commonPerformanceRepository);
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
        }

        [Test]
        public void GetAllPerformancesReturnsAllPerformances()
        {
            GenerateOnePerformanceAndOneHall();

            Assert.AreEqual(performanceRepository.GetCount(), 1);
        }

        [Test]
        public void GetPerformanceByIdReturnsPerformanceWithTheSameId()
        {
            GenerateOnePerformanceAndOneHall();

            int idToSearch = 1;
            Performance performance = performanceService.GetPerformanceById(idToSearch);

            Assert.IsNotNull(performance);
            Assert.AreEqual(idToSearch, performance.Id);
        }

        [Test]
        public void GetPerformanceByIdReturnsPerformanceWithHall()
        {
            GenerateOnePerformanceAndOneHall();

            Hall hall = performanceService.GetPerformanceById(1).Hall;

            Assert.IsNotNull(hall);
            Assert.AreEqual("Main hall", hall.Name);
            Assert.AreEqual(5, hall.Rows);
            Assert.AreEqual(5, hall.Columns);
        }

        [Test]
        public void AddPerformanceAddsPerformanceWithExpectedProperties()
        {
            GenerateOnePerformanceAndOneHall();

            Performance performance = performanceService.GetPerformanceById(1);

            Assert.AreEqual(1, performance.Id);
            Assert.AreEqual("Performance in main hall", performance.Name);
            Assert.AreEqual("A simple details", performance.Details);
            Assert.AreEqual(1, performance.HallId);
        }

        [Test]
        public void AddPerformanceWithInvalidHallIdThrowsException()
        {
            PerformanceViewModel performanceForm = new PerformanceViewModel(1, "Performance in invalid hall", -5, "Details");

            Assert.Throws<ArgumentNullException>(() => performanceService.AddPerformance(performanceForm));
        }

        [Test]
        public void AddPerformanceCreatesRequiredSeats()
        {
            GenerateOnePerformanceAndOneHall();

            Assert.AreEqual(seatService.GetSeats().Count, 5 * 5);
        }

        [Test]
        public void EditPerformanceNameAndDetailsChangesThem()
        {
            GenerateOnePerformanceAndOneHall();

            PerformanceViewModel performanceForm = new PerformanceViewModel(1, "New performance name", 1, "Another details");
            performanceService.EditPerformance(performanceForm, performanceService.GetPerformanceById(1));

            Performance newPerformance = performanceService.GetPerformanceById(1);

            Assert.AreEqual("New performance name", newPerformance.Name);
            Assert.AreEqual("Another details", newPerformance.Details);
        }

        [Test]
        public void EditPerformanceHallToBiggerChangesSeatCount()
        {
            GenerateOnePerformanceAndOneHall();

            Hall newHall = new Hall(2, "Bigger hall", 10, 10, DateTime.Now);
            hallService.AddHall(newHall);

            PerformanceViewModel performanceForm = new PerformanceViewModel(1, "Performance in main hall", 2, "A simple details");
            performanceService.EditPerformance(performanceForm, performanceService.GetPerformanceById(1));

            Assert.AreEqual(10 * 10, seatService.GetSeats().Select(s => s.PerformanceId == 1).Count());
        }

        [Test]
        public void EditPerformanceHallToSmallerChangesSeatCount()
        {
            GenerateOnePerformanceAndOneHall();

            Hall newHall = new Hall(2, "Bigger hall", 3, 3, DateTime.Now);
            hallService.AddHall(newHall);

            PerformanceViewModel performanceForm = new PerformanceViewModel(1, "Performance in main hall", 2, "A simple details");
            performanceService.EditPerformance(performanceForm, performanceService.GetPerformanceById(1));

            Assert.AreEqual(3 * 3, seatService.GetSeats().Select(s => s.PerformanceId == 1).Count());
        }

        [Test]
        public void DeletePerformanceDeletesThePerformance()
        {
            GenerateOnePerformanceAndOneHall();

            performanceService.DeletePerformance(1);

            Assert.AreEqual(0, performanceRepository.GetCount());
        }

        [Test]
        public void DeletePerformanceDeletesItsSeats()
        {
            GenerateOnePerformanceAndOneHall();

            performanceService.DeletePerformance(1);

            Assert.AreEqual(0, seatService.GetSeats().Select(s => s.PerformanceId == 1).Count());
        }

        [Test]
        public void DeletingPerformanceWithInvalidIdThrowsException()
        {
            GenerateOnePerformanceAndOneHall();

            Assert.Throws<ArgumentNullException>(() => performanceService.DeletePerformance(-5));
        }

        public void GenerateOnePerformanceAndOneHall()
        {
            Hall hall = new Hall(1, "Main hall", 5, 5, DateTime.Now);
            hallService.AddHall(hall);

            PerformanceViewModel performanceForm = new PerformanceViewModel(1, "Performance in main hall", hall.Id, "A simple details");
            performanceService.AddPerformance(performanceForm);
        }
    }
}
