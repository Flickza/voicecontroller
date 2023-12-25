namespace voicecontroller.Interfaces
{
    public interface IAudioRecorderService
    {
        Task StartRecordingAsync();
        Task StopRecordingAsync();
    }
}
