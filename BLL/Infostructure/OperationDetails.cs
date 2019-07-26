using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Infostructure
{
    public class  OperationDetails
    {
        public OperationDetails(bool success, string message, string prop)
        {
            Success = success;
            Message = message;
            Property = prop;
        }
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public string Property { get; private set; }
    }
}
