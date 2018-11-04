using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EBFP.BL.CIS;
using EBFP.BL.Helper;
using EBFP.Helper;

namespace EBFP.Web.Areas.CIS.Controllers
{
    public class InventoryArticlesController : Controller
    {
        private readonly ICISUnitOfWork _unitOfWork = new CISUnitOfWork();

        // GET: CIS/Articles
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetInventoryArticles(GridInfo gridInfo)
        {
            try
            {
                var ret = _unitOfWork.InventoryArticle.GetListResult(gridInfo);
                var retJson = ret.InventoryArticleListModel.Select(a => new
                {
                    a.Art_Code,
                    a.Art_Name,
                    a.Art_CreatedBy,
                    a.Art_CreatedDate,
                    a.Art_LastUpdateBy,
                    a.Art_LastUpdateDate,
                    a.Art_Id,
                    a.sArt_Id
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
        public ActionResult InventoryArticlesDetails(InventoryArticleModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Art_Code) || string.IsNullOrEmpty(model.Art_Name))
                    throw new Exception("All fields are required.");
                
                var exists = _unitOfWork.InventoryArticle.InventoryArticleExists(model.Art_Code, model.Art_Name, model.Art_Id);
                if (exists)
                    throw new Exception("Inventory Article already exists.");

                if (model.Art_Id > 0)
                {
                    _unitOfWork.InventoryArticle.UpdateInventoryArticle(model);
                }
                else
                {
                    model.Art_CreatedDate = DateTime.Now;
                    model.Art_Created_Emp_Id = CurrentUser.EmployeeId;
                    _unitOfWork.InventoryArticle.Add(model);
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

                var inventoryArticleId = Convert.ToInt32(sId.Decrypt());

                var currentlyUsed = _unitOfWork.InventoryArticle.CheckIfCurrentlyUsed(inventoryArticleId);
                if (currentlyUsed)
                    throw new Exception("This item is currently used in Physical Inventory.");

                _unitOfWork.InventoryArticle.DeleteByID(inventoryArticleId);
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
                var ret = _unitOfWork.InventoryArticle
                    .GetList(a => (a.Art_Name.Contains(search) || a.Art_Code.Contains(search))).Select(a => new
                    {
                        //text = a.Dir_Name + " " + a.Emp_LastName,
                        text = a.Art_Name,
                        id = a.Art_Id
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