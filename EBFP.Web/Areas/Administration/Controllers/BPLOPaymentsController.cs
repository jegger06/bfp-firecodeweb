using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EBFP.BL.Administration;
using EBFP.BL.Areas.FSIC.Model;
using EBFP.BL.Helper;
using EBFP.BL.HumanResources;
using EBFP.Helper;
using WebMatrix.WebData;

namespace EBFP.Web.Areas.Administration.Controllers
{
    public class BPLOPaymentsController : Controller
    {
        public BPLOPaymentsController()
        {
            unitOfWork = new AdministrationUnitOfWork();
        }

        public BPLOPaymentsController(AdministrationUnitOfWork admin)
        {
            unitOfWork = admin;
        }

        private IAdministrationUnitOfWork unitOfWork { get; }

        public ActionResult BPLOPayments()
        {
            var model = new BPLOPaymentsUploadModel();
            model.paymentList = new List<PaymentModel>();
            return View(model);
        }

        [HttpGet]
        public ActionResult DownloadTemplate()
        {
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            var fullPath = $"{rootPath}{"\\Content\\MISC\\Template\\BPLOPayments_Template.xls"}";
            return File(fullPath, "application/pdf", "BPLOPayments_Template.xls");
        }

        [HttpPost]
        public ActionResult UploadBPLOPayments(BPLOPaymentsUploadModel files)
        {
            try
            {
                var model = new BPLOPaymentsUploadModel();
                var lstBusiness = new List<PaymentModel>();
                unitOfWork.BPLOPayments.UploadPayments(files.uploadList, ref lstBusiness);

                model.paymentList = lstBusiness;
                if (lstBusiness.Count(a => a.Remarks != "OK") > 0)
                    @ViewBag.Message = "If click 'SUBMIT', only records with 'OK' remarks will be processed!";
                return View("BPLOPayments", model);
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
                return RedirectToAction("BPLOPayments", new { result = ViewBag.ExceptionMessage });
            }
        }

        [HttpPost]
        public ActionResult SubmitBPLOPayments(BPLOPaymentsUploadModel model)
        {
            try
            {
                IUnitOfWork oUnitOfWork = new UnitOfWork();
                var _listBusiness = model.paymentList;
                
                if (_listBusiness.Count > 0)
                {
                    foreach (var est in _listBusiness.Where(a => a.Remarks == "OK"))
                    {
                        var fsic = new FSICModel
                        {
                            Ref_FSIC_App_Id = Functions.NewID(),
                            FSIC_ApplicationDate = DateTime.Now,
                            FSIC_ApplicationType = (int)AP_ApplicationType.BPLOPayment,
                            FSIC_TaxYear = (int)Functions.GetEnumValueFromDescription<FSIC_TaxYear>(est.TaxYear),
                            FSIC_Est_Id = est.Est_Id,
                            FSIC_Status = (int)FSIC_Status.Collected,
                            IsSynced = false,
                            FSIC_Issue_Date = DateTime.Now,
                            FSIC_Unit_Id = CurrentUser.EmployeeUnitId
                        };
                        //Save in FSIC
                        var appId = oUnitOfWork.FireSafetyInspectionCertificate.SavePaymentFSIC(fsic);


                        var bploPayment = new BPLOPaymentsModel
                        {
                            Ref_BPLOP_Id = Functions.NewID(),
                            BPLOP_App_Id = appId,
                            BPLOP_CreatedDate = DateTime.Now,
                            BPLOP_Created_Emp_Id =CurrentUser.EmployeeId,
                            BPLOP_PayAmount = Convert.ToDecimal(est.Amount),
                            BPLOP_PayDate = Convert.ToDateTime(est.IssueDate),
                            BPLOP_Unit_Id = CurrentUser.EmployeeUnitId,
                            BPLOP_BasisOfAmountPaid = est.BasisOfAmountPaid
                        };

                        //Save in BPLO Payments
                        unitOfWork.BPLOPayments.SubmitPayment(bploPayment);
                    }

                    GeneratePaymentWithRemarks();
                }
                else
                {
                    //MessageBoxAdv.Show("Please upload excel file before submit.", "Error", MessageBoxButtons.OK,
                    //    MessageBoxIcon.Error);
                }

                return RedirectToAction("BPLOPayments", new { result = "Success" });


            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
                return RedirectToAction("BPLOPayments", new { result = ViewBag.ExceptionMessage });
            }
        }

        private void GeneratePaymentWithRemarks()
        {
            //var ans = MessageBoxAdv.Show(
            //    "Generated BPLO Payments with remarks will be generated, please select path to save.",
            //    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //if (ans == DialogResult.OK)
            //{
            //    //GENERATED REPORT WILL DEPEND ON THE SELECTED VALUE
            //    var dlg = new SaveFileDialog
            //    {
            //        FileName = "Generated BPLO Payments with Remarks",
            //        Filter = @"Excel 97-2003|*.xls|Excel Workbook|*.xlsx",
            //        DefaultExt = "xls",
            //        AddExtension = true
            //    };

            //    if (dlg.ShowDialog() == DialogResult.OK)
            //    {
            //        var destinationPath = dlg.FileName;
            //        IUnitOfWork oUnitOfWork = new UnitOfWork();

            //        //Generate Payments with Remarks
            //        var isSuccess = oUnitOfWork.BPLOPayments.GenerateUploadedPayments(_listBusiness, destinationPath);

            //        if (isSuccess)
            //            Notification.ShowSplash(new SplashMessage
            //            {
            //                MessageBody =
            //                    "Uploaded files has been saved and report has been generated successfully. Please check the file.",
            //                Title = "BPLO Payments",
            //                TimerInterval = 15000
            //            });
            //    }
            //}
        }

    }
}