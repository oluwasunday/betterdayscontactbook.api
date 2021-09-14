using AutoMapper;
using BetterDaysContactBook.Common;
using BetterDaysContactBook.Core;
using BetterDaysContactBook.Core.helper;
using BetterDaysContactBook.Models;
using BetterDaysContactBook.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BetterDaysContactBook.API.Controllers
{
    [Authorize]
    [Route("Users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IContactBookRepository _contactBookRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IImageService imageService;


        public UserController(IContactBookRepository contactBookRepository, IMapper mapper,
            IUserService userService, IImageService imageService)
        {
            _contactBookRepository = contactBookRepository ??
                throw new ArgumentNullException(nameof(contactBookRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _userService = userService;

            this.imageService = imageService;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet()]
        public async Task<ActionResult> GetUsers([FromQuery] PagingParams paging)
        {
            //bool user = HttpContext.User.IsInRole("Admin");
            try
            {
                return Ok(await _userService.GetAllUsers(paging));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("id")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                return Ok(await _userService.GetUserById(id));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }


        [HttpGet("Email")]
        [Authorize(Roles = "Regular")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                return Ok(await _userService.GetUserByEmail(email));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }



        [HttpPost("add-new")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddNew([FromBody] RegisterDTO user)
        {
            // set bad request if contact data is not provided in body  
            if (user == null)
                return BadRequest("Can't add null value");

            try
            {
                var result = await _userService.AddNewUser(user);
                return Ok(new
                {
                    message = $"Contact with the id {result.Id} is added successfully."
                });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (MissingFieldException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }


        [HttpPut("update/id")]
        [Authorize(Roles = "Admin, Regular")]
        public IActionResult Update([FromBody] UpdateUserDTO user)
        {
            // set bad request if contact data is not provided in body  
            var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
            //var us = LoggedUser.LoggedInUserId;

            try
            {
                var contact = _userService.Update(userId, user);
                return Ok(new
                {
                    message = $"Contact with the id {userId} is updated successfully."
                });
            }
            catch(MissingMemberException ex) 
            {
                return BadRequest(ex.Message);
            }
            catch(ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }


        [HttpDelete("delete/id")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _userService.DeleteUser(id);
                return NoContent();
            }
            catch (MissingMemberException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }


        [HttpPost("photo/id")]
        [Authorize(Roles = "Regular")]
        public async Task<IActionResult> UploadPhoto([FromForm] ImageDTO imageDTO)
        {
            try
            {
                //var userA = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

                var upload = await imageService.UploadAsync(imageDTO.Image);
                var result = new ImageAddedDTO()
                {
                    PublicId = upload.PublicId,
                    Url = upload.Url.ToString()
                };
                //await _userService.UpdatePhotoUrl(upload.Url.ToString());
                await _userService.UpdatePhotoUrl(result.Url);
                return Ok(result);
            }
            catch (BadImageFormatException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("search")]
        [Authorize(Roles = "Regular, Admin")]
        public IActionResult Search([FromForm] string searchTerm)
        {
            try
            {
                var searchResult = _userService.SearchUsersByTerm(searchTerm);
                if (searchResult.Count >= 1)
                    return Ok(searchResult);
                else 
                    return Ok(new SearchNullResponseDTO { Status = 200, Message = "No item found with the search term" });
            }
            catch (ArgumentNullException ex)
            { 
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
