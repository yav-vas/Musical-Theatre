using Musical_Theatre.Data.Context;
using Musical_Theatre.Repositories.Interfaces;

namespace Musical_Theatre.Repositories
{
    public class CommonRepository : ICommonRepository
    {
        private readonly Musical_TheatreContext _context;
        public CommonRepository(Musical_TheatreContext context)
        {
            _context = context;
        }
        public int Save()
        {
            return _context.SaveChanges();
        }
    }
}
