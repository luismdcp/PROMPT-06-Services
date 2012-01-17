using System.Collections.Generic;
using RESTBlogs.Repositories.Contracts;
using RESTBlogs.Services.Contracts;

namespace RESTBlogs.Services.Implementation
{
    public class BaseService<T, TKey> : IService<T, TKey>
    {
        protected IRepository<T, TKey> Repository;

        public List<T> GetAll()
        {
            return this.Repository.GetAll();
        }

        public List<T> GetAll(int pageIndex, int pageSize)
        {
            return this.Repository.GetAll(pageIndex, pageSize);
        }

        public T Get(TKey key)
        {
            return this.Repository.Get(key);
        }

        public void Create(T instance)
        {
            this.Repository.Create(instance);
        }

        public void Update(T instance)
        {
            this.Repository.Update(instance);
        }

        public void Delete(TKey id)
        {
            this.Repository.Delete(id);
        }

        public int Count()
        {
            return this.Repository.Count();
        }
    }
}