using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Fresher.Core.Entities;
using MISA.Fresher.Core.Interfaces.Infrastructure;
using MISA.Fresher.Core.Interfaces.Services;
namespace MISA.Fresher.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PositionsController : APIBaseController<Position>
    {
        public PositionsController(IBaseRepository<Position> baseRepository, IBaseServices<Position> baseServices):base(baseRepository, baseServices)
        {

        }
    }
}
