using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EBFP.BL.CIS;
using EBFP.BL.Helper;
using EBFP.Helper;

namespace EBFP.Web.Areas.CIS.Controllers
{
    public class PhysicalInventoryController : Controller
    {
        private readonly ICISUnitOfWork _unitOfWork = new CISUnitOfWork();

        // GET: CIS/PhysicalInventory
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetPhysicalInventoryList(GridInfo gridInfo)
        {
            try
            {
                var ret = _unitOfWork.PhysicalInventory.GetListResult(gridInfo);
                var retJson = ret.PhysicalInventoryListModel.Select(a => new
                {
                    a.PI_Dir_Name,
                    a.PI_IG_Name,
                    a.PI_Art_Name,
                    a.PI_Description,
                    a.PI_PropertyNumber,
                    a.PI_UnitOfMeasure,
                    a.PI_DateAcquired,
                    a.PI_UnitValue,
                    a.PI_Office,
                    a.PI_ARENumber,
                    a.PI_End_User,
                    a.sPI_Id,
                    a.PI_Id,
                    a.PI_Unit_Name
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

        public ActionResult PhysicalInventoryDetails(string sId, string PageStatus)
        {
            ViewBag.PageStatus = PageStatus;
            if (string.IsNullOrWhiteSpace(sId))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var physicalInventory = Convert.ToInt32(sId.Decrypt());
            var retPhysicalInventory = new PhysicalInventoryModel();
            if (physicalInventory > 0)
            {
                retPhysicalInventory = _unitOfWork.PhysicalInventory.GetPhysicalInventoryById(physicalInventory);
                if (retPhysicalInventory == null)
                    return HttpNotFound();
            }
            return View(retPhysicalInventory);
        }
   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PhysicalInventoryDetails(PhysicalInventoryModel model)
        {
            try
            {
                if (model.PI_Dir_Id <= 0 || model.PI_Art_Id <= 0 || model.PI_IG_Id <= 0)
                    throw new Exception("Directorates, Group and Article are required.");

                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(model.PI_Remarks)  && (model.PI_Emp_Id <= 0 || model.PI_Emp_Id == null))
                        throw new Exception("Remarks is required.");

                    var exists = _unitOfWork.PhysicalInventory.PhysicalInventoryExists(model.PI_PropertyNumber,model.PI_Id);
                    if (exists)
                        throw new Exception("Property Number already exists.");

                    if (model.PI_Id > 0)
                    {
                        _unitOfWork.PhysicalInventory.UpdatePhysicalInventory(model);
                        return RedirectToAction("PhysicalInventoryDetails", new
                        {
                            sId = model.PI_Id.ToString().Encrypt(),
                            PageStatus = PageStatus.Success.ToString()
                        });
                    }
                    else
                    {
                        model.PI_CreatedDate = DateTime.Now;
                        model.PI_Created_Emp_Id = CurrentUser.EmployeeId;
                        model.PI_Unit_Id = CurrentUser.EmployeeUnitId;
                        _unitOfWork.PhysicalInventory.Add(model);

                        _unitOfWork.Complete();

                        return RedirectToAction("Index", new
                        {
                            PageStatus = PageStatus.Success.ToString()
                        });
                    }
                  
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

                var physicalId = Convert.ToInt32(sId.Decrypt());

                _unitOfWork.PhysicalInventory.DeleteByID(physicalId);
                ViewBag.PageStatus = PageStatus.Success.ToString();
            }
            catch (Exception ex)
            {
                var ExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ExceptionMessage);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

      
    }
}