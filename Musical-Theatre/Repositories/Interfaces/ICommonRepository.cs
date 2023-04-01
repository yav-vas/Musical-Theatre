namespace Musical_Theatre.Repositories.Interfaces
{
    public interface ICommonRepository<T> where T : class
    {
        int Save();

        void Detach(T entity);
    }
}
