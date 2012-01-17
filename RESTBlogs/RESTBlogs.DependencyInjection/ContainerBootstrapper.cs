using RESTBlogs.Repositories.Contracts;
using RESTBlogs.Repositories.Implementation;
using RESTBlogs.Services.Contracts;
using RESTBlogs.Services.Implementation;
using StructureMap;

namespace RESTBlogs.DependencyInjection
{
    public static class ContainerBootstrapper
    {
        public static void BootstrapStructureMap()
        {
            ObjectFactory.Initialize(x =>
            {
                x.For<IBlogsRepository>().Use<BlogsRepository>();
                x.For<ICommentsRepository>().Use<CommentsRepository>();
                x.For<IPostsRepository>().Use<PostsRepository>();
                x.For<IBlogsService>().Use<BlogsService>();
                x.For<ICommentsService>().Use<CommentsService>();
                x.For<IPostsService>().Use<PostsService>();
            });
        }
    }
}