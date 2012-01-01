using System.Collections.Generic;
using RESTBlogs.Repositories.Contracts;
using RESTBlogs.Services.Contracts;

namespace RESTBlogs.Services.Implementation
{
    public class BaseService<T, TKey> : IService<T, TKey>
    {
        protected IRepository<T, TKey> repository;

        public List<T> GetAll()
        {
            return this.repository.GetAll();
        }

        public List<T> GetAll(int pageIndex, int pageSize)
        {
            return this.repository.GetAll(pageIndex, pageSize);
        }

        public T Get(TKey key)
        {
            return this.repository.Get(key);
        }

        public void Create(T instance)
        {
            this.repository.Create(instance);
        }

        public void Update(T instance)
        {
            this.repository.Update(instance);
        }

        public void Delete(TKey id)
        {
            this.repository.Delete(id);
        }

        public int Count()
        {
            return this.repository.Count();
        }
    }
}