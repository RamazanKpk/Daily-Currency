using Business.Interface.Abstract;
using DataAccess;
using Entitiy.Concrate;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Runtime.Caching;
using System.Text;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        DailyCurrencyContext db = new DailyCurrencyContext();
        private readonly IUserService _userService;
        public HomeController(IUserService userService)
        {
            _userService = userService;
        }
        public HomeController() { }
        
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult login()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult login(User user)
        //{


        //    //İlk login klasörün, İkinci Login sınıfının adıdır
        //    var logUser = _userService.UserLog(user);
        //    Login.Login.aktifKul =logUser ;
        //    if (Login.Login.aktifVarmi)
        //    {
        //        if (logUser.NormalExchangeRatesAuthorization == true)
        //        {
        //            return RedirectToAction("GetCurrency", "Currency");
        //        }
        //        else if (logUser.CrossExchangeRatesAuthorization == true)
        //        {
        //            return RedirectToAction("GetCrossCurrency", "Currency");
        //        }            
        //    }
        //    else
        //    {
        //        ViewBag.Mesaj = "Kullanıcı Bulunamadı";
        //        return View();
        //    }
        //    return RedirectToAction("login", "Home");
        //}
        public ActionResult logout()
        {
            Login.Login.cikisYap();
            return RedirectToAction("login", "Home");
        }


    }
}