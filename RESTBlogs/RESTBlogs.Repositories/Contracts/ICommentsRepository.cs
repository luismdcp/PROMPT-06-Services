using System.Collections.Generic;
using RESTBlogs.Domain;

namespace RESTBlogs.Repositories.Contracts
{
    public interface ICommentsRepository : IRepository<Comment, string>
    {
        List<Comment> GetAllFromUser(string user);
        List<Comment> GetAllFromPost(string postId);
        List<Comment> GetAllFromPost(string postId, int pageIndex, int pageSize);
    }
}