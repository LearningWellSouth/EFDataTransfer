using System;
using System.Collections.Generic;
using System.IO;

namespace EFDataTransfer
{
  public class Logger
  {
    private readonly List<string> _messages = new List<string>();

    public void PostError(string message)
    {
      LogAndPrintMessage(MakeMessagePrefix("!ERROR")+message);
    }

    private void LogAndPrintMessage(string message)
    {
      _messages.Add(message);
      Console.WriteLine(message);
    }

    public void WriteToFile(string filePath)
    {
      File.WriteAllLines(filePath, _messages);
    }

    public List<string> GetLog()
    {
      return _messages;
    }

    public void PostInfo(string s)
    {
      LogAndPrintMessage(MakeMessagePrefix("-INFO") + s);
    }

      private static string MakeMessagePrefix(string type)
    {
      return string.Format("{0}: {1} - ", type, DateTime.Now);
    }
  }
}
