using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStudio.Thirdparty.Exceptions
{
    public class InsufficientBudgetException : Exception
    {
        public InsufficientBudgetException(string message) : base(message)
        { }
    }
}
