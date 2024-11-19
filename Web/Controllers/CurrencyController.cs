using Business.Interface.Abstract;
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
    public class CurrencyController : Controller
    {
        private readonly IUserService _userService;
        
        public CurrencyController(IUserService userService)
        {
            _userService = userService;
        }
        public CurrencyController() { }
        [HttpGet]
        public ActionResult GetCurrency()
        {
            //bool DayliyCurr = _userService.CheckNormalCurrencyAuthorization(Login.Login.aktifKul);
            //if (!DayliyCurr)
            //{
            //    TempData["ErrorMessage"] = "Yetkisiz giriş!";
            //    return RedirectToAction("Index", "Home");
            //}
            return View();
        }

        [HttpGet]
        public ActionResult GetCrossCurrency()
        {
            bool CrossCurr = _userService.CheckCrossCurrencyAuthorization(Login.Login.aktifKul);
            if (!CrossCurr)
            {
                return RedirectToAction("Index", "Home");
            }
                return View();
        }

        [HttpGet]
        public JsonResult GetDailyCurrency()
        {
            //string cacheKey = "CacheDailyCurrencyData_" + DateTime.Now.ToString("yyyyyMMddHH");


            string apiUrl = ConfigurationManager.AppSettings["CurrencyServiceEndPoint"].ToString();
            var cachedData = MemoryCache.Default.Get("CacheDailyCurrencyData") as List<DailyCurrency>;

            if (cachedData == null)
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync(apiUrl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonData = response.Content.ReadAsStringAsync().Result;
                        cachedData = JsonConvert.DeserializeObject<List<DailyCurrency>>(jsonData);
                        MemoryCache.Default.Set("CacheDailyCurrencyData", cachedData, DateTimeOffset.Now.AddHours(2));
                    }
                }
            }
            return Json(cachedData, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetFilterDailyCurrency(FilterData filterData)
        {
            string apiUrl = ConfigurationManager.AppSettings["FilterCurrecyServiceEndPoint"];
            var jsonContent = JsonConvert.SerializeObject(filterData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var filterDataList = new List<DailyCurrency>();

            using (HttpClient client = new HttpClient())
            {

                HttpResponseMessage reponse = client.PostAsync(apiUrl, content).Result;
                if (reponse.IsSuccessStatusCode)
                {
                    string jsonData = reponse.Content.ReadAsStringAsync().Result;
                    filterDataList = JsonConvert.DeserializeObject<List<DailyCurrency>>(jsonData);
                }
            }
            return Json(filterDataList, JsonRequestBehavior.AllowGet);
        }
        
    }
}