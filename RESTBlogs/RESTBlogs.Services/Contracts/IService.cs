using System.Collections.Generic;

namespace RESTBlogs.Services.Contracts
{
    public interface IService<T, TKey>
    {
        List<T> GetAll();
        List<T> GetAll(int pageIndex, int pageSize);
        T Get(TKey key);
        void Create(T instance);
        void Update(T instance);
        void Delete(TKey id);
        int Count();
    }
}