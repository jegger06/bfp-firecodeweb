using System;
using System.Collections.Generic;
using EBFP.BL.Helper;

namespace EBFP.BL.Inventory
{
    public class PersonnelListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<PersonnelModel> PersonnelListModel { get; set; }
    }

    
    public class PersonnelModel
    {
        
        public string Rank_Name { get; set; }
        public string Full_Name { get; set; }
        public string Present_Designation { get; set; }
        public string Specific_Designation { get; set; }
        public string Contact_Number { get; set; }
        public string Email { get; set; }

        public int Municipality_Id { get; set; }
        public int Station_Id { get; set; }

    }
}