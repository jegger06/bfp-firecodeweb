using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using EBFP.BL.Helper;
using EBFP.BL.HumanResources;
using EBFP.Helper;

namespace EBFP.Web.Areas.HRIS.Controllers
{
    [Authorize]
    public class RankController : Controller
    {
        public RankController()
        {
            unitOfWork = new HRISUnitOfWork();
        }

        public RankController(HRISUnitOfWork employee)
        {
            unitOfWork = employee;
        }

        private IHRISUnitOfWork unitOfWork { get; }

        public ActionResult Rank()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetRanks(GridInfo gridInfo)
        {
            try
            {
                var ret = unitOfWork.Rank.GetListResult(gridInfo);
                var jsonResult = Json(new
                {
                    ret.DatatableInfo.recordsTotal,
                    recordsFiltered = ret.DatatableInfo.recordsTotal,
                    data = ret.RankList
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

        public async Task<ActionResult> RankDetails(string sId, string PageStatus)
        {
            ViewBag.PageStatus = PageStatus;
            if (string.IsNullOrWhiteSpace(sId))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var rankId = Convert.ToInt32(sId.Decrypt());
            var retUnit = new RankModel();
            if (rankId > 0)
            {
                retUnit = unitOfWork.Rank.GetRankById(rankId);
                if (retUnit == null)
                    return HttpNotFound();
            }
            return View(retUnit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RankDetails(RankModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Rank_Id > 0)
                    {
                        model.Rank_UpdatedBy = CurrentUser.EmployeeId;
                        model.Rank_UpdatedDate = DateTime.Now;
                        unitOfWork.Rank.UpdateRank(model);
                    }
                    else
                    {
                        model.Rank_CreatedBy = CurrentUser.EmployeeId;
                        model.Rank_CreatedDate = DateTime.Now;
                        unitOfWork.Rank.Add(model);
                    }

                    unitOfWork.Complete();
                    return RedirectToAction("Rank", "Rank");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
            }
            return View(model);
        }

        [HttpPost]
        public string Delete(string sId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sId))
                    throw new Exception("Cannot find item.");

                var rankId = Convert.ToInt32(sId.Decrypt());

                var inUsed = unitOfWork.Employee.CheckRank(rankId);
                if (inUsed)
                    throw new Exception("This rank is currently in used.");

                var rank = unitOfWork.Rank
                    .SingleOrDefault(a => a.Rank_Id == rankId);

                var success = unitOfWork.Rank.DeleteByID(rankId);
                if (success)
                {
                    var rankName = rank != null ? rank.Rank_Name : "";
                    var logsModel = new LogsModel
                    {
                        Log_Emp_Id = CurrentUser.EmployeeId,
                        Log_Remarks = "Rank - Deleted rank " + "'" + rankName + "'"
                    };
                    unitOfWork.Logs.InsertLogs(logsModel);
                }

                ViewBag.PageStatus = PageStatus.Success.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException?.Message ?? ex.Message;
            }

            return "Success";
        }
    }
}