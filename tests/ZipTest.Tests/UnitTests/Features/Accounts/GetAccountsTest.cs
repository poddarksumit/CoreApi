using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Xunit;
using ZipTest.Features.Accounts;

namespace ZipTest.Tests.UnitTests.Features.Accounts
{
    public class GetAccountsTest : TestBase
    {
        [Fact]
        public void GetAccounts_ValidationPass()
        {
            var queryValidator = new GetAccounts.QueryValidator();

            var invalidQuery = new GetAccounts.Query(new Models.GetCollectionRequest());
            Assert.True(queryValidator.Validate(invalidQuery.Request).IsValid);
        }

        [Fact]
        public void GetAccounts_ValidationFail_PageNumberNeg()
        {
            var queryValidator = new GetAccounts.QueryValidator();

            var invalidQuery = new GetAccounts.Query(new Models.GetCollectionRequest
            {
                PageNumber = -1
            });
            Assert.False(queryValidator.Validate(invalidQuery.Request).IsValid);
        }

        [Fact]
        public void GetAccounts_ValidationFail_PageNumberZero()
        {
            var queryValidator = new GetAccounts.QueryValidator();

            var invalidQuery = new GetAccounts.Query(new Models.GetCollectionRequest
            {
                PageNumber = 0
            });
            Assert.False(queryValidator.Validate(invalidQuery.Request).IsValid);
        }

        [Fact]
        public void GetAccounts_ValidationFail_PageSizeNeg()
        {
            var queryValidator = new GetAccounts.QueryValidator();

            var invalidQuery = new GetAccounts.Query(new Models.GetCollectionRequest
            {
                PageSize = -1
            });
            Assert.False(queryValidator.Validate(invalidQuery.Request).IsValid);
        }

        [Fact]
        public void GetAccounts_ValidationFail_PageSizeZero()
        {
            var queryValidator = new GetAccounts.QueryValidator();

            var invalidQuery = new GetAccounts.Query(new Models.GetCollectionRequest
            {
                PageSize = 0
            });
            Assert.False(queryValidator.Validate(invalidQuery.Request).IsValid);
        }

        [Fact]
        public async void Test_GetAccounts()
        {
            using (var context = ContextFactory.CreateContext())
            {
                Assert.Equal(0, context.Accounts.Count());

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

                List<Db.Model.Users> users = context.Users.AsNoTracking().ToList();
                foreach (Db.Model.Users user in users)
                {
                    context.Accounts.Add(new Db.Model.Accounts
                    {
                        AccountUserId = user.UserId,
                        User = user
                    });
                }
                context.SaveChanges();

                Assert.Equal(50, context.Accounts.Count());

                var query = new GetAccounts.Query(new Models.GetCollectionRequest());

                var handler = new GetAccounts.QueryHandler(context);
                var Accounts = await handler.Handle(query, default);

                Assert.NotNull(Accounts);
                Assert.Equal(50, Accounts.TotalItemCount);
                Assert.Equal(5, Accounts.PageCount);
                Assert.Equal(10, Accounts.Accounts.Count());
                Assert.Equal(10, Accounts.Accounts.Last().AccountId);

                query = new GetAccounts.Query(new Models.GetCollectionRequest
                {
                    PageNumber = 2
                });

                handler = new GetAccounts.QueryHandler(context);
                Accounts = await handler.Handle(query, default);

                Assert.NotNull(Accounts);
                Assert.Equal(20, Accounts.Accounts.Last().AccountId);

            }
        }
    }
}
