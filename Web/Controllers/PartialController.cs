using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class PartialController : Controller
    {
        // GET: Partial
        public PartialViewResult Menu()
        {
            DailyCurrencyContext db = new DailyCurrencyContext();
            //string userName = Login.Login.aktifKul.UserName;
            //ViewBag.User = userName;
            //ViewBag.Authorization = db.Users.FirstOrDefault(x => x.UserName == userName && 
            //x.NormalExchangeRatesAuthorization == true && x.CrossExchangeRatesAuthorization == true);
            return PartialView("_Menu");
        }
    }
}