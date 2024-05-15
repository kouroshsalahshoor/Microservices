using AuthApi.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Dtos.Auth;

namespace AuthApi.Controllers
{
    [Route("api/auth")]
    //[Route("api/[controller]")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        private ResponseDto _response;
        private readonly IAuthService _authService;

        public AuthApiController(IAuthService authService)
        {
            _authService = authService;
            _response = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            _response = await _authService.Register(dto);
            if (_response.IsSuccessful)
            {
                return Ok(_response);
            }
            return BadRequest(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var loginResponse = await _authService.Login(dto);
            if (loginResponse.User is null)
            {
                _response.IsSuccessful = false;
                _response.Errors = new List<string> { "UserName or Password incorrect!" };
                _response.Result = null;
                return BadRequest(_response);
            }
            _response.IsSuccessful = true;
            _response.Result = loginResponse;
            return Ok(_response);
        }
    }
}
