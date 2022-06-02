using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;


namespace electroTariffication.Controllers
{
    public class baseController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;

        public baseController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        protected IEnumerable<tariff> getCachedTariffs()
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
