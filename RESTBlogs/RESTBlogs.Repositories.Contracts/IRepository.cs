using System.Collections.Generic;

namespace RESTBlogs.Repositories.Contracts
{
    public interface IRepository<T, in TKey>
    {
        List<T> GetAll();
        List<T> GetAll(int pageIndex, int pageSize);
        T Get(TKey id);
        void Create(T instance);
        void Update(T instance);
        void Delete(TKey id);
        int Count();
    }
}