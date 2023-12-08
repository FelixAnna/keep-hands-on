using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStudio.Thirdparty.Exceptions
{
    public class NoSuchProfessionException : Exception
    {
        public NoSuchProfessionException(string personType) : base(personType)
        { }

    }
}
