using System;
using System.Text;

namespace RESTBlogs.Domain
{
    public class Blog
    {
        public string Id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTimeOffset published { get; set; }
        public DateTimeOffset updated { get; set; }
        public string author { get; set; }
        public string etag { get; set; }

        public string ToHtml()
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append("<fieldset>");
            buffer.Append("<legend>Blog</legend>");
            buffer.Append("<p><label>Name: </label>");
            buffer.Append(this.name);
            buffer.Append("</p>");
            buffer.Append("<p><label>Description: </label>");
            buffer.Append(this.description);
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
            buffer.Append("</fieldset>");

            return buffer.ToString();
        }
    }
}