
using System.Text;
using MISA.Fresher.Core.Entities;
using MISA.Fresher.Core.Interfaces.Infrastructure;
using MISA.Fresher.Core.Interfaces.Services;
using MISA.Fresher.Core.Exceptions;
using System.Text.RegularExpressions;
using MISA.Fresher.Core.Resources;
using MISA.Fresher.Core.EntityAttribute;
using System.Reflection;

namespace MISA.Fresher.Core.Services
{
    public class EmployeeServices :BaseServices<Employee>, IEmployeeServices
    {
        IBaseRepository<Employee> _repository;
        public EmployeeServices(IBaseRepository<Employee> baseRepository):base(baseRepository)
        {
            _repository = baseRepository;
        }
        /// <summary>
        /// thực hiện kiểm tra lại dữ liệu đầu vào riêng cho đối tượng Employee
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="InputValidateException"></exception>
        protected override void CheckDataValidateExtend(Employee obj)
        {
            var prop = typeof(Employee).GetProperty("EmployeeCode");
            if(prop== null) 
            {
                throw new InputValidateException(ResourceVN.PropertyNull);
            }
            else
            {
                var propValue = prop!.GetValue(obj);
                if (propValue != null)
                {
                    bool isEmployeeCode = Regex.IsMatch(propValue.ToString()!, @"\ANV-\d{4,6}\Z", RegexOptions.IgnoreCase);
                    if (!isEmployeeCode)
                    {
                        throw new InputValidateException(String.Format(ResourceVN.RegexValidate,prop.GetCustomAttribute<PropertyName>()?.Name));
                    }
                }
            }
            

        }
    }
}
