using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EBFP.BL.CIS;
using EBFP.BL.Helper;
using EBFP.Helper;

namespace EBFP.Web.Areas.CIS.Controllers
{
    public class InventoryGroupsController : Controller
    {
        private readonly ICISUnitOfWork _unitOfWork = new CISUnitOfWork();
        // GET: CIS/InventoryGroups
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetInventoryGroups(GridInfo gridInfo)
        {
            try
            {
                var ret = _unitOfWork.InventoryGroup.GetListResult(gridInfo);
                var retJson = ret.InventoryGroupListModel.Select(a => new
                {
                    a.IG_Code,
                    a.IG_Description,
                    a.IG_CreatedBy,
                    a.IG_CreatedDate,
                    a.IG_LastUpdateBy,
                    a.IG_LastUpdateDate,
                    a.IG_Id,
                    a.sIG_Id
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
        public ActionResult InventoryGroupsDetails(InventoryGroupModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.IG_Code) || string.IsNullOrEmpty(model.IG_Description))
                    throw new Exception("All fields are required.");
                var exists = _unitOfWork.InventoryGroup.InventoryGroupExists(model.IG_Code, model.IG_Description, model.IG_Id);
                if (exists)
                    throw new Exception("Inventory Group already exists.");

                if (model.IG_Id > 0)
                {
                    _unitOfWork.InventoryGroup.UpdateInventoryGroup(model);
                }
                else
                {
                    model.IG_CreatedDate = DateTime.Now;
                    model.IG_Created_Emp_Id = CurrentUser.EmployeeId;
                    _unitOfWork.InventoryGroup.Add(model);
                }
                _unitOfWork.Complete();
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

        [HttpGet]
        public ActionResult Delete(string sId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sId))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var inventoryGroupId = Convert.ToInt32(sId.Decrypt());

                var currentlyUsed = _unitOfWork.InventoryGroup.CheckIfCurrentlyUsed(inventoryGroupId);
                if (currentlyUsed)
                    throw new Exception("This item is currently used in Physical Inventory.");

                _unitOfWork.InventoryGroup.DeleteByID(inventoryGroupId);
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
        public JsonResult SelectionAutoComplete(string search)
        {
            try
            {
                var ret = _unitOfWork.InventoryGroup
                    .GetList(a => a.IG_Description.Contains(search) || a.IG_Code.Contains(search)).Select(a => new
                    {
                        //text = a.Dir_Name + " " + a.Emp_LastName,
                        text = a.IG_Description,
                        id = a.IG_Id
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