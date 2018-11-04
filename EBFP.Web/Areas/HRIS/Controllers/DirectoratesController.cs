using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EBFP.BL.Helper;
using EBFP.BL.HumanResources;
using EBFP.Helper;

namespace EBFP.Web.Areas.HRIS.Controllers
{
    public class DirectoratesController : Controller
    {
        private readonly IHRISUnitOfWork _unitOfWork = new HRISUnitOfWork();

        // GET: HRIS/Directorates
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetList(GridInfo gridInfo)
        {
            try
            {
                var ret = _unitOfWork.Directorates.GetListResult(gridInfo);
                var retJson = ret.DirectoratesListModel.Select(a => new
                {
                    a.Dir_Code,
                    a.Dir_Name,
                    a.Dir_CreatedBy,
                    a.Dir_CreatedDate,
                    a.Dir_LastUpdateBy,
                    a.Dir_LastUpdateDate,
                    a.Dir_Id,
                    a.sDir_Id
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
        public ActionResult DirectoratesDetails(DirectoratesModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Dir_Code) || string.IsNullOrEmpty(model.Dir_Name))
                    throw new Exception("All fields are required.");

                var exists = _unitOfWork.Directorates.DirectorateExists(model.Dir_Code, model.Dir_Name, model.Dir_Id);
                if (exists)
                    throw new Exception("Directorates already exists.");

                if (model.Dir_Id > 0)
                {
                    _unitOfWork.Directorates.UpdateDirectorates(model);
                }
                else
                {
                    model.Dir_CreatedDate = DateTime.Now;
                    model.Dir_Created_Emp_Id = CurrentUser.EmployeeId;
                    _unitOfWork.Directorates.Add(model);
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

                var dirId = Convert.ToInt32(sId.Decrypt());

                var currentlyUsed = _unitOfWork.Directorates.CheckIfCurrentlyUsed(dirId);
                if (currentlyUsed)
                    throw new Exception("This item is currently used in Physical Inventory.");

                _unitOfWork.Directorates.DeleteByID(dirId);
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
                var ret = _unitOfWork.Directorates
                    .GetList(a => a.Dir_Name.Contains(search) || a.Dir_Code.Contains(search)).Select(a => new
                    {
                        //text = a.Dir_Name + " " + a.Emp_LastName,
                        text = a.Dir_Name,
                        id = a.Dir_Id
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