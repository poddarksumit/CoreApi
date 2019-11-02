# ZipTest
 
This project is the assessment for an oppurtunity. The project is build on
- ASP.Net Core 2.2
- MS SQL (localdb)

The APIs are build using Feature pattern where all the feature are divided into it's own feature class. This helps in seperating the logic and avoid clutering the classes and funtionalities.

## What's Inside?
This project includes:

- APIs for users to
  - Create
  - Get
  - List
- APIs for accounts to
  - Create
  - List
- Unit test cases for all APIs

### Packages.

This project uses the following key nuget packages

- EF Core
  - For accessing database 
- FluentValidation
  - An easy way to validate request before request hits controller.
``` c#
RuleFor(m => m.EmailAddress).NotNull().WithErrorCode("ERR-U-1007").WithMessage("Please enter user's email address.");
RuleFor(m => m.EmailAddress).NotEmpty().WithErrorCode("ERR-U-1007").WithMessage("Please enter user's email address.");
RuleFor(m => m.EmailAddress).EmailAddress().WithErrorCode("ERR-U-1008").WithMessage("Please enter a valid email address.");
```
- AutoMapper
 - For mapping the DB model to response model
``` c#
mapper.Map<User>(dbUser)
```
- MediatR
 - Framework for writing API funtionality where we have to focus on the business logic instead of how it will be called.
- X.PagedList
 - Framework to handle pagination
``` c#
IPagedList<UserBase> pagedUsers = await users.ToPagedListAsync(query.Request.PageNumber, query.Request.PageSize, cancellationToken);
```
- Swashbuckle.AspNetCore
 - For Swagger
 
### Database.

This project currently works on MSSQL LocalDB.

- Table: Accounts
  - AccountId Int PrimaryKey
  - AccountUserId Int ForiegnKey
  - MonthlySalary money
  - MonthlyExpenses money
  - LastModifiedDt datetime
  - CreatedDt datetime
 
- Table: Users
  - UserId Int PrimaryKey
  - Name varchar(100)
  - Email varchar(250)
  - MonthlySalary money
  - MonthlyExpenses money
  - LastModifiedDt datetime
  - CreatedDt datetime
 
