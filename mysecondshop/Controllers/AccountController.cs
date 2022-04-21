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
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> singInManager;
        public AccountController(UserManager<ApplicationUser> usrMngr, SignInManager<ApplicationUser> singMng)
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
                ApplicationUser user = await userManager.FindByNameAsync(loginModel.Phone.Replace("+", "").Replace("(","").Replace(")","").Replace("-",""));
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
                    var user = new ApplicationUser
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
        public async Task<IActionResult> Cabinet(){
            var CurrentUser = await userManager.GetUserAsync(User);    
            var user = new UserDataViewModel()
            {
                User=new ApplicationUser()
                {
                    FirstName = CurrentUser.FirstName,
                    LastName = CurrentUser.LastName,
                    PatronymicName= CurrentUser.PatronymicName,
                    AdditionalPhone=CurrentUser.AdditionalPhone,
                },
                UserPhoto=CurrentUser.UserPhoto,
                Email = CurrentUser.Email,
                PhoneNumber = CurrentUser.PhoneNumber,
            };
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Cabinet(UserDataViewModel usermodel)
        {
            if (ModelState.IsValid)
            {

                var result = await userManager.FindByNameAsync(User.Identity.Name);/*usermodel.PhoneNumber.Replace("+", "").Replace("(", "").Replace(")", "").Replace("-", "")*/

                if (usermodel.Avatar != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        usermodel.Avatar.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        result.UserPhoto = fileBytes;
                        usermodel.UserPhoto = fileBytes;
                    }
                }

                if (!string.IsNullOrWhiteSpace(usermodel.User.FirstName) && usermodel.User.FirstName !=result.FirstName)
                {
                   result.FirstName = usermodel.User.FirstName;
                }
                else
                    ModelState.AddModelError("", "Поле имя не может быть пустым");
                if (!string.IsNullOrWhiteSpace(usermodel.User.LastName) && usermodel.User.LastName != result.LastName)
                {
                    result.LastName = usermodel.User.LastName;
                }
                else
                    ModelState.AddModelError("", "Поле фамилия не может быть пустым");
                if (!string.IsNullOrWhiteSpace(usermodel.User.PatronymicName) && usermodel.User.PatronymicName != result.PatronymicName)
                {
                    result.PatronymicName = usermodel.User.PatronymicName;
                }
                else
                    ModelState.AddModelError("", "E-mail не может быть пустым");
                if (!string.IsNullOrWhiteSpace(usermodel.Email) && usermodel.Email != result.Email)
                {
                    result.Email = usermodel.Email;
                }
                if (!string.IsNullOrWhiteSpace(usermodel.User.AdditionalPhone) && usermodel.User.AdditionalPhone != result.AdditionalPhone)
                {
                    result.AdditionalPhone = usermodel.User.AdditionalPhone;
                }
                await userManager.UpdateAsync(result);
            }
            
            return RedirectToAction("Cabinet");
        }
        public async Task<ActionResult> Logout(string returnUrl = "/")
        {
            await singInManager.SignOutAsync();
            return RedirectToAction(returnUrl);
        }
    }
}
