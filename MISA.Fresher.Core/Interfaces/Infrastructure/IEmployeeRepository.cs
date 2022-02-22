
using System.Text;

using MISA.Fresher.Core.Entities;
namespace MISA.Fresher.Core.Interfaces.Infrastructure
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        /// <summary>
        /// thực hiện phân trang dữ liệu cùng với tìm kiếm
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageNumber"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Paging<Employee> Paging(int PageSize, int PageNumber, string filter);
        /// <summary>
        /// thực hiện lấy mã nhân viên mới
        /// </summary>
        /// <returns></returns>
        public string NewEmployeeCode();
        /// <summary>
        /// thực hiện xóa nhiều nhân viên
        /// </summary>
        /// <param name="EmployeeIds"></param>
        /// <returns></returns>
        public int DeleteSomeEmployee(IEnumerable<Guid> EmployeeIds);
    }
}
