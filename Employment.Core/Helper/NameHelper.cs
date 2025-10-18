using Employment.Infrastructure.Entitys;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Job.Core.Helper
{
    public static class NameHelper
    {
        public static bool IsNameExist(AppUser User)
        {
            if (User.UserType == UserTypeEnum.Employee &&
               (string.IsNullOrWhiteSpace(User.Employees?.FirstName) ||
               string.IsNullOrWhiteSpace(User.Employees?.secoundName) ||
               string.IsNullOrWhiteSpace(User.Employees?.LastName)))
            {
                return false;
            }
            else if (User.UserType == UserTypeEnum.Company && string.IsNullOrWhiteSpace(User.Company.Name))
            {
                return false;
            }

            return true;
        }
    }
}
