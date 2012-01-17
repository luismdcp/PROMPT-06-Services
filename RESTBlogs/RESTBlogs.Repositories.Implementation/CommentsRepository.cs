using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using RESTBlogs.Domain;
using RESTBlogs.Repositories.Contracts;

namespace RESTBlogs.Repositories.Implementation
{
    public class CommentsRepository : BaseRepository<Comment>, ICommentsRepository
    {
        public CommentsRepository()
        {
            this.InitializeDocumentStore();
        }

        public List<Comment> GetAllFromPost(string postId)
        {
            using (IDocumentSession session = this.DocumentStore.OpenSession())
            {
                return session.Query<Comment>().Where(c => c.postId == postId).ToList();
            }
        }

        public List<Comment> GetAllFromPost(string postId, int pageIndex, int pageSize)
        {
            using (IDocumentSession session = this.DocumentStore.OpenSession())
            {
                return session.Query<Comment>().Where(c => c.postId == postId).Skip(pageIndex - 1).Take(pageSize).ToList();
            }
        }

        public List<Comment> GetAllFromUser(string userId)
        {
            using (IDocumentSession session = this.DocumentStore.OpenSession())
            {
                return session.Query<Comment>().Where(c => c.author == userId).ToList();
            }
        }
    }
}