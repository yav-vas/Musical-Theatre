using Musical_Theatre.Data.Models;

namespace Musical_Theatre.Repositories.Interfaces
{
    public interface IPerformanceRepository
    {
        int GetCount();
        Performance GetByIdWithHall(int id);
        IEnumerable<Performance> GetAllWithHall();
        void Add(Performance entity);
        int Edit(Performance entity);
        int Remove(Performance entity);
    }
}
