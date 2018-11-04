using System;
using System.ComponentModel.DataAnnotations;
using EBFP.BL.Helper;
using Newtonsoft.Json;

namespace EBFP.BL.Inventory
{
    public class FireRecordsModel
    {
        public int Fire_Id { get; set; }
        public int Fire_Municipality_Id { get; set; }
        public int count { get; set; }
        [Required]
        public string Fire_Year { get; set; }

        public Nullable<int> Fire_Incidents { get; set; }
        private string _sFire_Incidents = "";
        [Required]
        public string sFire_Incidents
        {
            get
            {
                return string.IsNullOrWhiteSpace(_sFire_Incidents) ?
                    Fire_Incidents.ToString() :
                    _sFire_Incidents;
            }
            set
            {
                this.Fire_Incidents = Functions.ConvertToSafeInteger(value);
            }
        }
        
       
        public Nullable<int> Fire_Injuries { get; set; }
        private string _sFire_Injuries = "";
        [Required]
        public string sFire_Injuries
        {
            get
            {
                return string.IsNullOrWhiteSpace(_sFire_Injuries) ?
                    Fire_Injuries.ToString() :
                    _sFire_Injuries;
            }
            set
            {
                this.Fire_Injuries = Functions.ConvertToSafeInteger(value);
            }
        }

        public Nullable<int> Fire_Deaths { get; set; }
        private string _sFire_Deaths = "";
        [Required]
        public string sFire_Deaths
        {
            get
            {
                return string.IsNullOrWhiteSpace(_sFire_Deaths) ?
                    Fire_Deaths.ToString() :
                    _sFire_Deaths;
            }
            set
            {
                this.Fire_Deaths = Functions.ConvertToSafeInteger(value);
            }
        }

        public Nullable<decimal> Fire_Damages { get; set; }
        private string _sFire_Damages = "";
        [Required]
        public string sFire_Damages
        {
            get
            {
                return string.IsNullOrWhiteSpace(_sFire_Damages) ?
                    Fire_Damages.ToString() :
                    _sFire_Damages;
            }
            set
            {
                this.Fire_Damages = Functions.ConvertToSafeDecimal(value);
            }
        }
        public bool CreatedFromAjax { get; set; }
        public int? RegionId { get; set; }
        public int? ProvinceId { get; set; }
        public int? UnitId { get; set; }
    }

    public class FireIncidentStatistics
    {
        public string Year { get; set; }
        public int FireIncidents { get; set; }
        public decimal Damages { get; set; }

        public decimal DamageInBillions => Math.Round((Damages/1000000000), 3);

        public int Deaths { get; set; }
        public int Injuries { get; set; }

        public decimal Count { get; set; }

        public int? RegionId { get; set; }
        public int? ProvinceId { get; set; }
        public int? UnitId { get; set; }
    }

    public class FireIncidentFiveYearStat
    {

        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public decimal Year1 { get; set; }
        public decimal Year2 { get; set; }
        public decimal Year3 { get; set; }
        public decimal Year4 { get; set; }
        public decimal Year5 { get; set; }

        public decimal Average { get; set; }
    }
    
}
