using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Core.Entities
{
    public class Paging<T>
    {
        #region Contructor
        public Paging(int a, int b, IEnumerable<T> c)
        {
            TotalPage = a;
            TotalRecord = b;
            Data = c;
        }
        public Paging()
        {

        }
        #endregion
        #region Prototyties
        public int TotalPage { get; set; }
        public int TotalRecord { get; set; }
        public IEnumerable<T>? Data { get; set; }
        #endregion

    }
}
