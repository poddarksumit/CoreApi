using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZipTest.Db;
using ZipTest.Features.Users;
using ZipTest.Models.Request;
using ZipTest.Models.Response;

namespace ZipTest.Features.Accounts
{
    /// <summary>
    /// Handler to add new account
    /// </summary>
    public static class CreateAccount
    {
        /// <summary>
        /// The command to add user.
        /// </summary>
        public class Command : IRequest<CreateAccountResponse>
        {
            /// <summary>
            /// Gets or sets details of new account.
            /// </summary>
            public CreateAccountRequest Request { get; set; }
        }

        /// <summary>
        /// Validates the new order request.
        /// </summary>
        public class CommandValidator : AbstractValidator<CreateAccountRequest>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CommandValidator"/> class.
            /// </summary>
            /// <param name="mediator">The command mediator.</param>
            public CommandValidator(ApplicationContext applicationContext, IMediator mediator, IConfiguration configuration)
            {
                // Validate EmailAddress
                RuleFor(m => m.EmailAddress).NotNull().WithErrorCode("ERR-A-1007").WithMessage("Please enter user's email address.");
                RuleFor(m => m.EmailAddress).NotEmpty().WithErrorCode("ERR-A-1007").WithMessage("Please enter user's email address.");
                RuleFor(m => m.EmailAddress).EmailAddress().WithErrorCode("ERR-A-1008").WithMessage("Please enter a valid email address.");

                // Check if account is already existing.
                RuleFor(m => m.EmailAddress).Custom((email, context) =>
                {
                    var user = applicationContext.Accounts.Include(x => x.User).FirstOrDefault(x => x.User.Email == email);
                    if (user != null)
                    {
                        context.AddFailure("Account has already been created with this email address.");
                    }
                });

                // Check if user exists and credit check passed.
                RuleFor(m => m.EmailAddress).Custom((email, context) =>
                {
                    var query = new GetUser.Query() { EmailAddress = email };
                    User userDetails = mediator.Send(query).Result;
                    
                    if (userDetails == null)
                    {
                        context.AddFailure("User with the corresponsing email address does not exists.");
                    }
                    else if (userDetails.MonthlySalary - userDetails.MonthlyExpenses <= double.Parse(configuration["creditLimit"]))
                    {
                        context.AddFailure("Credit limit criteria is not met.");
                    }
                });
            }
        }

        /// <summary>
        /// The create account request handler.
        /// </summary>
        public class CommandHandler : IRequestHandler<Command, CreateAccountResponse>
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
            public async Task<CreateAccountResponse> Handle(Command command, CancellationToken cancellationToken)
            {
                var query = new GetUser.Query() { EmailAddress = command.Request.EmailAddress };
                User userDetails = await mediator.Send(query);

                dbContext.Accounts.Add(new Db.Model.Accounts
                {
                    AccountUserId = userDetails.Id,
                    MonthlyExpenses = userDetails.MonthlyExpenses,
                    MonthlySalary = userDetails.MonthlySalary,
                    CreatedDt = DateTime.Now,
                    LastModifiedDt = DateTime.Now
                });

                await dbContext.SaveChangesAsync();

                var account = await dbContext.Accounts.AsNoTracking().FirstOrDefaultAsync(x => x.AccountUserId == userDetails.Id);

                return new CreateAccountResponse
                {
                    AccountId = account.AccountId
                };
            }
        }
    }
}
