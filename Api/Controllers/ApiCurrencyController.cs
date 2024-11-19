using Business.Interface.Abstract;
using Entitiy.Concrate;
using System.Threading.Tasks;
using System.Web.Http;


namespace Api.Controllers
{
    public class ApiCurrencyController : ApiController
    {
        private readonly ICurrencyService _currencyService;

        public ApiCurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;

        }
        [HttpGet]
        [Route("api/apicurrency/DailyCurrency")]
        public async Task<IHttpActionResult> GetCurrencyData()
        {
                var result = await _currencyService.CalculateCurrencyRate();
 
            return Json(result);
        }
        [HttpPost]
        [Route("api/apicurrency/FilterCurrencyByCurrencyCodeData")]
        public async Task<IHttpActionResult> FilterCurrencyByCurrencyCodeData([FromBody] FilterData filterData )
        {
            var resultFilter = await _currencyService.FilterCurrenciesByCurrencyCodeData(filterData);
            return Json(resultFilter);
        }

    }
}
