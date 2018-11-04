using System.Collections.Generic;
using System.Linq;
using EBFP.DataAccess;
using Queries.Core.Repositories;
using AutoMapper;

namespace EBFP.BL.HumanResources
{
    public class UnitUserInRoleBL : Repository<tblUnitsUserInRole, UnitUserInRoleModel>, IUnitUserInRole
    {
        public UnitUserInRoleBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }
        public void CreateMapping()
        {
            Mapper.CreateMap<tblUnitsUserInRole, UnitUserInRoleModel>().ReverseMap();
            Mapper.CreateMap<List<tblUnitsUserInRole>, List<UnitUserInRoleModel>>().ReverseMap();
            Mapper.CreateMap<List<tblUnitsUserInRole>, List<UnitUserInRoleModel>>();
        }

        public void SyncUnitsUserInRoleLocalToServer(List<UnitUserInRoleModel> model)
        {
            foreach (var unitUser in model)
            {
                var UnitUserInRole = BFPContext.tblUnitsUserInRole
                                            .FirstOrDefault(a => a.Unit_UIR_Unit_Id == unitUser.Unit_UIR_Unit_Id && a.Unit_UIR_Emp_Id == unitUser.Unit_UIR_Emp_Id);

                if (UnitUserInRole != null)
                {
                    var Unit_UIR_ID = UnitUserInRole.Unit_UIR_ID;
                    Mapper.Map(unitUser, UnitUserInRole);
                    UnitUserInRole.Unit_UIR_ID = Unit_UIR_ID;
                }
                else
                {
                    UnitUserInRole = new tblUnitsUserInRole();
                    Mapper.Map(unitUser, UnitUserInRole);
                    UnitUserInRole.Unit_UIR_ID = 0;

                    BFPContext.tblUnitsUserInRole.Add(UnitUserInRole);
                }
            }

            BFPContext.SaveChanges();
        }
    }
}