using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZipTest.Models;
using ZipTest.Models.Request;
using ZipTest.Models.Response;

namespace ZipTest.Features.Accounts
{
    /// <summary>
    /// A controller for handling Accounts queries and/or commands.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="mediator">The query/command mediator.</param>
        public AccountsController(IMediator mediator) => this.mediator = mediator;

        /// <summary>
        /// Returns the list of accounts.
        /// </summary>
        /// <param name="request">The accounts query request.</param>
        /// <returns>A <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(AccountCollection), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAccountsAsync([FromQuery] GetCollectionRequest request)
        {
            var query = new GetAccounts.Query(request);
            AccountCollection response = await mediator.Send(query);
            return Ok(response);
        }

        /// <summary>
        /// Returns account id of newly created account.
        /// </summary>
        /// <param name="request">The request to create account.</param>
        /// <returns>A <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CreateAccountResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAccountsAsync([FromBody] CreateAccountRequest request)
        {
            var accountCreationCommand = new CreateAccount.Command { Request = request };
            CreateAccountResponse response = await mediator.Send(accountCreationCommand);
            return Ok(response);
        }
    }
}
