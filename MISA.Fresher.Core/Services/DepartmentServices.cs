using MISA.Fresher.Core.Entities;
using MISA.Fresher.Core.Interfaces.Infrastructure;
using MISA.Fresher.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Fresher.Core.Exceptions;

namespace MISA.Fresher.Core.Services
{
    public class DepartmentServices :BaseServices<Department>, IDepartmentServices
    {
        public DepartmentServices(IBaseRepository<Department> baseRepository):base(baseRepository)
        {
        }
    }
}
