namespace FileCopyApp
{
    internal class ProgressModel
    {
        public ProgressModel(string logMessage, int progressValue = 0, int maxProgress = 0)
        {
            LogMessage = logMessage;
            ProgressValue = progressValue;
            MaxProgress = maxProgress;
        }

        public string LogMessage { get; set; }

        public int ProgressValue { get; set; }

        public int MaxProgress { get; set; }
    }
}