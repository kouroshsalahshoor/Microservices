﻿using AuthApi.Services.IServices;
using MessageSenders;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ISendMessage _rabbitMQSender;
        private readonly IConfiguration _configuration;

        public AuthApiController(IAuthService authService, ISendMessage rabbitMQSender, IConfiguration configuration)
        {
            _authService = authService;
            _rabbitMQSender = rabbitMQSender;
            _configuration = configuration;
            _response = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            _response = await _authService.Register(dto);
            if (_response.IsSuccessful)
            {
                _rabbitMQSender.Send(dto.Email, _configuration.GetValue<string>("TopicsAndQueues:RegisterUserQueue")!);

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

        [HttpPost("AssignToRole")]
        public async Task<IActionResult> AssignToRole([FromBody] RegisterDto dto)
        {
            _response = await _authService.AssignToRole(dto.UserName, dto.Role);
            if (_response.IsSuccessful)
            {
                return Ok(_response);
            }
            return BadRequest(_response);
        }

        [Authorize]
        [HttpPut("UpdateUser")]
        //[HttpPatch("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto dto)
        {
            _response = await _authService.UpdateUser(dto);
            if (_response.IsSuccessful)
            {
                return Ok(_response);
            }
            return BadRequest(_response);
        }
    }
}
