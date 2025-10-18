using Job.Core.Entitys;
using Job.Services.JobServices.DTOs.EmployeeDTO;
using Job.Services.JobServices.DTOs.SkillsDTO;
using Job.Services.JobServices.Results;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.Services
{
    public interface IEmployeeService
    {
        Task<Result> UpdateEmployee(EmployeeDTO updateEmployeeDTO);
        Task<Result<GetInforamtionDTO?>> GetEmployeeInformation();
        Task<Result> AddSkill(List<AddSkillDTO> SkillTypeIDDTO);
        Task<Result<List<FindSkillsDTO>>> FindSkills(string SkillName);
        Task<Result> Applyskill(int SkillID);
        Task<Result> DeleteSkill(int SkillID);
        Task<Result<GetInforamtionDTO?>?> GetEmployeeInformationByID(string EmployeeID);
    }
}
