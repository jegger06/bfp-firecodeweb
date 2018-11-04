using EBFP.BL.HumanResources;
using EBFP.Helper;
using EBFP.Web.Areas.Account.Models;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using EBFP.DataAccess;
using WebMatrix.WebData;
using System.Linq;
using System.Net;

namespace EBFP.Web.Areas.Account.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        IHRISUnitOfWork UnitOfWork { get; set; }
        // default constructor, establishes default service

        public AccountController() : this(new HRISUnitOfWork()) { }

        // overloaded 'injectable' constructor
        // ** Constructor Dependency Injection (DI).

        public AccountController(IHRISUnitOfWork employee) { this.UnitOfWork = employee; }
        // GET: Account/Account
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(User model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = UnitOfWork.Employee.GetEmployeeByUserName(model.Username);

                if (user != null)
                {
                   
                    if (WebSecurity.Login(model.Username, model.Password, false))
                    {

                        SetCustomAuthenticationCookie(model.Username, false);
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return Redirect("/Dashboard/Dashboard");

                            //return Redirect("/MainDashboard/index");
                        }
                    }
                }
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        public void SetCustomAuthenticationCookie(string userName, bool rememberMe, int? unitId = null, bool impersonate = false)
        {
            var user = UnitOfWork.Employee.GetEmployeeByUserName(userName);

            if (user != null)
            {
                if (unitId != null)
                {
                    user.Emp_Curr_Unit = unitId;
                }

                user.Impersonate = impersonate;

                var principalModel = new CustomPrincipalModel(user) {Username = userName};

                var serializer = new JavaScriptSerializer();

                string userData = serializer.Serialize(principalModel);

                var authCookie = FormsAuthentication.GetAuthCookie(userName, rememberMe);
                var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if (ticket != null)
                {
                    var newTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate,
                        ticket.Expiration, ticket.IsPersistent, userData);
                    authCookie.Value = FormsAuthentication.Encrypt(newTicket);
                }
                Response.Cookies.Add(authCookie);
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            WebSecurity.Logout();
            return Redirect("/MainDashboard/index");
        }
        public ActionResult MyProfile()
        {
            return View(new UserModel());
        }
    

        public ActionResult ChangePassword(string sEmp_Id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sEmp_Id))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                if (ModelState.IsValid)
                {
                    using (var BFPContext = new EBFPEntities())
                    {
                        var employeeId = Convert.ToInt32(sEmp_Id.Decrypt());
                        var detail = BFPContext.webpages_Membership.FirstOrDefault(a => a.UserId == employeeId);
                        if (detail != null)
                        {
                            var model = new UserModel() { Password = detail.PasswordDecrypted };
                            model.sEmp_Id = sEmp_Id;
                            return View(model);
                        }
                    }

                    ViewBag.PageStatus = PageStatus.Success.ToString();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
            }

            return View(new UserModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(UserModel model)
        { 
            try
            { 
                if (ModelState.IsValid)
                {
                    var employeeId = Convert.ToInt32(model.sEmp_Id.Decrypt());

                    if (string.IsNullOrWhiteSpace(model.NewPassword))
                        throw new Exception("Password is required");

                    using (var BFPContext = new EBFPEntities())
                    { 
                        var detail = BFPContext.tblEmployees.FirstOrDefault(a => a.Emp_Id == employeeId);
                        if (detail != null)
                        {
                            bool IsChangepasswordSuccess = WebSecurity.ChangePassword(detail.Emp_Username, model.Password, model.NewPassword);
                            IHRISUnitOfWork unitOfWork = new HRISUnitOfWork();
                            unitOfWork.User.UpdateDecryptedPassword(CurrentUser.EmployeeId, model.NewPassword);
                            model.Password = model.NewPassword;
                            if (!IsChangepasswordSuccess)
                            {
                                throw new Exception("Error changing your password. Please make sure that current password is correct.");
                            }

                            ViewBag.PageStatus = PageStatus.Success.ToString();
                        }
                    } 
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
        [AllowAnonymous]
        public ActionResult ForgotPassword(string username)
        {
            try
            {
                using (var bfpContext = new EBFPEntities())
                {
                    var detail = bfpContext.tblEmployees.SingleOrDefault(a => a.Emp_Username == username);
                    if (detail != null)
                    {
                        if (!string.IsNullOrEmpty(detail.Emp_EmailAddress))
                        {
                            var token = WebSecurity.GeneratePasswordResetToken(detail.Emp_Username);
                            var id = detail.Emp_Id.ToString().Encrypt();

                            IHRISUnitOfWork unitOfWork = new HRISUnitOfWork();
                            unitOfWork.Mail.SendConfirmDeviceEmail(detail.Emp_FirstName, detail.Emp_EmailAddress, token, id);
                        }
                        else
                        {
                            var jsonResult2 = Json(new { message = "No email address in this account." }, JsonRequestBehavior.AllowGet);
                            return jsonResult2;
                        }
                    }
                    else
                    {
                        var jsonResult3 = Json(new { message = "Invalid Username." }, JsonRequestBehavior.AllowGet);
                        return jsonResult3;
                    }
                }
            }
            catch (Exception ex)
            {
                var jsonResultError = Json(new
                {
                    message = ex.InnerException?.Message ?? ex.Message
                }, JsonRequestBehavior.AllowGet);
                return jsonResultError;
            }

            var jsonResult = Json(new { message = "Success" }, JsonRequestBehavior.AllowGet);
            return jsonResult;
           
        }

        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            var confirmationKey = Request.QueryString["confirmationKey"];
            var sEmpId = Request.QueryString["Id"];
            var retUnit = new UserModel();
            retUnit.ConfirmationKey = confirmationKey;
            retUnit.sEmp_Id = sEmpId;
            return View(retUnit);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(UserModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.NewPassword) || string.IsNullOrWhiteSpace(model.ConfirmNewPassword))
                    throw new Exception("All fields are required");

                if (model.NewPassword != model.ConfirmNewPassword)
                    throw new Exception("Confirm New Password' and 'NewPassword' do not match.");

                var employeeId = Convert.ToInt32(model.sEmp_Id.Decrypt());

                using (var BFPContext = new EBFPEntities())
                {
                    var detail = BFPContext.tblEmployees.FirstOrDefault(a => a.Emp_Id == employeeId);
                    if (detail != null)
                    {
                        var isChangepasswordSuccess = WebSecurity.ResetPassword(model.ConfirmationKey, model.NewPassword);

                        if (isChangepasswordSuccess)
                        {
                            IHRISUnitOfWork unitOfWork = new HRISUnitOfWork();
                            unitOfWork.User.UpdateDecryptedPassword(employeeId, model.NewPassword);
                        }
                        else
                        {
                            throw new Exception(
                                "Error changing your password. Please make sure that your request is not yet expired.");
                        }
                        model.Message = "Success";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
            }
           
            return View(model);
        }
    }
}