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
    public class HallServiceTests
    {
        private Musical_TheatreContext context;
        private IHallRepository hallRepository;
        private ISeatRepository seatRepository;
        private IHallService hallService;
        private IPerformanceRepository performanceRepository;
        private IPerformanceService performanceService;
        private ISeatService seatService;
        private ICommonRepository<Hall> commonRepository;
        private Hall hall;
        private Hall secondHall;
        private Performance performance;
        private PerformanceViewModel performanceViewModel;

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

             hall = new Hall(1, "Test Hall", 5, 5, DateTime.Now);
            secondHall = new Hall(1, "Test halls", 7, 7, DateTime.Now);
            performance = new Performance(1,"Test Performance",1,"Details");
            performanceViewModel= new PerformanceViewModel("Test Performance", 1, "Details");
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
        }

        [Test]
        public void GetAllHallsReturnsAllHalls()
        {

            hallRepository.Add(hall);

            var halls = hallService.GetHalls();

            Assert.AreEqual(halls.Count(), 1);
        }

        [Test]
        public void GetAllHallDataReturnsIEnumerableOfHalls()
        {

            hallRepository.Add(hall);

            var halls = hallService.GetHallData();

            Assert.AreEqual(halls.Count(), 1);

        }
        [Test]
        public void GetHallByIdReturnsHall()
        {
            hallRepository.Add(hall);
            var selectedHall = hallRepository.GetById(1);
            Assert.IsNotNull(selectedHall);
            Assert.AreEqual(selectedHall, hall);
        }
        [Test]
        public void AddHallReturnsNumberOfChanges()
        {
            int entities = hallRepository.Add(hall);
            Assert.AreEqual(1, entities);
        }
        [Test]
        public void EditHallReturnsNumberOfChanges() 
        {
            hallRepository.Add(hall);
            hallService.EditHall(1, secondHall);
           Hall changedHall = hallRepository.GetById(1);
            Assert.AreEqual(secondHall, changedHall);
        }
        [Test]
        public void EditHallAndChangeSeatsReturnsSeats()
        {
            hallRepository.Add(hall);
            performanceViewModel.PerformanceId= performance.Id;
            performanceService.AddPerformance(performanceViewModel);
            var seatsCount = seatRepository.GetAllSeatsForPerformance(performance).Count();
            hallService.EditHall(hall.Id, secondHall);
            seatService.SetNewSeatLayout(performance, hall.Rows, hall.Columns, secondHall.Rows, secondHall.Columns);
            var changedSeatsCount = seatRepository.GetAllSeatsForPerformance(performance).Count();
            Assert.AreNotEqual(seatsCount, changedSeatsCount);
        }
        [Test]
        public void DeleteHallReturnsHall() 
        {
           int entities = hallRepository.Add(hall);
            entities = hallRepository.Remove(hall);
            Assert.AreEqual(1, entities);
            Assert.IsEmpty(hallRepository.GetAll());
        }
        [Test]
        public void WrongHallIdReturnsNull()
        {
            
            Assert.IsNull(hallRepository.GetById(1));
        }
    }
}