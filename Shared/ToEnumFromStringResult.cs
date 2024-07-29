using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ToEnumFromStringResult<T>
    {
        public T Value { get; private set; }
        public bool Success { get; private set; }
        public string ErrorMessage { get; private set; }    

        private ToEnumFromStringResult(T value, bool success, string errorMessage)
        {
            Value = value;
            Success = success;
            ErrorMessage = errorMessage;
        }

        public static ToEnumFromStringResult<T> SuccessResult(T value)
        {
            return new ToEnumFromStringResult<T>(value, true, null!);
        }

        public static ToEnumFromStringResult<T> ErrorResult(string errorMessage)
        {
            return new ToEnumFromStringResult<T>(default(T)!, false, errorMessage);
        }
    }
}
