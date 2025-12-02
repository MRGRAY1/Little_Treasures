using System;
using System.IO;
using UnityEngine;

public static class Logger
{
    public static void Log(string message, UnityEngine.Object context = null)
    {
        Debug.Log(message, context);
        string logMessage = "(LOG) " + message;
        WriteToFile(logMessage);
    }

    public static void LogWarning(string message, UnityEngine.Object context = null)
    {
        Debug.LogWarning(message, context);
        string logMessage = "(WARNING) " + message;
        WriteToFile(logMessage);
    }

    public static void LogError(string message, UnityEngine.Object context = null)
    {
        Debug.LogError(message, context);
        string logMessage = "(ERROR) " + message;
        WriteToFile(logMessage);
    }


    private static StreamWriter LogWriter;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        string date = DateTime.Now.ToString("MMM_dd_yy");
        string baseFileName = date + "_Log";
        string extension = ".txt";

        string logsFolder = Path.Combine(Application.persistentDataPath, "logs");
        if (!Directory.Exists(logsFolder))
            Directory.CreateDirectory(logsFolder);

        string path;
        int sessionIndex = 0;

        do
        {
            string fileName = sessionIndex == 0
                ? baseFileName + extension
                : $"{baseFileName}_{sessionIndex}{extension}";

            path = Path.Combine(logsFolder, fileName);
            sessionIndex++;
        }
        while (File.Exists(path));

        LogWriter = new StreamWriter(path, append: true);
        LogWriter.AutoFlush = true;

        Application.quitting += OnQuit;

        Debug.Log($"Logging to: {path}");
    }

    public static void WriteToFile(string value)
    {
        if (LogWriter != null)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            LogWriter.WriteLine($"{timestamp}: {value}");
        }
    }

    private static void OnQuit()
    {
        if (LogWriter != null)
        {
            LogWriter.Close();
            LogWriter = null;
        }
    }
}