
using MISA.Fresher.Core.EntityAttribute;
using MISA.Fresher.Core.Enum;
using System.Text;

namespace MISA.Fresher.Core.Entities
{
    public class Employee:History
    {
        #region Constructor
        public Employee()
        {

        }
        #endregion
        #region Property
        [IdProperty]
        [PropertyName("Id nhân viên")]
        [TableColums]
        public Guid EmployeeId { get; set; }
        [NotEmptyValidate]
        [Code]
        [TableColums]
        [PropertyName("Mã nhân viên")]
        public string? EmployeeCode { get; set; }
        [NotEmptyValidate]
        [PropertyName("Họ tên nhân viên")]
        [TableColums]
        public string? FullName { get; set; }
        [PhoneNumber]
        [PropertyName("Số điện thoại")]
        [TableColums]
        public string? PhoneNumber { get; set; }
        [TableColums]
        [PropertyName("Giới tính")]
        public Gender? Gender { get; set; }
        [TableColums]
        [PropertyName("Địa chỉ")]
        public string? Address { get; set; }
        [TableColums]
        [CheckDateTime]
        [PropertyName("Ngày sinh")]
        public DateTime? DateOfBirth { get; set; }
        [TableColums]
        [Email]
        [PropertyName("Email")]
        public string? Email { get; set; }
        [TableColums]
        [NotEmptyValidate]
        [PropertyName("Id đơn vị")]
        public Guid DepartmentId { get; set; }
        [PropertyName("Tên đơn vị")]
        public string? DepartmentName { get; set; }
        [PropertyName("Mã đơn vị")]
        public string? DepartmentCode { get; set; }
        [TableColums]
        [PropertyName("Chức vụ")]
        public string? Position { get; set; }
        [TableColums]
        [PropertyName("Số CMND")]
        public string? IdentityNumber { get; set; }
        [TableColums]
        [PropertyName("Ngày cấp")]
        public DateTime? IdentityDate { get; set; }
        [TableColums]
        [PropertyName("Ngày tham gia")]
        public DateTime? JoinDate { get; set; }
        [TableColums]
        [PropertyName("Tình trạng công việc")]
        public WorkSatatus? WorkStatus { get; set; }
        [TableColums]
        [PropertyName("Lương")]
        public double? Salary { get; set; }
        [TableColums]
        [PropertyName("Số tài khoản ngân hàng")]
        public string? BankNumber { get; set; }
        [TableColums]
        [PropertyName("Tên ngân hàng")]
        public string? BankName { get; set; }
        [TableColums]
        [PropertyName("Chi nhánh ngân hàng")]
        public string? BankPlace { get; set; }
        [TableColums]
        [TelephoneNumber]
        [PropertyName("Số điện thoại cố định")]
        public string? TelephoneNumber { get; set; }
        [PropertyName("Giới tính")]
        public string? GenderName { get {
                switch (Gender)
                {
                    case Enum.Gender.FeMaleGender: return Core.Resources.ResourceVN.FeMaleGender;
                    case Enum.Gender.MaleGender: return Core.Resources.ResourceVN.MaleGender;
                    case Enum.Gender.OtherGender: return Core.Resources.ResourceVN.OtherGender;
                    default: return "";
                }
            }
            }
        [PropertyName("Tình trạng công việc")]
        public string? WorkStatusName
        {
            get
            {
                switch (WorkStatus)
                {
                    case Enum.WorkSatatus.Working: return Core.Resources.ResourceVN.WorkingStatus;
                    case Enum.WorkSatatus.WorkOff: return Core.Resources.ResourceVN.WorkOffStatus;
                    case Enum.WorkSatatus.Retire: return Core.Resources.ResourceVN.RetireStatus;
                    default: return "";
                }
            }
        }
        #endregion
        #region Method

        #endregion
    }
}
