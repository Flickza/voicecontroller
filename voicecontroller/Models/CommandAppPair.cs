using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace voicecontroller.Models
{
    public class CommandAppPair
    {
        // Property to store the command string
        public string Command { get; set; }

        // Property to store the file path of the application
        public string ApplicationPath { get; set; }

        // Constructor (optional, but useful for easy initialization)
        public CommandAppPair(string command, string appPath)
        {
            Command = command;
            ApplicationPath = appPath;
        }

        // Default constructor is necessary if you are using serialization/deserialization
        public CommandAppPair()
        {
        }

        // Optionally, you can override ToString() for easy debugging
        public override string ToString()
        {
            return $"Command: {Command}, AppPath: {ApplicationPath}";
        }
    }

}
