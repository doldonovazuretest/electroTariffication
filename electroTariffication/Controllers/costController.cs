using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace electroTariffication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class costController : baseController
    {

        private readonly IMemoryCache _memoryCache;

        public costController(IMemoryCache memoryCache) : base(memoryCache)
        {
            _memoryCache = memoryCache;
        }


        [HttpGet(Name = "cost")]
        public IEnumerable<cost> Get(int? consumption)
        {
            List<cost> result = new List<cost>();

            try
            {
                if (consumption != null && consumption is int)
                {
                    foreach (var _tarrif in getCachedTariffs()) result.Add(_tarrif.getCostData((int)consumption));
                }

                return result.OrderBy(x => x.annualCost);
            }
            catch (Exception)
            {
                // handle errors
            }

            return result;
        }
    }
}
