using Musical_Theatre.Data.Models;

namespace Musical_Theatre.Services.Interfaces
{
    public interface ISeatService
    {
        public List<Seat> GetSeats();

        public Seat? GetSeatById(int id);

        public Seat GetSeatByRowAndColumnAndPerformance(int row, int column, Performance? performance);

        public void AddSeatsForPerformance(Performance performance);

        public void SetNewSeatLayout(Performance performance, int currentRows, int currentColumns, int newRows, int newColumns);
    }
}
