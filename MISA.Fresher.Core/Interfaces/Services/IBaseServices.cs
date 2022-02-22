
using OfficeOpenXml;
using System.Text;


namespace MISA.Fresher.Core.Interfaces.Services
{
    public interface IBaseServices<ObjectType>
    {
        /// <summary>
        /// thực hiện thêm mới dữ liệu sau khi kiểm tra nghiệp vụ
        /// </summary>
        /// <param name="obj"></param>
        public void Insert(ObjectType obj);
        /// <summary>
        /// thực hiện cập nhật dữ liệu sau khi kiểm tra nghiệp vụ
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="id"></param>
        public void Update(ObjectType obj, Guid id);
        /// <summary>
        /// thực hiện xuất dữ liệu ra excel
        /// </summary>
        /// <param name="excludedProps"></param>
        /// <returns></returns>
        public ExcelPackage ExportExcel(ISet<string>? excludedProps);
    }
}
