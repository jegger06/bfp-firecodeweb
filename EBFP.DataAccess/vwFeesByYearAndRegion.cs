//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EBFP.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class vwFeesByYearAndRegion
    {
        public Nullable<int> Collected_Date { get; set; }
        public string UnitName { get; set; }
        public int UnitId { get; set; }
        public string Province { get; set; }
        public int ProvinceId { get; set; }
        public string Region { get; set; }
        public int RegionId { get; set; }
        public Nullable<decimal> TotalFee { get; set; }
    }
}