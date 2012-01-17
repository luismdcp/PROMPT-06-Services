using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Raven.Client;
using Raven.Client.Document;
using RESTBlogs.Repositories.Contracts;

namespace RESTBlogs.Repositories.Implementation
{
    public abstract class BaseRepository<T> : IRepository<T, string> 
    {
        protected DocumentStore DocumentStore;

        protected void InitializeDocumentStore()
        {
            this.DocumentStore = new DocumentStore
                                     {
                                         Url = ConfigurationManager.AppSettings["RavenDBServer"],
                                         DefaultDatabase = ConfigurationManager.AppSettings["RavenDBDatabase"]
                                     };

            this.DocumentStore.Initialize();
        }

        public List<T> GetAll()
        {
            using (IDocumentSession session = this.DocumentStore.OpenSession())
            {
                return session.Query<T>().ToList();
            }
        }

        public List<T> GetAll(int pageIndex, int pageSize)
        {
            using (IDocumentSession session = this.DocumentStore.OpenSession())
            {
                return session.Query<T>().Skip(pageIndex - 1).Take(pageSize).ToList();
            }
        }

        public T Get(string id)
        {
            using (IDocumentSession session = this.DocumentStore.OpenSession())
            {
                return session.Load<T>(id);
            }
        }

        public void Create(T instance)
        {
            using (IDocumentSession session = this.DocumentStore.OpenSession())
            {
                session.Store(instance, Guid.NewGuid());
                session.SaveChanges();
            }
        }

        public void Update(T instance)
        {
            using (IDocumentSession session = this.DocumentStore.OpenSession())
            {
                session.Store(instance, Guid.NewGuid());
                session.SaveChanges();
            }
        }

        public void Delete(string id)
        {
            using (IDocumentSession session = this.DocumentStore.OpenSession())
            {
                var instance = session.Load<T>(id);

                if (instance != null)
                {
                    session.Delete(instance);
                    session.SaveChanges();   
                }
            }
        }

        public int Count()
        {
            using (IDocumentSession session = this.DocumentStore.OpenSession())
            {
                return session.Query<T>().Count();
            }
        }
    }
}