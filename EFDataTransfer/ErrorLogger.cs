using System.Collections.Generic;
using System.IO;

namespace EFDataTransfer
{
  public class ErrorLogger
  {
    private List<string> errors = new List<string>();

    public void Add(string message)
    {
      errors.Add(message);
    }

    public void WriteToFile(string filePath)
    {
      File.WriteAllLines(filePath, errors);
    }

    public List<string> GetErrorLog()
    {
      return errors;
    }
  }
}
