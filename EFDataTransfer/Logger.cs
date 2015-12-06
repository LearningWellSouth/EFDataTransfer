using System;
using System.Collections.Generic;
using System.IO;

namespace EFDataTransfer
{
  public class Logger
  {
    private readonly List<string> _messages = new List<string>();
    private string _currentNote = "";

    public void PostError(string message)
    {
      LogAndPrintMessage(MakeMessagePrefix("!ERROR")+message);
    }

    private void LogAndPrintMessage(string message)
    {
      StoreCurrentValueOfRotatingNoteAndReset();
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
      StoreCurrentValueOfRotatingNoteAndReset();
      LogAndPrintMessage(MakeMessagePrefix("-INFO") + s);
    }

    private void StoreCurrentValueOfRotatingNoteAndReset()
    {
      if (_currentNote != "") _messages.Add(MakeMessagePrefix("note:" + _currentNote));
      _currentNote = "";
    }

    public void PostRotatingNote(string s)
    {
      _currentNote = s;
    }

    private static string MakeMessagePrefix(string type)
    {
      return string.Format("{0}: {1} - ", type, DateTime.Now);
    }
  }
}
