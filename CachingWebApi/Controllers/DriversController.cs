using CachingWebApi.Context;
using CachingWebApi.Models;
using CachingWebApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CachingWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly ICacheService _cacheService;
        private readonly AppDbContext _dbContext;

        public DriversController(ICacheService cacheService, AppDbContext dbContext)
        {
            _cacheService = cacheService;
            _dbContext = dbContext;
        }

        [HttpGet]

        public async Task<IActionResult> Get()
        {
            //check cash data
            var cacheData = await _cacheService.GetData<IEnumerable<Driver>>("drivers");
            if (cacheData != null && cacheData.Count() > 0)
                return Ok(cacheData);

            cacheData = await _dbContext.Drivers.ToListAsync();
            var expiryTime = DateTimeOffset.Now.AddSeconds(30);
            await _cacheService.SetData<IEnumerable<Driver>>("drivers",cacheData,expiryTime);
            return Ok(cacheData);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Driver value)
        {
            var expiryTime = DateTimeOffset.Now.AddSeconds(30);
            var addedObj = await _dbContext.Drivers.AddAsync(value);
            bool status = await _cacheService.SetData<Driver>($"driver{value.Id}",addedObj.Entity,expiryTime);

            await _dbContext.SaveChangesAsync();

            return Ok(addedObj.Entity);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            var exist = _dbContext.Drivers.FirstOrDefaultAsync(x => x.Id == Id);
            if(exist != null)
            {
                _dbContext.Remove(exist);
                await _cacheService.RemoveData($"driver{Id}");
                await _dbContext.SaveChangesAsync();
            }
            return NotFound();
        }

        
    }
}
