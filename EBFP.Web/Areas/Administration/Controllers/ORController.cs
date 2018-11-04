using System;
using System.Linq;
using System.Web.Mvc;
using EBFP.BL.Administration;
using EBFP.BL.Helper;
using EBFP.Helper;
using System.Net;

namespace EBFP.Web.Areas.Administration.Controllers
{
    [Authorize]
    public class ORController : Controller
    {
        IAdministrationUnitOfWork unitOfWork { get; }
        public ORController()
        {
            unitOfWork = new AdministrationUnitOfWork();
        }

        public ActionResult OR()
        {
            return View();
        }

        public ActionResult SpoiledOR()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetSpoiledOR(GridInfo gridInfo)
        {
            try
            {
                var ret = unitOfWork.SpoiledOR.GetSpoiledORList(gridInfo, CurrentUser.EmployeeUnitId);
                var jsonResult = Json(new
                {
                    ret.DatatableInfo.recordsTotal,
                    recordsFiltered = ret.DatatableInfo.recordsTotal,
                    data = ret.SpoiledORList
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
        public ActionResult SaveSpoiledORSeries(SpoiledORModel model)
        {
            try
            {
                if (model.SOR_Number <= 0)
                    throw new Exception("Please fill spoiled OR number!");

                var ifExist = unitOfWork.SpoiledOR.Find(a =>
                        a.SOR_Number == model.SOR_Number && a.SOR_Unit_Id == CurrentUser.EmployeeUnitId)
                    .FirstOrDefault();
                if (ifExist != null)
                    throw new Exception("Spoiled OR number already exist!");
                var isReleasedOR =
                    unitOfWork.ORSeries.ReleaseOrNumber(Convert.ToInt32(model.SOR_Number), CurrentUser.EmployeeUnitId);
                if (isReleasedOR)
                    throw new Exception("Spoiled OR number already released!");

                model.SOR_CreatedDate = DateTime.Now;
                model.SOR_Created_Emp_Id = CurrentUser.EmployeeId;
                model.Ref_SOR_Id = Functions.NewID();
                model.SOR_Unit_Id = CurrentUser.EmployeeUnitId;

                unitOfWork.SpoiledOR.Add(model);
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



        [HttpPost]
        public JsonResult GetOR(GridInfo gridInfo)
        {
            try
            {
                var ret = unitOfWork.ORSeries.GetSpoiledORList(gridInfo, CurrentUser.EmployeeUnitId);
                var jsonResult = Json(new
                {
                    ret.DatatableInfo.recordsTotal,
                    recordsFiltered = ret.DatatableInfo.recordsTotal,
                    data = ret.ORList
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
        public ActionResult SaveORSeries(ORSeriesModel model)
        {
            try
            {
                if (model.OR_StartSeries == 0 || model.OR_EndSeries == 0)
                    throw new Exception("Please fill start and end series!");

                if (model.OR_EndSeries <= model.OR_StartSeries)
                    throw new Exception("End series should be greater than start.");

         
                if (model.OR_Id > 0)
                {
                    var startSeries = unitOfWork.ORSeries.SingleOrDefault(
                        x => x.OR_Unit_Id == CurrentUser.EmployeeUnitId && x.IsDeleted != true &&
                             x.OR_Id != model.OR_Id, a => a.OrderBy(b => b.OR_StartSeries));

                    var endSeries = unitOfWork.ORSeries.SingleOrDefault(
                        x => x.OR_Unit_Id == CurrentUser.EmployeeUnitId && x.IsDeleted != true,
                        a => a.OrderByDescending(b => b.OR_EndSeries));

                    if (startSeries != null && endSeries != null)
                    {
                        if (model.OR_StartSeries >= startSeries.OR_StartSeries &&
                            model.OR_StartSeries <= endSeries.OR_EndSeries)
                            throw new Exception("Start series already exists!");
                        if (model.OR_EndSeries >= startSeries.OR_StartSeries &&
                            model.OR_EndSeries <= endSeries.OR_EndSeries)
                            throw new Exception("End series already exists!");
                    }

                    var orSeriesDet = new ORSeriesModel();
                    orSeriesDet = 
                      unitOfWork.ORSeries.SingleOrDefault(
                          a =>
                              a.OR_Id == model.OR_Id &&
                              a.OR_Unit_Id == CurrentUser.EmployeeUnitId);

                    if (orSeriesDet == null)
                        throw new Exception("Or Series cannot be found!");

                    model.OR_LastUpdateDate = DateTime.Now;
                    model.OR_LastUpdate_Emp_Id = CurrentUser.EmployeeId;

                    unitOfWork.ORSeries.UpdateORSeries(model);
                }
                else
                {

                    var startSeries = unitOfWork.ORSeries.SingleOrDefault(
                        x => x.OR_Unit_Id == CurrentUser.EmployeeUnitId && x.IsDeleted != true && (x.OR_StartSeries == model.OR_StartSeries || x.OR_EndSeries == model.OR_StartSeries));

                    if (startSeries != null)
                        throw new Exception("Start series already exists!");

                    var endSeries = unitOfWork.ORSeries.SingleOrDefault(
                        x => x.OR_Unit_Id == CurrentUser.EmployeeUnitId && x.IsDeleted != true && (x.OR_StartSeries == model.OR_EndSeries || x.OR_EndSeries == model.OR_EndSeries));

                    if (endSeries != null)
                        throw new Exception("End series already exists!");

                    model.OR_CreatedDate = DateTime.Now;
                    model.OR_Created_Emp_Id = CurrentUser.EmployeeId;
                    model.OR_Issue_Date = model.OR_Issue_Date;
                    model.OR_Unit_Id = CurrentUser.EmployeeUnitId;
                    model.Ref_OR_Id = Functions.NewID();
                    model.OR_StartSeries = model.OR_StartSeries;
                    model.OR_EndSeries = model.OR_EndSeries;

                    unitOfWork.ORSeries.Add(model);
                    unitOfWork.Complete();
                }
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


        [HttpPost]
        public ActionResult DeleteORSeries(int OR_StartSeries,int OR_EndSeries,int OR_Id)
        {
            try
            {
                    var series = unitOfWork.ApplicationPayment.SingleOrDefault(
                        a =>
                            a.AP_Unit_Id == CurrentUser.EmployeeUnitId && a.AP_ORNumber >= OR_StartSeries &&
                            a.AP_ORNumber <= OR_EndSeries);
                    if (series != null)
                        throw new Exception("Cannot delete because some series is currently in used");

                    var ORSeries = unitOfWork.ORSeries
                        .SingleOrDefault(a => a.OR_Id == OR_Id && a.OR_Unit_Id == CurrentUser.EmployeeUnitId);

                    if (ORSeries == null)
                        throw new Exception("OR series cannot be found.");

                    unitOfWork.ORSeries.TagAsDeleted(ORSeries, CurrentUser.EmployeeId);

                var jsonResult = Json(new { message = "Success" }, JsonRequestBehavior.AllowGet);
                return jsonResult;
            }
            catch (Exception ex)
            {
                var ExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ExceptionMessage);
            }
        }

    }
}