using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using RESTBlogs.Domain;
using RESTBlogs.Repositories.Contracts;

namespace RESTBlogs.Repositories.Implementation
{
    public class PostsRepository : BaseRepository<Post>, IPostsRepository
    {
        public PostsRepository()
        {
            this.InitializeDocumentStore();
        }

        public List<Post> GetAllFromBlog(string blogId)
        {
            using (IDocumentSession session = this.documentStore.OpenSession())
            {
                return session.Query<Post>().Where(p => p.blogId == blogId).ToList();
            }
        }

        public List<Post> GetAllFromBlog(string blogId, int pageIndex, int pageSize)
        {
            using (IDocumentSession session = this.documentStore.OpenSession())
            {
                return session.Query<Post>().Where(p => p.blogId == blogId).Skip(pageIndex - 1).Take(pageSize).ToList();
            }
        }
    }
}