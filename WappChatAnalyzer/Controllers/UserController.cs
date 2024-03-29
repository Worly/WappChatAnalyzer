﻿using Microsoft.AspNetCore.Mvc;
using WappChatAnalyzer.Auth;
using WappChatAnalyzer.DTOs;
using WappChatAnalyzer.Services;

namespace WappChatAnalyzer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("login")]
        public IActionResult LogIn(LogInDTO dto)
        {
            var response = userService.Authenticate(dto);

            if (response == null)
                return BadRequest(new ErrorBuilder().Add("WrongLogin"));

            return Ok(response);
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDTO dto)
        {
            var response = userService.Register(dto);

            if (response == null)
                return BadRequest(new ErrorBuilder().Add(nameof(dto.Email), "EmailTaken"));

            return Ok(response);
        }

        [HttpPost("verifyEmail")]
        public IActionResult VerifyEmail([FromQuery] string token)
        {
            var ok = userService.VerifyEmail(token);
            if (ok)
                return Ok();
            else
                return BadRequest();
        }

        [Authorize]
        [HttpPost("requestVerificationEmail")]
        public IActionResult RequestVerificationEmail()
        {
            var user = HttpContext.CurrentUser();
            if (user.VerifiedEmail)
                return BadRequest();

            userService.SendVerificationEmail(user);
            return Ok();
        }
    }
}
