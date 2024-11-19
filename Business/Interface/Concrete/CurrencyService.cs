using Business.Interface.Abstract;
using Entitiy.Concrate;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Xml;
using DataAccess;
using System.Xml.Linq;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using System.Data.Entity;
using Serilog;
using System.Runtime.InteropServices.ComTypes;

namespace Business
{
    public class CurrencyService : ICurrencyService
    {
        public CurrencyService() { }

        DailyCurrencyContext db = new DailyCurrencyContext();
        public List<DailyCurrency> GetCurrencyFromDatabase()
        {
            var lastDate = db.DailyCurrencies.OrderByDescending(o => o.Id).FirstOrDefault().Date;
            List<DailyCurrency> dbData = db.DailyCurrencies
                    .Where(x => x.Date.Year == lastDate.Year
                                         && x.Date.Month == lastDate.Month && x.Date.Day == lastDate.Day && x.Date.Hour == lastDate.Hour && x.Date.Minute == lastDate.Minute).ToList();
            return dbData;
        }

        public async Task<List<DailyCurrency>> GetCurrencies()
        {
            var url = "https://www.tcmb.gov.tr/kurlar/today.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(url);
            XmlNodeList currencyElements = xmlDoc.SelectNodes("Tarih_Date/Currency");
            List<DailyCurrency> currencies = new List<DailyCurrency>();
            try
            {
                foreach (XmlNode currencyElement in currencyElements)
                {
                    var currency = new DailyCurrency
                    {
                        Date = DateTime.Now,
                        CurrencyCode = currencyElement.Attributes["CurrencyCode"].Value.Trim(),
                        Unit = Convert.ToInt32(currencyElement.SelectSingleNode("Unit")?.InnerXml),
                        CurrencyName = currencyElement.SelectSingleNode("CurrencyName")?.InnerXml.Trim(),
                        ForexBuying = ConvertXmlStringToDecimal(currencyElement.SelectSingleNode("ForexBuying")?.InnerXml, CultureInfo.InvariantCulture),
                        ForexSelling = ConvertXmlStringToDecimal(currencyElement.SelectSingleNode("ForexSelling")?.InnerXml, CultureInfo.InvariantCulture),
                        BanknoteBuying = ConvertXmlStringToDecimal(currencyElement.SelectSingleNode("BanknoteBuying")?.InnerXml, CultureInfo.InvariantCulture),
                        BanknoteSelling = ConvertXmlStringToDecimal(currencyElement.SelectSingleNode("BanknoteSelling")?.InnerXml, CultureInfo.InvariantCulture),
                        CrossRateUSD = ConvertXmlStringToDecimal(currencyElement.SelectSingleNode("CrossRateUSD")?.InnerXml, CultureInfo.InvariantCulture),
                        CrossRateOther = ConvertXmlStringToDecimal(currencyElement.SelectSingleNode("CrossRateOther")?.InnerXml, CultureInfo.InvariantCulture)
                    };
                    currencies.Add(currency);
                }
                db.DailyCurrencies.AddRange(currencies);
                await db.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                    }
                }

                // Hatanın oluştuğu mesajı loglama
                LogStatus("Veritabanı işlemi sırasında bir hata oluştu.");
            }

            return currencies;
        }

        public async Task<List<DailyCurrency>> CalculateCurrencyRate()
        {
            var dbCurrencyData = GetCurrencyFromDatabase();
            var xmlData = await GetCurrencies();
            var resultData = new List<DailyCurrency>();
            foreach (var newCurrnecy in xmlData)
            {
                DailyCurrency daily = new DailyCurrency();
                var lastCurrenyData = dbCurrencyData.Where(x => x.CurrencyCode == newCurrnecy.CurrencyCode).OrderByDescending(p => p.Id).FirstOrDefault();
                if (lastCurrenyData != null)
                {
                    daily.FBChangeRate = CaclRate(newCurrnecy.ForexBuying, lastCurrenyData.ForexBuying);
                    daily.FSChangeRate = CaclRate(newCurrnecy.ForexSelling, lastCurrenyData.ForexSelling);
                    daily.BBChangeRate = CaclRate(newCurrnecy.BanknoteBuying, lastCurrenyData.BanknoteBuying);
                    daily.BSChangeRate = CaclRate(newCurrnecy.BanknoteSelling, lastCurrenyData.BanknoteSelling);
                    daily.CRUsdChangeRate = CaclRate(newCurrnecy.CrossRateUSD, lastCurrenyData.CrossRateUSD);
                    daily.CROChangeRate = CaclRate(newCurrnecy.CrossRateOther, lastCurrenyData.CrossRateOther);
                    daily.CurrencyCode = newCurrnecy.CurrencyCode;
                    daily.CurrencyName = newCurrnecy.CurrencyName;
                    daily.Date = newCurrnecy.Date;
                    daily.Unit = newCurrnecy.Unit;
                    daily.Id = newCurrnecy.Id;
                    daily.ForexBuying = newCurrnecy.ForexBuying;
                    daily.ForexSelling = newCurrnecy.ForexSelling;
                    daily.BanknoteBuying = newCurrnecy.BanknoteBuying;
                    daily.BanknoteSelling = newCurrnecy.BanknoteSelling;
                    daily.CrossRateUSD = newCurrnecy.CrossRateUSD;
                    daily.CrossRateOther = newCurrnecy.CrossRateOther;

                    resultData.Add(daily);
                }
            }
            return resultData;
        }

        public async Task<List<DailyCurrency>> FilterCurrenciesByCurrencyCodeData(FilterData model)
        {
            var xmlData = await GetCurrencies();
            var result = new List<DailyCurrency>();

            var query = db.DailyCurrencies;
            var list = query.ToList();

            if (model.StartDate.HasValue)
            {
                list = list.Where(x => x.Date >= model.StartDate.Value).ToList();
            }
            if (model.EndDate.HasValue)
            {
                list = list.Where(x => x.Date <= model.EndDate.Value).ToList();
            }
            if (!string.IsNullOrEmpty(model.CodeCurrency))
            {
                list = list.Where(x => x.CurrencyCode == model.CodeCurrency).ToList();
            }

            foreach (var item in list)
            {
                var newCurrnecy = xmlData.FirstOrDefault(x => x.CurrencyCode == item.CurrencyCode);
                result.Add(new DailyCurrency()
                {
                    Date = item.Date,
                    Unit = item.Unit,
                    CurrencyCode = item.CurrencyCode,
                    CurrencyName = item.CurrencyName,
                    ForexBuying = item.ForexBuying,
                    ForexSelling = item.ForexSelling,
                    BanknoteBuying = item.BanknoteBuying,
                    BanknoteSelling = item.BanknoteSelling,
                    CrossRateUSD = item.CrossRateUSD,
                    CrossRateOther = item.CrossRateOther,
                    FBChangeRate = CaclRate(newCurrnecy.ForexBuying, item.ForexBuying),
                    FSChangeRate = CaclRate(newCurrnecy.ForexSelling, item.ForexSelling),
                    BBChangeRate = CaclRate(newCurrnecy.BanknoteBuying, item.BanknoteBuying),
                    BSChangeRate = CaclRate(newCurrnecy.BanknoteSelling, item.BanknoteSelling),
                    CRUsdChangeRate = CaclRate(newCurrnecy.CrossRateUSD, item.CrossRateUSD),
                    CROChangeRate = CaclRate(newCurrnecy.CrossRateOther, item.CrossRateOther),
            });
            }
            return result;

        }
        public decimal ConvertXmlStringToDecimal(string xmlString, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(xmlString))
                return 0;
            decimal result;
            if (!decimal.TryParse(xmlString, NumberStyles.Any, cultureInfo, out result))
            {
                return 0;
            }
            return result;
        }

        public decimal CaclRate(decimal? firstCurrency, decimal? secondCurrency)
        {
            decimal result = 0;
            if (firstCurrency.HasValue && secondCurrency.HasValue && firstCurrency.Value != 0 && secondCurrency.Value != 0)
            {
                result = Convert.ToDecimal(((firstCurrency.Value - secondCurrency.Value) / secondCurrency.Value) * 100);
            }
            return result;
        }
        public void LogStatus(string message)
        {
            using (StreamWriter writer = new StreamWriter("log.txt", true))
            {
                writer.WriteLine($"message: {message} - Tarih: {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}");
            }
        }
    }
}
