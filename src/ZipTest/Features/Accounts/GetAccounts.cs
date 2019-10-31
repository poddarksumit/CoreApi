using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using X.PagedList;
using ZipTest.Db;
using ZipTest.Models;
using ZipTest.Models.Response;

namespace ZipTest.Features.Accounts
{
    /// <summary>
    /// The handler to get accounts.
    /// </summary>
    public static class GetAccounts
    {
        /// <summary>
        /// The request query.
        /// </summary>
        public class Query : IRequest<AccountCollection>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Query"/> class.
            /// </summary>
            /// <param name="request">The AccountCollectionRequest.</param>
            public Query(GetCollectionRequest request) => Request = request;

            /// <summary>
            /// Gets or Sets Request.
            /// </summary>
            public GetCollectionRequest Request { get; }
        }

        /// <summary>
        /// Provides validation of users requests.
        /// </summary>
        public class QueryValidator : AbstractValidator<GetCollectionRequest>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="QueryValidator"/> class.
            /// </summary>
            public QueryValidator()
            {
                RuleFor(m => m.PageNumber).GreaterThan(0);
                RuleFor(m => m.PageSize).GreaterThan(0);
            }
        }



        /// <summary>
        /// Handles user query requests.
        /// </summary>
        public class QueryHandler : IRequestHandler<Query, AccountCollection>
        {
            private readonly ApplicationContext dbContext;

            /// <summary>
            /// Initializes a new instance of the <see cref="QueryHandler"/> class.
            /// </summary>
            /// <param name="dbContext">The database context.</param>
            /// <param name="httpContextAccessor">The httpContextAccessor.</param>
            public QueryHandler(ApplicationContext applicationContext)
            {
                dbContext = applicationContext;
            }

            /// <inheritdoc/>
            public async Task<AccountCollection> Handle(Query query, CancellationToken cancellationToken)
            {
                IQueryable<Account> accounts = dbContext.Accounts.AsNoTracking().Include(x => x.User).Select(
                        x => new Account()
                        {
                            AccountId = x.AccountId,
                            User = new User
                            {
                                Id = x.User.UserId,
                                EmailAddress = x.User.Email,
                                Name = x.User.Name,
                                MonthlySalary = x.User.MonthlySalary,
                                MonthlyExpenses = x.User.MonthlyExpenses
                            }
                        }
                    );

                IPagedList<Account> pagedAccounts = await accounts.ToPagedListAsync(query.Request.PageNumber, query.Request.PageSize, cancellationToken);

                AccountCollection response = new AccountCollection
                {
                    Accounts = pagedAccounts,
                    TotalItemCount = pagedAccounts.TotalItemCount,
                    PageNumber = pagedAccounts.PageNumber,
                    PageCount = pagedAccounts.PageCount,
                    PageSize = pagedAccounts.PageSize
                };
                return response;
            }
        }
    }
}
