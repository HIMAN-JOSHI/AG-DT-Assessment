
namespace ApiTests.Models
{
    /// <summary>
    /// Represents a post
    /// </summary>
    public class Post
    {
        // Unique identifier for the post
        public long Id { get; set; }

        // Identifier for the userId
        public long UserId { get; set; }

        // Title of the post
        public string Title { get; set; }

        // Content/body of the post
        public string Body { get; set; } 

        /// <summary>
        /// Initializes a new post with user ID, title, and body.
        /// </summary>
        public Post(long userId, string title, string body)
        {
            UserId = userId;
            Title = title;
            Body = body;
        }
    }

}
