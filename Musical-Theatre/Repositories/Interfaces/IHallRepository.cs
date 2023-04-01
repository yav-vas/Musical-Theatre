using Musical_Theatre.Data.Models;

namespace Musical_Theatre.Repositories.Interfaces
{
    public interface IHallRepository
    {
        public int GetCount();

        public List<Hall> GetAll();

        public IEnumerable<Hall> GetData();

        public Hall GetById(int id);

        public int Add(Hall hall);

        public int Edit(Hall hall);

        public int Remove(Hall hall);
    }
}
