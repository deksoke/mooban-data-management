using AutoMapper;
using Common.DTO.AuthDTO;
using Common.IdentityManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Common.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiControllerBase
    {
        protected readonly IAuthenticationService authService;
        private readonly IMapper mapper;
        public AuthController(IMapper mapper, IAuthenticationService authService)
            : base()
        {
            this.authService = authService;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            var result = await authService.Login(loginDto);

            if (result.Succeeded)
            {
                return Ok(new { token = result.Data });
            }

            if (result.IsModelValid)
            {
                return Unauthorized();
            }

            return BadRequest();
        }

        //[HttpPost]
        //[Authorize]
        //[Route("reset-pass")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePasswordDto)
        //{
        //    var result = await authService.ChangePassword(changePasswordDto);

        //    if (result.Succeeded)
        //    {
        //        return Ok();
        //    }

        //    return BadRequest();
        //}

        [HttpPost]
        [AllowAnonymous]
        [Route("sign-up")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignUp(SignUpDTO signUpDto)
        {
            var result = await authService.SignUp(signUpDto);

            if (result.Succeeded)
                return Ok(new { token = result.Data });

            return BadRequest();
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[Route("request-pass")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> RequestPassword(RequestPasswordDTO requestPasswordDto)
        //{
        //    var result = await authService.RequestPassword(requestPasswordDto);

        //    if (result.Succeeded)
        //        return Ok(new
        //        {
        //            result.Data,
        //            Description = "Reset Token should be sent via Email. Token in response - just for testing purpose."
        //        });

        //    return BadRequest();
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[Route("restore-pass")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> RestorePassword(RestorePasswordDTO restorePasswordDto)
        //{
        //    var result = await authService.RestorePassword(restorePasswordDto);

        //    if (result.Succeeded)
        //        return Ok(new { token = result.Data });

        //    return BadRequest();
        //}

        [HttpPost]
        [Authorize]
        [Route("sign-out")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignOut()
        {
            return Ok();
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[Route("refresh-token")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> RefreshToken(RefreshTokenDTO refreshTokenDTO)
        //{
        //    var result = await authService.RefreshToken(refreshTokenDTO);

        //    if (result.Succeeded)
        //        return Ok(new { token = result.Data });

        //    return BadRequest();
        //}

    }
}
