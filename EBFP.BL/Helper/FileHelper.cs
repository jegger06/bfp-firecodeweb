using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using GemBox.Spreadsheet;
using EBFP.BL.Areas.HumanResources.Employee.Model;

namespace EBFP.BL.Helper
{
    public static class FileHelper
    {
        public static void CheckDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public static void MoveFile(string fileName, string oldPath, string newPath)
        {
            if (File.Exists(oldPath + fileName))
                File.Move(oldPath + fileName, newPath + fileName);
        }

        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        public static string SaveFile(string path, string oldFileName, HttpPostedFileBase file)
        {
            var buffer = Functions.ConvertToByte(file);
            if(!string.IsNullOrWhiteSpace(oldFileName))
                DeleteFile(path + oldFileName);

            var filename = Functions.NewGuid() + ".jpg";
            File.WriteAllBytes(path + filename, buffer);
            return filename;
        }

        public static void TransmitandDeleteFile(string FileName, string FilePathTotransmit)
        {
            try
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = GetMIMEType(FilePathTotransmit);
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                HttpContext.Current.Response.AppendHeader("Pragma", "no-cache");
                HttpContext.Current.Response.TransmitFile(FilePathTotransmit);
                HttpContext.Current.Response.Flush();
                File.Delete(FilePathTotransmit);
                HttpContext.Current.ApplicationInstance.CompleteRequest();


                //HttpContext.Current.Response.Clear();
                //HttpContext.Current.Response.ContentType =
                //    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName + ".xlsx");
                //HttpContext.Current.Response.AppendHeader("Pragma", "no-cache");
                //HttpContext.Current.Response.TransmitFile(FilePathTotransmit);
                //HttpContext.Current.Response.Flush();
                //File.Delete(FilePathTotransmit);
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static string GetMIMEType(string fileName)
        {
            if (MIMETypesDictionary.ContainsKey(Path.GetExtension(fileName).Remove(0, 1)))
            {
                return MIMETypesDictionary[Path.GetExtension(fileName).Remove(0, 1)];
            }
            return "unknown/unknown";
        }

        private static readonly Dictionary<string, string> MIMETypesDictionary = new Dictionary<string, string>
        {
            {"ai", "application/postscript"},
            {"aif", "audio/x-aiff"},
            {"aifc", "audio/x-aiff"},
            {"aiff", "audio/x-aiff"},
            {"asc", "text/plain"},
            {"atom", "application/atom+xml"},
            {"au", "audio/basic"},
            {"avi", "video/x-msvideo"},
            {"bcpio", "application/x-bcpio"},
            {"bin", "application/octet-stream"},
            {"bmp", "image/bmp"},
            {"cdf", "application/x-netcdf"},
            {"cgm", "image/cgm"},
            {"class", "application/octet-stream"},
            {"cpio", "application/x-cpio"},
            {"cpt", "application/mac-compactpro"},
            {"csh", "application/x-csh"},
            {"css", "text/css"},
            {"csv", "text/plain"},
            {"dcr", "application/x-director"},
            {"dif", "video/x-dv"},
            {"dir", "application/x-director"},
            {"djv", "image/vnd.djvu"},
            {"djvu", "image/vnd.djvu"},
            {"dll", "application/octet-stream"},
            {"dmg", "application/octet-stream"},
            {"dms", "application/octet-stream"},
            {"doc", "application/msword"},
            {"docx","application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
            {"dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template"},
            {"docm","application/vnd.ms-word.document.macroEnabled.12"},
            {"dotm","application/vnd.ms-word.template.macroEnabled.12"},
            {"dtd", "application/xml-dtd"},
            {"dv", "video/x-dv"},
            {"dvi", "application/x-dvi"},
            {"dxr", "application/x-director"},
            {"eps", "application/postscript"},
            {"etx", "text/x-setext"},
            {"exe", "application/octet-stream"},
            {"ez", "application/andrew-inset"},
            {"gif", "image/gif"},
            {"gram", "application/srgs"},
            {"grxml", "application/srgs+xml"},
            {"gtar", "application/x-gtar"},
            {"hdf", "application/x-hdf"},
            {"hqx", "application/mac-binhex40"},
            {"htm", "text/html"},
            {"html", "text/html"},
            {"ice", "x-conference/x-cooltalk"},
            {"ico", "image/x-icon"},
            {"ics", "text/calendar"},
            {"ief", "image/ief"},
            {"ifb", "text/calendar"},
            {"iges", "model/iges"},
            {"igs", "model/iges"},
            {"jnlp", "application/x-java-jnlp-file"},
            {"jp2", "image/jp2"},
            {"jpe", "image/jpeg"},
            {"jpeg", "image/jpeg"},
            {"jpg", "image/jpeg"},
            {"js", "application/x-javascript"},
            {"kar", "audio/midi"},
            {"latex", "application/x-latex"},
            {"lha", "application/octet-stream"},
            {"lzh", "application/octet-stream"},
            {"m3u", "audio/x-mpegurl"},
            {"m4a", "audio/mp4a-latm"},
            {"m4b", "audio/mp4a-latm"},
            {"m4p", "audio/mp4a-latm"},
            {"m4u", "video/vnd.mpegurl"},
            {"m4v", "video/x-m4v"},
            {"mac", "image/x-macpaint"},
            {"man", "application/x-troff-man"},
            {"mathml", "application/mathml+xml"},
            {"me", "application/x-troff-me"},
            {"mesh", "model/mesh"},
            {"mid", "audio/midi"},
            {"midi", "audio/midi"},
            {"mif", "application/vnd.mif"},
            {"mov", "video/quicktime"},
            {"movie", "video/x-sgi-movie"},
            {"mp2", "audio/mpeg"},
            {"mp3", "audio/mpeg"},
            {"mp4", "video/mp4"},
            {"mpe", "video/mpeg"},
            {"mpeg", "video/mpeg"},
            {"mpg", "video/mpeg"},
            {"mpga", "audio/mpeg"},
            {"ms", "application/x-troff-ms"},
            {"msh", "model/mesh"},
            {"mxu", "video/vnd.mpegurl"},
            {"nc", "application/x-netcdf"},
            {"oda", "application/oda"},
            {"ogg", "application/ogg"},
            {"pbm", "image/x-portable-bitmap"},
            {"pct", "image/pict"},
            {"pdb", "chemical/x-pdb"},
            {"pdf", "application/pdf"},
            {"pgm", "image/x-portable-graymap"},
            {"pgn", "application/x-chess-pgn"},
            {"pic", "image/pict"},
            {"pict", "image/pict"},
            {"png", "image/png"},
            {"pnm", "image/x-portable-anymap"},
            {"pnt", "image/x-macpaint"},
            {"pntg", "image/x-macpaint"},
            {"ppm", "image/x-portable-pixmap"},
            {"ppt", "application/vnd.ms-powerpoint"},
            {"pptx","application/vnd.openxmlformats-officedocument.presentationml.presentation"},
            {"potx","application/vnd.openxmlformats-officedocument.presentationml.template"},
            {"ppsx","application/vnd.openxmlformats-officedocument.presentationml.slideshow"},
            {"ppam","application/vnd.ms-powerpoint.addin.macroEnabled.12"},
            {"pptm","application/vnd.ms-powerpoint.presentation.macroEnabled.12"},
            {"potm","application/vnd.ms-powerpoint.template.macroEnabled.12"},
            {"ppsm","application/vnd.ms-powerpoint.slideshow.macroEnabled.12"},
            {"ps", "application/postscript"},
            {"qt", "video/quicktime"},
            {"qti", "image/x-quicktime"},
            {"qtif", "image/x-quicktime"},
            {"ra", "audio/x-pn-realaudio"},
            {"ram", "audio/x-pn-realaudio"},
            {"ras", "image/x-cmu-raster"},
            {"rdf", "application/rdf+xml"},
            {"rgb", "image/x-rgb"},
            {"rm", "application/vnd.rn-realmedia"},
            {"roff", "application/x-troff"},
            {"rtf", "text/rtf"},
            {"rtx", "text/richtext"},
            {"sgm", "text/sgml"},
            {"sgml", "text/sgml"},
            {"sh", "application/x-sh"},
            {"shar", "application/x-shar"},
            {"silo", "model/mesh"},
            {"sit", "application/x-stuffit"},
            {"skd", "application/x-koan"},
            {"skm", "application/x-koan"},
            {"skp", "application/x-koan"},
            {"skt", "application/x-koan"},
            {"smi", "application/smil"},
            {"smil", "application/smil"},
            {"snd", "audio/basic"},
            {"so", "application/octet-stream"},
            {"spl", "application/x-futuresplash"},
            {"src", "application/x-wais-source"},
            {"sv4cpio", "application/x-sv4cpio"},
            {"sv4crc", "application/x-sv4crc"},
            {"svg", "image/svg+xml"},
            {"swf", "application/x-shockwave-flash"},
            {"t", "application/x-troff"},
            {"tar", "application/x-tar"},
            {"tcl", "application/x-tcl"},
            {"tex", "application/x-tex"},
            {"texi", "application/x-texinfo"},
            {"texinfo", "application/x-texinfo"},
            {"tif", "image/tiff"},
            {"tiff", "image/tiff"},
            {"tr", "application/x-troff"},
            {"tsv", "text/tab-separated-values"},
            {"txt", "text/plain"},
            {"ustar", "application/x-ustar"},
            {"vcd", "application/x-cdlink"},
            {"vrml", "model/vrml"},
            {"vxml", "application/voicexml+xml"},
            {"wav", "audio/x-wav"},
            {"wbmp", "image/vnd.wap.wbmp"},
            {"wbmxl", "application/vnd.wap.wbxml"},
            {"wml", "text/vnd.wap.wml"},
            {"wmlc", "application/vnd.wap.wmlc"},
            {"wmls", "text/vnd.wap.wmlscript"},
            {"wmlsc", "application/vnd.wap.wmlscriptc"},
            {"wrl", "model/vrml"},
            {"xbm", "image/x-xbitmap"},
            {"xht", "application/xhtml+xml"},
            {"xhtml", "application/xhtml+xml"},
            {"xls", "application/vnd.ms-excel"},
            {"xml", "application/xml"},
            {"xpm", "image/x-xpixmap"},
            {"xsl", "application/xml"},
            {"xlsx","application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
            {"xltx","application/vnd.openxmlformats-officedocument.spreadsheetml.template"},
            {"xlsm","application/vnd.ms-excel.sheet.macroEnabled.12"},
            {"xltm","application/vnd.ms-excel.template.macroEnabled.12"},
            {"xlam","application/vnd.ms-excel.addin.macroEnabled.12"},
            {"xlsb","application/vnd.ms-excel.sheet.binary.macroEnabled.12"},
            {"xslt", "application/xslt+xml"},
            {"xul", "application/vnd.mozilla.xul+xml"},
            {"xwd", "image/x-xwindowdump"},
            {"xyz", "chemical/x-xyz"},
            {"zip", "application/zip"}
        };

        public static DataTable ReadExcelFileForBPLOPayment(string fileName)
        {
            // If using Professional version, put your serial key below.
            //SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");

            ExcelFile ef = ExcelFile.Load(fileName);

            DataTable dataTable = new DataTable();

            // Depending on the format of the input file, you need to change this:
            dataTable.Columns.Add("Business ID Number", typeof(string));
            dataTable.Columns.Add("Business Name", typeof(string));
            dataTable.Columns.Add("Trade Name", typeof(string));
            dataTable.Columns.Add("Business Address", typeof(string));
            dataTable.Columns.Add("Tax Year", typeof(string));
            dataTable.Columns.Add("Payment Amount", typeof(string));
            dataTable.Columns.Add("Payment Date", typeof(string));
            dataTable.Columns.Add("Remarks", typeof(string));

            // Select the first worksheet from the file.
            ExcelWorksheet ws = ef.Worksheets[0];

            ExtractToDataTableOptions options = new ExtractToDataTableOptions(0, 0, 1048576);
            options.ExtractDataOptions = ExtractDataOptions.StopAtFirstEmptyRow;
            options.ExcelCellToDataTableCellConverting += (sender, e) =>
            {
                if (!e.IsDataTableValueValid)
                {
                    // GemBox.Spreadsheet doesn't automatically convert numbers to strings in ExtractToDataTable() method because of culture issues; 
                    // someone would expect the number 12.4 as "12.4" and someone else as "12,4".
                    e.DataTableValue = e.ExcelCellValue == null ? null : e.ExcelCellValue.ToString();
                    e.Action = ExtractDataEventAction.Continue;
                }
            };

            // Extract the data from an Excel worksheet to the DataTable.
            // Data is extracted starting at first row and first column for 10 rows or until the first empty row appears.
            ws.ExtractToDataTable(dataTable, options);

            //Delete the first row (Column Names)
            if (dataTable.Rows[0].ItemArray[0].ToString().ToLower().Contains("business"))
            {
                dataTable.Rows[0].Delete();
                dataTable.AcceptChanges();
            }
            else
            {
                throw new Exception("Incorrect template. Please download the template and try again.");
            }

            return dataTable;
        }

        public static DataTable ReadExcelFileForAlphaList(string path)
        {
            // If using Professional version, put your serial key below.
            //SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");

            ExcelFile ef = ExcelFile.Load(path);
            DataTable dataTable = new DataTable();

            ExcelWorksheet ws = ef.Worksheets[0];

            dataTable = ws.CreateDataTable(new CreateDataTableOptions()
            {
                ColumnHeaders = true,
                StartRow = 0,
                NumberOfColumns = ws.CalculateMaxUsedColumns(),
                NumberOfRows = ws.Rows.Count,
                Resolution = ColumnTypeResolution.AutoPreferStringCurrentCulture
            });

            for (int i = dataTable.Rows.Count - 1; i >= 0; i += -1)
            {
                DataRow row = dataTable.Rows[i];
                if (row[0] == null)
                {
                    dataTable.Rows.Remove(row);
                }
                else if (string.IsNullOrEmpty(row[0].ToString()))
                {
                    dataTable.Rows.Remove(row);
                }
            }

            if (!dataTable.Columns[0].ToString().ToLower().Contains("account number"))
            {
                File.Delete(path);
                throw new Exception("Incorrect template. Please download the template and try again.");

            }
            
            return dataTable;
        }
    }
}