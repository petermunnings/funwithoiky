using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace oikonomos.common
{
    public static class Utils
    {
        public static bool ValidEmailAddress(string emailAddress)
        {
            Match match = Regex.Match(emailAddress, @"^[\w-]+(\.[\w-]+)*@([a-z0-9-]+(\.[a-z0-9-]+)*?\.[a-z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$");
            return match.Success;
        }

        public static string ConvertCellPhoneToInternational(string cellPhoneNo, string country)
        {
            cellPhoneNo = cellPhoneNo.Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty).Replace("-", string.Empty);
            switch (country)
            {
                case "South Africa":
                    {
                        cellPhoneNo = "27" + cellPhoneNo.Substring(1, cellPhoneNo.Length - 1);
                        break;
                    }
            }

            return cellPhoneNo;
        }
    }
}
