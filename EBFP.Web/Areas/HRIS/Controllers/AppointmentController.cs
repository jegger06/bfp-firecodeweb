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
    public class AppointmentController : Controller
    {
        public AppointmentController()
        {
            unitOfWork = new HRISUnitOfWork();
        }

        public AppointmentController(HRISUnitOfWork employee)
        {
            unitOfWork = employee;
        }

        private IHRISUnitOfWork unitOfWork { get; }

        public ActionResult Appointment(string type)
        {
            var model = new EmployeeAppointmentsSearchModel();
            if(string.IsNullOrWhiteSpace(type))
                return View(model);

            model.EA_AppoitmentStatus = (int) (int) AppointmentStatuses.Temporary;
            model.EA_AppointmentDate_From = DateTime.Now.AddYears(-1);
            if (type == "today")
                model.EA_AppointmentDate_To = DateTime.Now.AddYears(-1);
            else if (type == "threeDays")
                model.EA_AppointmentDate_To = DateTime.Now.AddDays(3).AddYears(-1);
            else if (type == "oneMonth")
                model.EA_AppointmentDate_To = DateTime.Now.AddMonths(1).AddYears(-1);

            return View(model);
        }

        [HttpPost]
        public JsonResult GetEmployeeAppointments(GridInfo gridInfo)
        {
            try
            {
                var ret = unitOfWork.EmployeeAppointments.GetListResult(gridInfo);
                var retJson = ret.EmployeeAppointmentsList
                .Select(a => new
                {
                    a.sEA_Id,
                    a.EA_Id,
                    a.EA_Emp_AccountNumber,
                    a.EA_Emp_Name,
                    a.EA_Rank_Name,
                    EA_AppoitmentStatus = ((AppointmentStatuses)a.EA_AppoitmentStatus).ToDescription(),
                    a.EA_AppointingAuthority,
                    EA_AppoitmentNature = ((AppointmentNature)a.EA_AppoitmentNature).ToDescription(),
                    a.EA_AttestingAuthority,
                    a.EA_AttestingAuthorityDesignation,
                    a.EA_ItemNumber,
                    a.EA_AppointmentDate
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

        public async Task<ActionResult> AppointmentDetails(string sId, string PageStatus)
        {
            ViewBag.PageStatus = PageStatus;
            if (string.IsNullOrWhiteSpace(sId))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var EA_Id = Convert.ToInt32(sId.Decrypt());
            var retEmpAppointment = new EmployeeAppointmentsModel();
            if (EA_Id > 0)
            {
                retEmpAppointment = unitOfWork.EmployeeAppointments.GetEmployeeAppointmentById(EA_Id);
                if (retEmpAppointment == null)
                    return HttpNotFound();
            }
            return View(retEmpAppointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AppointmentDetails(EmployeeAppointmentsModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.EA_Id > 0)
                    {
                        unitOfWork.EmployeeAppointments.UpdateEmployeeAppointment(model);
                    }
                    else
                    {
                        model.EA_Created_Emp_Id = CurrentUser.EmployeeId;
                        model.EA_CreatedDate = DateTime.Now;
                        unitOfWork.EmployeeAppointments.Add(model);
                    }

                    unitOfWork.Complete();
                    return RedirectToAction("Appointment", "Appointment");
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

                var empAppointmentId = Convert.ToInt32(sId.Decrypt());
                unitOfWork.EmployeeAppointments.DeleteByID(empAppointmentId);
                ViewBag.PageStatus = PageStatus.Success.ToString();
            }
            catch (Exception ex)
            {
                var ExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ExceptionMessage);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public JsonResult GetDashboardAppointmentCounter()
        {
            var res = unitOfWork.EmployeeAppointments.GetDashboardAppointmentCounter();
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}