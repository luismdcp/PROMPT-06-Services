using System.Collections.Generic;
using RESTBlogs.Domain;

namespace RESTBlogs.Repositories.Contracts
{
    public interface IBlogsRepository : IRepository<Blog, string>
    {
        List<Blog> GetBlogsFromUser(string user);
        List<string> GetTagCloud(string blogId);
    }
}