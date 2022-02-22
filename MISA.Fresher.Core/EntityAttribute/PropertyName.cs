using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Core.EntityAttribute
{
    [AttributeUsage(AttributeTargets.All)]
    public class PropertyName:Attribute
    {
        public string Name { get; set; } = "";
        public PropertyName(string name)
        {
            this.Name = name;
        }
    }
}
