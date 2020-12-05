using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Day04
{
    public class Passport
    {
        public string BirthYear { get; set; }
        public string IssueYear { get; set; }
        public string ExpirationYear { get; set; }
        public string Height { get; set; }
        public string HairColor { get; set; }
        public string EyeColor { get; set; }
        public string PassportId { get; set; }
        public string CountryId { get; set; }
        public bool IsValid { get => ValidatePassport(); }

        private static Dictionary<string, string> FieldMappings =>
            new Dictionary<string, string>
            {
                { "byr", nameof(BirthYear) },
                { "iyr", nameof(IssueYear) },
                { "eyr", nameof(ExpirationYear) },
                { "hgt", nameof(Height) },
                { "hcl", nameof(HairColor) },
                { "ecl", nameof(EyeColor) },
                { "pid", nameof(PassportId) },
                { "cid", nameof(CountryId) }
            };

        public static Passport ParseLinesIntoPassport(IEnumerable<string> passportLines)
        {
            var passport = new Passport();
            foreach (string line in passportLines)
            {
                string[] entries = line.Split(' ');
                foreach (string entry in entries)
                {
                    string[] pair = entry.Split(':');
                    var property = FieldMappings.FirstOrDefault(keyvalue => keyvalue.Key.Equals(pair[0])).Value;
                    passport.GetType().GetProperty(property).SetValue(passport, pair[1]);
                }
            }
            return passport;
        }

        private bool ValidatePassport()
        {
            if (ValidateBirthYear() is false ||
                ValidateIssueYear() is false ||
                ValidateExpirationYear() is false ||
                ValidateHeight() is false ||
                ValidateEyeColor() is false ||
                ValidateHairColor() is false ||
                ValidatePassportId() is false)
                return false;

            return true;
        }

        private bool ValidateBirthYear()
        {
            if (string.IsNullOrEmpty(BirthYear))
                return false;

            if (int.TryParse(BirthYear, out int birthYear) is false)
                return false;

            if (birthYear < 1920 || birthYear > 2002)
                return false;

            return true;
        }

        private bool ValidateIssueYear()
        {
            if (string.IsNullOrEmpty(IssueYear))
                return false;

            if (int.TryParse(IssueYear, out int issueYear) is false)
                return false;

            if (issueYear < 2010 || issueYear > 2020)
                return false;

            return true;
        }

        private bool ValidateExpirationYear()
        {
            if (string.IsNullOrEmpty(ExpirationYear))
                return false;

            if (int.TryParse(ExpirationYear, out int expirationYear) is false)
                return false;

            if (expirationYear < 2020 || expirationYear > 2030)
                return false;

            return true;
        }

        private bool ValidateHeight()
        {
            if (string.IsNullOrEmpty(Height))
                return false;

            string unit = Height.Substring(Height.Length - 2);
            string measurementString = Height.Substring(0, Height.Length - 2);

            if (int.TryParse(measurementString, out int measurement) is false)
                return false;

            switch (unit)
            {
                case "cm":
                    if (measurement < 150 || measurement > 193)
                        return false;
                    break;
                case "in":
                    if (measurement < 59 || measurement > 76)
                        return false;
                    break;
                default:
                    return false;
            }

            return true;
        }

        private bool ValidateHairColor()
        {
            if (string.IsNullOrEmpty(HairColor))
                return false;

            if (HairColor[0] != '#')
                return false;

            if (HairColor.Length != 7)
                return false;

            var colorCode = HairColor.Substring(HairColor.Length - 6);
            return colorCode.All(c => Char.IsLetterOrDigit(c));
        }

        private bool ValidateEyeColor()
        {
            if (string.IsNullOrEmpty(EyeColor))
                return false;

            string[] validColors = new string[]
            {
                "amb",
                "blu",
                "brn",
                "gry",
                "grn",
                "hzl",
                "oth"
            };

            return validColors.Contains(EyeColor);
        }

        private bool ValidatePassportId()
        {
            if (string.IsNullOrEmpty(PassportId))
                return false;

            if (PassportId.Length != 9)
                return false; 
            return PassportId.All(c => Char.IsDigit(c));
        }
    }
}
