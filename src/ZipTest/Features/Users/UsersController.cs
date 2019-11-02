using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ZipTest.Models;
using ZipTest.Models.Request;

namespace ZipTest.Features.Users
{
    /// <summary>
    /// A controller for handling Users queries and/or commands.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="mediator">The query/command mediator.</param>
        public UsersController(IMediator mediator) => this.mediator = mediator;

        /// <summary>
        /// Returns the list of users.
        /// </summary>
        /// <param name="request">The users query request.</param>
        /// <returns>A <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(Models.Response.UserCollection), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsersAsync([FromQuery] GetCollectionRequest request)
        {
            var query = new GetUsers.Query(request);
            Models.Response.UserCollection response = await mediator.Send(query);
            return Ok(response);
        }

        /// <summary>
        /// Returns the details of the requested user.
        /// </summary>
        /// <param name="request">The user query request.</param>
        /// <returns>A <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("{emailAddress}", Name = "Get")]
        [ProducesResponseType(typeof(Models.Response.User), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserAsync(string emailAddress)
        {
            var query = new GetUser.Query() { EmailAddress = emailAddress };
            Models.Response.User response = await mediator.Send(query);
            if (response == null)
            {
                return NotFound(new { error = "Specified user email address not found." });
            }
            return Ok(response);
        }

        /// <summary>
        /// Creates user with requested details.
        /// </summary>
        /// <param name="request">The user detail.</param>
        /// <returns>A <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost("Create")]
        [ProducesResponseType(typeof(Models.Response.CreateUserResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserRequest request)
        {
            var command = new CreateUser.Command() { User = request };
            Models.Response.CreateUserResponse response = await mediator.Send(command);
            return Ok(response);
        }
    }
}
