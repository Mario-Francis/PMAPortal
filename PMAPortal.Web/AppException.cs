using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web
{
    public class AppException:Exception
    {
        public AppException(string message, bool isValidationException = true, bool isUnauthorizedException = false, IEnumerable<string> errorItems = null) : base(message)
        {
            IsValidationException = isValidationException;
            IsUnauthorizedException = isUnauthorizedException;
            ErrorItems = errorItems;
        }
        public bool IsValidationException { get; }
        public bool IsUnauthorizedException { get; }
        public IEnumerable<string> ErrorItems { get; }
    }
}
