using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EBFP.BL.Administration;
using EBFP.BL.Helper;
using EBFP.BL.HumanResources;
using EBFP.Helper;
using WebMatrix.WebData;
using EBFP.BL.Establishment;

namespace EBFP.Web.Areas.Establishment.Controllers
{
    [Authorize]
    public class EstablishmentController : Controller
    {
        public EstablishmentController()
        {
            unitOfWork = new EstablishmentUnitOfWork();
        }

        public EstablishmentController(EstablishmentUnitOfWork establishment)
        {
            unitOfWork = establishment;
        }

        private IEstablishmentUnitOfWork unitOfWork { get; }

        public ActionResult Establishment(string PageStatus)
        {
            ViewBag.PageStatus = PageStatus;
            return View();
        }
        
        [HttpGet]
        public JsonResult SelectionAutoComplete(string search)
        {
            try
            {
                dynamic ret;
            
                     ret = unitOfWork.Establishment
                     .GetList(
                         a =>
                              a.Est_BusinessName.Contains(search) && a.Est_Unit_Id == CurrentUser.EmployeeUnitId).Select(a => new
                              //a.Est_BusinessName.Contains(search)).Select(a => new
                              {
                                 text = a.Est_BusinessName ,
                                 id = a.Est_Id
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

        [HttpPost]
        public JsonResult GetEstablishment(GridInfo gridInfo)
        {
            try
            {
                //var firstName = CurrentUser.FirstName;
                //bool isOverallAdmin = false;

                //if (firstName == "Administrator")
                //    isOverallAdmin = true;

                var ret = unitOfWork.Establishment.GetEstablishmentListByUnit(gridInfo, CurrentUser.EmployeeUnitId);
                var jsonResult = Json(new
                {
                    ret.DatatableInfo.recordsTotal,
                    recordsFiltered = ret.DatatableInfo.recordsTotal,
                    data = ret.EstablishmentList
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

        [OutputCache(Duration = 10, VaryByParam = "sEst_id")]
        public ActionResult EstablishmentDetails(string sEst_id, string PageStatus)
        {
            ViewBag.PageStatus = PageStatus;
            if (string.IsNullOrWhiteSpace(sEst_id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var establishmentId = Convert.ToInt32(sEst_id.Decrypt());
            var retEmployee = new EstablishmentModel();
            if (establishmentId > 0)
            {
                retEmployee = unitOfWork.Establishment.GetEstablishmentById(establishmentId);
                if (retEmployee == null)
                    return HttpNotFound();
            }

            return View(retEmployee);
        }


       [HttpPost]
        public ActionResult EstablishmentDetails(EstablishmentModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.sEst_Id))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var mpNumber = model.Est_MP_Number.TrimNullable();

                if (string.IsNullOrEmpty(model.Est_BusinessName.TrimNullable()))
                    throw new Exception("Establishment Name is required!");
                if (string.IsNullOrEmpty(mpNumber))
                    throw new Exception("MP / TIN / BIN is required!");

                var establishment = new EstablishmentModel();
                if (!string.IsNullOrEmpty(model.sEst_Id))
                {
                    establishment =
                        unitOfWork.Establishment.SingleOrDefault(
                            a =>
                                a.Est_MP_Number == mpNumber && a.Est_Id != model.Est_Id &&
                                a.Est_Unit_Id == CurrentUser.EmployeeUnitId);
                    if (establishment != null)
                        throw new Exception("MP / TIN / BIN has duplicates!");
                }
                else
                {
                    establishment =
                        unitOfWork.Establishment.SingleOrDefault(
                            a => a.Est_MP_Number == mpNumber && a.Est_Unit_Id == CurrentUser.EmployeeUnitId);
                    if (establishment != null)
                        throw new Exception("MP / TIN / BIN has duplicates!");
                }

                model.Est_LastUpdateDate = DateTime.Now;
                model.Est_LastUpdate_Emp_Id = CurrentUser.EmployeeId;

                if (ModelState.IsValid)
                {
                    int estId = Convert.ToInt32(model.sEst_Id.Decrypt());
                    if(estId == 0)
                    {
                        model.Ref_Est_Id = Functions.NewID();
                        model.Est_Unit_Id = CurrentUser.EmployeeUnitId;
                        model.Est_Created_Emp_Id = CurrentUser.EmployeeId;
                        model.Est_CreatedDate = DateTime.Now;

                        unitOfWork.Establishment.Add(model);
                        unitOfWork.Complete();

                        return RedirectToAction("Establishment", new
                        {
                            PageStatus = PageStatus.Success.ToString()
                        });
                        //model.Est_Id = unitOfWork.Establishment.GetEstablishmentIdByRefId(model.Ref_Est_Id);
                    }
                    else
                    {
                        unitOfWork.Establishment.UpdateEstablishment(model);
                    }

                    return RedirectToAction("EstablishmentDetails", new
                    {
                        sEst_Id = model.Est_Id.ToString().Encrypt(),
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

        //public ActionResult EstablishmentDetails()
        //{
        //    return View();
        //}
    }
}