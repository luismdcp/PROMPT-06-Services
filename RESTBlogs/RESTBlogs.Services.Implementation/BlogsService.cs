using System.Collections.Generic;
using RESTBlogs.Domain;
using RESTBlogs.Repositories.Contracts;
using RESTBlogs.Services.Contracts;

namespace RESTBlogs.Services.Implementation
{
    public class BlogsService : BaseService<Blog, string>, IBlogsService
    {
        public BlogsService(IBlogsRepository repository)
        {
            this.repository = repository;
        }

        public List<string> GetTagCloud(string blogId)
        {
            return ((IBlogsRepository) this.repository).GetTagCloud(blogId);
        }

        public List<Blog> GetBlogsFromUser(string user)
        {
            return ((IBlogsRepository) this.repository).GetBlogsFromUser(user);
        }
    }
}