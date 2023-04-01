using Musical_Theatre.Data.Models;
using Musical_Theatre.Repositories.Interfaces;
using Musical_Theatre.Services.Interfaces;

namespace Musical_Theatre.Services
{
    public class HallService : IHallService
    {
        private readonly ISeatService seatService;
        private readonly IHallRepository hallRepository;
        private readonly IPerformanceRepository performanceRepository;
        private readonly ICommonRepository<Hall> commonRepository;

        public HallService(ISeatService seatService, IHallRepository hallRepository, IPerformanceRepository performanceRepository, ICommonRepository<Hall> commonRepository)
        {
            this.seatService = seatService;
            this.hallRepository = hallRepository;
            this.performanceRepository = performanceRepository;
            this.commonRepository = commonRepository;
        }

        public List<Hall>? GetHalls()
        {
            if (hallRepository.GetAll() == null)
                throw new ArgumentNullException("Entity Halls is null!");

            List<Hall> halls = hallRepository.GetAll();
            return halls;
        }
        public IEnumerable<Hall> GetHallData()
        {
            return hallRepository.GetData();
        }
        public  Hall GetHallById(int id)
        {
            if (id == null)
                throw new ArgumentNullException("Id is null");

            if (hallRepository.GetAll() == null)
                throw new ArgumentNullException("Entity Halls is null!");

            var hall = hallRepository.GetById(id);

            if (hall == default)
                throw new ArgumentNullException("Hall with id " + id + " not found!");

            return hall;
        }

        // The method sets DateCreated to current date
        public int AddHall(Hall hall)
        {
            if (hallRepository.GetAll() == null)
                throw new ArgumentNullException("Entity Halls is null!");

            if (hall == null)
                throw new ArgumentNullException("Given hall is null");

            hall.DateCreated = DateTime.Now;

            
            int entitiesWritten = hallRepository.Add(hall);

            return entitiesWritten;
        }

        // The method keeps the DateCreated to previous set date
        public int EditHall(int? id, Hall newHall)
        {
            if (id == null)
                throw new ArgumentNullException("Id is null");

            if (hallRepository.GetAll() == null)
                throw new ArgumentNullException("Entity Halls is null!");

            if (newHall == null)
                throw new ArgumentNullException("Given newHall is null");

            if (id != newHall.Id)
                throw new ArgumentException("Id mismatch");

            var currentHall =  hallRepository.GetById(newHall.Id);
                
            if (currentHall == null)
                throw new ArgumentNullException("Hall with id " + id + " not found!");

            commonRepository.Detach(currentHall);

            var performances = performanceRepository.GetHallPerformances(newHall.Id);
            foreach (var performance in performances) 
            {
                int currentRowCount = currentHall.Rows;
                int currentColumnCount = currentHall.Columns;
                int newRowCount = newHall.Rows;
                int newColumnCount = newHall.Columns;
                seatService.SetNewSeatLayout(performance, currentRowCount, currentColumnCount, newRowCount, newColumnCount);
            }

            newHall.DateCreated = currentHall.DateCreated;


            int entitiesWritten = hallRepository.Edit(newHall);


            return entitiesWritten;
        }

        public int DeleteHall(int id)
        {
            if (id == null)
                throw new ArgumentNullException("Id is null");

            if (hallRepository.GetAll() == null)
                throw new ArgumentNullException("Entity Halls is null!");

            var hall = hallRepository.GetById(id);

            if (hall == null)
                throw new ArgumentNullException($"Hall with id {id} not found!");

            
            int entitiesWritten = hallRepository.Remove(hall);

            return entitiesWritten;
        }
    }
}
