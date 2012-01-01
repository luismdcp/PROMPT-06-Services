using System.Collections.Generic;
using RESTBlogs.Domain;
using RESTBlogs.Repositories.Contracts;
using RESTBlogs.Services.Contracts;

namespace RESTBlogs.Services.Implementation
{
    public class CommentsService : BaseService<Comment, string>, ICommentsService
    {
        public CommentsService(ICommentsRepository repository)
        {
            this.repository = repository;
        }

        public List<Comment> GetAllFromPost(string postId)
        {
            return ((ICommentsRepository) this.repository).GetAllFromPost(postId);
        }

        public List<Comment> GetAllFromPost(string postId, int pageIndex, int pageSize)
        {
            return ((ICommentsRepository) this.repository).GetAllFromPost(postId, pageIndex, pageSize);
        }

        public List<Comment> GetAllFromUser(string user)
        {
            return ((ICommentsRepository) this.repository).GetAllFromUser(user);
        }
    }
}