using Employment.Infrastructure.Context;
using Job.Services.JobServices.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.Business
{
    public class ManageImageService:IManageImageService
    {
        private readonly AppDBContext _Context;
        public ManageImageService(AppDBContext context)
        {
            _Context = context;
        }
        public void DeleteOldImage(string CompanyID)
        {
            var Com = _Context.Companies.FirstOrDefault(x => x.UserID == CompanyID);
            if (!string.IsNullOrEmpty(Com.Image) && System.IO.File.Exists(Com.Image))
            {
                System.IO.File.Delete(Com.Image);              
            }
           
        }
    }
}
