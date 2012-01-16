using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Syndication;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;
using RESTBlogs.Domain;
using RESTBlogs.IoC;
using RESTBlogs.Services.Contracts;
using StructureMap;

namespace RESTBlogs.Server.Service
{
    [ServiceContract]
    public class RESTBlogsService
    {
        #region Fields

        private string serviceURI;

        #endregion

        #region Constructor

        public RESTBlogsService()
        {
            this.serviceURI = ConfigurationSettings.AppSettings["ServiceURI"];
            ContainerBootstrapper.BootstrapStructureMap();
        }

        #endregion

        #region AtomPub service document

        [ServiceKnownType(typeof(AtomPub10ServiceDocumentFormatter))]
        [WebGet(UriTemplate = "/servicedoc")]
        public HttpResponseMessage GetServiceDoc(HttpRequestMessage request)
        {
            ServiceDocument doc = new ServiceDocument { BaseUri = new Uri(this.serviceURI) };
            List<ResourceCollectionInfo> resources = new List<ResourceCollectionInfo>();

            ResourceCollectionInfo blogsCollection = new ResourceCollectionInfo("Blogs", new Uri(String.Format("{0}/blogs", this.serviceURI)));
            blogsCollection.Accepts.Add("application/atom+xml;type=feed");
            resources.Add(blogsCollection);

            Workspace main = new Workspace("RESTBlogsService", resources);
            doc.Workspaces.Add(main);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
                                               {
                                                   Content = new ObjectContent(typeof(AtomPub10ServiceDocumentFormatter), doc.GetFormatter())
                                               };

            return response;
        }

        #endregion

        #region Blog operations

        [WebGet(UriTemplate = "/blogs")]
        public HttpResponseMessage GetBlogs(HttpRequestMessage request, int pageIndex = 1, int pageSize = 10)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            try
            {
                IBlogsService blogsService = ObjectFactory.GetInstance<IBlogsService>();
                List<Blog> blogs = blogsService.GetAll(pageIndex, pageSize);

                if (blogs != null)
                {
                    if (this.ClientAcceptsMediaType("text/html", request))
                    {
                        var blogsHtml = blogs.GenerateBlogsHtml();
                        response.Content = new ObjectContent<string>(blogsHtml, "text/html");
                    }
                    else
                    {
                        SyndicationFeed blogsFeed = new SyndicationFeed();
                        blogsFeed.Title = new TextSyndicationContent("Blogs List");
                        blogsFeed.LastUpdatedTime = new DateTimeOffset(DateTime.Now);
                        blogsFeed.Links.Add(SyndicationLink.CreateSelfLink(request.RequestUri));

                        SyndicationItem item = null;
                        List<SyndicationItem> itemList = new List<SyndicationItem>();
                        blogsFeed.Items = itemList;

                        foreach (var blog in blogs)
                        {
                            item = new SyndicationItem();
                            item.Id = blog.Id;
                            item.LastUpdatedTime = blog.updated;
                            item.PublishDate = blog.published;
                            item.Title = new TextSyndicationContent(blog.name);
                            item.Summary = new TextSyndicationContent(blog.description);

                            item.Links.Add(SyndicationLink.CreateSelfLink(new Uri(String.Format("{0}/{1}", this.serviceURI, blog.Id))));
                            item.Links.Add(SyndicationLink.CreateAlternateLink(request.RequestUri, "text/html"));

                            item.Links.Add(new SyndicationLink(new Uri(String.Format("{0}/{1}", this.serviceURI, blog.Id)), "service.edit", "Edit Blog", "application/atom+xml;type=feed", 0));
                            item.Links.Add(new SyndicationLink(new Uri(String.Format("{0}/{1}/posts", this.serviceURI, blog.Id)), "service.posts", "Blog posts", "application/atom+xml;type=feed", 0));

                            var pagingLinks = this.BuildPagingLinks(blogsService.Count(), pageIndex, pageSize, request.RequestUri);

                            foreach (var link in pagingLinks)
                            {
                                item.Links.Add(link);
                            }

                            item.Authors.Add(new SyndicationPerson(string.Empty, blog.author, string.Empty));

                            itemList.Add(item);
                        }

                        SyndicationFeedFormatter formatter = null;

                        if (this.ClientAcceptsMediaType("application/atom+xml", request))
                        {
                            formatter = blogsFeed.GetAtom10Formatter();
                        }
                        else
                        {
                            if (this.ClientAcceptsMediaType("application/rss+xml", request))
                            {
                                formatter = blogsFeed.GetRss20Formatter();
                            }
                        }

                        response.Content = new ObjectContent(typeof(SyndicationFeedFormatter), formatter);
                    }
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NoContent;
                }
            }
            catch (Exception)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        [WebInvoke(UriTemplate = "/blogs", Method = "POST")]
        public HttpResponseMessage CreateBlog(HttpRequestMessage request)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                IBlogsService blogsService = ObjectFactory.GetInstance<IBlogsService>();

                XmlReader reader = XmlReader.Create(request.Content.ReadAsStreamAsync().Result);
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                SyndicationItem item = feed.Items.FirstOrDefault();

                if (item != null)
                {
                    Blog newBlog = new Blog();
                    newBlog.name = item.Title.Text;
                    newBlog.description = item.Summary.Text;
                    newBlog.updated = item.LastUpdatedTime;
                    newBlog.published = item.PublishDate;

                    var author = item.Authors.FirstOrDefault();
                     
                    if (author != null)
                    {
                        newBlog.author = author.Name;
                        blogsService.Create(newBlog);

                        response.StatusCode = HttpStatusCode.Created;
                        response.Headers.Add("Location", String.Format("{0}/{1}", this.serviceURI, newBlog.Id));
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        [WebGet(UriTemplate = "/blogs/{id}")]
        public HttpResponseMessage GetBlog(string id, HttpRequestMessage request)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            
            try
            {
                IBlogsService blogsService = ObjectFactory.GetInstance<IBlogsService>();
                var blog = blogsService.Get(String.Format("blogs/{0}", id));

                var etag = request.Headers.IfNoneMatch.FirstOrDefault();

                if (etag != null && etag.Tag == blog.etag)
                {
                    response.StatusCode = HttpStatusCode.NotModified;
                }
                else
                {
                    if (blog != null)
                    {
                        if (this.ClientAcceptsMediaType("text/html", request))
                        {
                            response.Content = new ObjectContent<string>(blog.ToHtml(), "text/html");
                        }
                        else
                        {
                            SyndicationFeed blogFeed = new SyndicationFeed();
                            blogFeed.Title = new TextSyndicationContent("Single Blog");
                            blogFeed.LastUpdatedTime = new DateTimeOffset(DateTime.Now);
                            blogFeed.Links.Add(SyndicationLink.CreateSelfLink(request.RequestUri));

                            SyndicationItem item = new SyndicationItem();
                            List<SyndicationItem> itemList = new List<SyndicationItem>();
                            itemList.Add(item);
                            blogFeed.Items = itemList;

                            item.Id = blog.Id;
                            item.LastUpdatedTime = blog.updated;
                            item.PublishDate = blog.published;
                            item.Title = new TextSyndicationContent(blog.name);
                            item.Summary = new TextSyndicationContent(blog.description);

                            item.Links.Add(SyndicationLink.CreateSelfLink(request.RequestUri));
                            item.Links.Add(SyndicationLink.CreateAlternateLink(request.RequestUri, "text/html"));

                            item.Links.Add(new SyndicationLink(new Uri(String.Format("{0}/{1}", this.serviceURI, blog.Id)), "edit", "Edit blog", "application/atom+xml;type=feed", 0));
                            item.Links.Add(new SyndicationLink(new Uri(String.Format("{0}/{1}/posts", this.serviceURI, blog.Id)), "posts", "Blog posts", "application/atom+xml;type=feed", 0));

                            item.Authors.Add(new SyndicationPerson(string.Empty, blog.author, string.Empty));

                            SyndicationFeedFormatter formatter = null;

                            if (this.ClientAcceptsMediaType("application/atom+xml", request))
                            {
                                formatter = blogFeed.GetAtom10Formatter();
                            }
                            else
                            {
                                if (this.ClientAcceptsMediaType("application/rss+xml", request))
                                {
                                    formatter = blogFeed.GetRss20Formatter();
                                }
                            }

                            response.Content = new ObjectContent(typeof(SyndicationFeedFormatter), formatter);
                        }
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.NotFound;
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        [WebInvoke(UriTemplate = "/blogs/{id}", Method = "PUT")]
        public HttpResponseMessage UpdateBlog(string id, HttpRequestMessage request)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NoContent);

            try
            {
                IBlogsService blogsService = ObjectFactory.GetInstance<IBlogsService>();
                var blog = blogsService.Get(String.Format("blogs/{0}", id));

                if (blog != null)
                {
                    XmlReader reader = XmlReader.Create(request.Content.ReadAsStreamAsync().Result);
                    SyndicationFeed feed = SyndicationFeed.Load(reader);
                    SyndicationItem item = feed.Items.FirstOrDefault();

                    if (item != null)
                    {
                        blog.name = item.Title.Text;
                        blog.description = item.Summary.Text;
                        blog.updated = item.LastUpdatedTime;
                        blog.published = item.PublishDate;

                        var author = item.Authors.FirstOrDefault();

                        if (author != null)
                        {
                            blog.author = author.Name;
                            blogsService.Update(blog);
                        }
                        else
                        {
                            response.StatusCode = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                }
            }
            catch (Exception)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        [WebInvoke(UriTemplate = "/blogs/{id}", Method = "DELETE")]
        public HttpResponseMessage DeleteBlog(string id, HttpRequestMessage request)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NoContent);

            try
            {
                IBlogsService blogsService = ObjectFactory.GetInstance<IBlogsService>();
                var blog = blogsService.Get(String.Format("blogs/{0}", id));

                if (blog != null)
                {
                    blogsService.Delete(String.Format("blogs/{0}", id));
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                }
            }
            catch (Exception)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        [WebGet(UriTemplate = "/blogs/{id}/tagcloud")]
        public HttpResponseMessage GetBlogTagCloud(string id, HttpRequestMessage request)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            try
            {
                IBlogsService blogsService = ObjectFactory.GetInstance<IBlogsService>();
                var blog = blogsService.Get(String.Format("blogs/{0}", id));
                var tagCloud = blogsService.GetTagCloud(String.Format("blogs/{0}", id));

                if (blog != null)
                {
                    SyndicationFeed blogFeed = new SyndicationFeed();
                    blogFeed.Title = new TextSyndicationContent("Blog tag cloud");
                    blogFeed.LastUpdatedTime = new DateTimeOffset(DateTime.Now);
                    blogFeed.Links.Add(SyndicationLink.CreateSelfLink(request.RequestUri));

                    SyndicationItem item = new SyndicationItem();
                    List<SyndicationItem> itemList = new List<SyndicationItem>();
                    itemList.Add(item);
                    blogFeed.Items = itemList;

                    item.Id = blog.Id;
                    item.LastUpdatedTime = blog.updated;
                    item.PublishDate = blog.published;
                    item.Title = new TextSyndicationContent("Blog tag cloud");
                    item.Content = SyndicationContent.CreatePlaintextContent(BuildtagCloud(tagCloud));
                    item.Links.Add(SyndicationLink.CreateSelfLink(request.RequestUri));

                    SyndicationFeedFormatter formatter = null;

                    if (this.ClientAcceptsMediaType("application/atom+xml", request))
                    {
                        formatter = blogFeed.GetAtom10Formatter();
                    }
                    else
                    {
                        if (this.ClientAcceptsMediaType("application/rss+xml", request))
                        {
                            formatter = blogFeed.GetRss20Formatter();
                        }
                    }

                    response.Content = new ObjectContent(typeof(SyndicationFeedFormatter), formatter);
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        #endregion

        #region Posts operations

        [WebGet(UriTemplate = "/blogs/{id}/posts")]
        public HttpResponseMessage GetPosts(string id, HttpRequestMessage request, int pageIndex = 1, int pageSize = 10)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            try
            {
                IPostsService postsService = ObjectFactory.GetInstance<IPostsService>();
                var posts = postsService.GetAllFromBlog(String.Format("blogs/{0}", id), pageIndex, pageSize);

                if (posts != null)
                {
                    if (this.ClientAcceptsMediaType("text/html", request))
                    {
                        var postsHtml = posts.GeneratePostsHtml();
                        response.Content = new ObjectContent<string>(postsHtml, "text/html");
                    }
                    else
                    {
                        SyndicationFeed blogPostsFeed = new SyndicationFeed();
                        blogPostsFeed.Title = new TextSyndicationContent(String.Format("Blog {0} posts", id));
                        blogPostsFeed.LastUpdatedTime = new DateTimeOffset(DateTime.Now);
                        blogPostsFeed.Links.Add(SyndicationLink.CreateSelfLink(request.RequestUri));

                        SyndicationItem item = null;
                        List<SyndicationItem> itemList = new List<SyndicationItem>();
                        blogPostsFeed.Items = itemList;

                        foreach (var post in posts)
                        {
                            item = new SyndicationItem();
                            item.Id = post.Id;
                            item.LastUpdatedTime = post.updated;
                            item.PublishDate = post.published;
                            item.Title = new TextSyndicationContent(post.title);

                            item.Links.Add(SyndicationLink.CreateSelfLink(new Uri(String.Format("{0}/{1}/{2}", this.serviceURI, post.blogId, post.Id))));
                            item.Links.Add(SyndicationLink.CreateAlternateLink(request.RequestUri, "text/html"));

                            item.Links.Add(new SyndicationLink(new Uri(String.Format("{0}/{1}", this.serviceURI, post.blogId)), "service.blog", "Parent blog", "application/atom+xml;type=feed", 0));
                            item.Links.Add(new SyndicationLink(new Uri(String.Format("{0}/{1}/{2}", this.serviceURI, post.blogId, post.Id)), "service.edit", "Edit post", "application/atom+xml;type=feed", 0));
                            item.Links.Add(new SyndicationLink(new Uri(String.Format("{0}/{1}/{2}/{3}", this.serviceURI, post.blogId, post.Id, "service.comments")), "comments", "Post comments", "application/atom+xml;type=feed", 0));

                            var pagingLinks = this.BuildPagingLinks(postsService.Count(), pageIndex, pageSize, request.RequestUri);

                            foreach (var link in pagingLinks)
                            {
                                item.Links.Add(link);
                            }

                            item.Authors.Add(new SyndicationPerson(string.Empty, post.author, string.Empty));
                            item.Content = SyndicationContent.CreatePlaintextContent(post.content);

                            itemList.Add(item);
                        }

                        SyndicationFeedFormatter formatter = null;

                        if (this.ClientAcceptsMediaType("application/atom+xml", request))
                        {
                            formatter = blogPostsFeed.GetAtom10Formatter();
                        }
                        else
                        {
                            if (this.ClientAcceptsMediaType("application/rss+xml", request))
                            {
                                formatter = blogPostsFeed.GetRss20Formatter();
                            }
                        }

                        response.Content = new ObjectContent(typeof(SyndicationFeedFormatter), formatter);
                    }
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NoContent;
                }
            }
            catch (Exception)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        [WebInvoke(UriTemplate = "/blogs/{id}/posts", Method = "POST")]
        public HttpResponseMessage CreatePost(string id, HttpRequestMessage request)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            try
            {
                IPostsService postsService = ObjectFactory.GetInstance<IPostsService>();

                XmlReader reader = XmlReader.Create(request.Content.ReadAsStreamAsync().Result);
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                SyndicationItem item = feed.Items.FirstOrDefault();

                if (item != null)
                {
                    Post newPost = new Post();
                    newPost.content = ((TextSyndicationContent) item.Content).Text;
                    newPost.published = item.PublishDate;
                    newPost.updated = item.LastUpdatedTime;
                    newPost.title = item.Title.Text;

                    var author = item.Authors.FirstOrDefault();

                    if (author != null)
                    {
                        newPost.author = author.Name;
                        newPost.blogId = String.Format("blogs/{0}", id);
                        postsService.Create(newPost);

                        response.StatusCode = HttpStatusCode.Created;
                        response.Headers.Add("Location", String.Format("{0}/blogs/{1}/{2}", this.serviceURI, id, newPost.Id));
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        [WebGet(UriTemplate = "/blogs/{blogId}/posts/{id}")]
        public HttpResponseMessage GetPost(string blogId, string id, HttpRequestMessage request)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            try
            {
                IPostsService postsService = ObjectFactory.GetInstance<IPostsService>();
                var post = postsService.Get(String.Format("posts/{0}", id));

                var etag = request.Headers.IfNoneMatch.FirstOrDefault();

                if (etag != null && etag.Tag == post.etag)
                {
                    response.StatusCode = HttpStatusCode.NotModified;
                }
                else
                {
                    if (post != null)
                    {
                        if (this.ClientAcceptsMediaType("text/html", request))
                        {
                            response.Content = new ObjectContent<string>(post.ToHtml(), "text/html");
                        }
                        else
                        {
                            SyndicationFeed postFeed = new SyndicationFeed();
                            postFeed.Title = new TextSyndicationContent("Single Post");
                            postFeed.LastUpdatedTime = new DateTimeOffset(DateTime.Now);
                            postFeed.Links.Add(SyndicationLink.CreateSelfLink(request.RequestUri));

                            SyndicationItem item = new SyndicationItem();
                            List<SyndicationItem> itemList = new List<SyndicationItem>();
                            itemList.Add(item);
                            postFeed.Items = itemList;

                            item.Id = post.Id;
                            item.LastUpdatedTime = post.updated;
                            item.PublishDate = post.published;
                            item.Title = new TextSyndicationContent(post.title);
                            item.Content = new TextSyndicationContent(post.content);

                            item.Links.Add(SyndicationLink.CreateSelfLink(request.RequestUri));
                            item.Links.Add(SyndicationLink.CreateAlternateLink(request.RequestUri, "text/html"));

                            item.Links.Add(new SyndicationLink(request.RequestUri, "service.edit", "Edit Post", "application/atom+xml;type=feed", 0));
                            item.Links.Add(new SyndicationLink(new Uri(String.Format("{0}/blogs/{1}/{2}/{3}", this.serviceURI, blogId, post.Id, "service.comments")), "comments", "Post comments", "application/atom+xml;type=feed", 0));
                            item.Links.Add(new SyndicationLink(new Uri(String.Format("{0}/blogs/{1}", this.serviceURI, blogId)), "service.blog", "Parent blog", "application/atom+xml;type=feed", 0));

                            item.Authors.Add(new SyndicationPerson(string.Empty, post.author, string.Empty));

                            SyndicationFeedFormatter formatter = null;

                            if (this.ClientAcceptsMediaType("application/atom+xml", request))
                            {
                                formatter = postFeed.GetAtom10Formatter();
                            }
                            else
                            {
                                if (this.ClientAcceptsMediaType("application/rss+xml", request))
                                {
                                    formatter = postFeed.GetRss20Formatter();
                                }
                            }

                            response.Content = new ObjectContent(typeof(SyndicationFeedFormatter), formatter);
                        }
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.NotFound;
                    }
                }
            }
            catch (Exception)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        [WebInvoke(UriTemplate = "/blogs/{blogId}/posts/{id}", Method = "PUT")]
        public HttpResponseMessage UpdatePost(string blogId, string id, HttpRequestMessage request)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NoContent);

            try
            {
                IPostsService postsService = ObjectFactory.GetInstance<IPostsService>();
                var post = postsService.Get(String.Format("posts/{0}", id));

                if (post != null)
                {
                    XmlReader reader = XmlReader.Create(request.Content.ReadAsStreamAsync().Result);
                    SyndicationFeed feed = SyndicationFeed.Load(reader);
                    SyndicationItem item = feed.Items.FirstOrDefault();

                    if (item != null)
                    {
                        post.title = item.Title.Text;
                        post.updated = item.LastUpdatedTime;
                        post.published = item.PublishDate;
                        post.content = ((TextSyndicationContent) item.Content).Text;

                        var author = item.Authors.FirstOrDefault();

                        if (author != null)
                        {
                            post.author = author.Name;
                            postsService.Update(post);
                        }
                        else
                        {
                            response.StatusCode = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                }
            }
            catch (Exception)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        [WebInvoke(UriTemplate = "/blogs/{blogId}/posts/{id}", Method = "DELETE")]
        public HttpResponseMessage DeletePost(string blogId, string id, HttpRequestMessage request)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NoContent);

            try
            {
                IPostsService postsService = ObjectFactory.GetInstance<IPostsService>();
                var post = postsService.Get(String.Format("posts/{0}", id));

                if (post != null)
                {
                    postsService.Delete(String.Format("posts/{0}", id));
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        #endregion

        #region Comments operations

        [WebGet(UriTemplate = "/blogs/{blogId}/posts/{postId}/comments")]
        public HttpResponseMessage GetComments(string blogId, string postId, HttpRequestMessage request, int pageIndex = 1, int pageSize = 10)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            try
            {
                ICommentsService commentsService = ObjectFactory.GetInstance<ICommentsService>();
                var comments = commentsService.GetAllFromPost(String.Format("posts/{0}", postId), pageIndex, pageSize);

                if (comments != null)
                {
                    if (this.ClientAcceptsMediaType("text/html", request))
                    {
                        var commentsHtml = comments.GenerateCommentsHtml();
                        response.Content = new ObjectContent<string>(commentsHtml, "text/html");
                    }
                    else
                    {
                        SyndicationFeed postCommentsFeed = new SyndicationFeed();
                        postCommentsFeed.Title = new TextSyndicationContent(String.Format("Post {0} comments", postId));
                        postCommentsFeed.LastUpdatedTime = new DateTimeOffset(DateTime.Now);
                        postCommentsFeed.Links.Add(SyndicationLink.CreateSelfLink(request.RequestUri));

                        SyndicationItem item = null;
                        List<SyndicationItem> itemList = new List<SyndicationItem>();
                        postCommentsFeed.Items = itemList;

                        foreach (var comment in comments)
                        {
                            item = new SyndicationItem();
                            item.Id = comment.Id;
                            item.LastUpdatedTime = comment.updated;
                            item.PublishDate = comment.published;

                            item.Links.Add(SyndicationLink.CreateSelfLink(new Uri(String.Format("{0}/blogs/{1}/posts/{2}/{3}", this.serviceURI, blogId, postId, comment.Id))));
                            item.Links.Add(SyndicationLink.CreateAlternateLink(request.RequestUri, "text/html"));

                            item.Links.Add(new SyndicationLink(new Uri(String.Format("{0}/blogs/{1}/posts/{2}", this.serviceURI, blogId, postId)), "service.post", "Parent post", "application/atom+xml;type=feed", 0));
                            item.Links.Add(new SyndicationLink(new Uri(String.Format("{0}/blogs/{1}/posts/{2}/{3}", this.serviceURI, blogId, postId, comment.Id)), "service.edit", "Edit comment", "application/atom+xml;type=feed", 0));

                            var pagingLinks = this.BuildPagingLinks(commentsService.Count(), pageIndex, pageSize, request.RequestUri);

                            foreach (var link in pagingLinks)
                            {
                                item.Links.Add(link);
                            }

                            item.Authors.Add(new SyndicationPerson(string.Empty, comment.author, string.Empty));
                            item.Content = SyndicationContent.CreatePlaintextContent(comment.content);

                            itemList.Add(item);
                        }


                        SyndicationFeedFormatter formatter = null;

                        if (this.ClientAcceptsMediaType("application/atom+xml", request))
                        {
                            formatter = postCommentsFeed.GetAtom10Formatter();
                        }
                        else
                        {
                            if (this.ClientAcceptsMediaType("application/rss+xml", request))
                            {
                                formatter = postCommentsFeed.GetRss20Formatter();
                            }
                        }

                        response.Content = new ObjectContent(typeof(SyndicationFeedFormatter), formatter);
                    }    
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NoContent;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        [WebInvoke(UriTemplate = "/blogs/{blogId}/posts/{postId}/comments", Method = "POST")]
        public HttpResponseMessage CreateComment(string blogId, string postId, HttpRequestMessage request)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            try
            {
                ICommentsService commentsService = ObjectFactory.GetInstance<ICommentsService>();

                XmlReader reader = XmlReader.Create(request.Content.ReadAsStreamAsync().Result);
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                SyndicationItem item = feed.Items.FirstOrDefault();

                if (item != null)
                {
                    Comment newComment = new Comment();
                    newComment.content = ((TextSyndicationContent) item.Content).Text;
                    newComment.published = item.PublishDate;
                    newComment.updated = item.LastUpdatedTime;

                    var author = item.Authors.FirstOrDefault();

                    if (author != null)
                    {
                        newComment.author = author.Name;
                        newComment.postId = String.Format("posts/{0}", postId);
                        commentsService.Create(newComment);

                        response.StatusCode = HttpStatusCode.Created;
                        response.Headers.Add("Location", String.Format("{0}/blogs/{1}/posts/{2}/{3}", this.serviceURI, blogId, postId, newComment.Id));
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError; 
            }

            return response;
        }

        [WebGet(UriTemplate = "/blogs/{blogId}/posts/{postId}/comments/{id}")]
        public HttpResponseMessage GetComment(string blogId, string postId, string id, HttpRequestMessage request)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            try
            {
                ICommentsService commentsService = ObjectFactory.GetInstance<ICommentsService>();
                var comment = commentsService.Get(String.Format("comments/{0}", id));

                var etag = request.Headers.IfNoneMatch.FirstOrDefault();

                if (etag != null && etag.Tag == comment.etag)
                {
                    response.StatusCode = HttpStatusCode.NotModified;
                }
                else
                {
                    if (comment != null)
                    {
                        if (this.ClientAcceptsMediaType("text/html", request))
                        {
                            response.Content = new ObjectContent<string>(comment.ToHtml(), "text/html");
                        }
                        else
                        {
                            SyndicationFeed commentFeed = new SyndicationFeed();
                            commentFeed.Title = new TextSyndicationContent("Single Comment");
                            commentFeed.LastUpdatedTime = new DateTimeOffset(DateTime.Now);
                            commentFeed.Links.Add(SyndicationLink.CreateSelfLink(request.RequestUri));

                            SyndicationItem item = new SyndicationItem();
                            List<SyndicationItem> itemList = new List<SyndicationItem>();
                            itemList.Add(item);
                            commentFeed.Items = itemList;

                            item.Id = comment.Id;
                            item.LastUpdatedTime = comment.updated;
                            item.PublishDate = comment.published;
                            item.Content = new TextSyndicationContent(comment.content);

                            item.Links.Add(SyndicationLink.CreateSelfLink(request.RequestUri));
                            item.Links.Add(SyndicationLink.CreateAlternateLink(request.RequestUri, "text/html"));

                            item.Links.Add(new SyndicationLink(request.RequestUri, "service.edit", "Edit Comment", "application/atom+xml;type=feed", 0));
                            item.Links.Add(new SyndicationLink(new Uri(String.Format("{0}/blogs/{1}/posts/{2}/{3}", this.serviceURI, blogId, postId, comment.Id)), "service.comments", "Post comments", "application/atom+xml;type=feed", 0));
                            item.Links.Add(new SyndicationLink(new Uri(String.Format("{0}/blogs/{1}/posts/{2}", this.serviceURI, blogId, postId)), "service.post", "Parent post", "application/atom+xml;type=feed", 0));

                            item.Authors.Add(new SyndicationPerson(string.Empty, comment.author, string.Empty));

                            SyndicationFeedFormatter formatter = null;

                            if (this.ClientAcceptsMediaType("application/atom+xml", request))
                            {
                                formatter = commentFeed.GetAtom10Formatter();
                            }
                            else
                            {
                                if (this.ClientAcceptsMediaType("application/rss+xml", request))
                                {
                                    formatter = commentFeed.GetRss20Formatter();
                                }
                            }

                            response.Content = new ObjectContent(typeof(SyndicationFeedFormatter), formatter);
                        }
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.NotFound;
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        [WebInvoke(UriTemplate = "/blogs/{blogId}/posts/{postId}/comments/{id}", Method = "PUT")]
        public HttpResponseMessage UpdateComment(string blogId, string postId, string id, HttpRequestMessage request)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NoContent);

            try
            {
                ICommentsService commentsService = ObjectFactory.GetInstance<ICommentsService>(); ;
                var comment = commentsService.Get(String.Format("comments/{0}", id));

                if (comment != null)
                {
                    XmlReader reader = XmlReader.Create(request.Content.ReadAsStreamAsync().Result);
                    SyndicationFeed feed = SyndicationFeed.Load(reader);
                    SyndicationItem item = feed.Items.FirstOrDefault();

                    if (item != null)
                    {
                        comment.updated = item.LastUpdatedTime;
                        comment.published = item.PublishDate;
                        comment.content = ((TextSyndicationContent) item.Content).Text;

                        var author = item.Authors.FirstOrDefault();

                        if (author != null)
                        {
                            comment.author = author.Name;
                            commentsService.Update(comment);
                        }
                        else
                        {
                            response.StatusCode = HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        [WebInvoke(UriTemplate = "/blogs/{blogId}/posts/{postId}/comments/{id}", Method = "DELETE")]
        public HttpResponseMessage DeleteComment(string blogId, string postId, string id, HttpRequestMessage request)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NoContent);

            try
            {
                ICommentsService commentsService = ObjectFactory.GetInstance<ICommentsService>();
                var comment = commentsService.Get(String.Format("comments/{0}", id));

                if (comment != null)
                {
                    commentsService.Delete(String.Format("comments/{0}", id));
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        #endregion

        #region Helper methods

        private string BuildtagCloud(List<string> tags)
        {
            StringBuilder buffer = new StringBuilder();

            foreach (var tag in tags)
            {
                buffer.Append(String.Format("{0};", tag));
            }

            return buffer.ToString();
        }

        private List<SyndicationLink> BuildPagingLinks(int totalCount, int pageIndex, int pageSize, Uri uri)
        {
            List<SyndicationLink> linksBuffer = new List<SyndicationLink>();
            int numberOfPages = pageSize >= totalCount ? 1 : totalCount / pageSize;

            SyndicationLink firstPage = new SyndicationLink(new Uri(String.Format("{0}?pageIndex=1&pageSize={1}", uri.AbsoluteUri, pageSize)), "first", "First Page", "application/atom+xml;type=feed", 0);
            SyndicationLink lastPage = new SyndicationLink(new Uri(String.Format("{0}?pageIndex={1}&pageSize={2}", uri.AbsoluteUri, numberOfPages, pageSize)), "last", "Last Page", "application/atom+xml;type=feed", 0);

            SyndicationLink previousPage = new SyndicationLink(new Uri(String.Format("{0}?pageIndex={1}&pageSize={2}", uri.AbsoluteUri, pageIndex - 1, pageSize)), "first", "First Page", "application/atom+xml;type=feed", 0);
            SyndicationLink nextPage = new SyndicationLink(new Uri(String.Format("{0}?pageIndex={1}&pageSize={2}", uri.AbsoluteUri, pageIndex + 1, pageSize)), "first", "First Page", "application/atom+xml;type=feed", 0);

            linksBuffer.Add(firstPage);
            linksBuffer.Add(lastPage);

            if (pageIndex - 1 > 1)
            {
                linksBuffer.Add(previousPage);
            }

            if (pageIndex + 1 < numberOfPages)
            {
                linksBuffer.Add(nextPage);
            }

            return linksBuffer;
        }

        private bool ClientAcceptsMediaType(string mediaType, HttpRequestMessage request)
        {
            return request.Headers.Accept.Any(m => m.MediaType == mediaType);
        }

        #endregion
    }
}