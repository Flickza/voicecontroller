using voicecontroller.Services;
using static System.Net.Mime.MediaTypeNames;

namespace voicecontroller
{
    public partial class MainPage : ContentPage
    {
        bool isListening = false;

        public MainPage()
        {
            InitializeComponent();
        }
        private async void OnCaptureVoiceClicked(object sender, EventArgs e)
        {
            if(isListening)
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
                
                var detectSpeech = new SpeechRecognitionService();
                var text = await detectSpeech.RecognizeSpeechAsync();
                VoiceResult.Text = text;
                
                var handleCommand = new HandleCommandService(text);
                await handleCommand.ProcessCommand();

                isListening = false;
                CaptureVoice.Text = "Start voice command";
                SemanticScreenReader.Announce("Listening started");
            }
        }
    }

}
