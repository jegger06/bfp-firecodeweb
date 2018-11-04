using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EBFP.BL.Administration;
using EBFP.BL.Helper;
using EBFP.Helper;
using EBFP.BL.HumanResources;

namespace EBFP.Web.Areas.Administration.Controllers
{
    [Authorize]
    public class DepositsController : Controller
    {
        IAdministrationUnitOfWork unitOfWork { get;}

        public DepositsController()
        {
            unitOfWork = new AdministrationUnitOfWork();
        }

        public ActionResult Deposits()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetDeposit(GridInfo gridInfo)
        {
            try
            {
                var ret = unitOfWork.Deposits.GetDepositList(gridInfo, CurrentUser.EmployeeUnitId);
                var jsonResult = Json(new
                {
                    ret.DatatableInfo.recordsTotal,
                    recordsFiltered = ret.DatatableInfo.recordsTotal,
                    data = ret.DepositList
                }, JsonRequestBehavior.AllowGet);

                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
                return null;
            }
        }


        [HttpPost]
        public ActionResult SaveDeposit(DepositsModel model)
        {
            try
            {
                if (model.Dep_Depositor_Emp_Id <= 0 || string.IsNullOrEmpty(model.Dep_LC_No) ||
                  string.IsNullOrEmpty(model.Dep_Bank))
                    throw new Exception("Please fill all fields");
                
                model.Dep_CreatedDate = DateTime.Now;
                model.Dep_Created_Emp_Id = CurrentUser.EmployeeId;
                model.Dep_Unit_Id = CurrentUser.EmployeeUnitId;            
                model.Ref_Dep_Id = Functions.NewID();

                unitOfWork.Deposits.Add(model);
                unitOfWork.Complete();
            }

            catch (Exception ex)
            {
                var jsonResultError = Json(new
                {
                    message = ex.InnerException?.Message ?? ex.Message
                }, JsonRequestBehavior.AllowGet);
                return jsonResultError;
            }

            var jsonResult = Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpGet]
        public JsonResult SelectionAutoComplete(string search)
        {
            try
            {
                var ounitOfWork = new HRISUnitOfWork();

                var ranks = ounitOfWork.Rank.GetList();
                //dynamic ret;
                  var  ret = ounitOfWork.Employee
                       .GetList(
                           a =>
                               a.Emp_FirstName.Contains(search) || a.Emp_LastName.Contains(search) ||
                                  a.Emp_Number.Contains(search) && a.Emp_IsDeleted != true && a.Emp_Curr_Unit == CurrentUser.EmployeeUnitId).Select(a => new
                                  //a.Emp_Number.Contains(search) && a.Emp_IsDeleted != true).Select(a => new
                                  {
                                   text = (a.Emp_Curr_Rank != null && a.Emp_Curr_Rank > 0 ? ranks.FirstOrDefault(b => b.Rank_Id == a.Emp_Curr_Rank).Rank_Name : "") + " " + a.Emp_FirstName + " " + a.Emp_MiddleName + " " + a.Emp_LastName,
                                   id = a.Emp_Id
                               }).OrderBy(a => a.text);
                
                var jsonResult = Json(new
                {
                    data = ret
                }, JsonRequestBehavior.AllowGet);

                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
                return null;
            }
        }
    }
}