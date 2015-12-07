using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EFDataTransfer.Test
{
  [TestClass]
  public class TestSQLScriptHandler
  {
    private Transfer _transfer;
    private Logger _logger;

    [TestInitialize]
    public void SetUp()
    {
      _logger = new Logger();
      _transfer = new Transfer(_logger);      
    }

    [TestMethod]
    public void ReadingLine()
    {
      AssertReadStringFromGivenLine("", "");
      AssertReadStringFromGivenLine("abc", "abc");
      AssertReadStringFromGivenLine("","--abc");
      AssertReadStringFromGivenLine("abc","abc--def");
      AssertReadStringFromGivenLine("eriks_migration","@SOURCE_DATABASE");
    }

    private void AssertReadStringFromGivenLine(string expected, string lead)
    {
      Assert.AreEqual(expected, _transfer.ReadLineFromScript(GenerateStreamFromString(lead)));
    }

    public StreamReader GenerateStreamFromString(string s)
    {
      var stream = new MemoryStream();
      var writer = new StreamWriter(stream,Encoding.UTF8);
      writer.Write(s);
      writer.Flush();
      stream.Position = 0;
      return new StreamReader(stream);
    }
  }
}
