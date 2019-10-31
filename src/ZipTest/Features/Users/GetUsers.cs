using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using X.PagedList;
using ZipTest.Db;
using ZipTest.Models;
using ZipTest.Models.Response;

namespace ZipTest.Features.Users
{
    /// <summary>
    /// The handler for returning lists of users
    /// </summary>
    public static class GetUsers
    {
        /// <summary>
        /// The request query.
        /// </summary>
        public class Query : IRequest<UserCollection>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Query"/> class.
            /// </summary>
            /// <param name="request">The UserCollectionRequest.</param>
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
                RuleFor(m => m.PageNumber).GreaterThan(0).WithErrorCode("ERR-U-1001").WithMessage("Please enter valid PageNumber");
                RuleFor(m => m.PageSize).GreaterThan(0).WithErrorCode("ERR-U-1002").WithMessage("Please enter valid PageSize");
            }
        }

        /// <summary>
        /// Handles user query requests.
        /// </summary>
        public class QueryHandler : IRequestHandler<Query, UserCollection>
        {
            private readonly ApplicationContext dbContext;

            /// <summary>
            /// Initializes a new instance of the <see cref="QueryHandler"/> class.
            /// </summary>
            /// <param name="dbContext">The database context.</param>
            public QueryHandler(ApplicationContext applicationContext)
            {
                dbContext = applicationContext;
            }

            /// <inheritdoc/>
            public async Task<UserCollection> Handle(Query query, CancellationToken cancellationToken)
            {
                IQueryable<UserBase> users = dbContext.Users.AsNoTracking().Select(
                        x => new UserBase()
                        {
                            Id = x.UserId,
                            EmailAddress = x.Email,
                            Name = x.Name
                        }
                    );

                IPagedList<UserBase> pagedUsers = await users.ToPagedListAsync(query.Request.PageNumber, query.Request.PageSize, cancellationToken);

                UserCollection response = new UserCollection
                {
                    Users = pagedUsers,
                    TotalItemCount = pagedUsers.TotalItemCount,
                    PageNumber = pagedUsers.PageNumber,
                    PageCount = pagedUsers.PageCount,
                    PageSize = pagedUsers.PageSize
                };

                return response;
            }
        }
    }
}
