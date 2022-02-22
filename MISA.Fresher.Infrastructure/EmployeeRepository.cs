
using System.Text;
using MISA.Fresher.Core.Entities;
using MISA.Fresher.Core.Interfaces.Infrastructure;
using MySqlConnector;
using Dapper;
namespace MISA.Fresher.Infrastructure
{
    public class EmployeeRepository :BaseRepository<Employee>, IEmployeeRepository
    {
        public Paging<Employee> Paging(int PageSize, int PageNumber, string? filter)
        {
            //1. khởi tạo kết nối tới csdl
            using (_connection = new MySqlConnection(ConnectionString))
            {
                //2. khởi tạo truy vấn
                string sqlCommand = "";
                int x = (PageNumber - 1) * PageSize;
                int TotalRecord = 0;
                
                string ColumFilter = "FullName LIKE CONCAT('%',@empfilter,'%') OR EmployeeCode LIKE CONCAT('%',@empfilter,'%') OR PhoneNumber LIKE CONCAT('%',@empfilter,'%')";
                if (string.IsNullOrEmpty(filter))
                {
                    sqlCommand = $"SELECT * FROM Employeeview ORDER BY EmployeeCode LIMIT @PageSize OFFSET @PageNumber";
                    TotalRecord = _connection.Query<Employee>("Select * from EmployeeView").Count();
                }
                else
                {
                    sqlCommand = $"SELECT * FROM Employeeview WHERE {ColumFilter}  ORDER BY EmployeeCode LIMIT @PageSize OFFSET @PageNumber";
                    TotalRecord = _connection.Query<Employee>($"Select * From Employeeview WHERE  {ColumFilter}", param: new {empfilter=filter}).Count();
                }
                DynamicParameters para = new DynamicParameters();
                para.Add("@empfilter", filter);
                para.Add("@PageSize", PageSize);
                para.Add("PageNumber", x);
                //4. thực hiện truy vấn
                var result = _connection.Query<Employee>(sqlCommand, param: para);
                
                float tp = (float)TotalRecord / PageSize;
                int TotalPage=(int)Math.Ceiling(tp);
                Paging<Employee> paging = new Paging<Employee>(TotalPage,TotalRecord,result);
                return paging;
            }
                
        }
        public string NewEmployeeCode()
        {
            using (_connection = new MySqlConnection(ConnectionString))
            {
                string sqlComand = "SELECT MAX(CAST(SUBSTR(EmployeeCode,4) AS int)) AS max  FROM employee LIMIT 1";
                var result = _connection.QueryFirstOrDefault<string>(sqlComand);
                int code = 0;
                if (string.IsNullOrEmpty(result.ToString()))
                {
                    return "NV-0001";
                }
                if (int.TryParse(result.ToString(), out code))
                {
                    code = code + 1;
                    if (((float)code / 10) < 1) return $"NV-000{code}";
                    if (((float)code / 100) < 1) return $"NV-00{code}";
                    if (((float)code / 1000) < 1) return $"NV-0{code}";
                }
                return $"NV-{code}";
            }
        }

        public int DeleteSomeEmployee(IEnumerable<Guid> EmployeeIds)
        {
            string sqlComand = "DELETE FROM Employee WHERE EmployeeId in @EmployeeIds";
            using (_connection = new MySqlConnection(ConnectionString))
            {
                var result = _connection.Execute(sqlComand, param: new {EmployeeIds});
                return result;
            }
        }
    }
}
