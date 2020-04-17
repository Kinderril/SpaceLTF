using System;
using System.IO;
using UnityEngine;

public class LogHandler : Singleton<LogHandler>
{
    private StreamWriter _writer;
    public void Init()
    {
        try
        {

            var logsDir = "Logs";
            //        var way = $"Log {DateTime.Now.Date.ToShortDateString()} {DateTime.Now.ToLongDateString()}.txt";
            var way = $"{logsDir}/Log {DateTime.Now.ToString("MM/dd/yyyy H-mm")}.log";
            //        var way = $"Log.txt";
            if (!Directory.Exists(logsDir))
            {
                Directory.CreateDirectory(logsDir);
            }
            if (!File.Exists(way))
            {
                File.Create(way).Dispose();
            }
            var file = File.Open(way, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            var writer = new StreamWriter(file);
            //        var reader = new StreamReader(file);

            //        File.Create(way);
            _writer = writer;// File.AppendText(way);
            _writer.Write("\n\n=============== Game started ================\n\n");
            DontDestroyOnLoad(gameObject);
            Application.RegisterLogCallback(HandleLog);
        }
        catch (Exception e)
        {
              Debug.LogError("Error on creating log");
            
        }
    }

    private void HandleLog(string condition, string stackTrace, LogType type)
    {
        string logEntr;
        if (type == LogType.Error)
        {
            logEntr = Namings.Format("\n {0} {1} {2}\n{3}"
                , DateTime.Now, type, condition, Environment.StackTrace);
        }
        else
        {

            logEntr = Namings.Format("\n {0} {1}  {2}"
                , DateTime.Now, type, condition);
        }

        //        MyServer.SendLog(logEntry);
        _writer.Write(logEntr);
    }

    void OnDestroy()
    {
        _writer.Close();
    }
}