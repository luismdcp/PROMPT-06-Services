using System.Collections.Generic;
using RESTBlogs.Domain;

namespace RESTBlogs.Services.Contracts
{
    public interface IPostsService : IService<Post, string>
    {
        List<Post> GetAllFromBlog(string blogId);
        List<Post> GetAllFromBlog(string blogId, int pageIndex, int pageSize);
    }
}