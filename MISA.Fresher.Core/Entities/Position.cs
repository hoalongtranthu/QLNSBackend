using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Fresher.Core.EntityAttribute;
namespace MISA.Fresher.Core.Entities
{
    public class Position:History
    {
        #region Constructor
        public Position()
        {

        }
        #endregion
        #region Property
        [IdProperty]
        public Guid PositionId { get; set; }
        [NotEmptyValidate]
        [Code]
        public string? PositionCode { get; set; }
        [NotEmptyValidate]
        public string? PositionName { get; set; }
        public string? Description { get; set; }
        public Guid? ParentId { get; set; }
        #endregion
        #region Method

        #endregion
    }
}
