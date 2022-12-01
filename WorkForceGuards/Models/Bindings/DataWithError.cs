using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.Bindings
{
    public class DataWithError
    {
        public DataWithError() { }
        public DataWithError(object result, string errorMessage) {
            Result = result;
            ErrorMessage = errorMessage;
        }
        public object Result { get; set; }

        public string ErrorMessage { get; set; }
    }
}
