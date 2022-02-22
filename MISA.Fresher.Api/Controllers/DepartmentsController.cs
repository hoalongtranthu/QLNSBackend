using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Fresher.Core.Entities;
using MISA.Fresher.Core.Interfaces.Infrastructure;
using MISA.Fresher.Core.Interfaces.Services;
using MISA.Fresher.Core.Exceptions;
using MISA.Fresher.Core.Services;
namespace MISA.Fresher.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentsController : APIBaseController<Department>
    {
        public DepartmentsController(IBaseRepository<Department> baseRepository, IBaseServices<Department> baseServices):base(baseRepository, baseServices)
        {

        }
    }
}
