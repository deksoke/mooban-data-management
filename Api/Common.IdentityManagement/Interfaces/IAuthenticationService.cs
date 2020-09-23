using Common.DTO.AuthDTO;
using Common.IdentityManagement.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.IdentityManagement
{
    public interface IAuthenticationService
    {
        Task<AuthResult<Token>> Login(LoginDTO loginDto);
        Task<AuthResult<Token>> ChangePassword(ChangePasswordDTO changePasswordDto);
        Task<AuthResult<Token>> SignUp(SignUpDTO signUpDto);
        //Task<AuthResult<string>> RequestPassword(RequestPasswordDTO requestPasswordDto);
        //Task<AuthResult<Token>> RestorePassword(RestorePasswordDTO restorePasswordDto);
        Task<AuthResult<Token>> SignOut();
        //Task<AuthResult<Token>> RefreshToken(RefreshTokenDTO refreshTokenDto);
        Task<Token> GenerateToken(int userId);
    }
}
