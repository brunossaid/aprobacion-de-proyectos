using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Entities;   
using Application.DTOs;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _userService.GetAllAsync();

            var userDtos = users.Select(user => new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = new ApproverRoleDto
                {
                    Id = user.Role,
                    Name = user.RoleNavigation.Name
                }
            }).ToList();

            return Ok(userDtos);
        }
    }
}
