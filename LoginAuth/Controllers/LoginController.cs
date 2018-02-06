using LoginAuth.Helpers;
using LoginAuth.Models;
using Plugin.Security.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LoginAuth.Controllers
{
    public class LoginController : Controller
    {
        private DataContext db = new DataContext();

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");
        }
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Products");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Login(AppUser user, bool permanecerLogado)
        {
            try
            {
                var pwd = new PasswordEncoder();

                var hash = pwd.Encode(pwd.DefaultSalt[5] + user.Password, EncryptType.SHA_512);

                AppUser u = db.AppUsers.FirstOrDefault(o => o.Email.Equals(user.Email) &&
                o.Password.Equals(hash));

                if (u != null)
                {
                    FormsAuthentication.SetAuthCookie(u.Email, permanecerLogado);

                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    ModelState.AddModelError("", "Usuario ou senha inváidos");
                }

                return View();
            }
            catch (Exception ex)
            {
                ApplicationLog.Save(ex, ApplicationLog.ImpactLevel.High);
                return View();
            }
        }


        public async Task<ActionResult> CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateAccount(AppUser user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var pwd = new PasswordEncoder();

                    var hash = pwd.Encode(pwd.DefaultSalt[5] + user.Password, EncryptType.SHA_512);
                    user.Password = hash;

                    var role = db.UserRoles.FirstOrDefault(x => x.Id.ToString() == "822f4875-0aca-11e8-bbbb-7a79195ecca3");                    
                    user.UserRoleId = role.Id;

                    db.AppUsers.Add(user);
                    await db.SaveChangesAsync();

                    AppUser u = db.AppUsers.FirstOrDefault(o => o.Name.Equals(user.Name) &&
                    o.Password.Equals(hash));

                    if (u != null)
                    {
                        FormsAuthentication.SetAuthCookie(u.Email, true);
                        return RedirectToAction("Index", "Products");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Erro");
                    }

                    return RedirectToAction("Index", "Products");
                }

                return View(user);
            }
            catch (Exception ex)
            {
                ApplicationLog.Save(ex, ApplicationLog.ImpactLevel.High);
                return View();
            }
           
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}