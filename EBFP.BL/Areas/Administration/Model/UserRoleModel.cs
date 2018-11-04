using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EBFP.BL.Helper;
using EBFP.Helper;

namespace EBFP.BL.Administration
{
    public class UserRoleListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<UserRoleModel> UserRoleListModel { get; set; }
    }

    public class UserRoleSearchModel
    {
        public string Role_Name { get; set; }
        public string Role_Description { get; set; }
    }


    public class UserRoleModel
    {
        public UserRoleModel()
        {
            RoleAccessList = new List<RoleAccessModel>();
            //    this.tblUserInRole = new tblUserInRole();
        }

        public string sRole_ID => Role_ID.ToString().Encrypt();

        public int Role_ID { get; set; }

        [Required]
        public string Role_Name { get; set; }

        public string Role_Description { get; set; }
        public int Role_CreatedBy { get; set; }
        public DateTime Role_CreatedDate { get; set; }
        public int Role_ModifiedBy { get; set; }
        public DateTime Role_ModifiedDate { get; set; }
        public int NumberOfAccess { get; set; }
        public bool Role_DefaultAccess { get; set; }
        public bool Role_AllAccess { get; set; }

        public List<RoleAccessModel> RoleAccessList { get; set; }
    }
}