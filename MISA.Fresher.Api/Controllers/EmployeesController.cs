using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Fresher.Core.Entities;
using MISA.Fresher.Core.Exceptions;
using MISA.Fresher.Core.Interfaces.Infrastructure;
using MISA.Fresher.Core.Interfaces.Services;
using MISA.Fresher.Core.Resources;

namespace MISA.Fresher.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : APIBaseController<Employee>
    {
        //khởi tạo các interface
        IEmployeeRepository _EmployeeRepository;
        //gán các phương thức cho các interface thông qua các class đã impliment các interface này
        public EmployeesController(IEmployeeRepository employeeRepository,IEmployeeServices employeeServices):base(employeeRepository, employeeServices)
        {
            _EmployeeRepository = employeeRepository;
        }
        //phân trang danh sách nhân viên
        /// <summary>
        /// Thực hiện phân trang
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageNumber"></param>
        /// <param name="empfilter"></param>
        /// <returns></returns>
        [HttpGet("Pagination")]
        public IActionResult Paging(int PageSize, int PageNumber, string? empfilter)
        {
            try
            {
                var result = _EmployeeRepository.Paging(PageSize, PageNumber, empfilter!);
                return Ok(result);
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
        /// <summary>
        /// Thực hiện lấy mã nhân viên mới
        /// </summary>
        /// <returns></returns>
        [HttpGet("NewEmployeeCode")]
        public IActionResult GetNewEmployeeCode()
        {
            try
            {
                var result = _EmployeeRepository.NewEmployeeCode();
                return StatusCode(200, result);
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
        /// <summary>
        /// thực hiện xóa nhiều nhân viên
        /// </summary>
        /// <param name="EmployeeIds"></param>
        /// <returns></returns>
        [HttpDelete("DeleteSomeEmployee")]
        public IActionResult DeleteSomeEmployee(IEnumerable<Guid> EmployeeIds)
        {
            try
            {
                var result= _EmployeeRepository.DeleteSomeEmployee(EmployeeIds);
                return StatusCode(200, result);
            }
            catch(Exception e)
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
        [HttpGet("ExcelFile")]
        public IActionResult GetExcelFile()
        {
            using var excel = _iBaseServices.ExportExcel(new HashSet<string> { "EmployeeId", "DepartmentId","Gender","WorkStatus" });
            var stream = new MemoryStream();
            excel.SaveAs(stream);
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "danh_sach_nhan_vien.xlsx");
        }
    }
}
