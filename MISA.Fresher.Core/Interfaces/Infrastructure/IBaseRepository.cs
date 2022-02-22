
using System.Text;


namespace MISA.Fresher.Core.Interfaces.Infrastructure
{
    public interface IBaseRepository<ObjectType>
    {
        /// <summary>
        /// Thực hiện lấy tất cả các bản ghi trong cơ sở dữ liệu
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ObjectType> GetAll();
        /// <summary>
        /// thực hiện lấy 1 bản ghi theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ObjectType GetById(Guid id);
        /// <summary>
        /// thực hiện thêm 1 bản ghi mới
        /// </summary>
        /// <param name="obj"></param>        
        public void Add(ObjectType obj);
        /// <summary>
        /// thực hiện cập nhật thông tin 1 bản ghi theo id
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="id"></param>
        public void Update(ObjectType obj, Guid id);
        /// <summary>
        /// thực hiện xóa 1 bản ghi theo id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(Guid id);
        /// <summary>
        /// kiểm tra xem mã đối tượng có tồn tại trong hệ thống hay không
        /// </summary>
        /// <param name="status"></param>
        /// <param name="obj"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckDuplicateCode(int status, ObjectType obj, Guid? id);
        /// <summary>
        /// kiểm tra xem id đối tượng có trong hệ thống chưa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExistId(Guid id);
    }
}
