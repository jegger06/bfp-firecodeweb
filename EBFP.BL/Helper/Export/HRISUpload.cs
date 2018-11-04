using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using EBFP.BL.HumanResources;
using EBFP.DataAccess;

namespace EBFP.BL.Helper
{
    public class HRISUpload : EntityFrameworkBase, IHRISUpload
    {
        public HRISUpload(EBFPEntities _context)
        {
            context_ = _context;
        }

        public string SaveAlphaList(HttpPostedFileBase file)
        {
            //IHRISUnitOfWork unitOfWork = new HRISUnitOfWork();
            //var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

            //if ((file == null) || (file.ContentLength <= 0) || string.IsNullOrEmpty(file.FileName)) return "";
            //var path = $"{applicationPath}{@"\Content\MISC\Generated\AlphaList_"}{DateTime.Now.Ticks}{".xlsx"}";
            //file.SaveAs(path);

            //var dtExcel = FileHelper.ReadExcelFileForAlphaList(path);
            //File.Delete(path);

            //if (dtExcel.Rows.Count == 0)
            //    return "No data on the file.";
            ////Update / Save
            //var newUsers = new List<string>();
            //unitOfWork.Employee.UploadAlphaList(dtExcel,ref newUsers);
            return "Success";
        }
    }

    public interface IHRISUpload
    {
        string SaveAlphaList(HttpPostedFileBase file);
    }
}