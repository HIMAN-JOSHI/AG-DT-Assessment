using Microsoft.Extensions.Logging;
using RestSharp;


namespace ApiTests.Helpers
{
    /// <summary>
    /// A helper class for making HTTP requests using RestClient.
    /// </summary>
    public class ApiHelper
    {
        private readonly RestClient _client;  // RestClient instance for making HTTP requests
        private readonly ILogger _logger;    // Logger for logging API request and response details

        /// <summary>
        /// Initializes the ApiHelper with a base URL and logger.
        /// </summary>
        public ApiHelper(String baseUrl, ILogger logger)
        {
            _client = new RestClient(baseUrl);  // Initialize RestClient with the provided base URL
            _logger = logger;  // Assign the logger instance
        }

        /// <summary>
        /// Creates a RestRequest with the specified endpoint and HTTP method.
        /// </summary>
        private RestRequest CreateRequest(String endpointUrl, Method method)
        {
            var request = new RestRequest(endpointUrl, method);  // Create a new request with the method and endpoint
            _logger.LogInformation($"Created {method} request for {endpointUrl}");  // Log the request creation
            return request;
        }

        /// <summary>
        /// Executes the given request asynchronously and logs the response.
        /// </summary>
        private async Task<RestResponse> ExecuteRequestAsync(RestRequest request)
        {
            var response = await _client.ExecuteAsync(request);  // Execute the request asynchronously
            LogResponse(response);  // Log the response details
            return response;
        }

        /// <summary>
        /// Sends a request with a body asynchronously (POST/PUT).
        /// </summary>
        private async Task<RestResponse> SendRequestWithBodyAsync(String endpointUrl, Method method, object body = null)
        {
            var request = CreateRequest(endpointUrl, method);  // Create the request
            request.AddJsonBody(body);  // Add the body to the request if provided
            return await ExecuteRequestAsync(request);  // Execute the request and return the response
        }

        /// <summary>
        /// Sends a GET request to the specified endpoint.
        /// </summary>
        public async Task<RestResponse> GetAsync(String endpointUrl)
        {
            var request = CreateRequest(endpointUrl, Method.Get);  // Create a GET request
            return await ExecuteRequestAsync(request);  // Execute and return the response
        }

        /// <summary>
        /// Sends a POST request to the specified endpoint with an optional body.
        /// </summary>
        public async Task<RestResponse> PostAsync(String endpointUrl, object body = null)
        {
            return await SendRequestWithBodyAsync(endpointUrl, Method.Post, body);  // Send a POST request
        }

        /// <summary>
        /// Sends a PUT request to the specified endpoint with an optional body.
        /// </summary>
        public async Task<RestResponse> PutAsync(String endpointUrl, object body = null)
        {
            return await SendRequestWithBodyAsync(endpointUrl, Method.Put, body);  // Send a PUT request
        }

        /// <summary>
        /// Sends a DELETE request to the specified endpoint.
        /// </summary>
        public async Task<RestResponse> DeleteAsync(String endpointUrl)
        {
            var request = CreateRequest(endpointUrl, Method.Delete);  // Create a DELETE request
            return await ExecuteRequestAsync(request);  // Execute and return the response
        }

        /// <summary>
        /// Logs the details of the response (status code, content, and errors).
        /// </summary>
        private void LogResponse(RestResponse response)
        {
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");  // Log the status code
            _logger.LogInformation($"Response Content: {response.Content}");  // Log the response content
            if (!response.IsSuccessful)  // If the request was not successful
            {
                _logger.LogError($"Request failed with status {response.StatusCode}: {response.ErrorMessage}");  // Log error details
            }
        }
    }
}
