

namespace FlavorFind.BusLogic.Respository
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(int id);
    }
}
