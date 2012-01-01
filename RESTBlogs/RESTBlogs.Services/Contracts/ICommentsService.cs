using System.Collections.Generic;
using RESTBlogs.Domain;

namespace RESTBlogs.Services.Contracts
{
    public interface ICommentsService : IService<Comment, string>
    {
        List<Comment> GetAllFromUser(string user);
        List<Comment> GetAllFromPost(string postId);
        List<Comment> GetAllFromPost(string postId, int pageIndex, int pageSize);
    }
}