using Employment.Infrastructure.Context;
using Job.Core.Entitys;
using Job.Services.JobServices.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.Services
{
    public class cacheService:IcacheService
    {
        private readonly IMemoryCache _cache;
        
        public cacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<List<SkillsType>> GetSkillsAsync(Func<Task<List<SkillsType>>> factory)
        {
            return await _cache.GetOrCreateAsync("AllSkills", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(120);
                return await factory();
            });
        }


    }
}
