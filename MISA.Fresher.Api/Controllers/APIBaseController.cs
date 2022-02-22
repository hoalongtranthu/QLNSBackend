using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Fresher.Core.Exceptions;
using MISA.Fresher.Core.Interfaces.Infrastructure;
using MISA.Fresher.Core.Interfaces.Services;
using MISA.Fresher.Core.Resources;
namespace MISA.Fresher.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class APIBaseController<ObjectType> : ControllerBase
    {
        #region field
        protected IBaseRepository<ObjectType> _iBaseRepository;
        protected IBaseServices<ObjectType> _iBaseServices;
        #endregion
        #region constructor
        public APIBaseController(IBaseRepository<ObjectType> iBaseRepository, IBaseServices<ObjectType> iBaseServices)
        {
            _iBaseRepository = iBaseRepository;
            _iBaseServices = iBaseServices;
        }
        #endregion
        #region method
        //lấy tất cả các bản ghi
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = _iBaseRepository.GetAll();
                return StatusCode(200,result);
            }
            catch (Exception e)
            {
                var response = new
                {
                    DevMsg = e.Message,
                    UserMsg = string.Format(ResourceVN.HaveError),
                    data = "",
                };
                return StatusCode(500, response);
            }

        }
        //lấy bản ghi theo id
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var result = _iBaseRepository.GetById(id);
                return StatusCode(200,result);
            }
            catch (Exception e)
            {
                var response = new
                {
                    DevMsg = e.Message,
                    UserMsg = string.Format(ResourceVN.HaveError),
                    data = "",
                };
                return StatusCode(500, response);
            }
        }
        //thêm mới 1 bản ghi
        [HttpPost]
        public IActionResult Post(ObjectType obj)
        {
            try
            {
                _iBaseServices.Insert(obj);
                return StatusCode(201);
            }
            catch (InputValidateException ex)
            {
                var response = new
                {
                    DevMsg = ex.Message,
                    UserMsg = ex.Message,
                    data = "",
                };
                return StatusCode(400, response);
            }
            catch (Exception e)
            {
                var response = new
                {
                    DevMsg = e.Message,
                    UserMsg = string.Format(ResourceVN.HaveError),
                    data = "",
                };
                return StatusCode(500, response);
            }
        }
        //sửa thông tin 1 bản ghi theo id
        [HttpPut("{id}")]
        public IActionResult Put(ObjectType obj, Guid id)
        {
            try
            {
                _iBaseServices.Update(obj, id);
                return StatusCode(200);
            }
            catch (InputValidateException ex)
            {
                var response = new
                {
                    DevMsg = ex.Message,
                    UserMsg = ex.Message,
                    data = "",
                };
                return StatusCode(400, response);
            }
            catch (Exception e)
            {
                var response = new
                {
                    DevMsg = e.Message,
                    UserMsg = string.Format(ResourceVN.HaveError),
                    data = "",
                };
                return StatusCode(500, response);
            }

        }
        //xóa 1 bản ghi theo id
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _iBaseRepository.Delete(id);
                return StatusCode(200);
            }
            catch (Exception e)
            {
                var response = new
                {
                    DevMsg = e.Message,
                    UserMsg = string.Format(ResourceVN.HaveError),
                    data = "",
                };
                return StatusCode(500, response);
            }
        }
        #endregion
    }
}
