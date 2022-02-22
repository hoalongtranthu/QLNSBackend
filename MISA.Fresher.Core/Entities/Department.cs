using MISA.Fresher.Core.EntityAttribute;
using System.Text;

namespace MISA.Fresher.Core.Entities
{
    public class Department:History
    {
        #region Contructor
        public Department()
        {

        }
        #endregion
        #region Prototype
        [IdProperty]
        [TableColums]
        [NotEmptyValidate]
        [PropertyName("Id Đơn vị")]
        public Guid DepartmentId { get; set; }
        [Code]
        [TableColums]
        [NotEmptyValidate]
        [PropertyName("Mã đơn vị")]
        public string? DepartmentCode { get; set; }
        [TableColums]
        [NotEmptyValidate]
        [PropertyName("Tên đơn vị")]
        public string? DepartmentName { get; set; }
        [TableColums]
        [PropertyName("Mô tả")]
        public string? Description { get; set; }
        #endregion
        #region Method

        #endregion
    }
}
