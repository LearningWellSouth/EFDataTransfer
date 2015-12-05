using System.Data;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EFDataTransfer.Test
{
  [TestClass]
  public class TestAddressAndPostalCodeHandling
  {
    private AddressParser _parser = null;
    private ErrorLogger _logger;

    private delegate void Expression();

    [TestInitialize]
    public void SetUp()
    {
      _logger = new ErrorLogger();
      _parser = new AddressParser(_logger);
    }

    [TestMethod]
    public void ExtractingAddressAndStreetNumber()
    {
      AssertException<NullReferenceException>(() => { extractAddress(null); });
      AssertAddressPartsAre("", "", 0);
      AssertAddressPartsAre("address", "ADDRESS", 0);
      AssertAddressPartsAre("address 14", "ADDRESS", 14);
      AssertAddressPartsAre("lång rö gatan 20", "LÅNG RÖ GATAN", 20);
      AssertAddressPartsAre("allé 20 B", "ALLÉ", 20, "20 B");
      AssertAddressPartsAre("address 20A", "ADDRESS", 20, "20A");
    }

    [TestMethod]
    public void ExtractingFirstInteger()
    {
      Assert.AreEqual(0, AddressParser.ExtractBeginingOfStringAsInteger(null));
      Assert.AreEqual(0, AddressParser.ExtractBeginingOfStringAsInteger(""));
      Assert.AreEqual(1, AddressParser.ExtractBeginingOfStringAsInteger("1"));
      Assert.AreEqual(123, AddressParser.ExtractBeginingOfStringAsInteger("123"));
      Assert.AreEqual(123, AddressParser.ExtractBeginingOfStringAsInteger("123-1"));
    }

    [TestMethod]
    public void ExtractingPostalNumber()
    {
      Assert.AreEqual(null, extractPostalNumber(null));
      Assert.AreEqual(null, extractPostalNumber(""));
      Assert.AreEqual(null, extractPostalNumber("Vetlanda"));
      Assert.AreEqual(null, extractPostalNumber("123"));
      Assert.AreEqual("12345", extractPostalNumber("  12345  "));
      Assert.AreEqual("12345", extractPostalNumber("12345"));
      Assert.AreEqual("24233", extractPostalNumber("24233Höör"));
      Assert.AreEqual("CH-1073", extractPostalNumber("CH-1073"));
      Assert.AreEqual("BE-1380", extractPostalNumber("BE-1380"));
      Assert.AreEqual("GB-KT112EX", extractPostalNumber("GB-KT112EX"));

      Assert.AreEqual(4, _logger.GetErrorLog().Count);
    }

    [TestMethod]
    public void ExtractingCity()
    {
      Assert.AreEqual("", extractCity(null));
      Assert.AreEqual("", extractCity(""));
      Assert.AreEqual("CITY", extractCity("city"));
      Assert.AreEqual("CITY", extractCity(" \t\ncity\t\n "));
    }

    private string extractCity(string city)
    {
      return _parser.ParseAddress("", null, city).City;
    }

    private string extractPostalNumber(string postalNumber)
    {
      return _parser.ParseAddress("",postalNumber,null).PostalNumber;
    }

    [TestMethod]
    public void TestCheckingOfStreetNumberSpan() {
      var row = createDataRowForPostalCodeModel();

      Assert.IsTrue(Transfer.isStreetNumberWithinMaxAndMin(row, 0));
      Assert.IsTrue(Transfer.isStreetNumberWithinMaxAndMin(row, 2));
      Assert.IsTrue(Transfer.isStreetNumberWithinMaxAndMin(row, 3));
      Assert.IsFalse(Transfer.isStreetNumberWithinMaxAndMin(row, 4));
    }

    [TestMethod]
    public void TestHasCorrectPostalNumberType()
    {
      var row = createDataRowForPostalCodeModel();
      var oddAddress = makeAddressWithOddStreetNumber();
      var evenAddress = makeAddressWithEvenStreetNumber();

      row[Transfer.POSTALCODEMODEL_CODETYPE] = "NJ";
      Assert.IsTrue(Transfer.hasCorrectPostalNumberType(row, evenAddress));
      Assert.IsFalse(Transfer.hasCorrectPostalNumberType(row, oddAddress));

      row[Transfer.POSTALCODEMODEL_CODETYPE] = "NU";
      Assert.IsTrue(Transfer.hasCorrectPostalNumberType(row, oddAddress));
      Assert.IsFalse(Transfer.hasCorrectPostalNumberType(row, evenAddress));

      row[Transfer.POSTALCODEMODEL_CODETYPE] = "foo";
      Assert.IsTrue(Transfer.hasCorrectPostalNumberType(row, oddAddress));
      Assert.IsTrue(Transfer.hasCorrectPostalNumberType(row, evenAddress));
    }

    private Address makeAddressWithEvenStreetNumber()
    {
      return _parser.ParseAddress("House 2", "", "");
    }

    private Address makeAddressWithOddStreetNumber()
    {
      return _parser.ParseAddress("House 3","","");
    }

    private static DataRow createDataRowForPostalCodeModel()
    {
      var table = createPostalCodeModel_TableModel();
      var row = table.NewRow();
      row["StreetNoLowest"] = "1";
      row["StreetNoHighest"] = "3";
      row[Transfer.POSTALCODEMODEL_CODETYPE] = "NU";
      return row;
    }

    private static DataTable createPostalCodeModel_TableModel()
    {
      var table = new DataTable();
      table.Columns.Add(new DataColumn("StreetNoLowest", typeof (string)));
      table.Columns.Add(new DataColumn("StreetNoHighest", typeof (string)));
      table.Columns.Add(new DataColumn(Transfer.POSTALCODEMODEL_CODETYPE, typeof (string)));
      return table;
    }

    private Address extractAddress(string addr)
    {
      return _parser.ParseAddress(addr,null,null);
    }

    private void AssertAddressPartsAre(string address, string expectName, int expectNumber)
    {
      Assert.AreEqual(expectName, extractAddress(address).StreetName, "For address: "+address);
      Assert.AreEqual(expectNumber, extractAddress(address).StreetNumber, "For address: " + address);
    }

    private void AssertAddressPartsAre(string address, string expectName, int expectNumber, string expectFullNumber)
    {
      AssertAddressPartsAre(address,expectName,expectNumber);
      Assert.AreEqual(expectFullNumber, extractAddress(address).StreetNumberFull, "For address: " + address);
    }

    private static void AssertException<T>(Expression work) where T : Exception
    {
      try
      {
        work();
        Assert.Fail("no exception thrown");
      }
      catch (T)
      {
        Assert.IsTrue(true);
      }
      catch (Exception exc)
      {
        Assert.Fail("wrong type of exception thown. Expected: "+typeof (T)+" was "+exc);
      }
    }
  }
}
