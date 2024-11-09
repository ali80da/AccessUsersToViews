using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AccessTo.Web.Main.DataMo.ManageRole
{
    public class RoleDataMo
    {

        [Display(Name = "نام مقام")]
        [Required(ErrorMessage = "لطفـا {0} خودتــو وارد کــن")]
        [MaxLength(50, ErrorMessage = "{0} نمی توانــه بیشتــر از {1} کاراکتــر باشــه .")]
        public required string RoleTitle { get; set; }

    }






    
}