using ApiTests.Helpers;
using ApiTests.Models;
using ApiTests.Services;
using Common.Helpers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using Assert = NUnit.Framework.Assert;
using DescriptionAttribute = NUnit.Framework.DescriptionAttribute;


namespace ApiTests
{
    /// <summary>
    /// Contains test methods for validating various API endpoints related to post resources.
    /// </summary>
    [TestFixture]
    
    public class PostTests : BaseApiTest
    {
        // Required PostService instance for making API calls
        public required PostService _postService;

        /// <summary>
        /// Setup method runs before each test. Initializes the PostService.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _postService = new PostService(ApiHelper, Logger);
        }

        /// <summary>
        /// Test that ensures retrieving all posts returns a successful response with HTTP 200 OK and non-empty content.
        /// </summary>
        [Test, Description("Validates that retrieving all posts returns a successful response with HTTP 200 OK and non-empty content.")]

        public async Task Test_GetAllPosts_ReturnsSuccess()
        {
            try
            {
                // Log the start of the test
                WriteMessagesToLogsAndReports("Starting Test: Test_GetAllPosts_ReturnsSuccess");

                // Make the API call to get all posts
                var response = await _postService.GetAllPostsAsync();
                WriteMessagesToReports("Received response");

                // Assertions to validate response
                Assert.That(response.IsSuccessful, "Expected a successful response.");
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.Content, Is.Not.Empty, "Expected a non-empty content.");

                // Log the completion of the test
                WriteMessagesToLogsAndReports("Completed Test: Test_GetAllPosts_ReturnsSuccess");
            }
            catch (Exception ex)
            {
                // Handle and log exceptions
                WriteExceptionsToLogsAndReports(ex);
            }
        }


        /// <summary>
        /// Test that ensures retrieving a post by its ID returns the expected status and content.
        /// Verifies HTTP 200 OK and title for existing post, or HTTP 404 for non-existent post.
        /// </summary>
        [Test, Description("Verifies that retrieving a post by its ID returns HTTP 200 OK and contains the expected title when found, or HTTP 404 Not Found when the post does not exist.")]

        [TestCaseSource(typeof(TestDataLoader), nameof(TestDataLoader.LoadTestCases), new object[] { "GetPostTestCases" })]
        public async Task Test_GetPostById_Should_ReturnSuccess(int postId, String expectedTitle)
        {
            try
            {
                WriteMessagesToLogsAndReports("Starting Test: Test_GetPostById_Should_ReturnSuccess");

                // Make the API call to get a post by ID
                var response = await _postService.GetPostByIdAsync(postId);
                WriteMessagesToReports("Received response");

                // If the expected title is null, validate 404 status code
                if (expectedTitle == null)
                {
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound), "Expected HTTP 404 Not Found for non-existent post.");
                }
                else
                {
                    // Otherwise, validate 200 OK and the presence of the expected title in the content
                    Assert.That(response.IsSuccessful, "Expected a successful response.");
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Expected HTTP 200 OK status code.");
                    Assert.That(response.Content, Contains.Substring(expectedTitle), "Expected content to contain the post title.");
                }

                WriteMessagesToLogsAndReports("Completed Test: Test_GetPostById_Should_ReturnSuccess");
            }
            catch (Exception ex)
            {
                WriteExceptionsToLogsAndReports(ex);
            }
        }

        /// <summary>
        /// Tests creating a new post by validating the response code against the expected HTTP status.
        /// </summary>
        [Test, Description("Tests the creation of a new post by validating the response code against the expected HTTP status.")]
        [TestCaseSource(typeof(TestDataLoader), nameof(TestDataLoader.LoadTestCases), new object[] { "CreatePostTestCases" })]
        public async Task Test_CreatePost(long userId, String title, String body, int expectedStatusCode)
        {
            try
            {
                WriteMessagesToLogsAndReports("Starting test: Test_CreatePost ");

                // Create a new post object
                var newPost = new Post(userId, title, body);

                // Make the API call to create the post
                var response = await _postService.CreatePostAsync(newPost);
                WriteMessagesToReports("Received response");

                // Assert that the status code matches the expected value
                Assert.That((int)response.StatusCode, Is.EqualTo(expectedStatusCode), $"Expected HTTP {expectedStatusCode} for CreatePost");

                WriteMessagesToLogsAndReports("Completed Test: Test_CreatePost ");
            }
            catch (Exception ex)
            {
                WriteExceptionsToLogsAndReports(ex);
            }
        }

        /// <summary>
        /// Tests updating an existing post, verifying the HTTP status code after updating the post's title and body.
        /// </summary>
        [Test, Description("Validates the update functionality for an existing post by checking that the response code matches the expected HTTP status after updating the post's title and body.")]
        [TestCaseSource(typeof(TestDataLoader), nameof(TestDataLoader.LoadTestCases), new object[] { "UpdatePostTestCases" })]
        public async Task Test_UpdatePost(long postId, String newTitle, String newBody, int expectedStatusCode)
        {
            try
            {
                WriteMessagesToLogsAndReports("Starting test: Test_UpdatePost ");

                // Prepare the updated post details
                var updatedPost = new
                {
                    title = newTitle,
                    body = newBody
                };

                // Make the API call to update the post
                var response = await _postService.UpdatePostAsync(postId, updatedPost);
                WriteMessagesToReports("Received response");

                // Assert the status code matches the expected value
                Assert.That((int)response.StatusCode, Is.EqualTo(expectedStatusCode), $"Expected HTTP {expectedStatusCode} for UpdatePost");

                WriteMessagesToLogsAndReports("Completed Test: Test_UpdatePost ");
            }
            catch (Exception ex)
            {
                WriteExceptionsToLogsAndReports(ex);
            }
        }

        /// <summary>
        /// Tests the deletion of a post, ensuring that the correct status code is returned after the deletion.
        /// </summary>
        [Test, Description("Tests the deletion of a specified post and verifies that the HTTP response code matches the expected status.")]
        [TestCaseSource(typeof(TestDataLoader), nameof(TestDataLoader.LoadTestCases), new object[] { "DeletePostTestCases" })]
        public async Task Test_DeletePost(long postId, int expectedStatus)
        {
            try
            {
                WriteMessagesToLogsAndReports("Starting test: Test_DeletePost ");

                // Make the API call to delete the post
                var response = await _postService.DeletePostAsync(postId);
                WriteMessagesToReports("Received response");

                // Assert the status code is as expected
                Assert.That((int)response.StatusCode, Is.EqualTo(expectedStatus), $"Expected HTTP {expectedStatus} for DeletePost");

                WriteMessagesToLogsAndReports("Completed Test: Test_DeletePost ");
            }
            catch (Exception ex)
            {
                WriteExceptionsToLogsAndReports(ex);
            }
        }

        /// <summary>
        /// Verifies adding a comment to a post and checks the HTTP response code.
        /// </summary>
        [Test, Description("Validates adding a comment to a specific post and ensures that the response code is as expected.")]
        [TestCaseSource(typeof(TestDataLoader), nameof(TestDataLoader.LoadTestCases), new object[] { "AddCommentTestCases" })]
        public async Task Test_AddCommentToPost(long postId, String name, String email, String body, int expectedStatusCode)
        {
            try
            {
                WriteMessagesToLogsAndReports("Starting test: Test_AddCommentToPost ");

                // Create a new comment object
                var newComment = new Comment(postId, name, email, body);

                // Make the API call to add the comment to the post
                var response = await _postService.AddCommentToPostAsync(postId, newComment);
                WriteMessagesToReports("Received response");

                // Assert the status code is as expected
                Assert.That((int)response.StatusCode, Is.EqualTo(expectedStatusCode), $"Expected HTTP {expectedStatusCode} for AddCommentToPost");

                WriteMessagesToLogsAndReports("Completed Test: Test_AddCommentToPost ");
            }
            catch (Exception ex)
            {
                WriteExceptionsToLogsAndReports(ex);
            }
        }

        /// <summary>
        /// Validates retrieving comments for a post and compares the actual comment count to the expected count.
        /// </summary>
        [Test, Description("Checks that retrieving comments for a specific post returns a successful HTTP 200 response and matches the expected comment count.")]
        [TestCaseSource(typeof(TestDataLoader), nameof(TestDataLoader.LoadTestCases), new object[] { "GetCommentsForPostTestCases" })]
        public async Task Test_GetCommentsForPost(long postId, int expectedCommentCount)
        {
            try
            {
                WriteMessagesToLogsAndReports("Starting test: Test_GetCommentsForPost ");

                // Retrieve the comments for the post
                var response = await _postService.GetCommentsForPostAsync(postId);
                WriteMessagesToReports("Received response");

                // Assert that the response was successful and status code is 200 OK
                Assert.That(response.IsSuccessful, Is.True, "Expected successful response for GetCommentsForPost");
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                Comment[]? comments = Newtonsoft.Json.JsonConvert.DeserializeObject<Comment[]>(response.Content);
                Assert.That(comments, Is.Not.Null, "Comments are null");
                Assert.That(comments.Length, Is.EqualTo(expectedCommentCount), "Expected comment count mismatch.");
                
                WriteMessagesToLogsAndReports("Completed Test: Test_GetCommentsForPost ");
            }
            catch (Exception ex)
            {
                WriteExceptionsToLogsAndReports(ex);
            }
        }

        /// <summary>
        /// Measures the response time for retrieving a post by ID. 
        /// Ensures that the response time completes within 2 seconds.
        /// </summary>
        [Test, Description("Measures the response time of retrieving a post by ID, ensuring it completes within 2 seconds.")]
        public async Task GetPost_ResponseTime_ShouldBeUnderTwoSeconds()
        {
            try
            {
                // Log the start of the test
                WriteMessagesToLogsAndReports("Starting test: GetPost_ResponseTime_ShouldBeUnderTwoSeconds ");

                // Start a stopwatch to measure response time
                var stopWatch = System.Diagnostics.Stopwatch.StartNew();
                var response = await _postService.GetPostByIdAsync(1);
                WriteMessagesToReports("Received response");

                // Stop the stopwatch after receiving the response
                stopWatch.Stop();

                // Assert that the response time is under 2 seconds
                Assert.That(stopWatch.ElapsedMilliseconds, Is.LessThanOrEqualTo(2000), "Response time exceeded 2 seconds.");

                // Log the completion of the test
                WriteMessagesToLogsAndReports("Completed Test: GetPost_ResponseTime_ShouldBeUnderTwoSeconds ");
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during the test
                WriteExceptionsToLogsAndReports(ex);
            }
        }

        /// <summary>
        /// Verifies that the response content type for retrieving a post by ID is 'application/json'.
        /// </summary>
        [Test, Description("Verifies that the response content type for retrieving a post by ID is 'application/json'.")]
        public async Task GetPost_ResponseContentType_ShouldBeJson()
        {
            try
            {
                // Log the start of the test
                WriteMessagesToLogsAndReports("Starting test: GetPost_ResponseContentType_ShouldBeJson ");

                // Make the API call to retrieve the post by ID
                var response = await _postService.GetPostByIdAsync(1);
                WriteMessagesToReports("Received response");

                // Assert that the response content type is 'application/json'
                Assert.That("application/json", Is.EqualTo(response.ContentType), "Response Content-Type is not application/json.");

                // Log the completion of the test
                WriteMessagesToLogsAndReports("Completed Test: GetPost_ResponseContentType_ShouldBeJson ");
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during the test
                WriteExceptionsToLogsAndReports(ex);
            }
        }

        /// <summary>
        /// Checks that the response for retrieving a post by ID contains the required fields: Id, UserId, Title, and Body.
        /// </summary>
        [Test, Description("Checks that the response for retrieving a post by ID contains required fields (Id, UserId, Title, and Body).")]
        public async Task GetPost_Response_ShouldContainRequiredFields()
        {
            try
            {
                // Log the start of the test
                WriteMessagesToLogsAndReports("Starting test: GetPost_Response_ShouldContainRequiredFields ");

                // Make the API call to retrieve the post by ID
                var response = await _postService.GetPostByIdAsync(1);
                WriteMessagesToReports("Received response");

                // If the response content is empty or null, fail the test
                if (String.IsNullOrEmpty(response.Content))
                {
                    Assert.Fail("Response content is null or empty");
                    return;
                }

                // Deserialize the response content into a Post object
                Post? postData = JsonConvert.DeserializeObject<Post>(response.Content);

                // Assert that the response content was successfully deserialized
                Assert.That(postData, Is.Not.Null, "The response content could not be deserialized into a Post object.");

                // Assert that the required fields are not null in the response
                Assert.Multiple(() =>
                {
                    Assert.That(postData.Id, Is.Not.Null, "Id field is null in the response");
                    Assert.That(postData.UserId, Is.Not.Null, "UserId field is null in the response");
                    Assert.That(postData.Title, Is.Not.Null, "Title field is null in the response");
                    Assert.That(postData.Body, Is.Not.Null, "Body field is null in the response");
                });

                // Log the completion of the test
                WriteMessagesToLogsAndReports("Completed Test: GetPost_Response_ShouldContainRequiredFields ");
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during the test
                WriteExceptionsToLogsAndReports(ex);
            }
        }

        /// <summary>
        /// Logs exceptions to both logs and reports when a test fails.
        /// </summary>
        /// <param name="ex">The exception that occurred during the test.</param>
        private void WriteExceptionsToLogsAndReports(Exception ex) {

            Logger.LogError($"Test case failed due to : {ex}");
            ReportingHelper.LogFail($"Test case failed due : {ex}");
        }

        /// <summary>
        /// Logs messages to both logs and reports.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        private void WriteMessagesToLogsAndReports(String message) {

            WriteMessagesToLogs(message);
            WriteMessagesToReports(message);
        }

        /// <summary>
        /// Logs messages to the logs.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        private void WriteMessagesToLogs(String message) {
            Logger.LogInformation(message);
        }

        /// <summary>
        /// Logs messages to the reports.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        private void WriteMessagesToReports(String message) {
            ReportingHelper.LogInfo(message);
        }
        
    }
}