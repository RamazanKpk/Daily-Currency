using Entitiy.Concrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interface.Abstract
{
    public interface ICurrencyService
    {
        Task<List<DailyCurrency>> CalculateCurrencyRate();
        Task<List<DailyCurrency>> GetCurrencies();
        Task<List<DailyCurrency>> FilterCurrenciesByCurrencyCodeData(FilterData model);
    }
}
