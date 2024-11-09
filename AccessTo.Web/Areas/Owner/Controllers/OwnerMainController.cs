using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace AccessTo.Web.Areas.Owner.Controllers
{
    [Authorize]
    [Area("owner")]
    public class OwnerMainController : Controller { }
}