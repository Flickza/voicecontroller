using System.Diagnostics;
using voicecontroller.Models;

namespace voicecontroller.Services
{
    public class HandleCommandService
    {
        LoadExistingPairsService _loadExistingPairsService;
        string _CommandTextToProcess;
        List<CommandAppPair>? _CommandDictionary;
        public HandleCommandService(string CommandTextToProcess)
        {
            _loadExistingPairsService = new LoadExistingPairsService();
            _CommandTextToProcess = CommandTextToProcess;
        }
        public async Task InitializeAsync()
        {
            _CommandDictionary = await _loadExistingPairsService.LoadExistingPairsAsync();
        }


        public async Task<string> ProcessCommand()
        {
            if (_CommandTextToProcess == null)
            {
                return await Task.FromResult("No command recieved.");
            }
            else
            {
                // test if the command is in the dictionary
                var commandAppPair = _CommandDictionary?.FirstOrDefault(c => c.Command.ToLower() == _CommandTextToProcess.ToLower());
                if (commandAppPair != null)
                {
                    await LaunchApplication(commandAppPair.ApplicationPath);
                    return await Task.FromResult($"Command running...");
                }
                else
                {
                    // if it is not, announce that the command is not recognized
                    SemanticScreenReader.Announce("Command not recognized");
                    return await Task.FromResult($"Command not recognized. {_CommandTextToProcess}");
                }
            }
        }
        public static Task LaunchApplication(string appPath)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = appPath,
                    UseShellExecute = true
                };

                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"Error launching application: {ex.Message}");
            }
            return Task.CompletedTask;
        }
    }
}
