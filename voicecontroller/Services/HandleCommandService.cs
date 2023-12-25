using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace voicecontroller.Services
{
    public class HandleCommandService
    {
        string _CommandTextToProcess;
        Dictionary<string, string> _CommandDictionary = new Dictionary<string, string>();
        public HandleCommandService(string CommandTextToProcess)
        {
            _CommandTextToProcess = CommandTextToProcess;
        }

        public async Task<bool> ProcessCommand()
        {
            if (_CommandTextToProcess == null)
            {
                return await Task.FromResult(false);
            }
            else
            {
                //IF TEXT CONTAINS "OPEN"
                if (_CommandTextToProcess.ToLower().Contains("open"))
                {
                    if (_CommandTextToProcess.ToLower().Contains("spotify"))
                    {
                        await Launcher.OpenAsync("spotify:");
                    }
                }
                return await Task.FromResult(true);
            }
        }
    }
}
