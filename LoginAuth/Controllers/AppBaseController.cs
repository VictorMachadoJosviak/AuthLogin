using LoginAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoginAuth.Controllers
{
    public class AppBaseController : Controller
    {
        protected DataContext db = new DataContext();
        public AppUser CurrentUser
        {
            get
            {
                var usr = System.Web.HttpContext.Current.User.Identity.Name;
                return db.AppUsers.FirstOrDefault(o => o.Email.Equals(usr));
            }
        }
    }
}