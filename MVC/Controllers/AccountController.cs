using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using RestWebAppl.Models.ViewModels;
using RestWebAppl.Models;

namespace RestWebAppl.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> singInManager;
        //repository with orders 
        private IOrderRepository orderRepository;
        public AccountController(IOrderRepository _ordRep,UserManager<ApplicationUser> _usrMngr, SignInManager<ApplicationUser> _singMng)
        {
            orderRepository = _ordRep;
            userManager = _usrMngr;
            singInManager = _singMng;
        }
        //Returns PartialView for login modal window 
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            return PartialView(new LoginModel());
        }
        //DELETE FOR PRODUCTION 
        //LOGIN AS MODERATOR ROLE 
        [AllowAnonymous]
        public async Task<IActionResult> LoginModer()
        {
            await singInManager.SignOutAsync();
            await singInManager.PasswordSignInAsync("380685494492", "123123123", false, false);
            return RedirectToAction("Index","Home");
        }
        //SIGN IN
        // Takes loginModel{Phone number, Password}
        [AllowAnonymous]
        [HttpPost]
        public async Task<JsonResult> Login([FromBody]LoginModel loginModel)
        {

            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByNameAsync(loginModel.Phone.Replace("+", "").Replace("(","").Replace(")","").Replace("-","").Replace("_",""));
                if (user != null)
                {
                    await singInManager.SignOutAsync();
                    if ((await singInManager.PasswordSignInAsync(user, loginModel.Password, false, false)).Succeeded)
                    {
                        return Json(new{success=true});
                    }
                }
            }
            ModelState.AddModelError("", "Непрвильный номер телефона или пароль, повторите ещё раз");
            return Json(new { success = false });
        }
        //Gets PartialView for registration modal
        [AllowAnonymous]
        public ActionResult RegistrationPartial()
        {
            return PartialView(new LoginModel());
        }
        //Post method to register user
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> RegistrationPartial([FromBody]LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                return await CreateUser(loginModel);
            }
            return Json(new { success = false });
        }
        //Gets view with user information
        public async Task<IActionResult> Cabinet() 
        {
            UserDataViewModel user = await ShowUserInfo();
           return View(user);
        } 
        [HttpPost]
        public async Task<IActionResult> Cabinet(UserDataViewModel usermodel)
        {
            if (ModelState.IsValid)
            {
                await ChangeUserData(usermodel);
                if (usermodel.OldPassword != null && usermodel.NewPassword != null)
                {
                    await ChangeUserPassword(usermodel.OldPassword, usermodel.NewPassword);
                }
                if (TempData["error"] == null)
                {
                    TempData["message"] = $"Данные успешно изменены";
                }
            }
            return RedirectToAction("Cabinet");
        }
        //LOGOUT 
        public async Task<ActionResult> Logout()
        {
            await singInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }

        //Gets current user and fill model with user info and returns it 
        private async Task<UserDataViewModel> ShowUserInfo()
        {
            var CurrentUser = await userManager.GetUserAsync(User);
            var user = new UserDataViewModel()
            {
                User = new ApplicationUser()
                {
                    FirstName = CurrentUser.FirstName,
                    LastName = CurrentUser.LastName,
                    PatronymicName = CurrentUser.PatronymicName,
                    AdditionalPhone = CurrentUser.AdditionalPhone,
                },
                Orders = orderRepository.Orders.Where(o => o.UserId == CurrentUser.UserName),
                UserPhoto = CurrentUser.UserPhoto,
                Email = CurrentUser.Email,
                PhoneNumber = CurrentUser.PhoneNumber,

            };
            return user;
        }
        // Changes current user information
        private async Task ChangeUserData(UserDataViewModel usermodel)
        {
            var result = await userManager.GetUserAsync(User);/*usermodel.PhoneNumber.Replace("+", "").Replace("(", "").Replace(")", "").Replace("-", "")*/

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
            if (!string.IsNullOrWhiteSpace(usermodel.User.FirstName) && usermodel.User.FirstName != result.FirstName)
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
        public async Task ChangeUserPassword(string OldPassword,string NewPassword)
        {
            var result = await userManager.GetUserAsync(User);
            var passwordVartification = userManager.PasswordHasher.VerifyHashedPassword(result, result.PasswordHash, OldPassword);
            var newPasswordVartification = userManager.PasswordHasher.VerifyHashedPassword(result, result.PasswordHash, NewPassword);
            if (newPasswordVartification == PasswordVerificationResult.Success)
            {
                TempData["error"] = $"Старый и новый пароли совпадают";
                return;
            }
            else if (passwordVartification == PasswordVerificationResult.Success)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(result);
                userManager.ResetPasswordAsync(result, token, NewPassword);
            }
            else
            {
                TempData["error"] = $"Не правильный пароль";
                ModelState.AddModelError("", "Не правильный пароль");
            }
        }
        private async Task<JsonResult> CreateUser(LoginModel loginModel)
        {
            if (userManager.Users.FirstOrDefault(u => u.PhoneNumber == loginModel.Phone) == null)
            {
                var user = new ApplicationUser
                {
                    UserName = loginModel.Phone.Replace("+", "").Replace("(", "").Replace(")", "").Replace("-", ""),
                    PhoneNumber = loginModel.Phone,
                };
                await userManager.CreateAsync(user, loginModel.Password);
                TempData["message"] = $"Аккаунт успешно зарегестрирован";
                return Json("serverlogin");
            }
            else
            {
                TempData["error"] = $"Аккаунт не зарегестрирован";
                return Json("serverlogin");
            }    
        }
    }
}
