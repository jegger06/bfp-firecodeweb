

using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Web;

namespace EBFP.BL.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Functions
    { 
        public static int GetAge(DateTime? date)
        {
            if (!(date.HasValue)) return 0;
            int age = (int)Math.Floor((DateTime.Now - date.Value).TotalDays / 365.25D);
            return age;
        }
        public static string NewID()
        {
            var uid = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
            return uid;
        }

        public static string NewGuid()
        {
            return Guid.NewGuid().ToString();
        }

        public static Nullable<DateTime> GetRetirementDate(DateTime? birthDate, int? rank)
        {
            if (!(birthDate.HasValue)) return null;
            if (!(rank.HasValue)) return null;

            DateTime retireDate = new DateTime();
            if (rank == 1) //NUP
                retireDate = birthDate.Value.AddYears(65);
            else
                retireDate = birthDate.Value.AddYears(56);

            return retireDate;
        }


        public static Nullable<DateTime> GetOptionalRetirementDate(DateTime? enteredGovernmentService, int years)
        {
            if (!(enteredGovernmentService.HasValue)) return null;
            
            DateTime retireDate = new DateTime();
            retireDate = enteredGovernmentService.Value.AddYears(years);

            return retireDate;
        }


        public static void ResetStaticVariables()
        { 
            Selections.SelectionReset();
        }

        public static T GetEnumValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException();
            FieldInfo[] fields = type.GetFields();
            var field = fields
                            .SelectMany(f => f.GetCustomAttributes(
                                typeof(DescriptionAttribute), false), (
                                    f, a) => new { Field = f, Att = a }).SingleOrDefault(a => ((DescriptionAttribute)a.Att)
                                .Description == description);
            return field == null ? default(T) : (T)field.Field.GetRawConstantValue();
        }

        public static byte[] ConvertToByte(HttpPostedFileBase file)
        {
            byte[] imageByte = null;
            var rdr = new BinaryReader(file.InputStream);
            imageByte = rdr.ReadBytes(file.ContentLength);
            return imageByte;
        }

        public static decimal ConvertToSafeDecimal(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                decimal value;
                if (Decimal.TryParse(input, out value))
                {
                    return value;
                }
            }
            return 0;
        }

        public static int ConvertToSafeInteger(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                int value;
                if (int.TryParse(input.Replace(",", ""), out value))
                {
                    return value;
                }
            }
            return 0;
        }

        public static List<int> ExcludedRegions()
        {
            var excludedRegionsList = new List<int> { (int)Region.NHQ, (int)Region.NTFI};

            return excludedRegionsList;

        }

        public static List<int> OfficerRanks()
        {
            var officerRanksList = new List<int>
            {
                (int)Rank.DIR, (int)Rank.CSUPT, (int)Rank.SSUPT, (int)Rank.SUPT, (int)Rank.CINSP
                ,(int)Rank.SINSP,(int)Rank.INSP
            };

            return officerRanksList;

        }

        public static List<int> CompliantStatus()
        {
           return (new List<int>
            {
                (int) EstablishmentStatus.For_Issuance_of_FSEC,
                (int) EstablishmentStatus.Issued_FSEC,
                (int) EstablishmentStatus.For_Issuance_of_FSIC,
                (int) EstablishmentStatus.Issued_FSIC
            });

        }

        public static List<int> NonCompliantStatus()
        {
            return (new List<int>
            {
                 (int) EstablishmentStatus.For_Issuance_of_NTC,
                (int) EstablishmentStatus.Issued_NTC,
                (int) EstablishmentStatus.For_Issuance_of_NTCV,
                (int) EstablishmentStatus.Issued_NTCV,
                (int) EstablishmentStatus.For_Issuance_of_Abatement_Order,
                (int) EstablishmentStatus.Issued_Abatement_Order
            });

        }

        public static List<int> ClosureStatus()
        {
            return (new List<int>
            {
                (int) EstablishmentStatus.For_Issuance_of_Closure_Notice,
                (int) EstablishmentStatus.Issued_Closure_Notice,
                (int) EstablishmentStatus.Closed
            });

        }
        public static List<int> NonOfficerRanks()
        {
            var nonOfficerRanksList = new List<int>
            {
                (int)Rank.SFO4, (int)Rank.SFO3, (int)Rank.SFO2, (int)Rank.SFO1, (int)Rank.FO3
                ,(int)Rank.FO2,(int)Rank.FO1
            };

            return nonOfficerRanksList;

        }

        private static Regex ALL_Z_REGEX = new Regex("^[zZ]+$");

        public static string GetNextColumn(string currentColumn)
        {
            // AZ would become BA
            char lastPosition = currentColumn[currentColumn.Length - 1];

            if (ALL_Z_REGEX.IsMatch(currentColumn))
            {
                string result = String.Empty;
                for (int i = 0; i < currentColumn.Length; i++)
                    result += "A";
                return result + "A";
            }
            else if (lastPosition == 'Z')
                return GetNextColumn(currentColumn.Remove(currentColumn.Length - 1, 1)) + "A";
            else
                return currentColumn.Remove(currentColumn.Length - 1, 1) + (++lastPosition).ToString();
        }
    }


    public static class ExtensionMethods
    {
        public static string ToGenderFullName(this string value)
        { 
            value = FindFullname(Selections.Genders, value);
            return value;
        }

        public static string ToCivilStatusFullName(this string value)
        { 
            value = FindFullname(Selections.CivilStatus, value);
            return value;
        }

        public static string ToRankFullName(this string value)
        { 
            value = FindFullname(Selections.Ranks, value);
            return value;
        }

        public static string ToEligibilityFullName(this string value)
        {
            value = FindFullname(Selections.Eligibilities, value);
            return value;
        }

        public static string ToPresentDesginationFullName(this string value)
        {
            value = FindFullname(Selections.JobFunctions, value);
            return value;
        }

        public static string ToRegionFullName(this string unitID)
        {
            var fullname = Selections.Units.FirstOrDefault(s => s.Value == unitID);
            if (fullname != null)
                unitID = fullname.Group.Name;
            else
                unitID = "";

            return unitID;
        }
        
        public static string ToUnitFullName(this string value)
        {
            value = FindFullname(Selections.Units, value);
            return value;
        }

        public static string ToRegionId(this string value)
        {
            if (value == null) return "";
            value = FindId(Selections.Region, value);
            return value;
        }

        private static string FindFullname(List<System.Web.Mvc.SelectListItem> selectItems, string value)
        {
            try
            {
                var fullname = selectItems.FirstOrDefault(s => s.Value == value);
                if (fullname != null)
                    value = fullname.Text;
                else
                    value = "";

                return value;
            }
            catch (Exception ex)
            {
                var xx = ex.Message;
            }
            return "";
        }

        private static string FindId(List<System.Web.Mvc.SelectListItem> selectItems, string value)
        {
            try
            {
                var fullname = selectItems.FirstOrDefault(s => s.Text == value);
                if (fullname != null)
                    value = fullname.Value;
                else
                    value = "";

                return value;
            }
            catch (Exception ex)
            {
                var xx = ex.Message;
            }
            return "";
        }
    }

}
