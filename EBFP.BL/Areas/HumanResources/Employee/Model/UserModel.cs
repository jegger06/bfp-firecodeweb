
namespace EBFP.BL.HumanResources
{
    using System.ComponentModel.DataAnnotations;
    public class UserModel
    { 
        public string Password { get; set; } 
        [MinLength(6, ErrorMessage = "Password be between 5 and 10 characters")]
        public string NewPassword { get; set; } 
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; } 
        public string OldPassword { get; set; }
        public string sEmp_Id { get; set; }
        public string ConfirmationKey { get; set; }
        public string Message { get; set; }
    }
}
