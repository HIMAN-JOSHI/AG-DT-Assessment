
namespace ApiTests.Models
{
    /// <summary>
    /// Represents a comment. 
    /// </summary>
    public class Comment
    {
        // Identifier of the post
        public long PostId { get; set; }

        // Unique identifier for the comment
        public long Id { get; set; }

        // Name of the commenter
        public string Name { get; set; }

        // Email address of the commenter
        public string Email { get; set; }

        // Content/body of the comment
        public string Body { get; set; } 

        /// <summary>
        /// Initializes a new comment with post ID, name, email, and content.
        /// </summary>
        public Comment(long postId, string name, string email, string body)
        {
            PostId = postId;
            Name = name;
            Email = email;
            Body = body;
        }
    }

}
