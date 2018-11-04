using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EBFP.DataAccess;
using Queries.Core.Repositories;
using System.Data.Entity;
using EBFP.Helper;

namespace EBFP.BL.Administration
{
    public class MembershipBL : Repository<webpages_Membership, MembershipModel>, IMembership
    {
        public MembershipBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }
        public void CreateMapping()
        {
            Mapper.CreateMap<webpages_Membership, MembershipModel>().ReverseMap();
            Mapper.CreateMap<List<webpages_Membership>, List<MembershipModel>>().ReverseMap();
            Mapper.CreateMap<List<webpages_Membership>, List<MembershipModel>>();
        }
        public List<MembershipModel> GetMembershipByUnitId(int unitId)
        {
            var mebershipDet = new List<MembershipModel>();

            var employees =  BFPContext.tblEmployees
                                        .Where(a => a.Emp_Curr_Unit == unitId && a.Emp_Username != "portaladmin")
                                        .Select(a => a.Emp_Id).ToList();

            if (employees == null) return mebershipDet;

            foreach(var emp_Id in employees)
            {
                var user = BFPContext.webpages_Membership.FirstOrDefault(a => a.UserId == emp_Id);
                if (user != null)
                {
                    var membership = new MembershipModel();
                    user.PasswordDecrypted = user.PasswordDecrypted.Encrypt();
                    Mapper.Map(user, membership); 
                    mebershipDet.Add(membership);
                }

            }

            return mebershipDet;
        }


        public void SyncMembership(List<MembershipModel> model)
        {
            //get item to be synced
            var membershipList = model.Select(a => a.UserId).ToList();

            //get item to be updated
            var itemToUpdate = BFPContext.webpages_Membership.Where(a => membershipList.Contains(a.UserId)).ToList();

            //update
            foreach (var item in itemToUpdate)
            {
                var itemFromLocal = model.FirstOrDefault(a => a.UserId == item.UserId); 
                item.PasswordChangedDate = itemFromLocal.PasswordChangedDate;
                item.PasswordDecrypted = itemFromLocal.PasswordDecrypted;
                
            }
            BFPContext.SaveChanges();

        }
    }
}