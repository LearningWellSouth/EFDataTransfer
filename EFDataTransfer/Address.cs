using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace EFDataTransfer
{
  public struct Address
  {
    public string StreetName;
    public int StreetNumber;
    public string StreetNumberFull;
    public string PostalNumber;
    public string City;

    public bool isEvenStreetNumber()
    {
      return (StreetNumber % 2) == 0;
    }
  }

  public class AddressParser {
    private static readonly Regex StreetAddressMatcher = new Regex(@"^([\D]+)( ([0-9]+)[ \D]*)?$");
    private Logger _logger;

    public AddressParser(Logger logger)
    {
      _logger = logger;
    }

    public Address ParseAddress(object address, object postalNumber, object city)
    {
      if (address == null)
        throw new NullReferenceException();
      var match = StreetAddressMatcher.Match(Convert.ToString(address)).Groups;

      return new Address()
      {
        StreetName = extractStreetName(match),
        StreetNumber = extractStreetNumber(match),
        StreetNumberFull = match[2].Value.Trim(),
        PostalNumber = ExtractPostalNumber(Convert.ToString(postalNumber)),
        City = ExtractCity(Convert.ToString(city))
      };
    }

    private static string ExtractCity(string city)
    {
      if (string.IsNullOrEmpty(city)) return "";
      return city.ToUpper(CultureInfo.InvariantCulture).Trim();
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

    private string ExtractPostalNumber(string postalNumber)
    {
      postalNumber = postalNumber.Trim();
      var numeric = ExtractBeginingOfStringAsInteger(postalNumber);
      if (numeric > 9999 && numeric < 99999) return numeric.ToString();

      if (!isValidEnglishOrFrenchPostalNumber(postalNumber))
      {
        _logger.PostError("Postalnumber "+postalNumber+" is neither swedish nor international format");
        return null;
      }

      return postalNumber;
    }

    private static bool isValidEnglishOrFrenchPostalNumber(string val)
    {
      return Regex.IsMatch(val, @"^\D{2}-.{4,8}$");
    }
  }
}
