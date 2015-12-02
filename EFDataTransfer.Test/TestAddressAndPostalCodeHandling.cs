using System.Data;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EFDataTransfer.Test
{
  [TestClass]
  public class TestAddressAndPostalCodeHandling
  {
    private delegate void Expression();
    
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
      Assert.AreEqual(0, Address.ExtractBeginingOfStringAsInteger(null));
      Assert.AreEqual(0, Address.ExtractBeginingOfStringAsInteger(""));
      Assert.AreEqual(1, Address.ExtractBeginingOfStringAsInteger("1"));
      Assert.AreEqual(123, Address.ExtractBeginingOfStringAsInteger("123"));
      Assert.AreEqual(123, Address.ExtractBeginingOfStringAsInteger("123-1"));
    }

    [TestMethod]
    public void ExtractingPostalNumber()
    {
      Assert.AreEqual("", extractPostalNumber(null));
      Assert.AreEqual("", extractPostalNumber(""));
      Assert.AreEqual("12345", extractPostalNumber("12345"));
      Assert.AreEqual("24233", extractPostalNumber("24233Höör"));
      Assert.AreEqual("CH-1073", extractPostalNumber("CH-1073"));
      Assert.AreEqual("BE-1380", extractPostalNumber("BE-1380"));
      Assert.AreEqual("12345", extractPostalNumber("1234567890"));
      Assert.AreEqual("GB-KT112EX", extractPostalNumber("GB-KT112EX"));
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
      return Address.extractAddressParts("", null, city).City;
    }

    private static string extractPostalNumber(string postalNumber)
    {
      return Address.extractAddressParts("",postalNumber,null).PostalNumber;
    }

    [TestMethod]
    public void TestCheckingOfStreetNumberSpan() {
      var table = new DataTable();
      table.Columns.Add(new DataColumn("StreetNoLowest", typeof (string)));
      table.Columns.Add(new DataColumn("StreetNoHighest", typeof (string)));
      var row = table.NewRow();
      row["StreetNoLowest"] = "1";
      row["StreetNoHighest"] = "2";

      Assert.IsTrue(Transfer.isStreetNumberWithinMaxAndMin(row, 0));
      Assert.IsTrue(Transfer.isStreetNumberWithinMaxAndMin(row, 1));
      Assert.IsFalse(Transfer.isStreetNumberWithinMaxAndMin(row, 3));
    }

    private Address extractAddress(string addr)
    {
      return Address.extractAddressParts(addr,null,null);
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
