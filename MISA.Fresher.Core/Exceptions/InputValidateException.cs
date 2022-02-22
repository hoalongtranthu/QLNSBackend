using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Core.Exceptions
{
    public class InputValidateException: Exception
    {
        string message = "";
        public InputValidateException(string msg)
        {
            this.message = msg;
        }
        public override string Message
        {
            get
            {
                return message;
            }
        }
    }
}
