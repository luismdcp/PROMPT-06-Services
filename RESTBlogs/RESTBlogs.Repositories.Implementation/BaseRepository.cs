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
        protected DocumentStore documentStore;

        protected void InitializeDocumentStore()
        {
            this.documentStore = new DocumentStore();
            this.documentStore.Url = ConfigurationSettings.AppSettings["RavenDBServer"];
            this.documentStore.DefaultDatabase = ConfigurationSettings.AppSettings["RavenDBDatabase"];
            this.documentStore.Initialize();
        }

        public List<T> GetAll()
        {
            using (IDocumentSession session = this.documentStore.OpenSession())
            {
                return session.Query<T>().ToList();
            }
        }

        public List<T> GetAll(int pageIndex, int pageSize)
        {
            using (IDocumentSession session = this.documentStore.OpenSession())
            {
                return session.Query<T>().Skip(pageIndex - 1).Take(pageSize).ToList();
            }
        }

        public T Get(string id)
        {
            using (IDocumentSession session = this.documentStore.OpenSession())
            {
                return session.Load<T>(id);
            }
        }

        public void Create(T instance)
        {
            using (IDocumentSession session = this.documentStore.OpenSession())
            {
                session.Store(instance, Guid.NewGuid());
                session.SaveChanges();
            }
        }

        public void Update(T instance)
        {
            using (IDocumentSession session = this.documentStore.OpenSession())
            {
                session.Store(instance, Guid.NewGuid());
                session.SaveChanges();
            }
        }

        public void Delete(string id)
        {
            using (IDocumentSession session = this.documentStore.OpenSession())
            {
                var instance = session.Load<T>(id);

                if (instance != null)
                {
                    session.Delete<T>(instance);
                    session.SaveChanges();   
                }
            }
        }

        public int Count()
        {
            using (IDocumentSession session = this.documentStore.OpenSession())
            {
                return session.Query<T>().Count();
            }
        }
    }
}