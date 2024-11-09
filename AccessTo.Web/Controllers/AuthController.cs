using AccessTo.Web.Main.DataMo.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis;

namespace AccessTo.Web.Controllers
{
    public class AuthController : MainController
    {

        #region Constructor

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AuthController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #endregion


        #region SIGN UP

        [HttpGet("sign-up")]
        public IActionResult SignUP()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");

            }
            return View();
        }

        [HttpPost("sign-up"), ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUP(SignupDataMo model)
        {
            if (ModelState.IsValid)
            {

                var user = new IdentityUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    EmailConfirmed = true
                };
                var res = await _userManager.CreateAsync(user, model.Password);

                if (res.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                foreach (var err in res.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }

            return View(model);
        }


        #endregion

        #region SIGN IN

        [HttpGet("sign-in")]
        public IActionResult SignIN(string returnTo = "/")
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");

            }
            ViewData["returnTo"] = returnTo;
            return View();
        }

        [HttpPost("sign-in"), ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIN(SigninDataMo mo, string returnTo = "/")
        {

            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");

            }

            // ReturnTO
            ViewData["returnTo"] = returnTo;
            
            if (ModelState.IsValid)
            {
                var res = await _signInManager.PasswordSignInAsync(mo.UserName, mo.Password, mo.RememberMe, true);
                if (res.Succeeded)
                {
                    // returnTo
                    if (returnTo != "/" && Url.IsLocalUrl(returnTo))
                        return Redirect(returnTo);


                    return RedirectToAction("Index", "Home");
                }
                if (res.IsLockedOut)
                {
                    ViewData["ErrorMessage"] = "Acc is Loucked";
                    return View(mo);
                }
                ModelState.AddModelError("", "Wrong");
            }

            return View(mo);
        }

        #endregion

        #region SIGN OUT

        [HttpGet]
        public async Task<IActionResult> SignOUT()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("SignIN", "Auth");
        }

        #endregion

    }
}