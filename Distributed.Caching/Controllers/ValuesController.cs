﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace Distributed.Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly IDistributedCache _distributedCache;

        public ValuesController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet("Set")]
        public async Task<IActionResult> Set(string name,string surname)
        {
            await _distributedCache.SetStringAsync("name", name, options: new() { AbsoluteExpiration = DateTime.Now.AddSeconds(30),SlidingExpiration = TimeSpan.FromSeconds(5)});
            await _distributedCache.SetAsync("surname", Encoding.UTF8.GetBytes(surname), options: new() { AbsoluteExpiration = DateTime.Now.AddSeconds(30), SlidingExpiration = TimeSpan.FromSeconds(5) });

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var name = await _distributedCache.GetAsync("name");
            var surname = await _distributedCache.GetStringAsync("surname");
            return Ok(Encoding.UTF8.GetString(name)+" "+surname);
        }
    }
}
