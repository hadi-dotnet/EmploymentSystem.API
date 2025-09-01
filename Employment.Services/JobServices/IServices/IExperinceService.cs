using Job.Services.JobServices.DTOs.EmployeeDTO;
using Job.Services.JobServices.DTOs.ExperinceDTO;
using Job.Services.JobServices.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.Services
{
    public interface IExperinceService
    {
        Task<Result> AddExperince(ExperinceDTO experienceDTO);
        Task<Result> UpdateExperince(ExperinceDTO experienceDTO, int ExperinceID);
        Task<Result> DeleteExperince(int ExperinceID);
        Task<Result> ApplyExperince(int ExperinceID);
    }
}
