using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AccessTo.Web.Main.DataMo.Auth
{

    public class SigninDataMo
    {
        [Display(Name = "نـام کاربــری")]
        [Required(ErrorMessage = "لطفـا {0} را وارد کنیــد")]
        [MaxLength(150, ErrorMessage = "{0} نمی توانــد بیشتــر از {1} کاراکتــر باشــد .")]
        public required string UserName { get; set; }

        [Display(Name = "کلمـه عبـور")]
        [Required(ErrorMessage = "لطفـا {0} را وارد کنیــد")]
        [MaxLength(200, ErrorMessage = "{0} نمی توانــد بیشتــر از {1} کاراکتــر باشــد .")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Display(Name = "مـرا به خاطـر بسپــار")]
        public bool RememberMe { get; set; }

    }

}