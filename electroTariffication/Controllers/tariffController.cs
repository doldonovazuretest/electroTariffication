using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace electroTariffication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tariffController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;

        public tariffController(IMemoryCache memoryCache) 
        {
            _memoryCache = memoryCache;
        }


        [HttpGet(Name = "tarrifs")]
        public IEnumerable<tariff> Get()
        {
            try
            {
                return getCachedTariffs();
            }
            catch(Exception)
            {
                // more appropriate handle method would be suitable but outside of current scope
                return new List<tariff>();
            }
        }

        [HttpGet("calculate")]
        public IEnumerable<tariffCalculatedCost> calculate(int? consumption)
        {
            List<tariffCalculatedCost> result = new List<tariffCalculatedCost>();

            try
            {
                if (consumption != null && consumption is int)
                {
                    foreach (var _tarrif in getCachedTariffs()) result.Add(_tarrif.getCostData((int)consumption));
                }

                return result.OrderBy(x => x.cost);
            }
            catch (Exception)
            {
                // handle errors
            }

            return result;  
        }

        IEnumerable<tariff> getCachedTariffs()
        {
            if (!_memoryCache.TryGetValue("tariffCollection", out IEnumerable<tariff> tariffs))
            {
                tariffs = new List<tariff>
                    {
                        new tariff(5, 0.22m, 0, "basic electricity tariff", (_paramteres)
                        => { return _paramteres.Item2.baseRate * 12 + Convert.ToDecimal(_paramteres.Item1) * _paramteres.Item2.additionalFee; }),
                        new tariff(800, 0.30m, 4000, "Packaged tariff", (_paramteres) =>
                        {
                            if(_paramteres.Item1 <= 0) return 0;

                            decimal _diff = Convert.ToDecimal(_paramteres.Item1) - _paramteres.Item2.threshhold;

                            if(_diff <= 0) return _paramteres.Item2.baseRate;

                            return _paramteres.Item2.baseRate + Convert.ToDecimal(_diff) * _paramteres.Item2.additionalFee;
                        })
                    };

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                   .SetSlidingExpiration(TimeSpan.FromSeconds(1200));

                _memoryCache.Set("tariffCollection", tariffs, cacheEntryOptions);
            }

            return tariffs;
        }
    }
}
