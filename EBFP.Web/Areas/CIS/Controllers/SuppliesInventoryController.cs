using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EBFP.BL.CIS;
using EBFP.BL.Helper;
using EBFP.Helper;

namespace EBFP.Web.Areas.CIS.Controllers
{
    public class SuppliesInventoryController : Controller
    {
        private readonly ICISUnitOfWork _unitOfWork = new CISUnitOfWork();

        // GET: CIS/Supplies
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetSuppliesInventory(GridInfo gridInfo)
        {
            try
            {
                var ret = _unitOfWork.InventorySupplies.GetListResult(gridInfo);
                var retJson = ret.InventorySuppliesModel.Select(a => new
                {
                    a.SI_Id,
                    a.SI_Art_Id,
                    a.SI_Art_Name,
                    a.SI_Description,
                    a.SI_StockNumber,
                    a.SI_UnitOfMeasure,
                    a.SI_UnitValue,
                    a.SI_OnHand,
                    a.SI_TotalAmount,
                    a.SI_Quantity,
                    a.SI_DateAcquired,
                    a.sSI_Id
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
        public ActionResult SuppliesInventoryDetails(InventorySuppliesModel model)
        {
            try
            {
                if (model.SI_Art_Id < 0 || string.IsNullOrEmpty(model.SI_Description) || string.IsNullOrEmpty(model.SI_StockNumber) ||
                    string.IsNullOrEmpty(model.SI_UnitOfMeasure) || (model.SI_UnitValue ?? 0) <= 0 || (model.SI_Quantity ?? 0) <= 0)
                    throw new Exception("All fields are required.");

                _unitOfWork.InventorySupplies.SaveSuppliesInventory(model);
            }
            catch (Exception ex)
            {
                var jsonResultError = Json(new
                {
                    message = ex.InnerException?.Message ?? ex.Message
                }, JsonRequestBehavior.AllowGet);
                return jsonResultError;
            }

            var jsonResult = Json(new {message = "success"}, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }


        [HttpPost]
        public JsonResult GetSuppliesOutInventory(GridInfo gridInfo)
        {
            try
            {
                var ret = _unitOfWork.InventoryOutSupplies.GetListResult(gridInfo);
                var retJson = ret.InventoryOutSuppliesModel.Select(a => new
                {
                    a.SI_Id,
                    a.SIO_Id,
                    a.SIO_Emp_Id,
                    a.SIO_Emp_Name,
                    a.SIO_OutDate,
                    a.SIO_QuantityOut,
                    a.SIO_Remarks,
                    a.sSIO_Id
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
        public ActionResult SuppliesOutInventoryDetails(InventoryOutSuppliesModel model)
        {
            try
            {
                if (model.SIO_OutDate.Year < 1990  || model.SIO_QuantityOut <= 0)
                    throw new Exception("Date and Quantity fields are required.");

                if (model.SIO_Emp_Id <= 0 && string.IsNullOrWhiteSpace(model.SIO_Remarks))
                    throw new Exception("Remarks field is required.");

                _unitOfWork.InventoryOutSupplies.SaveOutSuppliesInventory(model);
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
        public ActionResult Delete(string sId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sId))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var suppliesInventoryId = Convert.ToInt32(sId.Decrypt());

                var hasExisitingOutSupply = _unitOfWork.InventoryOutSupplies.Find(a => a.SI_Id == suppliesInventoryId);
                if (hasExisitingOutSupply.Any())
                {
                    ViewBag.PageStatus = PageStatus.Error.ToString();
                    throw new Exception("Cannot delete this item, an existing supply has already been out.");
                }

                _unitOfWork.InventorySupplies.DeleteByID(suppliesInventoryId);
                ViewBag.PageStatus = PageStatus.Success.ToString();
            }
            catch (Exception ex)
            {
                var ExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ExceptionMessage);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        public ActionResult DeleteOut(string sId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sId))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var outId = Convert.ToInt32(sId.Decrypt());

                _unitOfWork.InventoryOutSupplies.DeleteByID(outId);
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