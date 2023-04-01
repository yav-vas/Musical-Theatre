using Musical_Theatre.Data.Models;
using Musical_Theatre.Models;
using Musical_Theatre.Repositories.Interfaces;
using Musical_Theatre.Services.Interfaces;

namespace Musical_Theatre.Services
{
    public class PerformanceService : IPerformanceService
    {
        private readonly IPerformanceRepository performanceRepository;
        private readonly ISeatService seatService;
        private readonly IHallRepository hallRepository;
        private readonly ICommonRepository<Performance> commonRepository;

        public PerformanceService(IPerformanceRepository performanceRepository,ISeatService seatService, IHallRepository hallRepository, ICommonRepository<Performance> commonRepository)
        {
            this.performanceRepository = performanceRepository;
            this.seatService = seatService;
            this.hallRepository = hallRepository;
            this.commonRepository = commonRepository;
        }

        public  IEnumerable<Performance>? GetPerformances()
        {
            IEnumerable<Performance> performances = performanceRepository.GetAllWithHall();
            return performances;
        }

        public  Performance? GetPerformanceById(int id)
        {
            if (id == default)
                throw new ArgumentNullException("Id is null");

            var performance = performanceRepository.GetByIdWithHall(id);

            if (performance == default)
                throw new ArgumentNullException("Performance with id " + id + " not found!");

            return performance;
        }
        public Hall? GetPerformanceHall(int id)
        {
            if (id == null)
                throw new ArgumentNullException("Id is null");

            if (performanceRepository.GetAll() == null)
                throw new ArgumentNullException("Entity Hall is null!");

            var hall =  hallRepository.GetById(id);

            if (hall == default)
                throw new ArgumentNullException("Hall with id " + id + " not found!");

            return hall;
        }

        public int AddPerformance(PerformanceViewModel performanceForm)
        {
            int performancesCount = performanceRepository.GetCount();
            Hall hall = hallRepository.GetById(performanceForm.HallId);
            int rowsCount = hall.Rows;
            int columnsCount = hall.Columns;

            if (performanceForm == null)
                throw new ArgumentNullException("Given hall is null");
            Performance performance = new Performance() 
            {
                Id = performancesCount += 1,
                Name = performanceForm.Name,
                Hall= hall,
                HallId= performanceForm.HallId,
                Details= performanceForm.Details
            };
            performanceRepository.Add(performance);
            seatService.AddSeatsForPerformance(performance);

           
            return 1;
        }
        public  int EditPerformance(PerformanceViewModel performanceForm, Performance performance)
        {
            Hall hall = hallRepository.GetById(performanceForm.HallId);

            if (hall == null)
                throw new ArgumentException($"Hall with id {performanceForm.HallId} not found.");

            if (performanceRepository.GetAll() == null)
                throw new ArgumentNullException("Entity Performance is null!");

            if (performanceForm == null)
                throw new ArgumentNullException("Given hall is null");

            if (performance == null)
                throw new ArgumentNullException($"Performance with Id {performanceForm.PerformanceId} doesn't exist.");

            int currentHallId = performance.HallId;
            int newHallId = performanceForm.HallId;

            if (currentHallId != newHallId)
            {
                seatService.SetNewSeatLayout(performance, performance.Hall.Rows, performance.Hall.Columns, hall.Rows, hall.Columns);
            }
            performance.Details = performanceForm.Details;
            performance.HallId = performanceForm.HallId;
            performance.Name = performanceForm.Name;

            
            int entitiesWritten =  performanceRepository.Edit(performance);


            // TODO: could return boolean
            return entitiesWritten;


        }
        public  int DeletePerformance(int id)
        {
            var performance = GetPerformanceById(id);
            if (performance != null)
            {
                return performanceRepository.Remove(performance);
            }

            return 0;
        }

    }
}
