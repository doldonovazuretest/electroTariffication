using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace electroTariffication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tariffController : baseController
    {
        private readonly IMemoryCache _memoryCache;

        public tariffController(IMemoryCache memoryCache) : base(memoryCache)
        {
            _memoryCache = memoryCache;
        }


        [HttpGet(Name = "tariff")]
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
    }
}
