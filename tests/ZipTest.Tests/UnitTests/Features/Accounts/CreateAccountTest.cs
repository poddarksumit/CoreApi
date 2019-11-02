using MediatR;
using System;
using Xunit;
using static ZipTest.Features.Accounts.CreateAccount;
using User = ZipTest.Features.Users.CreateUser;

namespace ZipTest.Tests.UnitTests.Features.Accounts
{
    public class CreateAccountTest : TestBase
    {
        [Fact]
        public void CreateAccount_ValidationFail_Empty()
        {
            using (var context = ContextFactory.CreateContext())
            {
                var validator = new CommandValidator(context, (IMediator)Provider.GetService(typeof(IMediator)), Configuration);

                Assert.False(validator.Validate(new Models.Request.CreateAccountRequest()).IsValid);
            }
        }

        [Fact]
        public void CreateAccount_ValidationFail_EmailNull()
        {
            using (var context = ContextFactory.CreateContext())
            {
                var validator = new CommandValidator(context, (IMediator)Provider.GetService(typeof(IMediator)), Configuration);

                Assert.False(validator.Validate(new Models.Request.CreateAccountRequest
                {
                    EmailAddress = null
                }).IsValid);
            }
        }

        [Fact]
        public void CreateAccount_ValidationFail_EmailEmpty()
        {
            using (var context = ContextFactory.CreateContext())
            {
                var validator = new CommandValidator(context, (IMediator)Provider.GetService(typeof(IMediator)), Configuration);

                Assert.False(validator.Validate(new Models.Request.CreateAccountRequest
                {
                    EmailAddress = string.Empty
                }).IsValid);
            }
        }

        [Fact]
        public void CreateAccount_ValidationFail_Emailinvalid()
        {
            using (var context = ContextFactory.CreateContext())
            {
                var validator = new CommandValidator(context, (IMediator)Provider.GetService(typeof(IMediator)), Configuration);

                Assert.False(validator.Validate(new Models.Request.CreateAccountRequest
                {
                    EmailAddress = "ddsda"
                }).IsValid);
            }
        }

        [Fact]
        public async void CreateAccount_ValidationFail_AccountExists()
        {
            using (var context = ContextFactory.CreateContext())
            {
                var emailId = $"{new Guid()}@test.com";
                var request = new Models.Request.UserRequest
                {
                    EmailAddress = emailId,
                    Name = "Test",
                    MonthlyExpenses = 1000,
                    MonthlySalary = 9000
                };


                var command = new User.Command
                {
                    User = request
                };

                var userHandler = new User.CommandHandler(context, (IMediator)Provider.GetService(typeof(IMediator)));
                var userCreateResponse = await userHandler.Handle(command, default);

                var accountCommand = new Command { Request = new Models.Request.CreateAccountRequest { EmailAddress = emailId } };

                var accountHandler = new CommandHandler(context, (IMediator)Provider.GetService(typeof(IMediator)));
                var accountCreateResponse = await accountHandler.Handle(accountCommand, default);

                var validator = new CommandValidator(context, (IMediator)Provider.GetService(typeof(IMediator)), Configuration);

                Assert.False(validator.Validate(new Models.Request.CreateAccountRequest
                {
                    EmailAddress = emailId
                }).IsValid);

            }
        }

        [Fact]
        public void CreateAccount_ValidationFail_EmailNotExists()
        {
            using (var context = ContextFactory.CreateContext())
            {
                var emailId = $"{new Guid()}@test.com";

                var validator = new CommandValidator(context, (IMediator)Provider.GetService(typeof(IMediator)), Configuration);

                Assert.False(validator.Validate(new Models.Request.CreateAccountRequest
                {
                    EmailAddress = emailId
                }).IsValid);

            }
        }

        [Fact]
        public async void CreateAccount_ValidationFail_CreditLimitNotMet()
        {
            using (var context = ContextFactory.CreateContext())
            {
                var emailId = $"{new Guid()}@test.com";
                var request = new Models.Request.UserRequest
                {
                    EmailAddress = emailId,
                    Name = "Test",
                    MonthlyExpenses = 1000,
                    MonthlySalary = 1500
                };


                var command = new User.Command
                {
                    User = request
                };

                var userHandler = new User.CommandHandler(context, (IMediator)Provider.GetService(typeof(IMediator)));
                var userCreateResponse = await userHandler.Handle(command, default);

                var validator = new CommandValidator(context, (IMediator)Provider.GetService(typeof(IMediator)), Configuration);

                Assert.False(validator.Validate(new Models.Request.CreateAccountRequest
                {
                    EmailAddress = emailId
                }).IsValid);

            }
        }

        [Fact]
        public async void CreateAccount_Success()
        {
            using (var context = ContextFactory.CreateContext())
            {
                var emailId = $"{new Guid()}@test.com";
                var request = new Models.Request.UserRequest
                {
                    EmailAddress = emailId,
                    Name = "Test",
                    MonthlyExpenses = 1000,
                    MonthlySalary = 2500
                };


                var command = new User.Command
                {
                    User = request
                };

                var userHandler = new User.CommandHandler(context, (IMediator)Provider.GetService(typeof(IMediator)));
                var userCreateResponse = await userHandler.Handle(command, default);

                var accountCommand = new Command { Request = new Models.Request.CreateAccountRequest { EmailAddress = emailId } };

                var accountHandler = new CommandHandler(context, (IMediator)Provider.GetService(typeof(IMediator)));
                var accountCreationResponse = await accountHandler.Handle(accountCommand, default);

                Assert.NotNull(accountCreationResponse);
                Assert.NotEqual(0, accountCreationResponse.AccountId);

            }
        }
    }
}
