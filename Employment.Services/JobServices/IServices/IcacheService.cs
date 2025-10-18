using Job.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.IServices
{
    public interface IcacheService
    {
        Task<List<SkillsType>> GetSkillsAsync(Func<Task<List<SkillsType>>> factory);
    }
}
