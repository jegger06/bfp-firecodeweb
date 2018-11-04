
namespace EBFP.BL.Administration
{
    using System;

    public partial class MembershipModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public DateTime? CreateDate { get; set; }

        public string ConfirmationToken { get; set; }

        public bool? IsConfirmed { get; set; }

        public DateTime? LastPasswordFailureDate { get; set; }

        public int PasswordFailuresSinceLastSuccess { get; set; }

        public string Password { get; set; }

        public DateTime? PasswordChangedDate { get; set; }

        public string PasswordSalt { get; set; }

        public string PasswordVerificationToken { get; set; }

        public DateTime? PasswordVerificationTokenExpirationDate { get; set; }

        public string PasswordDecrypted { get; set; }
    }

}
