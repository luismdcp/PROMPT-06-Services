using RESTBlogs.Repositories.Contracts;
using RESTBlogs.Repositories.Implementation;
using RESTBlogs.Services.Contracts;
using RESTBlogs.Services.Implementation;
using StructureMap;

namespace RESTBlogs.IoC
{
    public static class ContainerBootstrapper
    {
        public static void BootstrapStructureMap()
        {
            ObjectFactory.Initialize(x =>
            {
                x.ForRequestedType<IBlogsRepository>().TheDefaultIsConcreteType<BlogsRepository>();
                x.ForRequestedType<ICommentsRepository>().TheDefaultIsConcreteType<CommentsRepository>();
                x.ForRequestedType<IPostsRepository>().TheDefaultIsConcreteType<PostsRepository>();
                x.ForRequestedType<IBlogsService>().TheDefaultIsConcreteType<BlogsService>();
                x.ForRequestedType<ICommentsService>().TheDefaultIsConcreteType<CommentsService>();
                x.ForRequestedType<IPostsService>().TheDefaultIsConcreteType<PostsService>();
            });
        }
    }
}