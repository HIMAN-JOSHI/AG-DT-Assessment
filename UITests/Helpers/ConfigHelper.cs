using Microsoft.Extensions.Configuration;


namespace UITests.Helpers
{
    // Utility class to handle configuration-related operations for retrieving UI element paths
    public static class ConfigHelper
    {
        // Private field to hold the configuration root, which provides access to the JSON configuration
        private static readonly IConfigurationRoot _config;

        // Static constructor to initialize the configuration by loading data from "UiPaths.json"
        static ConfigHelper()
        {
            // Get the base directory of the application, where the configuration file is expected to be located
            var basePath = AppContext.BaseDirectory;

            // Build the configuration object using "UiPaths.json", setting reloadOnChange to true to refresh on file changes
            _config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("UiPaths.json", optional: false, reloadOnChange: true)
                .Build();
        }

        // Retrieves the specified XPath or selector from the JSON configuration using the page and key parameters
        // Throws an exception if the path is not found in the configuration
        public static String GetPath(String page, String key) =>
            _config[$"{page}:{key}"] ?? throw new ArgumentNullException($"Path '{page}:{key}' not found in configuration.");
    }

}
