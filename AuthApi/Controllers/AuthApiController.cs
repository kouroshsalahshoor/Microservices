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
        public async Task<IActionResult> Login()
        {
            return Ok();
        }
    }
}
