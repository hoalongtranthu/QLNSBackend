using MISA.Fresher.Core.EntityAttribute;
using MISA.Fresher.Core.Exceptions;
using MISA.Fresher.Core.Interfaces.Infrastructure;
using MISA.Fresher.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reflection;
using MISA.Fresher.Core.Resources;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace MISA.Fresher.Core.Services
{
    public class BaseServices<ObjectType> : IBaseServices<ObjectType>
    {
        IBaseRepository<ObjectType> _baseRepository;
        public BaseServices(IBaseRepository<ObjectType> baseRepository)
        {
            _baseRepository = baseRepository;
        }
        /// <summary>
        /// thực hiện thêm mới dữ liệu sau khi kiểm tra các nghiệp vụ
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="InputValidateException"></exception>
        public void Insert(ObjectType obj)
        {
            //kiểm tra điều kiện đầu vào chung
            CheckDataValidate(obj);
            //kiểm tra điều kiện đầu vào của các lớp kế thừa nếu có
            CheckDataValidateExtend(obj);
            //kiểm tra xem có trùng mã khi thêm hay không
            if (_baseRepository.CheckDuplicateCode(0, obj, null))
            {
                throw new InputValidateException(String.Format(ResourceVN.DuplicateValue,typeof(ObjectType).GetProperty($"{typeof(ObjectType).Name}Code")?.GetCustomAttribute<PropertyName>()?.Name));
            }
            //thêm mới bản ghi vào hệ thống
            _baseRepository.Add(obj);
        }
        /// <summary>
        /// thực hiện cập nhật dữ liệu sau khi kiểm tra các nghiệp vụ
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="id"></param>
        /// <exception cref="InputValidateException"></exception>

        public void Update(ObjectType obj, Guid id)
        {
            //kiểm tra xem id có tồn tại hay không
            bool exist = _baseRepository.CheckExistId(id);
            if (!exist)
            {
                throw new InputValidateException(String.Format(ResourceVN.NotExist,"Id"));
            }
            //kiểm tra dữ liệu đầu vào chung
            CheckDataValidate(obj);
            //kiểm tra dữ liệu đầu vào của các lớp kế thừa nếu có
            CheckDataValidateExtend(obj);
            //kiểm tra trùng mã khi sửa thông tin bản ghi
            if (_baseRepository.CheckDuplicateCode(1, obj, id))
            {
                throw new InputValidateException(String.Format(ResourceVN.DuplicateValue, typeof(ObjectType).GetProperty($"{typeof(ObjectType).Name}Code")?.GetCustomAttribute<PropertyName>()?.Name));
            }
            //thực hiện sửa thông tin bản ghi trong hệ thống
            _baseRepository.Update(obj, id);
        }
        /// <summary>
        /// Xây dựng 
        /// </summary>
        /// <param name="excludedProps"></param>
        /// <returns></returns>
        public ExcelPackage ExportExcel(ISet<string>? excludedProps)
        {
            var excel = new ExcelPackage();
            excel.Workbook.Properties.Author = "Misa company";
            excel.Workbook.Properties.Title = "Employee List";
            string title = "DANH SÁCH NHÂN VIÊN";
            var sheet = excel.Workbook.Worksheets.Add("Employees");
            var props = typeof(ObjectType).GetProperties();
            if (excludedProps != null)
            {
                props = props.ToHashSet().ExceptBy(excludedProps, prop => prop.Name).ToArray();
            }
            sheet.Cells[1, 1, 1, props.Length + 1].Merge = true;
            sheet.Cells[2, 1, 2, props.Length + 1].Merge = true;
            sheet.Cells[1, 1, 1, props.Length + 1].Value = title;
            sheet.Cells[1, 1, 1, props.Length + 1].Style.HorizontalAlignment=ExcelHorizontalAlignment.Center;
            sheet.Cells[1, 1, 1, props.Length + 1].Style.Font.Bold=true;
            sheet.Cells[1, 1, 1, props.Length + 1].Style.Font.Size = 16;
            sheet.Cells[2, 1, 2, props.Length + 1].Value = "";
            sheet.Cells[3, 1].Value = "STT";
            
            
            for (int i = 0; i < props.Length; i++)
            {
                var cell = sheet.Cells[3, i + 2];
                cell.Value = props[i].GetCustomAttribute<PropertyName>()?.Name ?? props[i].Name;
            }
            var header = sheet.Cells[3, 1, 3, props.Length+1];
            header.Style.Fill.PatternType = ExcelFillStyle.Solid;
            header.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            header.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            header.Style.Font.Bold = true;
            var data = _baseRepository.GetAll().ToList();
            for (var i = 0; i < data.Count; i++)
            {
                sheet.Cells[i + 4, 1].Style.Indent = 1;
                sheet.Cells[i + 4, 1].Value = i + 1;
                for (var j = 0; j < props.Length; j++)
                {
                    var cell = sheet.Cells[i + 4, j + 2];
                    cell.Style.Indent = 1;
                    var val = props[j].GetValue(data[i]);
                    switch (val)
                    {
                        case DateTime:
                            cell.Style.Numberformat.Format = "dd/mm/yyyy";
                            cell.Style.Indent = 0;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            
                            break;
                    }
                    
                    cell.Value = val;
                }
            }
            sheet.Cells.AutoFitColumns();
            return excel;
        }

        /// <summary>
        /// thực hiện kiểm tra hợp lệ các dữ liệu đầu vào
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="InputValidateException"></exception>
        private void CheckDataValidate(ObjectType obj)
        {

            // kiểm tra các thuộc tính bắt buộc
            foreach(var prop in typeof(ObjectType).GetProperties().Where(p=> Attribute.IsDefined(p, typeof(NotEmptyValidate))))
            {
                var propValue= prop.GetValue(obj);
                var propName =prop.GetCustomAttribute<PropertyName>()?.Name;
                if ( propValue==null || string.IsNullOrEmpty(propValue.ToString()))
                {
                    throw new InputValidateException(String.Format(ResourceVN.NotEmptyValidate,propName));
                }
            }
            //kiểm tra email
            foreach(var prop in typeof(ObjectType).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Email))))
            {
                var propValue = prop.GetValue(obj)?.ToString();
                if (!string.IsNullOrEmpty(propValue))
                {
                    bool isEmail = Regex.IsMatch(propValue.ToString()!, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                    if (!isEmail)
                    {
                        throw new InputValidateException(String.Format(ResourceVN.RegexValidate,prop.GetCustomAttribute<PropertyName>()?.Name));
                    }
                }
            }
            //kiểm tra số điện thoại
            foreach (var prop in typeof(ObjectType).GetProperties().Where(p => Attribute.IsDefined(p, typeof(PhoneNumber))))
            {
                var propValue = prop.GetValue(obj);
                if(propValue!= null)
                {
                    if (!string.IsNullOrEmpty(propValue.ToString()))
                    {
                        bool isPhoneNumber = Regex.IsMatch(propValue.ToString()!, @"\A[0-9]{10}\Z", RegexOptions.IgnoreCase);
                        if (!isPhoneNumber)
                        {
                            throw new InputValidateException(String.Format(ResourceVN.RegexValidate,prop.GetCustomAttribute<PropertyName>()?.Name));
                        }
                    }
                }
                
            }
            //kiểm tra số điện thoại cố định
            foreach (var prop in typeof(ObjectType).GetProperties().Where(p => Attribute.IsDefined(p, typeof(TelephoneNumber))))
            {
                var propValue = prop.GetValue(obj);
                if (propValue != null)
                {
                    if (!string.IsNullOrEmpty(propValue.ToString()))
                    {
                        bool isPhoneNumber = Regex.IsMatch(propValue.ToString()!, @"\A[(]\d{3}[)] \d{3}-\d{4}\Z", RegexOptions.IgnoreCase);
                        if (!isPhoneNumber)
                        {
                            throw new InputValidateException(String.Format(ResourceVN.RegexValidate,prop.GetCustomAttribute<PropertyName>()?.Name));
                        }
                    }
                }

            }
            //Kiểm tra ngày tháng
            foreach (var prop in typeof(ObjectType).GetProperties().Where(p => Attribute.IsDefined(p, typeof(CheckDateTime))))
            {
                var propValue= prop.GetValue(obj);
                DateTime d;
                if(propValue!= null)
                {
                    if(!DateTime.TryParse(propValue.ToString(), out d))
                    {
                        throw new InputValidateException(String.Format(ResourceVN.RegexValidate,prop.GetCustomAttribute<PropertyName>()?.Name));
                    }
                    if (d.Year < 1900)
                    {
                        throw new InputValidateException(String.Format(ResourceVN.ValueMustMoreThan,"Năm",1900));
                    }
                    if (DateTime.Compare(d, DateTime.Now) > 0)
                    {
                        throw new InputValidateException(String.Format(ResourceVN.ValueMustLessThan, prop.GetCustomAttribute<PropertyName>()?.Name, "Ngày hiện tại"));
                    }
                } 
            }
        }
        /// <summary>
        /// tạo ra 1 phương thức virtual để cho hàm con gọi khi cần kiểm tra hợp lệ dữ liệu riêng
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void CheckDataValidateExtend(ObjectType obj) { }
    }
}
