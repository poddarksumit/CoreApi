using MediatR;
using System;
using Xunit;
using static ZipTest.Features.Users.CreateUser;

namespace ZipTest.Tests.UnitTest.Features.Users
{
    public class CreateUserTest : TestBase
    {
        [Fact]
        public void CreateUser_ValidationFail_Empty()
        {
            var validator = new CommandValidator((IMediator)Provider.GetService(typeof(IMediator)));

            Assert.False(validator.Validate(new Models.Request.UserRequest()).IsValid);
        }

        [Fact]
        public void CreateUser_ValidationFail_NameNull()
        {
            var validator = new CommandValidator((IMediator)Provider.GetService(typeof(IMediator)));

            Assert.False(validator.Validate(new Models.Request.UserRequest
            {
                Name = null
            }).IsValid);
        }

        [Fact]
        public void CreateUser_ValidationFail_NameEmpty()
        {
            var validator = new CommandValidator((IMediator)Provider.GetService(typeof(IMediator)));

            Assert.False(validator.Validate(new Models.Request.UserRequest
            {
                Name = string.Empty
            }).IsValid);
        }

        [Fact]
        public void CreateUser_ValidationFail_MonthlySalaryNeg()
        {
            var validator = new CommandValidator((IMediator)Provider.GetService(typeof(IMediator)));

            Assert.False(validator.Validate(new Models.Request.UserRequest
            {
                MonthlySalary = -100
            }).IsValid);
        }

        [Fact]
        public void CreateUser_ValidationFail_MonthlyExpensesNeg()
        {
            var validator = new CommandValidator((IMediator)Provider.GetService(typeof(IMediator)));

            Assert.False(validator.Validate(new Models.Request.UserRequest
            {
                MonthlyExpenses = -90
            }).IsValid);
        }

        [Fact]
        public void CreateUser_ValidationFail_EmailNull()
        {
            var validator = new CommandValidator((IMediator)Provider.GetService(typeof(IMediator)));

            Assert.False(validator.Validate(new Models.Request.UserRequest
            {
                EmailAddress = null
            }).IsValid);
        }

        [Fact]
        public void CreateUser_ValidationFail_EmailEmpty()
        {
            var validator = new CommandValidator((IMediator)Provider.GetService(typeof(IMediator)));

            Assert.False(validator.Validate(new Models.Request.UserRequest
            {
                EmailAddress = string.Empty
            }).IsValid);
        }

        [Fact]
        public void CreateUser_ValidationFail_EmailInvalid()
        {
            var validator = new CommandValidator((IMediator)Provider.GetService(typeof(IMediator)));

            Assert.False(validator.Validate(new Models.Request.UserRequest
            {
                EmailAddress = "Sdads"
            }).IsValid);
        }

        [Fact]
        public void CreateUser_ValidationFail_EmailInvalid1()
        {
            var validator = new CommandValidator((IMediator)Provider.GetService(typeof(IMediator)));

            Assert.False(validator.Validate(new Models.Request.UserRequest
            {
                EmailAddress = "Sdads@.com"
            }).IsValid);
        }

        [Fact]
        public void CreateUser_ValidationTrue()
        {
            var validator = new CommandValidator((IMediator)Provider.GetService(typeof(IMediator)));

            Assert.True(validator.Validate(new Models.Request.UserRequest
            {
                Name = "Test",
                EmailAddress = "Sdads@s.com",
                MonthlyExpenses = 1000,
                MonthlySalary = 9000
            }).IsValid);
        }

        [Fact]
        public void CreateUser_ValidationFail_EmailAlreadyAvail()
        {
            using (var context = ContextFactory.CreateContext())
            {
                context.Users.Add(new Db.Model.Users
                {
                    Email = $"test.test@test.com",
                    Name = "Test"
                });
                context.SaveChanges();

                var request = new Models.Request.UserRequest
                {
                    EmailAddress = "test.test@test.com"
                };

                var validator = new CommandValidator((IMediator)Provider.GetService(typeof(IMediator)));
                Assert.False(validator.Validate(new Models.Request.UserRequest
                {
                    EmailAddress = "test.test@test.com"
                }).IsValid);

            }
        }

        [Fact]
        public async void CreateUser_Success()
        {
            using (var context = ContextFactory.CreateContext())
            {

                var request = new Models.Request.UserRequest
                {
                    EmailAddress = $"{new Guid()}@test.com",
                    Name = "Test",
                    MonthlyExpenses = 1000,
                    MonthlySalary = 9000
                };


                var command = new Command
                {
                    User = request
                };

                var handler = new CommandHandler(context, (IMediator)Provider.GetService(typeof(IMediator)));
                var userCreateResponse = await handler.Handle(command, default);

                Assert.NotNull(userCreateResponse);
                Assert.NotEqual(0, userCreateResponse.Id);

            }


        }
    }
}
