using System.Collections.Generic;
using AutoMapper;
using EBFP.DataAccess;
using Queries.Core.Repositories;
using System.Linq;
using EBFP.BL.Establishment;
using AutoMapper.QueryableExtensions;
using System;
using System.Data;
using GemBox.Spreadsheet;
using System.Drawing;
using System.IO;
using System.Web;
using EBFP.BL.Helper;
using EBFP.Helper;
using System.Text;

namespace EBFP.BL.Administration
{
    public class BPLOPaymentBL : Repository<tblBPLOPayments, BPLOPaymentModel>, IBPLOPayment
    {
        public BPLOPaymentBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblBPLOPayments, BPLOPaymentsModel>();
            Mapper.CreateMap<tblBPLOPayments, BPLOPaymentsModel>().ReverseMap();
            Mapper.CreateMap<List<tblBPLOPayments>, List<BPLOPaymentModel>>().ReverseMap();
            Mapper.CreateMap<List<tblBPLOPayments>, List<BPLOPaymentModel>>();

            Mapper.CreateMap<tblEstablishments, EstablishmentModel>().ReverseMap();
            Mapper.CreateMap<List<tblEstablishments>, List<EstablishmentModel>>().ReverseMap();
            Mapper.CreateMap<List<tblEstablishments>, List<EstablishmentModel>>();
        }


        public List<EstablishmentModel> GetExistingEstablishments(List<string> mpNumberList, int unitId)
        {
            var existing = BFPContext.tblEstablishments.Where(a => mpNumberList.Contains(a.Est_MP_Number) && a.Est_Unit_Id == unitId);
            return existing.Project().To<EstablishmentModel>().ToList();
        }

        public bool SubmitPayment(BPLOPaymentsModel payment)
        {
            var entity = new tblBPLOPayments();
            Mapper.Map<BPLOPaymentsModel, tblBPLOPayments>(payment, entity);

            BFPContext.tblBPLOPayments.Add(entity);

            BFPContext.SaveChanges();
            return true;
        }


        public bool GenerateUploadedPayments(List<PaymentModel> paymentList, string destinationPath)
        {
            string applicationPath = AppDomain.CurrentDomain.BaseDirectory.Replace(@"\bin\Debug\", "");
            var newFile = $"{destinationPath}";
            var template = $"{applicationPath}{@"\Areas\Administration\BPLO Payments\template\BPLOPayments_Template.xls"}";

            //SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var ws = ef.Worksheets["Sheet1"];
            var cell = ws.Cells;

            int row = 2;
            foreach (var payment in paymentList)
            {
                cell["A" + row].Value = payment.BusinessIdNumber;
                cell["B" + row].Value = payment.BusinessName;
                cell["C" + row].Value = payment.TradeName;
                cell["D" + row].Value = payment.BusinessAddress;
                cell["E" + row].Value = payment.TaxYear;
                cell["F" + row].Value = payment.Amount;
                cell["G" + row].Value = payment.IssueDate;
                if (payment.Remarks != "OK")
                {
                    foreach (char character in "ABCDEFGH")
                    {
                        cell[character.ToString() + row].Style.Font.Color = Color.Red;
                        cell[character.ToString() + row].Style.FillPattern.SetSolid(Color.BlanchedAlmond);
                    }
                }

                cell["H" + row++].Value = payment.Remarks;
            }

            ef.Save(newFile);

            return true;
        }

        public void SyncBPLOPaymentLocalToServer(List<BPLOPaymentModel> localModelList)
        {
            foreach (var local in localModelList)
            {
                var payment = BFPContext.tblBPLOPayments
                    .FirstOrDefault(a => a.Ref_BPLOP_Id == local.Ref_BPLOP_Id &&
                                         a.BPLOP_Unit_Id == local.BPLOP_Unit_Id);

                if (payment == null)
                {
                    payment = new tblBPLOPayments();
                    Mapper.Map(local, payment);
                    BFPContext.tblBPLOPayments.Add(payment);
                }
                else
                {
                    var bplopId = payment.BPLOP_Id;

                    Mapper.Map(local, payment);
                    payment.BPLOP_Id = bplopId;
                }
            }

            BFPContext.SaveChanges();
        }

        public void UploadPayments(HttpPostedFileBase file, ref List<PaymentModel> _listBusiness)
        {
            //List<PaymentModel> _listBusiness = new List<PaymentModel>();
            var hasError = false;
            var _isValid = false;
            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

            if ((file == null) || (file.ContentLength <= 0) || string.IsNullOrEmpty(file.FileName)) throw new Exception("No data on the file.");
            var path = $"{applicationPath}{@"Content\MISC\Generated\BPLOPayment_"}{DateTime.Now.Ticks}{".xls"}";
            file.SaveAs(path);

            var dtExcel = FileHelper.ReadExcelFileForBPLOPayment(path);
            File.Delete(path);

            if (dtExcel.Rows.Count == 0)
                throw new Exception("No data on the file.");
            

            var list = dtExcel.AsEnumerable()
                       .Select(r => r.Field<string>("Business ID Number"))
                       .ToList();

            var existingEstablishments = GetExistingEstablishments(list, CurrentUser.EmployeeUnitId);
            
            var listUploadedPayments = new List<PaymentModel>();
            if (dtExcel.Rows.Count == 0)
                throw new Exception("No data on the file.");

            foreach (DataRow row in dtExcel.Rows)
            {
                var editedRow = CheckExcelValuesFormat(row);
                var errorMessage = editedRow["Remarks"].ToString();

                if (!string.IsNullOrWhiteSpace(errorMessage))
                    hasError = true;

                var model = new PaymentModel
                {
                    BusinessIdNumber = row["Business ID Number"].ToString(),
                    BusinessName = row["Business Name"].ToString(),
                    TradeName = row["Trade Name"].ToString(),
                    BusinessAddress = row["Business Address"].ToString(),
                    TaxYear = row["Tax Year"].ToString(),
                    Amount = row["Payment Amount"].ToString(),
                    IssueDate = editedRow["Payment Date"].ToString(),
                    Remarks = errorMessage
                };

                listUploadedPayments.Add(model);
            }

            //Checking of existing establishments
            foreach (var item in listUploadedPayments)
            {
                var model = new PaymentModel();

                var establishment =
                    existingEstablishments.FirstOrDefault(a => a.Est_MP_Number == item.BusinessIdNumber);
                if (establishment != null && string.IsNullOrWhiteSpace(item.Remarks))
                {
                    //Existing establishment
                    model.BusinessName = establishment.Est_BusinessName;
                    model.TradeName = establishment.Est_BusinessTradeName;
                    model.BusinessAddress = establishment.Est_BusinessAddress;
                    model.Est_Id = establishment.Ref_Est_Id;
                    model.Remarks = "OK";
                }
                else
                {
                    StringBuilder remarks = new StringBuilder();
                    //NOT Existing Establishments
                    _isValid = false;
                    model.Est_Id = "";
                    model.BusinessName = item.BusinessName;
                    model.BusinessAddress = item.BusinessAddress;
                    model.TradeName = item.TradeName;
                    if (establishment == null)
                    {
                        remarks.Append("Establishment not yet on file");
                    }
                    if (!string.IsNullOrWhiteSpace(item.Remarks))
                    {
                        if (establishment == null)
                            remarks.Append(Environment.NewLine);

                        remarks.Append(item.Remarks);
                    }

                    model.Remarks = remarks.ToString();

                }

                model.BusinessIdNumber = item.BusinessIdNumber;
                model.Amount = item.Amount;
                model.IssueDate = item.IssueDate;
                model.TaxYear = item.TaxYear;

                _listBusiness.Add(model);
            }

            if (hasError)
                throw new Exception("There are some errors in the file. Please correct the data before submitting.");
        }

        private DataRow CheckExcelValuesFormat(DataRow row)
        {
            StringBuilder errorMessage = new StringBuilder();

            if (string.IsNullOrWhiteSpace(row["Business ID Number"].ToString()) ||
                             string.IsNullOrWhiteSpace(row["Payment Amount"].ToString()) ||
                             string.IsNullOrWhiteSpace(row["Payment Date"].ToString()))
            {
                errorMessage.Append("'Business ID Number', 'Pay Amount' and 'Payment Date' is required.");
            }

            //Check format for Payment Amount
            if (!string.IsNullOrWhiteSpace(row["Payment Amount"].ToString()))
            {
                decimal amount;
                bool isNumeric = decimal.TryParse(row.ItemArray[5].ToString(), out amount);
                if (!isNumeric)
                {
                    if (!string.IsNullOrWhiteSpace(errorMessage.ToString()))
                        errorMessage.Append(Environment.NewLine);

                    errorMessage.Append("'Payment Amount' is not in a correct format.");
                }
            }

            //Check format for Payment Date
            if (!string.IsNullOrWhiteSpace(row["Payment Date"].ToString()))
            {
                DateTime paymentDate;
                bool isDateTime = DateTime.TryParse(row["Payment Date"].ToString(), out paymentDate);
                if (!isDateTime)
                {
                    if (!string.IsNullOrWhiteSpace(errorMessage.ToString()))
                        errorMessage.Append(Environment.NewLine);

                    errorMessage.Append("'Payment Date' is not in a valid date.");
                }
                else
                {
                    row["Payment Date"] = Convert.ToDateTime(row["Payment Date"].ToString()).ToString("MM-dd-yyyy");
                }
            }

            row["Remarks"] = errorMessage.ToString();

            return row;
        }
    }
}