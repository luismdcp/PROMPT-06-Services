using System;
using System.Collections.Generic;
using System.Text;

namespace RESTBlogs.Domain
{
    public class Post
    {
        public string Id { get; set; }
        public string content { get; set; }
        public DateTimeOffset published { get; set; }
        public DateTimeOffset updated { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public List<string> tags { get; set; }
        public string blogId { get; set; }
        public string etag { get; set; }

        public Post()
        {
            this.tags = new List<string>();
        }

        public string ToHtml()
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append("<fieldset>");
            buffer.Append("<legend>Post</legend>");
            buffer.Append("<p><label>Title: </label>");
            buffer.Append(this.title);
            buffer.Append("</p>");
            buffer.Append("<p><label>Published: </label>");
            buffer.Append(this.published.ToString());
            buffer.Append("</p>");
            buffer.Append("<p><label>Updated: </label>");
            buffer.Append(this.updated.ToString());
            buffer.Append("</p>");
            buffer.Append("<p><label>Author: </label>");
            buffer.Append(this.author);
            buffer.Append("</p>");
            buffer.Append("<p><label>Content: </label>");
            buffer.Append(this.content);
            buffer.Append("</p>");
            buffer.Append("<fieldset>");
            buffer.Append("<legend>Tags</legend>");

            foreach (var tag in this.tags)
            {
                buffer.Append("<p>");
                buffer.Append(tag);
                buffer.Append("</p>");
            }

            buffer.Append("</fieldset>");
            buffer.Append("</fieldset>");

            return buffer.ToString();
        }
    }
}