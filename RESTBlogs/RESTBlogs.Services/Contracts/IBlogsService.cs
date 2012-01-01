using System.Collections.Generic;
using RESTBlogs.Domain;

namespace RESTBlogs.Services.Contracts
{
    public interface IBlogsService : IService<Blog, string>
    {
        List<Blog> GetBlogsFromUser(string user);
        List<string> GetTagCloud(string blogId);
    }
}