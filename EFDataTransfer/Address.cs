using System;
using System.Text.RegularExpressions;

namespace EFDataTransfer
{
  public struct Address
  {
    private static readonly Regex Matcher = new Regex(@"^([\D]+)( ([0-9]+)[ \D]*)?$");
    public string StreetName;
    public int StreetNumber;
    public string StreetNumberFull;
    public string PostalNumber;

    public static Address makeEmpty()
    {
      return new Address() { StreetName = "", StreetNumberFull = "", StreetNumber = 0 };
    }

    public static Address extractAddressParts(object address, object postalNumber)
    {
      if (address == null)
        throw new NullReferenceException();
      var match = Matcher.Match(Convert.ToString(address)).Groups;

      return new Address()
      {
        StreetName = extractStreetName(match),
        StreetNumber = extractStreetNumber(match),
        StreetNumberFull = match[2].Value.Trim(),
        PostalNumber = ExtractPostalNumber(Convert.ToString(postalNumber))
      };
    }
    private static string extractStreetName(GroupCollection groups)
    {
      return (groups.Count <= 0 ? "" : groups[1].Value.ToUpper());
    }

    private static int extractStreetNumber(GroupCollection groups)
    {
      if (groups.Count <= 1) return 0;
      var val = groups[3].Value;
      return string.IsNullOrEmpty(val) ? 0 : (Convert.ToInt32(val));
    }

    public static int ExtractBeginingOfStringAsInteger(string input)
    {
      return string.IsNullOrEmpty(input) ? 0 : Convert.ToInt32(Regex.Replace("0" + input, @"[\D]+.*$", ""));
    }

    public bool isEvenStreetNumber()
    {
      return (StreetNumber%2) == 0;
    }

    public static string ExtractPostalNumber(string postalNumber)
    {
      if(string.IsNullOrEmpty(postalNumber)) return "";
      var number = ExtractBeginingOfStringAsInteger(postalNumber);
      if (number > 0) return number.ToString().Substring(0,5);
      return postalNumber;
    }
  }
}
