using System.Collections.Generic;
using RESTBlogs.Domain;

namespace RESTBlogs.Repositories.Contracts
{
    public interface IPostsRepository : IRepository<Post, string>
    {
        List<Post> GetAllFromBlog(string blogId);
        List<Post> GetAllFromBlog(string blogId, int pageIndex, int pageSize);
    }
}