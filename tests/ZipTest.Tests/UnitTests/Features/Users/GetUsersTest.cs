using System.Linq;
using Xunit;
using ZipTest.Features.Users;

namespace ZipTest.Tests.IntegrationTests.Features.Users
{
    public class GetUsersTest : TestBase
    {
        [Fact]
        public void GetUsers_ValidationPass()
        {
            var queryValidator = new GetUsers.QueryValidator();

            var invalidQuery = new GetUsers.Query(new Models.GetCollectionRequest());
            Assert.True(queryValidator.Validate(invalidQuery.Request).IsValid);
        }

        [Fact]
        public void GetUsers_ValidationFail_PageNumberNeg()
        {
            var queryValidator = new GetUsers.QueryValidator();

            var invalidQuery = new GetUsers.Query(new Models.GetCollectionRequest
            {
                PageNumber = -1
            }) ;
            Assert.False(queryValidator.Validate(invalidQuery.Request).IsValid);
        }

        [Fact]
        public void GetUsers_ValidationFail_PageNumberZero()
        {
            var queryValidator = new GetUsers.QueryValidator();

            var invalidQuery = new GetUsers.Query(new Models.GetCollectionRequest
            {
                PageNumber = 0
            });
            Assert.False(queryValidator.Validate(invalidQuery.Request).IsValid);
        }

        [Fact]
        public void GetUsers_ValidationFail_PageSizeNeg()
        {
            var queryValidator = new GetUsers.QueryValidator();

            var invalidQuery = new GetUsers.Query(new Models.GetCollectionRequest
            {
                PageSize = -1
            });
            Assert.False(queryValidator.Validate(invalidQuery.Request).IsValid);
        }

        [Fact]
        public void GetUsers_ValidationFail_PageSizeZero()
        {
            var queryValidator = new GetUsers.QueryValidator();

            var invalidQuery = new GetUsers.Query(new Models.GetCollectionRequest
            {
                PageSize = 0
            });
            Assert.False(queryValidator.Validate(invalidQuery.Request).IsValid);
        }

        [Fact]
        public async void Test_GetUsers()
        {
            using (var context = ContextFactory.CreateContext())
            {
                Assert.Equal(0, context.Users.Count());

                for (int i = 0; i < 50; i++)
                {
                    var user = new Db.Model.Users
                    {
                        Email = $"test.{i}@test.com",
                        Name = $"test.{i}"
                    };
                    context.Users.Add(user);
                }
                context.SaveChanges();

                Assert.Equal(50, context.Users.Count());

                var query = new GetUsers.Query(new Models.GetCollectionRequest());

                var handler = new GetUsers.QueryHandler(context);
                var users = await handler.Handle(query, default);

                Assert.NotNull(users);
                Assert.Equal(50, users.TotalItemCount);
                Assert.Equal(5, users.PageCount);
                Assert.Equal(10, users.Users.Count());
                Assert.Equal(10, users.Users.Last().Id);

                query = new GetUsers.Query(new Models.GetCollectionRequest
                {
                    PageNumber = 2
                });

                handler = new GetUsers.QueryHandler(context);
                users = await handler.Handle(query, default);

                Assert.NotNull(users); 
                Assert.Equal(20, users.Users.Last().Id);

            }
        }
    }
}
