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
            this.Repository = repository;
        }

        public List<string> GetTagCloud(string blogId)
        {
            return ((IBlogsRepository) this.Repository).GetTagCloud(blogId);
        }

        public List<Blog> GetBlogsFromUser(string user)
        {
            return ((IBlogsRepository) this.Repository).GetBlogsFromUser(user);
        }
    }
}