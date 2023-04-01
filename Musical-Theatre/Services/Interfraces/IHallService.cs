using Musical_Theatre.Data.Models;

namespace Musical_Theatre.Services.Interfaces
{
    public interface IHallService
    {
        public List<Hall>? GetHalls();

        public IEnumerable<Hall> GetHallData();

        public Hall GetHallById(int id);

        public int AddHall(Hall hall);

        public int EditHall(int? id, Hall newHall);

        public int DeleteHall(int id);
    }
}
