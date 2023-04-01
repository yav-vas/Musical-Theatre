using Musical_Theatre.Data.Models;
using Musical_Theatre.Models;

namespace Musical_Theatre.Services.Interfaces
{
    public interface IPerformanceService
    {
        public  IEnumerable<Performance>? GetPerformances();

        public Performance? GetPerformanceById(int id);

        public Hall? GetPerformanceHall(int id);

        public int AddPerformance(PerformanceViewModel performanceForm);

        public int EditPerformance(PerformanceViewModel performanceForm, Performance performance);

        public int DeletePerformance(int id);
    }
}
