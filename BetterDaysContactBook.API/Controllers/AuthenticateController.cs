using BetterDaysContactBook.Core;
using BetterDaysContactBook.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterDaysContactBook.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/User")]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthentication _authentication;
        public AuthenticateController(IAuthentication authentication)
        {
            this._authentication = authentication;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO userRequest)
        {
            try
            {
                return Ok(await _authentication.Login(userRequest));
            }
            catch (AccessViolationException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO regRequest)
        {
            try
            {
                var result = await _authentication.Register(regRequest);
                return Created("", result);// Ok(await _authentication.Register(regRequest));
            }
            catch (MissingFieldException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
