﻿using System.Collections.Generic;
using RESTBlogs.Domain;
using RESTBlogs.Repositories.Contracts;
using RESTBlogs.Services.Contracts;

namespace RESTBlogs.Services.Implementation
{
    public class PostsService : BaseService<Post, string>, IPostsService
    {
        public PostsService(IPostsRepository repository)
        {
            this.repository = repository;
        }

        public List<Post> GetAllFromBlog(string blogId)
        {
            return ((IPostsRepository) this.repository).GetAllFromBlog(blogId);
        }

        public List<Post> GetAllFromBlog(string blogId, int pageIndex, int pageSize)
        {
            return ((IPostsRepository) this.repository).GetAllFromBlog(blogId, pageIndex, pageSize);
        }
    }
}