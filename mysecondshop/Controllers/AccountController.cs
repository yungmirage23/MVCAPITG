using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using RestWebAppl.Models.ViewModels;
using RestWebAppl.Models;
using System.Threading.Tasks;
using System.Net;

namespace RestWebAppl.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> singInManager;
        public AccountController(UserManager<IdentityUser> usrMngr, SignInManager<IdentityUser> singMng)
        {
            userManager = usrMngr;
            singInManager = singMng;
        }
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            return PartialView(new LoginModel { ReturnUrl = returnUrl });
        }

        
        //[ValidateAntiForgeryToken]
        [AllowAnonymous]
        [HttpPost]
        public async Task<JsonResult> Login([FromBody]LoginModel loginModel)
        {

            if (ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByNameAsync(loginModel.Phone.Replace("+", "").Replace("(","").Replace(")","").Replace("-",""));
                if (user != null)
                {
                    await singInManager.SignOutAsync();
                    if ((await singInManager.PasswordSignInAsync(user, loginModel.Password, false, false)).Succeeded)
                    {
                        /*var host = "http://localhost:5000";
                        var path = loginModel.ReturnUrl;
                        path = String.Join(
                            "/",
                            path.Split("/").Select(s => System.Net.WebUtility.UrlEncode(s))
                        );*/
                        var serverlogin = new Response {returnUrl=loginModel.ReturnUrl, dateTime = DateTime.Now.ToLongTimeString(), status = true };
                        return Json(serverlogin);
                    }
                }
            }
            var serverfail = new Response { returnUrl=loginModel.ReturnUrl, dateTime = DateTime.Now.ToLongTimeString(), status = false };
            ModelState.AddModelError("", "Непрвильный номер телефона или пароль, повторите ещё раз");
            return Json(serverfail);
        }

        [AllowAnonymous]
        public ActionResult RegistrationPartial(string returnUrl)
        {
            return PartialView(new LoginModel { ReturnUrl = returnUrl });
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegistrationPartial([FromBody]LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                if (userManager.Users.FirstOrDefault(u => u.PhoneNumber == loginModel.Phone) == null)
                {
                    var user = new IdentityUser
                    {
                        UserName=loginModel.Phone.Replace("+","").Replace("(","").Replace(")","").Replace("-",""),
                        PhoneNumber =loginModel.Phone,
                    };
                    await userManager.CreateAsync(user,loginModel.Password);
                    var serverlogin = new Response { returnUrl=loginModel.ReturnUrl, dateTime = DateTime.Now.ToLongTimeString(), status = true };
                    return Json(serverlogin);
                } 
            }
            var serverfail = new Response { returnUrl = loginModel.ReturnUrl, dateTime = DateTime.Now.ToLongTimeString(), status = false };
            return Json(serverfail);
        }

        public IActionResult Cabinet()
        {
            return View();
        }


        public async Task<ActionResult> Logout(string returnUrl = "/")
        {
            await singInManager.SignOutAsync();
            return RedirectToAction(returnUrl);
        }
    }
}
