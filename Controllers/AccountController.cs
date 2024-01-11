using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoreCnice.Connect;
using CoreCnice.Domain;
using CoreCnice.UtilsCs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BulSoftCMS.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();
        // GET: /<controller>/
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn([Bind("USER_UN,USER_PW")] ESHOP_USER eSHOP_USER)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (String.IsNullOrEmpty(eSHOP_USER.USER_UN))
                    {
                        ViewBag.Error = "Vui lòng nhập tài khoản";
                        return View();
                    }
                    else if (String.IsNullOrEmpty(eSHOP_USER.USER_PW))
                    {
                        ViewBag.Error = "Vui lòng nhập mật khẩu";
                        return View();
                    }
                    else
                    {
                        var LogResult = await _context.ESHOP_USER.SingleOrDefaultAsync(m => m.USER_UN == eSHOP_USER.USER_UN && m.USER_PW == eSHOP_USER.USER_PW);
                        if (LogResult == null)
                        {

                            ViewBag.Error = "Tài khoản hoặc mật khẩu không tồn tại";
                            return View();
                        }
                        else
                        {
                            int groupid = Utils.CIntDef(LogResult.GROUP_ID);
                            var logGroup = await _context.ESHOP_GROUP.SingleOrDefaultAsync(m => m.GROUP_ID == groupid);
                            List<Claim> claims = new List<Claim>
    {
      new Claim(ClaimTypes.Name, LogResult.USER_ID.ToString()),
        new Claim(ClaimTypes.Email, LogResult.GROUP_ID.ToString()),
         new Claim(ClaimTypes.GroupSid, logGroup.GROUP_TYPE.ToString())
    };
                            // create identity
                            ClaimsIdentity identity = new ClaimsIdentity(claims, "cookie");

                            // create principal
                            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                            await HttpContext.SignInAsync(
               scheme: "cookie",
               principal: principal,
               properties: new AuthenticationProperties
               {
                   IsPersistent = true, // for 'remember me' feature
                   ExpiresUtc = DateTime.UtcNow.AddMinutes(100)
               });
                            return Redirect("/Admin/Dashboard");
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {

                }
            }
            return View(eSHOP_USER);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Logout(string requestPath)
        {
            await HttpContext.SignOutAsync(
                    scheme: "cookie");

            return RedirectToAction("/quantriweb");
        }
    }
}
