using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using voicecontroller.Models;

namespace voicecontroller.Services
{
    public class LoadExistingPairsService
    {
        public LoadExistingPairsService() { }

        public async Task<List<CommandAppPair>> LoadExistingPairsAsync()
        {
            try
            {
                var file = Path.Combine(FileSystem.AppDataDirectory, "commandAppPairs.json");

                // Check if the file exists
                if (!File.Exists(file))
                {
                    // If the file does not exist, return an empty list
                    return new List<CommandAppPair>();
                }

                // Read the file content
                string json = await File.ReadAllTextAsync(file);

                // Deserialize the JSON string to a list of CommandAppPair objects
                var commandAppPairs = JsonSerializer.Deserialize<List<CommandAppPair>>(json);

                // If the deserialization returns null (e.g., empty or corrupted file), return an empty list
                return commandAppPairs ?? new List<CommandAppPair>();
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                Console.WriteLine($"Error loading command-app pairs: {ex.Message}");
                return new List<CommandAppPair>();
            }
        }
    }
}
