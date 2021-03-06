﻿using System;
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

      public string GetKeyString()
      {
          return City + ":" + StreetName;
      }

      public int GetPostalCodeAsInteger()
      {
          var value = 0;
          return (Int32.TryParse(PostalNumber, out value) ? value : 0);
      }
  }

  public class AddressParser {
      private static readonly Regex StreetAddressMatcher = new Regex(@"^([\D]+)(( ?([0-9]+)[^ ]*).*)?$");
    private Logger _logger;

    public AddressParser(Logger logger)
    {
      _logger = logger;
    }

    public Address ParseAddress(object address, object postalNumber, object city)
    {
        if (address == null)
            throw new NullReferenceException();
        var match = StreetAddressMatcher.Match(Convert.ToString(address));

        if (!match.Success)
            _logger.PostError(string.Format("Address: {0} could not be interpreted correctly", address));
        return new Address()
        {
            StreetName = _extractStreetName(match.Groups),
            StreetNumber = _extractStreetNumber(match.Groups),
            StreetNumberFull = match.Groups[2].Value.Trim(),
            PostalNumber = ExtractPostalNumber(Convert.ToString(postalNumber)),
            City = ExtractCity(Convert.ToString(city))
        };
    }

    private static string ExtractCity(string city)
    {
      return string.IsNullOrEmpty(city) ? "" : city.ToUpper(CultureInfo.InvariantCulture).Trim();
    }

    private string _extractStreetName(GroupCollection groups)
    {
      return (groups.Count <= 0 ? "" : groups[1].Value.ToUpper().TrimEnd());
    }

    private static int _extractStreetNumber(GroupCollection groups)
    {
      if (groups.Count <= 1) return 0;
      var val = groups[4].Value;
      return string.IsNullOrEmpty(val) ? 0 : (Convert.ToInt32(val));
    }

    public static int ExtractBeginingOfStringAsInteger(string input)
    {
      return string.IsNullOrEmpty(input) ? 0 : Convert.ToInt32("0"+Regex.Replace(input.TrimStart(), @"[\D]+.*$", ""));
    }

    private string ExtractPostalNumber(string postalNumber)
    {
        postalNumber = Regex.Replace(postalNumber,@"\s","");
        var numeric = ExtractBeginingOfStringAsInteger(postalNumber);
        if (numeric > 9999 && numeric <= 99999) return numeric.ToString();

        if (isValidEnglishOrFrenchPostalNumber(postalNumber))
            return postalNumber;
        
        _logger.PostError("Postalnumber \""+postalNumber+"\" has invalid format");
        return null;
    }

    private static bool isValidEnglishOrFrenchPostalNumber(string val)
    {
      return Regex.IsMatch(val, @"^\D{2}-.{4,8}$");
    }
  }
}
