using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EBFP.BL.Helper;
using EBFP.Helper;

namespace EBFP.BL.HumanResources
{
    public class DirectoratesListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<DirectoratesModel> DirectoratesListModel { get; set; }
    }

    public class DirectoratesSearchModel
    {
        public string Dir_Code { get; set; }
        public string Dir_Name { get; set; }
        public bool IsSearch { get; set; }
    }

    public class DirectoratesModel
    {
        public string sDir_Id
        {
            get { return Dir_Id.ToNullSafeString().Encrypt(); }

        }

        public int Dir_Id { get; set; }
        [Required]
        public string Dir_Code { get; set; }
        [Required]
        public string Dir_Name { get; set; }
        public int Dir_Created_Emp_Id { get; set; }
        public DateTime Dir_CreatedDate { get; set; }
        public int? Dir_LastUpdate_Emp_Id { get; set; }
        public DateTime? Dir_LastUpdateDate { get; set; }
        public string Dir_CreatedBy { get; set; }
        public string Dir_LastUpdateBy { get; set; }
        
    }

}