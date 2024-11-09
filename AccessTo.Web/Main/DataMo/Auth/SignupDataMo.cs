using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AccessTo.Web.Main.DataMo.Auth
{
    public class SignupDataMo
    {

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفـا {0} خودتــو وارد کــن")]
        [MaxLength(50, ErrorMessage = "{0} نمی توانــه بیشتــر از {1} کاراکتــر باشــه .")]
        //[Remote("IsUserNameInUse", "Auth", HttpMethod = "POST", AdditionalFields = "__RequestVerificationToken")]
        public required string UserName { get; set; }

        [EmailAddress(ErrorMessage = "فرمت ایمیل وارد شده معتبر نیست")]
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفـا {0} خودتــو وارد کــن")]
        [MaxLength(50, ErrorMessage = "{0} نمی توانــه بیشتــر از {1} کاراکتــر باشــه .")]
        //[Remote("IsEmailInUse", "Auth", HttpMethod = "POST", AdditionalFields = "__RequestVerificationToken")]
        public required string Email { get; set; }

        [Display(Name = "رمزعبور")]
        [Required(ErrorMessage = "لطفـا {0} خودتــو وارد کــن")]
        [MaxLength(200, ErrorMessage = "{0} نمی توانــه بیشتــر از {1} کاراکتــر باشــه .")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Display(Name = "تکرار رمزعبور")]
        [Required(ErrorMessage = "لطفـا {0} خودتــو وارد کــن")]
        [MaxLength(200, ErrorMessage = "{0} نمی توانــه بیشتــر از {1} کاراکتــر باشــه .")]
        [Compare(nameof(Password), ErrorMessage = "کلمـه هــای عبـور باهــم تفــاوت دارنـد...")]
        [DataType(DataType.Password)]
        public required string ConfirmPassword { get; set; }

    }
}