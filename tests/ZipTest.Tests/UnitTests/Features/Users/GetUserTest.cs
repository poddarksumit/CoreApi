using System.Linq;
using Xunit;
using ZipTest.Features.Users;

namespace ZipTest.Tests.UnitTests.Features.Users
{
    public class GetUserTest : TestBase
    {

        [Fact]
        public void GetUser_ValidationFail_Empty()
        {
            var queryValidator = new GetUser.QueryValidator();

            var invalidQuery = new GetUser.Query() { EmailAddress = "" };
            Assert.False(queryValidator.Validate(invalidQuery.EmailAddress).IsValid);
        }

        [Fact]
        public void GetUser_ValidationFail_InvalidEmailFormat()
        {
            var queryValidator = new GetUser.QueryValidator();

            var invalidQuery = new GetUser.Query() { EmailAddress = "adasdasds" };
            Assert.False(queryValidator.Validate(invalidQuery.EmailAddress).IsValid);
        }

        [Fact]
        public void GetUser_ValidationFail_InvalidEmailFormat1()
        {
            var queryValidator = new GetUser.QueryValidator();

            var invalidQuery = new GetUser.Query() { EmailAddress = "adasdasds@.com" };
            Assert.False(queryValidator.Validate(invalidQuery.EmailAddress).IsValid);
        }

        [Fact]
        public void GetUser_ValidationPass_EmailFormat()
        {
            var queryValidator = new GetUser.QueryValidator();

            var invalidQuery = new GetUser.Query() { EmailAddress = "adas@dasds.com" };
            Assert.True(queryValidator.Validate(invalidQuery.EmailAddress).IsValid);
        }

        [Fact]
        public async void TestGetUser_Pass()
        {
            using (var context = ContextFactory.CreateContext())
            {
                Assert.Equal(0, context.Users.Count());

                for (int i = 0; i < 5; i++)
                {
                    var user = new Db.Model.Users
                    {
                        Email = $"test.{i}@test.com",
                        Name = "Test"
                    };
                    context.Users.Add(user);
                }
                context.SaveChanges();

                Assert.Equal(5, context.Users.Count());

                var queryValidator = new GetUser.QueryValidator();

                var query = new GetUser.Query { EmailAddress = "test.2@test.com" };
                Assert.True(queryValidator.Validate(query.EmailAddress).IsValid);

                var handler = new GetUser.QueryHandler(context, AutoMapper.Mapper.Instance);
                var userDetails = await handler.Handle(query, default);

                Assert.NotNull(userDetails);
            }
        }

        [Fact]
        public async void TestGetUser_Fail()
        {
            using (var context = ContextFactory.CreateContext())
            {
                Assert.Equal(0, context.Users.Count());

                for (int i = 0; i < 5; i++)
                {
                    var user = new Db.Model.Users
                    {
                        Email = $"test.{i}@test.com",
                        Name = "Test"
                    };
                    context.Users.Add(user);
                }
                context.SaveChanges();

                Assert.Equal(5, context.Users.Count());

                var queryValidator = new GetUser.QueryValidator();

                var query = new GetUser.Query { EmailAddress = "test.20@test.com" };
                Assert.True(queryValidator.Validate(query.EmailAddress).IsValid);

                var handler = new GetUser.QueryHandler(context, AutoMapper.Mapper.Instance);
                var userDetails = await handler.Handle(query, default);

                Assert.Null(userDetails);
            }
        }
    }
}
