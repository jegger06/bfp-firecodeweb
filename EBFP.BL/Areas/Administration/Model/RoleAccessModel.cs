using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBFP.DataAccess;

namespace EBFP.BL.Administration
{
    public class RoleAccessModel
    {
        public int RA_ID { get; set; }
        public int RA_Role_ID { get; set; }
        public int RA_PageSecurityID { get; set; }
    }
}
