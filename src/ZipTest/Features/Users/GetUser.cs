using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using ZipTest.Db;
using ZipTest.Models.Response;
using DbModel = ZipTest.Db.Model;

namespace ZipTest.Features.Users
{
    /// <summary>
    /// The handler for returning requested user details
    /// </summary>
    public static class GetUser
    {
        /// <summary>
        /// The request query.
        /// </summary>
        public class Query : IRequest<User>
        {
            /// <summary>
            /// Gets or Sets Request.
            /// </summary>
            public string EmailAddress { get; set; }
        }

        /// <summary>
        /// Provides validation of users requests.
        /// </summary>
        public class QueryValidator : AbstractValidator<string>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="QueryValidator"/> class.
            /// </summary>
            public QueryValidator()
            {
                RuleFor(q => q).NotNull().WithErrorCode("ERR-U-1003").WithMessage("Please enter user's email address");
                RuleFor(q => q).NotEmpty().WithErrorCode("ERR-U-1003").WithMessage("Please enter user's email address");
                RuleFor(q => q).EmailAddress().WithErrorCode("ERR-U-1004").WithMessage("Please enter a valid email address");
            }
        }



        /// <summary>
        /// Handles user query requests.
        /// </summary>
        public class QueryHandler : IRequestHandler<Query, User>
        {
            private readonly ApplicationContext dbContext;
            private readonly IMapper mapper;

            /// <summary>
            /// Initializes a new instance of the <see cref="QueryHandler"/> class.
            /// </summary>
            /// <param name="dbContext">The database context.</param>
            public QueryHandler(ApplicationContext applicationContext, IMapper mapperProfile)
            {
                dbContext = applicationContext;
                mapper = mapperProfile;
            }

            /// <inheritdoc/>
            public async Task<User> Handle(Query query, CancellationToken cancellationToken)
            {
                DbModel.Users dbUser = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => string.Equals(x.Email, query.EmailAddress, System.StringComparison.OrdinalIgnoreCase));

                return (dbUser == null)? null : mapper.Map<User>(dbUser);
            }
        }
    }
}
