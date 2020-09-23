using Common.DTO.AuthDTO;
using Common.IdentityManagement;
using Common.IdentityManagement.Identity;
using Common.IdentityManagement.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Common.IdentityManagement.Services
{
    public class AuthenticationService<TUser> : IAuthenticationService
        where TUser : Entities.User, new()
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IJwtManager jwtManager;
        private readonly IUserManager userManager;
        public AuthenticationService(
            IHttpContextAccessor httpContextAccessor,
            IJwtManager jwtManager,
            IUserManager userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.jwtManager = jwtManager;
            this.userManager = userManager;
        }

        public async Task<AuthResult<Token>> Login(LoginDTO loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Username) || string.IsNullOrEmpty(loginDto.Password))
                return AuthResult<Token>.UnvalidatedResult;

            var user = await userManager.FindByNameAsync(loginDto.Username);

            if (user != null && user.Id > 0 && !user.IsDeleted)
            {
                if (await userManager.CheckPasswordAsync(user, loginDto.Password))
                {
                    var token = jwtManager.GenerateToken(user);
                    return AuthResult<Token>.TokenResult(token);
                }
            }

            return AuthResult<Token>.UnauthorizedResult;
        }

        public async Task<AuthResult<Token>> ChangePassword(ChangePasswordDTO changePasswordDto)
        {
            if (changePasswordDto == null ||
                string.IsNullOrEmpty(changePasswordDto.ConfirmPassword) ||
                string.IsNullOrEmpty(changePasswordDto.Password) ||
                changePasswordDto.Password != changePasswordDto.ConfirmPassword
            )
                return AuthResult<Token>.UnvalidatedResult;

            int.TryParse(this.httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value, out var currentUserId);
            if (currentUserId > 0)
            {
                var succeeded = await userManager.ChangePasswordAsync(currentUserId, null, changePasswordDto.Password);
                if (succeeded)
                    return AuthResult<Token>.SucceededResult;
            }

            return AuthResult<Token>.UnauthorizedResult;
        }

        public async Task<AuthResult<Token>> SignUp(SignUpDTO signUpDto)
        {
            if (signUpDto == null ||
                string.IsNullOrEmpty(signUpDto.Username) ||
                string.IsNullOrEmpty(signUpDto.Password) ||
                string.IsNullOrEmpty(signUpDto.ConfirmPassword) ||
                string.IsNullOrEmpty(signUpDto.FullName) ||
                string.IsNullOrEmpty(signUpDto.Email) ||
                signUpDto.Password != signUpDto.ConfirmPassword
            )
                return AuthResult<Token>.UnvalidatedResult;

            var newUser = new TUser { Username = signUpDto.Username, Email = signUpDto.Email, FullName = signUpDto.FullName };

            var succeeded = await userManager.CreateAsync(newUser, signUpDto.Password);

            if (succeeded)
            {
                if (newUser.Id > 0)
                {
                    await userManager.AddToRoleAsync(newUser.Id, Roles.User);
                    var token = jwtManager.GenerateToken(newUser);
                    return AuthResult<Token>.TokenResult(token);
                }
            }

            return AuthResult<Token>.UnauthorizedResult;
        }

        //public async Task<AuthResult<string>> RequestPassword(RequestPasswordDTO requestPasswordDto)
        //{
        //    if (requestPasswordDto == null ||
        //        string.IsNullOrEmpty(requestPasswordDto.Email))
        //        return AuthResult<string>.UnvalidatedResult;

        //    var user = await userManager.FindByEmailAsync(requestPasswordDto.Email);

        //    if (user != null && user.Id > 0 && !user.IsDeleted)
        //    {
        //        var passwordResetToken = await userManager.GeneratePasswordResetTokenAsync(user.Id);
        //        return AuthResult<string>.TokenResult(passwordResetToken);
        //    }

        //    return AuthResult<string>.UnvalidatedResult;
        //}

        //public async Task<AuthResult<Token>> RestorePassword(RestorePasswordDTO restorePasswordDto)
        //{
        //    if (restorePasswordDto == null ||
        //        string.IsNullOrEmpty(restorePasswordDto.Email) ||
        //        string.IsNullOrEmpty(restorePasswordDto.Token) ||
        //        string.IsNullOrEmpty(restorePasswordDto.NewPassword) ||
        //        string.IsNullOrEmpty(restorePasswordDto.ConfirmPassword) ||
        //        string.IsNullOrEmpty(restorePasswordDto.ConfirmPassword) ||
        //        restorePasswordDto.ConfirmPassword != restorePasswordDto.NewPassword
        //    )
        //        return AuthResult<Token>.UnvalidatedResult;

        //    var user = await userManager2.FindByEmailAsync(restorePasswordDto.Email);

        //    if (user != null && user.Id > 0 && !user.IsDeleted)
        //    {
        //        var result = await userManager2.ResetPasswordAsync(user.Id, restorePasswordDto.Token, restorePasswordDto.NewPassword);

        //        if (result.Succeeded)
        //        {
        //            var token = jwtManager.GenerateToken(user);
        //            return AuthResult<Token>.TokenResult(token);
        //        }
        //    }

        //    return AuthResult<Token>.UnvalidatedResult;
        //}

        public Task<AuthResult<Token>> SignOut()
        {
            throw new System.NotImplementedException();
        }

        public async Task<AuthResult<Token>> RefreshToken(RefreshTokenDTO refreshTokenDto)
        {
            var refreshToken = refreshTokenDto?.Token?.Refresh_token;
            if (string.IsNullOrEmpty(refreshToken))
                return AuthResult<Token>.UnvalidatedResult;

            try
            {
                var principal = jwtManager.GetPrincipal(refreshToken, isAccessToken: false);
                int.TryParse(principal.Identity.GetUserId(), out var currentUserId);

                var user = await userManager.FindByIdAsync(currentUserId);

                if (user != null && user.Id > 0 && !user.IsDeleted)
                {
                    var token = jwtManager.GenerateToken(user);
                    return AuthResult<Token>.TokenResult(token);
                }
            }
            catch (Exception)
            {
                return AuthResult<Token>.UnauthorizedResult;
            }

            return AuthResult<Token>.UnauthorizedResult;
        }

        public async Task<Token> GenerateToken(int userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user != null && user.Id > 0)
            {
                return jwtManager.GenerateToken(user);
            }

            return null;
        }
    }
}
