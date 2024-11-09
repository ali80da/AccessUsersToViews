/*using Microsoft.AspNetCore.Cors.Infrastructure;*/
using Microsoft.AspNetCore.Mvc;

namespace AccessTo.Web.Controllers
{
    public class HomeController : MainController
    {

        #region Constructor 

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        #endregion

        /* صفحه اصلی */
        [HttpGet("/")]
        public IActionResult Index()
        {
            _logger.LogInformation("START, Access to Home Controller");
            return View();
        }









    }
}