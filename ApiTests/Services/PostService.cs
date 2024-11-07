using ApiTests.Helpers;
using ApiTests.Models;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace ApiTests.Services
{
    /// <summary>
    /// Service class for interacting with the Post-related API endpoints.
    /// </summary>
    public class PostService
    {
        // Helper for making API requests
        private readonly ApiHelper _apiHelper;

        // Logger for logging API request/response details
        private readonly ILogger<PostService> _logger; 

        /// <summary>
        /// Initializes the PostService with dependencies.
        /// </summary>
        public PostService(ApiHelper apiHelper, ILogger<PostService> logger)
        {
            _apiHelper = apiHelper ?? throw new ArgumentNullException(nameof(apiHelper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET /posts - Retrieve all posts
        public async Task<RestResponse> GetAllPostsAsync()
        {
            _logger.LogInformation("Sending request to retrieve all posts.");
            var response = await _apiHelper.GetAsync("/posts");
            LogResponseDetails(response);
            return response;
        }

        // GET /posts/{postId} - Retrieve a specific post by ID
        public async Task<RestResponse> GetPostByIdAsync(int postId)
        {
            _logger.LogInformation($"Sending request to retrieve post with ID: {postId}");
            var response = await _apiHelper.GetAsync($"/posts/{postId}");
            LogResponseDetails(response);
            return response;
        }

        // POST /posts - Create a new post
        public async Task<RestResponse> CreatePostAsync(object post)
        {
            _logger.LogInformation("Sending request to create a new post.");
            var response = await _apiHelper.PostAsync("/posts", post);
            LogResponseDetails(response);
            return response;
        }

        // PUT /posts/{postId} - Update an existing post by ID
        public async Task<RestResponse> UpdatePostAsync(long postId, object updatedPost)
        {
            _logger.LogInformation($"Sending request to update post with ID: {postId}");
            var response = await _apiHelper.PutAsync($"/posts/{postId}", updatedPost);
            LogResponseDetails(response);
            return response;
        }

        // DELETE /posts/{postId} - Delete a post by ID
        public async Task<RestResponse> DeletePostAsync(long postId)
        {
            _logger.LogInformation($"Sending request to delete post with ID: {postId}");
            var response = await _apiHelper.DeleteAsync($"/posts/{postId}");
            LogResponseDetails(response);
            return response;
        }

        // Adds a comment to a specific post
        public async Task<RestResponse> AddCommentToPostAsync(long postId, Comment comment)
        {
            var endpointUrl = $"posts/{postId}/comments";
            _logger.LogInformation($"Adding a comment to post with ID: {postId}");
            var response = await _apiHelper.PostAsync(endpointUrl, comment);
            LogResponseDetails(response);
            if (response.IsSuccessful)
            {
                _logger.LogInformation("Comment successfully added.");
            }
            else
            {
                _logger.LogError($"Failed to add comment. Status: {response.StatusCode}, Error: {response.ErrorMessage}");
            }
            return response;
        }

        // Retrieves comments for a specific post
        public async Task<RestResponse> GetCommentsForPostAsync(long postId)
        {
            var endpointUrl = $"comments?postId={postId}";
            _logger.LogInformation($"Retrieving comments for post with ID: {postId}");
            var response = await _apiHelper.GetAsync(endpointUrl);
            if (response.IsSuccessful)
            {
                _logger.LogInformation("Comments successfully retrieved.");
            }
            else
            {
                _logger.LogInformation($"Failed to retrieve comments. Status: {response.StatusCode}, Error: {response.ErrorMessage}");
            }
            LogResponseDetails(response);
            return response;
        }

        // Helper method to log the response details
        private void LogResponseDetails(RestResponse response)
        {
            _logger.LogInformation($"Response Status Code:  {response.StatusCode}");
            _logger.LogInformation($"Response Content: {response.Content}");
            if (!response.IsSuccessful)
            {
                _logger.LogError($"Request failed with status {response.StatusCode}: {response.ErrorMessage}");
            }
        }
    }
}
