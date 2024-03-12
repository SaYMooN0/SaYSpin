namespace SaYSpin.src
{
    public static class Logger
    {
        private static string _fileToLog;

        public static void Init()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string _logsFolder = Path.Combine(basePath, "Logs");
            Directory.CreateDirectory(_logsFolder);
            _fileToLog = Path.Combine(_logsFolder, $"{CurrentTime()}-log.txt");

            if (!File.Exists(_fileToLog))
                File.Create(_fileToLog).Close();
            
        }

        public static void Log(string message) =>
            File.AppendAllText(_fileToLog, message + Environment.NewLine);
        public static void Log(object obj) => Log(obj.ToString());
        private static string CurrentTime() => DateTime.Now.ToString("yyyy-MM-dd-H-mm-ss");
    }
}
