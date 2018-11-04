using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EBFP.BL.CIS;
using EBFP.BL.Helper;
using EBFP.Helper;

namespace EBFP.Web.Areas.CIS.Controllers
{
    public class UnserviceableController : Controller
    {
        private readonly ICISUnitOfWork _unitOfWork = new CISUnitOfWork();

        // GET: CIS/PhysicalInventory
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetUnserviceableList(GridInfo gridInfo)
        {
            try
            {
                var ret = _unitOfWork.Unserviceable.GetListResult(gridInfo);
                var retJson = ret.UnserviceableListModel.Select(a => new
                {
                    a.UPI_ReportingOffice,
                    a.UPI_WMR,
                    a.sUPI_Id,
                    a.UPI_Id
                });
                var jsonResult = Json(new
                {
                    ret.DatatableInfo.recordsTotal,
                    recordsFiltered = ret.DatatableInfo.recordsTotal,
                    data = retJson
                }, JsonRequestBehavior.AllowGet);

                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetUnserviceablePI(GridInfo gridInfo)
        {
            try
            {
                var ret = _unitOfWork.PhysicalInventory.GetUnserviceablePIListResult(gridInfo);
                var retJson = ret.PhysicalInventoryListModel.Select(a => new
                {
                    a.PI_Id,
                    a.PI_Description,
                    a.PI_UnitValue,
                    a.PI_DateAcquired,
                    a.PI_PropertyNumber,
                    a.sPI_Id
                });

                var jsonResult = Json(new
                {
                    ret.DatatableInfo.recordsTotal,
                    recordsFiltered = ret.DatatableInfo.recordsTotal,
                    data = retJson
                }, JsonRequestBehavior.AllowGet);

                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
                return null;
            }
        }

        public ActionResult UnserviceableDetails(string sId, string PageStatus)
        {
            ViewBag.PageStatus = PageStatus;
            if (string.IsNullOrWhiteSpace(sId))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var unserviceableId = Convert.ToInt32(sId.Decrypt());
            var retUnserviceable = new UnserviceableModel();
            if (unserviceableId > 0)
            {
                retUnserviceable = _unitOfWork.Unserviceable.GetUnserviceableById(unserviceableId);
               
                if (retUnserviceable == null)
                    return HttpNotFound();

                retUnserviceable.PhysicalInventoryList = _unitOfWork.PhysicalInventory.GetList(a => a.PI_UPI_Id == retUnserviceable.UPI_Id,
                   q => q.OrderBy(d => d.PI_Description));
            }
            return View(retUnserviceable);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnserviceableDetails(UnserviceableModel model)
        {
            try
            {
                if(model.PhysicalInventoryList == null  || model.PhysicalInventoryList.Count <= 0)
                    throw new Exception("Please add Unserviceable item.");

                if (ModelState.IsValid)
                {
                    var exists = _unitOfWork.Unserviceable.UnserviceableExists(model.UPI_WMR, model.UPI_Id);
                    if (exists)
                        throw new Exception("WMR already exists.");

                    if (model.UPI_Id > 0)
                    {
                        _unitOfWork.Unserviceable.UpdateUnserviceable(model);

                        return RedirectToAction("UnserviceableDetails", new
                        {
                            sId = model.UPI_Id.ToString().Encrypt(),
                            PageStatus = PageStatus.Success.ToString()
                        });
                    }

                    model.UPI_CreatedDate = DateTime.Now;
                    model.UPI_Created_Emp_Id = CurrentUser.EmployeeId;
                    model.UPI_Id = _unitOfWork.Unserviceable.Add(model);
                    
                    if (model.PhysicalInventoryList.Count > 0)
                        _unitOfWork.Unserviceable.UpdateToUnserviceable(model);

                    return RedirectToAction("Index", new
                    {
                        PageStatus = PageStatus.Success.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(string sId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sId))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var unserviceableId = Convert.ToInt32(sId.Decrypt());

                _unitOfWork.Unserviceable.DeleteByID(unserviceableId);
                ViewBag.PageStatus = PageStatus.Success.ToString();
            }
            catch (Exception ex)
            {
                var ExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ExceptionMessage);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult PhysicalInventoryRecordEditor()
        {
            var physicalInventoryModel = new PhysicalInventoryModel();
            return PartialView("~/Areas/CIS/Views/Unserviceable/Include/UnserviceableDetails/PhysicalInventory/Editor.cshtml",
                physicalInventoryModel);
        }



        [HttpGet]
        public JsonResult SelectionAutoComplete(string search)
        {
            try
            {
                var ret = _unitOfWork.PhysicalInventory
                    .GetList(a => (a.PI_Description.Contains(search) || a.PI_PropertyNumber.Contains(search)) && a.PI_UPI_Id == null).Select(a => new
                    {
                        text = "[" + a.PI_PropertyNumber + "]-" + a.PI_Description,
                        id = a.PI_Id
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