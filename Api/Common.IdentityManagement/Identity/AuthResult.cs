using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.IdentityManagement.Identity
{
    public class AuthResult<T>
    {
        private AuthResult(T data)
        {
            Succeeded = true;
            IsModelValid = true;
            Data = data;
        }

        private AuthResult(bool isSucceeded, bool isModelValid)
        {
            Succeeded = isSucceeded;
            IsModelValid = isModelValid;
        }

        private AuthResult(bool isSucceeded)
        {
            Succeeded = isSucceeded;
            IsModelValid = isSucceeded;
        }

        public bool Succeeded { get; }
        public T Data { get; }
        public bool IsModelValid { get; }

        public static AuthResult<T> UnvalidatedResult => new AuthResult<T>(false);
        public static AuthResult<T> UnauthorizedResult => new AuthResult<T>(false, true);
        public static AuthResult<T> SucceededResult => new AuthResult<T>(true);
        public static AuthResult<T> TokenResult(T token)
        {
            return new AuthResult<T>(token);
        }
    }

}
