# Description 

Example of completing task 1 from the test task.

The task does not say anything about the possibility/impossibility of using ORM:
- I used EF Core, CodeFirst approach and migrations

The task does not specify whether it is necessary to log information about requests and responses in the DB with data or a separate one:
- I made a compromise option: I used one DB with different contexts. If necessary, the logs can be moved to a separate DB in 10 minutes

The task says "before saving the data, the table must be cleared"
- I did it through TRUNCATE, and not by deleting all rows

## About the structure of the solution:
- To understand the solution, you need to look at the Zvonarev.FinBeat.Test.BusinessLogic project, where UseCases contains implementations of features with separation from the task/business requirements
- The solution uses CQRS and Mediator. The logic of executing commands/queries is always located next to the objects of the commands/queries themselves and the word "Handler" is added to the name
- The choice of such a solution structure is due to the desire for a "clean" implementation, to simplify the solution as much as possible (simple != easy) in terms of reflecting the specified requirements and the ability to change the business logic
- A minimal set of tests is provided. Tests of command handlers using mocked dependencies (for example, a database) are not provided due to the need to save time
- Swagger documentation is connected to WebAPI. The JSON example from the task is used for the data recording method

- Table cleaning and data recording is carried out within a transaction that is committed when the call is completed. Implemented through a separate middleware
- Logging in the database is implemented through a separate middleware

## DB table structures:

 ```
 CREATE TABLE [dbo].[DataEntries](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[Code] [int] NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_DataEntries] PRIMARY KEY CLUSTERED ([Id] ASC)
 )

 CREATE NONCLUSTERED INDEX [IX_DataEntries_Code] ON [dbo].[DataEntries]
 (
	[Code] ASC
 )

 CREATE UNIQUE NONCLUSTERED INDEX [IX_DataEntries_OrderId] ON [dbo].[DataEntries]
 (
	[OrderId] ASC
 )
 ```
 Таблица логов:
 ```
 CREATE TABLE [log].[Api2](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ReferenceId] [nvarchar](450) NOT NULL,
	[HttpId] [nvarchar](450) NOT NULL,
	[InitiatorIp] [nvarchar](450) NOT NULL,
	[Method] [nvarchar](450) NOT NULL,
	[Url] [nvarchar](450) NOT NULL,
	[Payload] [nvarchar](max) NULL,
	[ResponseCode] [int] NOT NULL,
	[Response] [nvarchar](max) NULL,
	[ResponseTime] [time](7) NOT NULL,
	[Headers] [nvarchar](max) NOT NULL DEFAULT (N'')
 CONSTRAINT [PK_Api2] PRIMARY KEY CLUSTERED ([Id] ASC)
 )
 ```
