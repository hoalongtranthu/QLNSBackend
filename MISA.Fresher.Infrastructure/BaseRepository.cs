using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using Dapper;
using MISA.Fresher.Core.Interfaces.Infrastructure;
using MISA.Fresher.Core.EntityAttribute;
using MISA.Fresher.Core.Entities;
using MISA.Fresher.Core.Exceptions;

namespace MISA.Fresher.Infrastructure
{
    public class BaseRepository<ObjectType>:IBaseRepository<ObjectType>
    {
        //khai báo field để kết nối csdl
        //protected readonly string ConnectionString = "Server = 47.241.69.179;Port = 3306; Database = MISA.CukCuk_Demo_NVMANH_copy;User Id = dev; Password = manhmisa;Allow User Variables=True";
        protected readonly string ConnectionString = "Server = 127.0.0.1;Port = 3306; Database = misa-web12-ducla;User Id = root; Password = ahihi123456789;Allow User Variables=True";
        protected MySqlConnection? _connection;
        //lấy tên object truyền vào
        string objName = typeof(ObjectType).Name;

        public void Add(ObjectType obj)
        {
            using (_connection = new MySqlConnection(ConnectionString))
            {
                var colum=new StringBuilder();
                var value = new StringBuilder();
                string delimiter = "";
                
                DynamicParameters dynamicParameters = new DynamicParameters();
                foreach (var prop in typeof(ObjectType).GetProperties().Where(p => Attribute.IsDefined(p, typeof(TableColums))))
                {
                    var propName = prop.Name;
                    var propValue = prop.GetValue(obj);
                    bool id = Attribute.IsDefined(prop, typeof(IdProperty));
                    if(id==true || propName== $"{objName}Id")
                    {
                        propValue = Guid.NewGuid();
                    }
                    colum.Append($"{delimiter}{propName}");
                    value.Append($"{delimiter}@{propName}");
                    dynamicParameters.Add($"@{propName}", propValue);
                    delimiter = ",";
                }
                string sqlCommand = $"INSERT INTO {objName}({colum.ToString()}) VALUES({value.ToString()})";
                var result = _connection.Execute(sqlCommand, param: dynamicParameters);
            }
        }

        public bool CheckDuplicateCode(int status, ObjectType obj, Guid? id)
        {
            //1. Gọi phương thức để kết nối đến csdl
            using (_connection = new MySqlConnection(ConnectionString))
            {
                //nếu đang kiểm tra trùng mã khi thêm mới
                if (status == 0)
                {
                    string sqlCommand1 = $"SELECT {objName}Code FROM {objName} WHERE {objName}Code= @{objName}Code ";
                    DynamicParameters para= new DynamicParameters();
                    string? value = "";
                    var prop = typeof(ObjectType).GetProperties().FirstOrDefault(p => Attribute.IsDefined(p, typeof(Code)));
                    if (prop == null)
                    {
                        throw new InputValidateException("Thiếu định nghĩa attribute");
                    }
                    else
                    {
                        value = prop.GetValue(obj)!.ToString();
                    }
                    para.Add($"{objName}Code", value);
                    string Check = _connection.QueryFirstOrDefault<string>(sqlCommand1, param: para);
                    if (Check != null)
                    {
                        return true;
                    }
                }
                //nếu kiểm tra trùng mã khi đang cập nhật
                if (status == 1)
                {
                    string sqlCommand1 = $"SELECT {objName}Code FROM {objName} WHERE {objName}Code= @{objName}Code AND {objName}Id != @{objName}Id";
                    DynamicParameters para = new DynamicParameters();
                    string? value = "";
                    var prop = typeof(ObjectType).GetProperties().FirstOrDefault(p => Attribute.IsDefined(p, typeof(Code)));
                    if (prop == null)
                    {
                        throw new InputValidateException("Thiếu định nghĩa attribute");
                    }
                    else
                    {
                        value = prop.GetValue(obj)!.ToString();
                    }
                    para.Add($"@{objName}Code", value);
                    para.Add($"@{objName}Id", id);
                    string Check = _connection.QueryFirstOrDefault<string>(sqlCommand1, param: para);
                    if (Check != null)
                    {
                        return true;
                    }
                }
                return false;
            }
            }

        public bool CheckExistId(Guid id)
        {
            using(_connection = new MySqlConnection(ConnectionString))
            {
                string sqlCommand = $"SELECT {objName}Id FROM {objName} WHERE {objName}Id = @Id";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@Id", id);
                var result= _connection.QueryFirstOrDefault<Guid>(sqlCommand, param: dynamicParameters);
                if (string.IsNullOrEmpty(result.ToString()))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }


        //xóa đối tượng theo id
        public void Delete(Guid id)
        {
            //1. khởi tạo kết nối với csdl
            using (_connection = new MySqlConnection(ConnectionString))
            {
                //2. khởi tạo truy vấn xóa
                string sqlCommand = $"DELETE FROM {objName} Where {objName}Id= @{objName}Id";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add($"@{objName}Id",id);
                //3. thực hiện xóa
                var result = _connection.Execute(sqlCommand, param: dynamicParameters);
            }
        }
        //lấy ra tất cả các đối tượng
        public IEnumerable<ObjectType> GetAll()
        {
            // 1.Gọi phương thức để kết nối đến csdl
            using (_connection = new MySqlConnection(ConnectionString))
            {
                //2. thực hiện truy vấn
                var result = _connection.Query<ObjectType>($"SELECT * FROM {objName}view");
                //3. gửi phản hồi về client
                return result;
            }
        }
        //lấy đối tượng theo id
        public ObjectType GetById(Guid id)
        {
            //1. Gọi phương thức để kết nối đến csdl
            using (_connection = new MySqlConnection(ConnectionString))
            {
                //2. khởi tạo truy vấn
                string sqlCommand = $"SELECT * FROM {objName}view WHERE {objName}Id= @{objName}Id";
                DynamicParameters para = new DynamicParameters();
                para.Add($"@{objName}Id", id);
                //3. Thực hiện truy vấn
                var result = _connection.QueryFirstOrDefault<ObjectType>(sqlCommand, param: para);
                //4. gửi phản hồi về client
                return result;
            }
        }



        public void Update(ObjectType obj, Guid id)
        {
            var columSet= new StringBuilder();
            string delimiter = "";
            DynamicParameters dynamicParameters = new DynamicParameters();
            foreach(var prop in typeof(ObjectType).GetProperties().Where(p=>Attribute.IsDefined(p,typeof(TableColums))))
            {
                var name = prop.Name;
                var value= prop.GetValue(obj);
                bool isId= Attribute.IsDefined(prop, typeof(IdProperty));
                
                if (isId|| name == $"{objName}Id")
                {
                    dynamicParameters.Add($"@{name}", id);
                    continue;
                }
                columSet.Append($"{delimiter}{name}=@{name}");
                dynamicParameters.Add($"@{name}", value);
                delimiter = ",";
            }

            string sqlCommand = $"UPDATE {objName} SET {columSet.ToString()} WHERE {objName}Id=@{objName}Id";
            using(_connection=new MySqlConnection(ConnectionString))
            {
                _connection.Execute(sqlCommand, param: dynamicParameters);
            }
        }
    }
}
