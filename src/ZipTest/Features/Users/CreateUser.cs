using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZipTest.Db;
using DbModel = ZipTest.Db.Model;
using ZipTest.Models.Request;

namespace ZipTest.Features.Users
{
    /// <summary>
    /// Handler to add new user
    /// </summary>
    public static class CreateAccount
    {
        /// <summary>
        /// The command to add user.
        /// </summary>
        public class Command : IRequest<Models.Response.CreateUserResponse>
        {
            /// <summary>
            /// Gets or sets new user.
            /// </summary>
            public UserRequest User { get; set; }
        }

        /// <summary>
        /// Validates the new order request.
        /// </summary>
        public class CommandValidator : AbstractValidator<UserRequest>
        {
            private readonly IMediator mediator;
            private readonly ApplicationContext dbContext;


            /// <summary>
            /// Initializes a new instance of the <see cref="CommandValidator"/> class.
            /// </summary>
            /// <param name="mediator">The command mediator.</param>
            public CommandValidator(IMediator mediator, ApplicationContext applicationContext)
            {
                this.mediator = mediator;
                this.dbContext = applicationContext;

                // Validate Name
                RuleFor(m => m.Name).NotEmpty().WithErrorCode("ERR-U-1005").WithMessage("Please enter user's name.");
                RuleFor(m => m.Name).NotNull().WithErrorCode("ERR-U-1005").WithMessage("Please enter user's name.");

                // Validate MonthlyExpense
                RuleFor(m => m.MonthlyExpenses).GreaterThanOrEqualTo(0).WithErrorCode("ERR-U-1006").WithMessage("Please enter a valid monthly expenses.");

                // Validate MonthlySalary
                RuleFor(m => m.MonthlySalary).GreaterThanOrEqualTo(0).WithErrorCode("ERR-U-1006").WithMessage("Please enter a valid monthly salary.");

                // Validate EmailAddress
                RuleFor(m => m.EmailAddress).NotNull().WithErrorCode("ERR-U-1007").WithMessage("Please enter user's email address.");
                RuleFor(m => m.EmailAddress).NotEmpty().WithErrorCode("ERR-U-1007").WithMessage("Please enter user's email address.");
                RuleFor(m => m.EmailAddress).EmailAddress().WithErrorCode("ERR-U-1008").WithMessage("Please enter a valid email address.");

                // unique email address
                RuleFor(x => x.EmailAddress).Custom((email, context) =>
                {
                    var query = new GetUser.Query() { EmailAddress = email };
                    Models.Response.User response = mediator.Send(query).Result;
                    if (response != null)
                    {
                        context.AddFailure("Email address already exists.");
                    }
                });
            }
        }

        /// <summary>
        /// The AddUser request handler.
        /// </summary>
        public class CommandHandler : IRequestHandler<Command, Models.Response.CreateUserResponse>
        {
            private readonly ApplicationContext dbContext;
            private readonly IMediator mediator;

            /// <summary>
            /// Initializes a new instance of the <see cref="CommandHandler"/> class.
            /// </summary>
            /// <param name="dbContext">The db context.</param>
            public CommandHandler(ApplicationContext applicationContext, IMediator mediator)
            {
                dbContext = applicationContext;
                this.mediator = mediator;
            }

            /// <inheritdoc/>
            public async Task<Models.Response.CreateUserResponse> Handle(Command command, CancellationToken cancellationToken)
            {
                var request = command.User;
                dbContext.Users.Add(new DbModel.Users
                {
                    Email = request.EmailAddress,
                    MonthlyExpenses = request.MonthlyExpenses,
                    MonthlySalary = request.MonthlySalary,
                    Name = request.Name,
                    CreatedDt = DateTime.Now,
                    LastModifiedDt = DateTime.Now
                });

                await dbContext.SaveChangesAsync(default);

                var query = new GetUser.Query() { EmailAddress = request.EmailAddress };
                Models.Response.User userDetails = await mediator.Send(query);

                return new Models.Response.CreateUserResponse
                {
                    Id = userDetails.Id
                };
            }
        }
    }
}
