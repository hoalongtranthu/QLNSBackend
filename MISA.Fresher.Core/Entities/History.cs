using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Fresher.Core.EntityAttribute;
namespace MISA.Fresher.Core.Entities
{
    public class History
    {
        #region Cotructor
        #endregion
        #region Property
        [CheckDateTime]
        [PropertyName("Ngày tạo")]
        public DateTime? CreatedDate { get;} = DateTime.Now;
        [PropertyName("Người tạo")]
        public string? CreatedBy { get; set; } = "admin";
        [CheckDateTime]
        [PropertyName("Ngày sửa")]
        public DateTime? ModifiedDate { get; set; }
        [PropertyName("Người sửa")]
        public string? ModifiedBy { get; set; }
        #endregion

        #region Method
        #endregion

    }
}
