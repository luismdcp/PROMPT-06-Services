using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using RESTBlogs.Domain;
using RESTBlogs.Repositories.Contracts;

namespace RESTBlogs.Repositories.Implementation
{
    public class BlogsRepository : BaseRepository<Blog>, IBlogsRepository
    {
        public BlogsRepository()
        {
            this.InitializeDocumentStore();
        }

        public List<string> GetTagCloud(string blogId)
        {
            using (IDocumentSession session = this.documentStore.OpenSession())
            {
                return session.Query<Post>().Where(p => p.blogId == blogId).SelectMany(p => p.tags).Distinct().ToList();
            }
        }

        public List<Blog> GetBlogsFromUser(string userId)
        {
            using (IDocumentSession session = this.documentStore.OpenSession())
            {
                return session.Query<Blog>().Where(b => b.author == userId).ToList();
            }
        }
    }
}