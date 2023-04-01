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
    public class TicketServiceTests
    {
        private Musical_TheatreContext context;
        private ITicketService ticketService;
        private ITicketRepository ticketRepository;
        private IPerformanceRepository performanceRepository;
        private IPerformanceService performanceService;
        private ISeatRepository seatRepository;
        private ISeatService seatService;
        private IHallRepository hallRepository;
        private ICommonRepository<Performance> commonPerformanceRepository;
        private ICommonRepository<Hall> commonHallRepository;
        private IHallService hallService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<Musical_TheatreContext>().UseInMemoryDatabase("TestDb");

            context = new Musical_TheatreContext(options.Options);

            ticketRepository = new TicketRepository(context);
            performanceRepository = new PerformanceRepository(context);
            seatRepository = new SeatRepository(context);
            hallRepository = new HallRepository(context);
            seatService = new SeatService(seatRepository);
            commonPerformanceRepository = new CommonRepository<Performance>(context);
            commonHallRepository = new CommonRepository<Hall>(context);
            performanceService = new PerformanceService(performanceRepository, seatService, hallRepository, commonPerformanceRepository);
            hallService = new HallService(seatService, hallRepository, performanceRepository, commonHallRepository);

            ticketService = new TicketService(performanceRepository, ticketRepository, seatRepository);
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
        }

        [Test]
        public void GetAllTicketsReturnsAllTickets()
        {
            GenerateOnePerformanceAndOneHallAndOneTicket();

            Assert.AreEqual(1, ticketService.GetTickets().Count);
        }

        [Test]
        public void BuyTicketForTakenSeatThrowsException()
        {
            GenerateOnePerformanceAndOneHallAndOneTicket();

            TicketViewModel ticketForm = new TicketViewModel("Performance in main hall", 3, 3);

            Assert.Throws<ArgumentException>(() => ticketService.BuyTicket(1, ticketForm));
        }

        [Test]
        public void BuyTicketForNonEistingSeatThrowsException()
        {
            GenerateOnePerformanceAndOneHallAndOneTicket();

            TicketViewModel ticketForm = new TicketViewModel("Performance in main hall", 6, 6);

            Assert.Throws<ArgumentException>(() => ticketService.BuyTicket(1, ticketForm));
        }

        [Test]
        public void TicketsAreRemovedWhenHallIsChanged()
        {
            GenerateOnePerformanceAndOneHallAndOneTicket();

            Assert.AreEqual(1, ticketService.GetTickets().Count());

            Hall newHall = new Hall(1, "Main hall", 2, 2, DateTime.Now);
            hallService.EditHall(1, newHall);

            Assert.AreEqual(0, ticketService.GetTickets().Count());
        }

        [Test]
        public void AllTicketsOfDifferentPerformancesAreAffectedWhenHallIsChanged()
        {
            GenerateOnePerformanceAndOneHallAndOneTicket();

            PerformanceViewModel performanceForm = new PerformanceViewModel(2, "New performance in main hall", 1, "New details");
            performanceService.AddPerformance(performanceForm);

            Ticket ticket = new Ticket(2, seatService.GetSeatByRowAndColumnAndPerformance(3, 3, performanceRepository.GetByIdWithHall(2)).Id);
            ticketRepository.Add(ticket);

            Assert.AreEqual(2, ticketService.GetTickets().Count());

            Hall newHall = new Hall(1, "Main hall", 2, 2, new DateTime());
            hallService.EditHall(1, newHall);

            Assert.AreEqual(0, ticketService.GetTickets().Count());
        }

        [Test]
        public void OnlyTicketsFromAffectedHallsAreDeleted()
        {
            GenerateOnePerformanceAndOneHallAndOneTicket();

            PerformanceViewModel performanceForm = new PerformanceViewModel(2, "New performance in main hall", 1, "New details");
            performanceService.AddPerformance(performanceForm);

            Ticket ticket = new Ticket(2, seatService.GetSeatByRowAndColumnAndPerformance(3, 3, performanceRepository.GetByIdWithHall(2)).Id);
            ticketRepository.Add(ticket);

            Hall hall = new Hall(2, "Big hall", 10, 10, DateTime.Now);
            hallService.AddHall(hall);

            PerformanceViewModel performanceFormBigHall = new PerformanceViewModel(3, "Big hall performance", 2, "Big details");
            performanceService.AddPerformance(performanceFormBigHall);

            Ticket ticketBigHall = new Ticket(3, seatService.GetSeatByRowAndColumnAndPerformance(3, 3, performanceRepository.GetByIdWithHall(3)).Id);
            ticketRepository.Add(ticketBigHall);

            Assert.AreEqual(3, ticketService.GetTickets().Count());

            Hall newHall = new Hall(1, "Main hall", 2, 2, new DateTime());
            hallService.EditHall(1, newHall);

            Assert.AreEqual(1, ticketService.GetTickets().Count());
        }

        [Test]
        public void ChangedPerformanceHallToASmallerOneDeletesTickets()
        {
            GenerateOnePerformanceAndOneHallAndOneTicket();

            Hall newHall = new Hall(2, "Smaller hall", 2, 2, DateTime.Now);
            hallService.AddHall(newHall);

            PerformanceViewModel performanceForm = new PerformanceViewModel(1, "Performance in smaller hall", 2, "A simple details");
            Performance performance = performanceRepository.GetByIdWithHall(1);

            Assert.AreEqual(1, ticketService.GetTickets().Count());

            performanceService.EditPerformance(performanceForm, performance);

            Assert.AreEqual(0, ticketService.GetTickets().Count());
        }

        public void GenerateOnePerformanceAndOneHallAndOneTicket()
        {
            Hall hall = new Hall(1, "Main hall", 5, 5, DateTime.Now);
            hallRepository.Add(hall);

            PerformanceViewModel performanceForm = new PerformanceViewModel(1, "Performance in main hall", hall.Id, "A simple details");
            performanceService.AddPerformance(performanceForm);

            Ticket ticket = new Ticket(1, seatService.GetSeatByRowAndColumnAndPerformance(3, 3, performanceRepository.GetByIdWithHall(1)).Id);
            ticketRepository.Add(ticket);
        }
    }
}
