using voicecontroller.Models;
using voicecontroller.Services;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Maui.Controls.Compatibility.Platform.UWP;
using System.Collections.ObjectModel;
using Windows.UI.Core;
using Microsoft.UI.Xaml.Input;
using Windows.System;

namespace voicecontroller
{
    public partial class MainPage : ContentPage
    {
        bool isListening = false;
        private string? selectedAppPath;
        LoadExistingPairsService _loadExistingPairsService;

        public ObservableCollection<CommandAppPair> CommandAppPairs { get; set; }


        public MainPage()
        {
            InitializeComponent();

            _loadExistingPairsService = new LoadExistingPairsService();

            CommandAppPairs = new ObservableCollection<CommandAppPair>();

            commandsListView.ItemsSource = CommandAppPairs;

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadCommandAppPairsAsync();
        }
        private async void OnCaptureVoiceClicked(object sender, EventArgs e)
        {
            if (isListening)
            {
                isListening = false;
                CaptureVoice.Text = "Start voice command";
                VoiceResult.Text = "";
                SemanticScreenReader.Announce("Listening stopped");
            }
            else
            {
                isListening = true;
                CaptureVoice.Text = "Listening...";
                SemanticScreenReader.Announce("Listening started");

                var detectSpeech = new SpeechRecognitionService();
                var text = await detectSpeech.RecognizeSpeechAsync();
                VoiceResult.Text = text;

                var handleCommand = new HandleCommandService(text);
                
                await handleCommand.InitializeAsync();
                var response = await handleCommand.ProcessCommand();

                VoiceResult.Text = response;
                isListening = false;
                CaptureVoice.Text = "Start voice command";

                await Task.Delay(2000);
                VoiceResult.Text = "";
            }
        }
        private async void OnSaveCommandClicked(object sender, EventArgs e)
        {
            if(selectedAppPath == null)
            {
                await DisplayAlert("Error", "Please select an app", "OK");
                return;
            }
            string commandString = commandStringEntry.Text;
            string appPath = selectedAppPath; // This should be set when an app is selected

            if (string.IsNullOrWhiteSpace(commandString) || string.IsNullOrWhiteSpace(appPath))
            {
                // Display an error message or handle the empty fields
                return;
            }

            // Create a model for your data
            var commandAppPair = new CommandAppPair
            {
                Command = commandString,
                ApplicationPath = appPath
            };

            // Save to local storage (example uses a JSON file)
            await SaveCommandAppPairAsync(commandAppPair);

            // Update your UI or collection to reflect the new data
        }

        private async Task SaveCommandAppPairAsync(CommandAppPair pair)
        {
            try
            {
                // Assuming you have a method to load existing pairs and save them
                var existingPairs = await _loadExistingPairsService.LoadExistingPairsAsync();
                //check for duplicates
                bool duplicate = false;
                foreach (var existingPair in existingPairs)
                {
                    if (existingPair.Command == pair.Command)
                    {
                        // Display an error message or handle the duplicate
                        duplicate = true;
                        return;
                    }
                }
                if (!duplicate)
                {
                    existingPairs.Add(pair);

                    string json = JsonSerializer.Serialize(existingPairs);

                    // Save the JSON to a file
                    var file = Path.Combine(FileSystem.AppDataDirectory, "commandAppPairs.json");
                    File.WriteAllText(file, json);
                    await LoadCommandAppPairsAsync();
                }
                else
                {
                    await DisplayAlert("Error", "Command already exists", "OK");
                }
            }
            catch (Exception er)
            {
                //open dialog with exception
                await DisplayAlert("Error", er.Message, "OK");
            }
        }

        private async void OnSelectAppClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.Default.PickAsync();
                if (result != null)
                {
                    selectedAppPath = result.FullPath;
                    // Update your UI to display the selected path
                }
            }
            catch (Exception er)
            {
                //open dialog with exception
                await DisplayAlert("Error", er.Message, "OK");
            }
        }
        private async void RemoveCommandClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is CommandAppPair commandAppPair)
            {
                CommandAppPairs.Remove(commandAppPair);
                await SaveCommandAppPairsAsync();
            }
        }
        private async Task SaveCommandAppPairsAsync()
        {
            var file = Path.Combine(FileSystem.AppDataDirectory, "commandAppPairs.json");
            string json = JsonSerializer.Serialize(CommandAppPairs.ToList());
            await File.WriteAllTextAsync(file, json);
        }
        public async Task LoadCommandAppPairsAsync()
        {
            var loadedPairs = await _loadExistingPairsService.LoadExistingPairsAsync(); // Your method to load data
            CommandAppPairs.Clear();

            foreach (var pair in loadedPairs)
            {
                CommandAppPairs.Add(pair);
            }
        }

    }

}
