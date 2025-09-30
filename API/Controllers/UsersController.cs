using Microsoft.AspNetCore.Mvc;
using TaskFlow_Monitor.Core.Models;
using TaskFlow_Monitor.Domain.Interfaces.Services;
using TaskFlow_Monitor.API.DTO;

namespace TaskFlow_Monitor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService) =>
            _usersService = usersService;

        [HttpPost]
        public async Task<ActionResult<Guid>> Add(UserRequest request)
        {
            var newId = Guid.NewGuid();

            var newUser = new UserEntity
            {
                Id = newId,
                Name = request.name,
                Email = request.email
            };

            await _usersService.Add(newUser);

            return Ok(newId);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserResponse>> Get([FromRoute] Guid id)
        {
            var user = await _usersService.Get(id);

            if (user == null) return NotFound();

            var response = new UserResponse(id, user.Name, user.Email);

            return Ok(response);
        }
    }
}
