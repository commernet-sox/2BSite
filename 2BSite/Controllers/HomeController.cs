using _2BSite.Models;
using _2BSite.Service.DTO.Identity;
using _2BSite.Service.Interface.Identity;
using _2BSite.Service.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace _2BSite.Controllers
{
    public class HomeController : BaseController
    {
        //Logger _logger = (Logger)LogManager.GetCurrentClassLogger(typeof(Logger));
        
        private IUserService _IUserService;
        public HomeController(IUserService userService)
        { 
            
            _IUserService = userService;
        }

        public IActionResult Index()
        {
            //_logger.Trace("测试日志。。。");
            return View();
        }

        public IActionResult Privacy()
        {
            //Console.WriteLine(_JwtSettings.Issuer);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Test(string Province)
        {
            var res = new TestModel() { Data=Province};
            return PartialView("/Views/Home/Part_1.cshtml",res);
            //return View("Index",res);
            //return Json("success");
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginViewModel model,string returnUrl=null)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _IUserService.ValidateUser(model.UserName, model.Password);
                    if (user != null)
                    {
                        //用户标识
                        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                        //identity.AddClaim(new Claim("CompanyId", user.CompanyId.HasValue ? user.CompanyId.Value.ToString() : ""));
                        //identity.AddClaim(new Claim("CompanyName", user.IsAdmin ? "管理员" : (user.Company != null ? user.Company : "")));
                        identity.AddClaim(new Claim(ClaimTypes.Sid, model.UserName));
                        identity.AddClaim(new Claim(ClaimTypes.Name, user.AliasName));

                        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties() { IsPersistent = model.RememberMe });

                        var user_permissions = this._IUserService.GetUserPermission(model.UserName);

                        HttpContext.Session.SetString("User", Newtonsoft.Json.JsonConvert.SerializeObject(user));
                        HttpContext.Session.SetString("UserPermissions", Newtonsoft.Json.JsonConvert.SerializeObject(user_permissions));

                        MenuModel menuModel = new MenuModel();

                        menuModel.User = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDTO>(HttpContext.Session.GetString("User"));

                        menuModel.UserPermissions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserPermission>>(HttpContext.Session.GetString("UserPermissions"));
                        //_logger.Debug("登录成功");
                        if (returnUrl == null)
                        {
                            returnUrl = TempData["returnUrl"]?.ToString();
                        }
                        if (returnUrl != null)
                        {
                            if (returnUrl.ToLower() == "/home/index")
                            {
                                return RedirectToAction(nameof(HomeController.Index), "Home");
                            }
                            else
                            {
                                return Redirect(returnUrl);
                            }
                        }
                        else
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "帐号密码错误、被禁用或者不存在");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("", "参数错误");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("User");
            HttpContext.Session.Remove("UserPermissions");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Home");
        }
        public IActionResult GetUserInfo()
        {
            var data = _IUserService.GetAll().ToList();
            return Json(data);
        }
        public IActionResult Denied()
        {
            return View();
        }
    }
    public class TestModel
    {
        public string Data { get; set; }
    }
}
