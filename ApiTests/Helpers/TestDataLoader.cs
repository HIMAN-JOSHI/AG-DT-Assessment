namespace ApiTests.Helpers
{
    /// <summary>
    /// Provides methods for loading and managing test data for test execution.
    /// </summary>
    public static class TestDataLoader
    {
        // Method to load test cases by key
        public static IEnumerable<object[]> LoadTestCases(String key) {

            return allData.TryGetValue(key, out var data) ? data : new List<object[]>();
        }

        // Dictionary to store test data for different operations
        private static readonly Dictionary<String, List<object[]>> allData = new()
        {
            {
                "GetPostTestCases", new List<object[]>
                { 
                    // Test for valid postId and expected title
                    new object[] { 1, "sunt aut facere repellat provident occaecati excepturi optio reprehenderit" },
                    // Test for non-existent postId (negative test case)
                    new object[] { 7777, null},
                    // Edge case
                    new object[] { 0, null}
                }
            },
            {
                "CreatePostTestCases", new List<object[]>
                { 
                    // Valid post creation with userId, title and body
                    new object[] { 1, "New Test Post", "This is a test post body", 201},
                    // Test case for creating a post with empty title and body (negative test case)
                    new object[] { 1, "", "", 201},
                    
                    // Test case for invalid userId
                    // new object[] { -1, "Invalid User Post", "This post should fail", 400}
                }
            },
            {
                "UpdatePostTestCases", new List<object[]>
                { 
                    // Test for valid postId, title and body update
                    new object[] {1, "Updated title", "Updated body", 200 },
                    // Test for updating non-existent postId (negative test case)
                    new object[] { 7777, "Non-existent Post", "This should fail", 500},
                    // Test for empty body update
                    new object[] {1, "Post title updated", "", 200 }
                }
            },
            {
                "DeletePostTestCases", new List<object[]>
                { 
                    // Test for valid postId deletion
                    new object[] { 1, 200},
                    
                    // Test for deletion of a non-existent postId
                    //new object[] { 7777, 404},
                    
                }
            },
            {
                "AddCommentTestCases", new List<object[]>
                { 
                    // Valid comment creation
                    new object[] {1, "Test Comment", "abc@xyz.com", "This is a comment body.", 201 },
                    // Test case for empty comment name (negative test case)
                    new object[] {1, "", "invalid@xyz.com", "", 201},
                    // Test case for invalid email format
                    new object[] {1, "Comment", "invalid-email", "Comment body", 201 }
                }
            },
            {
                "GetCommentsForPostTestCases", new List<object[]>
                { 
                    // Valid postId with expected comment count
                    new object[] {1, 5 },
                    // Test case fr non-existent postId (negative case)
                    new object[] { 7777, 0},
                    // Edge case
                    new object[] { 0, 0}
                }
            }
        };
    }
}
