using System;
using System.Linq;
using System.Web.Mvc;
using EBFP.BL.Administration;
using EBFP.BL.Helper;
using EBFP.Helper;

namespace EBFP.Web.Areas.Administration.Controllers
{
    [Authorize]
    public class OPSController : Controller
    {
        IAdministrationUnitOfWork unitOfWork { get; }
        public OPSController()
        {
            unitOfWork = new AdministrationUnitOfWork();
        }

        public ActionResult SpoiledOPS()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetSpoiledOPS(GridInfo gridInfo)
        {
            try
            {
                var ret = unitOfWork.SpoiledOPS.GetSpoiledOPSList(gridInfo, CurrentUser.EmployeeUnitId);
                var jsonResult = Json(new
                {
                    ret.DatatableInfo.recordsTotal,
                    recordsFiltered = ret.DatatableInfo.recordsTotal,
                    data = ret.SpoiledOPSList
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
        public ActionResult SaveOPSSeries(SpoiledOPSModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.SOPS_Number))
                    throw new Exception("Please fill spoiled OPS number!");
            
                var ifExist = unitOfWork.SpoiledOPS.Find(a =>
                        a.SOPS_Number == model.SOPS_Number && a.SOPS_Unit_Id == CurrentUser.EmployeeUnitId)
                    .FirstOrDefault();
                if (ifExist != null)
                    throw new Exception("Spoiled OPS number already exist!");
                var isReleasedOPS =
                    unitOfWork.OPSSeries.ReleasedOPS(model.SOPS_Number, CurrentUser.EmployeeUnitId);
                if (isReleasedOPS)
                    throw new Exception("Spoiled OPS number already released!");

               
                model.SOPS_CreatedDate = DateTime.Now;
                model.SOPS_Created_Emp_Id = CurrentUser.EmployeeId;
                model.Ref_SOPS_Id = Functions.NewID();
                model.SOPS_Unit_Id = CurrentUser.EmployeeUnitId;

                unitOfWork.SpoiledOPS.Add(model);
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
       
    }
}