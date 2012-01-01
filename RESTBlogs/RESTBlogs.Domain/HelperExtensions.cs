using System.Collections.Generic;
using System.Text;

namespace RESTBlogs.Domain
{
    public static class HelperExtensions
    {
        public static string GenerateBlogsHtml(this List<Blog> items)
        {
            StringBuilder buffer = new StringBuilder();

            foreach (var item in items)
            {
                buffer.Append(item.ToHtml());
            }

            return buffer.ToString();
        }

        public static string GeneratePostsHtml(this List<Post> items)
        {
            StringBuilder buffer = new StringBuilder();

            foreach (var item in items)
            {
                buffer.Append(item.ToHtml());
            }

            return buffer.ToString();
        }

        public static string GenerateCommentsHtml(this List<Comment> items)
        {
            StringBuilder buffer = new StringBuilder();

            foreach (var item in items)
            {
                buffer.Append(item.ToHtml());
            }

            return buffer.ToString();
        }
    }
}
