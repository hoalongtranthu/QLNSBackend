using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Fresher.Core.EntityAttribute;

namespace MISA.Fresher.Core.Enum
{
    public enum Gender
    {
        [PropertyName("Nữ")]
        FeMaleGender = 0,
        [PropertyName("Nam")]
        MaleGender = 1,
        [PropertyName("Khác")]
        OtherGender =2,
    }
    public enum WorkSatatus
    {
        [PropertyName("Đang làm việc")]
        Working =0,
        [PropertyName("Đã nghỉ việc")]
        WorkOff =1,
        [PropertyName("Đã nghỉ hưu")]
        Retire =2,
    }
}
