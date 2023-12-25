using Windows.Media.SpeechRecognition;
using System.Threading.Tasks;

public class SpeechRecognitionService
{
    private SpeechRecognizer _speechRecognizer;

    public SpeechRecognitionService()
    {
        _speechRecognizer = new SpeechRecognizer();
    }

    public async Task<string> RecognizeSpeechAsync()
    {
        try
        {

            if (await IsSpeechPrivacyPolicyAccepted() == false)
            {
                return "Speech privacy policy not accepted. Please enable speech services in settings.";
            }

            // Compile the default speech recognition constraints (optional)
            await _speechRecognizer.CompileConstraintsAsync();

            // Start recognition
            SpeechRecognitionResult result = await _speechRecognizer.RecognizeAsync();

            if (result.Status == SpeechRecognitionResultStatus.Success)
            {
                return result.Text;
            }
            else
            {
                return $"Error in recognition: {result.Status}";
            }
        } catch(Exception e)
        {
            return $"Error in recognition: {e.Message}";
        }
    }

    private async Task<bool> IsSpeechPrivacyPolicyAccepted()
    {
        // Compile constraints and check the result
        var compilationResult = await _speechRecognizer.CompileConstraintsAsync();
        return compilationResult.Status == SpeechRecognitionResultStatus.Success;
    }
    private async Task<bool> IsSpeechSessionFinished()
    {
        // Compile constraints and check the result
        var compilationResult = await _speechRecognizer.CompileConstraintsAsync();
        return compilationResult.Status == SpeechRecognitionResultStatus.Success;
    }
}
