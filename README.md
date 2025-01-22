# Todo.CleanArchitecture

Todo app based on Milan Jovanovic's course 'Pragmatic Clean Architecture' at https://www.courses.milanjovanovic.tech/courses

## Features
- Clean Architecture with four layers: Domain, Application, Infrastructure and WebApi.
- CQRS with MediatR: Command Query Responsibility Segregation.
- DDD: Domain-Driven Design.
- Outbox Pattern: persist domain events in the database and treat them later in another process.
- Database: MySql or Postgres.
- Caching: Redis or In-Memory.
- ORM: EFCore or Dapper.
- Healthchecks: for database and caching.
- Logging with Serilog and Seq.
- OpenApi / Swagger.
- Testing: Unit, Integration, Functional and Architecture.



## Solution Structure

### Project WebApi:
- Endpoints: sending requests, through the mediator pipeline, for every action (add, complete, delete, getall, getbyid)
- Infrastructure: CustomResults (helper for endpoints's result) and GlobalExceptionHandler.
- Middleware: RequestContextLoggingMiddleware (adds CorrelationId property to logging)
- OpenApi: ConfigureSwaggerGenOptions

### Project Infrastructure:
- Caching: CacheOptions (common expiration time), InMemoryCacheService, RedisCacheService
- Database: DbConnectionFactory (mysql or postgres), UnitOfWork (transactions in commands) and sql mappers for dapper (DataOnly and TodoItemTitle)
- Outbox: classes for Outbox pattern
- Repositories: implementation of Domain.ITodoItemsRepository
- Setup / Config: configuration of CacheType (In-Memory / Redis), ORM (Dapper / EF Core) and DatabaseType (MySql or Postgres)
- Time: implementation of SharedKernel.IDateTimeProvider

### Project Application:
- Classes for adding behaviors to the pipeline of MediatR: querycaching, requestlogging, validation.
- Interfaces for Caching: ICachedQuery, ICacheService.
- Interfaces for Data: IDbConnectionFactory, IUnitOfWork.
- Interfaces for Messaging (CQRS): ICommand, ICommandHandler, IQuery, IQueryHandler.
- TodoItems: commands, queries and handlers for every action (add, complete, delete, getall, getbyid)

### Project Domain:
- Domain-Driven Design
- Domain Errors
- Domain Events
- Value Objects (TodoItemTitle.cs)

### Project SharedKernel:
- Common classes and interfaces for the other projects.

### Test projects:
- Domain.UnitTests
- Application.UnitTests
- Application.IntegrationTests
- WebApi.FunctionalTests
- ArchitectureTests

### Getting Started:
- Choose cache, database and ORM at Infrastructure/Setup/Config
- Comment/Uncomment the db section of docker-compose based on the chosen database.
- The scripts for initializing the database are init-mysql.sql and init-postgres.sql
- In visual studio, select docker-compose as Startup Item

### Testing:
- Uses docker containers based on Infrastructure/Setup/Config
- Uses its own initializers scripts (init-mysql.sql and init-postgres.sql)


