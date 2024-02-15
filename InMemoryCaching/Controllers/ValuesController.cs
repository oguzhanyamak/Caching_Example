using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemory.Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly IMemoryCache _memoryCache;

        public ValuesController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _memoryCache.Set("name", "Auction",options: new() { AbsoluteExpiration = DateTime.Now,SlidingExpiration = TimeSpan.FromSeconds(5)});
        }

        [HttpGet]
        public long Get()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();


            string value = string.Empty;
            long elapsedMs = 0;

            if (_memoryCache.TryGetValue<string>("name",out value))
            {
                //return _memoryCache.Get<string>("name");

                watch.Stop();
                elapsedMs = watch.ElapsedMilliseconds;
                return elapsedMs;

            }
            watch.Stop();
            elapsedMs = watch.ElapsedMilliseconds;
            return elapsedMs;

        }
    }
}
