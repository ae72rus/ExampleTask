# Описание
Пример выполнения задачи 1 из тестового задания.
В задаче ничего не сказано про возможность/невозможность использования ORM:
	- я применил EF Core, подход CodeFirst и миграции

В задаче не уточнено нужно ли логгировать информацию о запросах и ответах в БД с данными или отдельную:
	- сделал компромиссный вариант: использовал одну БД с разными контекстами. При необходимости логи можно вынести в отдельную БД за 10 минут

В задаче сказано "перед сохранением данных таблицу необходимо очистить"
	- сделал через TRUNCATE, а не через удаление всех строк

По структуре решения:
	- Для понимания решения нужно смотреть с проекта Zvonarev.FinBeat.Test.BusinessLogic, где в UseCases расположены реализации фичей с разделением из задания/бизнес требований
	- В решении использован CQRS и Mediator. Логика выполнения команд/запросов всегда лежит рядом с объектами самих команд/запросов и в названии добавлено слово "Handler"
	- Выбор такой структуры решения обусловлен стремлением к "чистой" реализации, для максимального упрощения решения (просто != легко) с точки зрения отражения заданных требований и возможности изхменения бизнес логики
	- Предоставлен минимальный набор тестов. Тесты обработчиков команд с использованием замоканных зависимостей (например БД) не представлены из-за необходимости сэкономить время
	- К WebAPI подключена Swagger документация. Для метода записи данных использован пример JSON из задания

- Очистка таблицы и запись данных осуществляется в рамках транзакции, которая коммитится при завершении вызова. Реализовано через отдельный middleware
- Логгирование в БД реализовано через отдельный middleware

Структуры таблиц БД:
 Таблица данных:
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

Задача 2
 Запрос 1:
```
select 
	cl.ClientName,
	count(cl.Id)
from test.ClientContacts cc
join test.Clients cl on cc.ClientId = cl.Id
group by cl.Id, cl.ClientName
```

Запрос 2:
```
select * from test.Clients where Id in (
	select 
		cl.Id
	from test.ClientContacts cc
	join test.Clients cl on cc.ClientId = cl.Id
	group by cl.Id
	having count(cl.Id) > 2
)
```

Задача 3
Не в зачет т.к. мне помог чат GPT, благодаря чему я открыл для себя LEAD OVER
```
select 
Id,
Sd,
Nd Ed
from (
	select 
	Id,
	Dt Sd,
	LEAD(Dt) Over (Partition by Id order by Dt) Nd
	from test.ClientDates
) x
where Nd is not null
```
