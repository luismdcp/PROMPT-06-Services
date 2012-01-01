using System;
using System.Text;

namespace RESTBlogs.Domain
{
    public class Comment
    {
        public string Id { get; set; }
        public string content { get; set; }
        public DateTimeOffset published { get; set; }
        public DateTimeOffset updated { get; set; }
        public string author { get; set; }
        public string postId { get; set; }

        public string ToHtml()
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append("<fieldset>");
            buffer.Append("<legend>Comment</legend>");
            buffer.Append("<p><label>Content: </label>");
            buffer.Append(this.content);
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