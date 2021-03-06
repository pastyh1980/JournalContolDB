USE [journal]
GO
/****** Object:  Table [dbo].[check_backup]    Script Date: 15.11.2017 16:30:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[check_backup](
	[id] [int] NOT NULL,
	[reg_worker] [int] NOT NULL,
	[delete_worker] [int] NULL,
	[reg_date] [datetime] NOT NULL,
	[check_date] [date] NOT NULL,
	[check_worker] [varchar](50) NOT NULL,
	[check_subunit] [varchar](4) NOT NULL,
	[td_kd] [varchar](50) NOT NULL,
	[control_indicator] [varchar](255) NOT NULL,
	[count_operations] [int] NOT NULL,
	[fail_count] [varchar](11) NOT NULL,
	[fail_description] [varchar](255) NOT NULL,
	[isActive] [bit] NOT NULL,
	[isCorrect] [bit] NOT NULL,
	[delete_date] [datetime] NULL,
	[sector] [varchar](100) NULL,
	[isFail] [bit] NOT NULL,
	[isOld] [varchar](3) NULL,
	[primaty_key] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_check_backup] PRIMARY KEY CLUSTERED 
(
	[primaty_key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[events_backup]    Script Date: 15.11.2017 16:30:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[events_backup](
	[id] [int] NOT NULL,
	[check_id] [int] NOT NULL,
	[developer] [int] NOT NULL,
	[report_worker] [int] NULL,
	[delete_worker] [int] NULL,
	[fail_reason] [varchar](255) NULL,
	[description] [varchar](255) NOT NULL,
	[respons_worker] [varchar](100) NOT NULL,
	[due_date] [date] NOT NULL,
	[develop_date] [datetime] NOT NULL,
	[report] [varchar](15) NULL,
	[proof_inf] [varchar](255) NULL,
	[report_date] [datetime] NULL,
	[isActive] [bit] NOT NULL,
	[isCorrect] [bit] NOT NULL,
	[delete_date] [datetime] NULL,
	[isOld] [varchar](3) NULL,
	[primary_ket] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_events_backup] PRIMARY KEY CLUSTERED 
(
	[primary_ket] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Trigger [dbo].[deleteCheck]    Script Date: 15.11.2017 16:30:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE TRIGGER [dbo].[deleteCheck] ON [dbo].[check] --Указывается таблица, для которой будет действовать триггер
 FOR DELETE --Действие, на которое будет срабатывать триггер
 AS
 IF SYSTEM_USER = 'user' OR  SYSTEM_USER = 'admin'--Проверка текущего пользователя
	ROLLBACK TRAN; --Откат транзакции удаления
GO
ALTER TABLE [dbo].[check] ENABLE TRIGGER [deleteCheck]
GO
/****** Object:  Trigger [dbo].[insertCheck]    Script Date: 15.11.2017 16:30:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[insertCheck] ON [dbo].[check]
FOR INSERT
AS
IF SYSTEM_USER = 'admin'--Проверка текущего пользователя
	ROLLBACK TRAN; --Откат транзакции удаления
GO
ALTER TABLE [dbo].[check] ENABLE TRIGGER [insertCheck]
GO
/****** Object:  Trigger [dbo].[updateCheck]    Script Date: 15.11.2017 16:30:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[updateCheck] ON [dbo].[check]
FOR UPDATE
AS
IF SYSTEM_USER = 'admin'--Проверка текущего пользователя
	ROLLBACK TRAN; --Откат транзакции удаления
ELSE
BEGIN
	INSERT INTO dbo.check_backup (id, reg_worker, delete_worker, reg_date, check_date, check_worker, check_subunit, td_kd, control_indicator, 
	count_operations, fail_count, fail_description, isActive, isCorrect, delete_date, sector, isFail, isOld)
		SELECT deleted.*, 'OLD' FROM deleted;

	INSERT INTO dbo.check_backup (id, reg_worker, delete_worker, reg_date, check_date, check_worker, check_subunit, td_kd, control_indicator, 
	count_operations, fail_count, fail_description, isActive, isCorrect, delete_date, sector, isFail, isOld)
		SELECT inserted.*, 'NEW' FROM inserted;
END
GO
ALTER TABLE [dbo].[check] ENABLE TRIGGER [updateCheck]
GO
/****** Object:  Trigger [dbo].[deleteCheckBackup]    Script Date: 15.11.2017 16:30:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 CREATE TRIGGER [dbo].[deleteCheckBackup] ON [dbo].[check_backup] --Указывается таблица, для которой будет действовать триггер
 FOR DELETE --Действие, на которое будет срабатывать триггер
 AS
 IF SYSTEM_USER = 'user' OR  SYSTEM_USER = 'admin'--Проверка текущего пользователя
	ROLLBACK TRAN; --Откат транзакции удаления
GO
ALTER TABLE [dbo].[check_backup] ENABLE TRIGGER [deleteCheckBackup]
GO
/****** Object:  Trigger [dbo].[updateCheckBackup]    Script Date: 15.11.2017 16:30:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 CREATE TRIGGER [dbo].[updateCheckBackup] ON [dbo].[check_backup] --Указывается таблица, для которой будет действовать триггер
 FOR UPDATE --Действие, на которое будет срабатывать триггер
 AS
 IF SYSTEM_USER = 'user' OR  SYSTEM_USER = 'admin'--Проверка текущего пользователя
	ROLLBACK TRAN; --Откат транзакции удаления
GO
ALTER TABLE [dbo].[check_backup] ENABLE TRIGGER [updateCheckBackup]
GO
/****** Object:  Trigger [dbo].[deleteEvent]    Script Date: 15.11.2017 16:30:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE TRIGGER [dbo].[deleteEvent] ON [dbo].[events] --Указывается таблица, для которой будет действовать триггер
 FOR DELETE --Действие, на которое будет срабатывать триггер
 AS
 IF SYSTEM_USER = 'user' OR  SYSTEM_USER = 'admin'--Проверка текущего пользователя
	ROLLBACK TRAN; --Откат транзакции удаления
GO
ALTER TABLE [dbo].[events] ENABLE TRIGGER [deleteEvent]
GO
/****** Object:  Trigger [dbo].[insertEvent]    Script Date: 15.11.2017 16:30:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[insertEvent] ON [dbo].[events]
FOR INSERT
AS
IF SYSTEM_USER = 'admin'--Проверка текущего пользователя
	ROLLBACK TRAN; --Откат транзакции удаления
GO
ALTER TABLE [dbo].[events] ENABLE TRIGGER [insertEvent]
GO
/****** Object:  Trigger [dbo].[updateEvent]    Script Date: 15.11.2017 16:30:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[updateEvent] ON [dbo].[events]
FOR UPDATE
AS
IF SYSTEM_USER = 'admin'--Проверка текущего пользователя
	ROLLBACK TRAN; --Откат транзакции удаления
ELSE
BEGIN
	INSERT INTO dbo.events_backup (id, check_id, developer, report_worker, delete_worker, fail_reason, [description], 
	respons_worker, due_date, develop_date, report, proof_inf, report_date, isActive, isCorrect, delete_date, isOld)
		SELECT deleted.*, 'OLD' FROM deleted;

	INSERT INTO dbo.events_backup (id, check_id, developer, report_worker, delete_worker, fail_reason, [description], 
	respons_worker, due_date, develop_date, report, proof_inf, report_date, isActive, isCorrect, delete_date, isOld)
		SELECT inserted.*, 'NEW' FROM inserted;
END
GO
ALTER TABLE [dbo].[events] ENABLE TRIGGER [updateEvent]
GO
/****** Object:  Trigger [dbo].[deleteEventsBackup]    Script Date: 15.11.2017 16:30:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 CREATE TRIGGER [dbo].[deleteEventsBackup] ON [dbo].[events_backup] --Указывается таблица, для которой будет действовать триггер
 FOR DELETE --Действие, на которое будет срабатывать триггер
 AS
 IF SYSTEM_USER = 'user' OR  SYSTEM_USER = 'admin'--Проверка текущего пользователя
	ROLLBACK TRAN; --Откат транзакции удаления
GO
ALTER TABLE [dbo].[events_backup] ENABLE TRIGGER [deleteEventsBackup]
GO
/****** Object:  Trigger [dbo].[updateEventsBackup]    Script Date: 15.11.2017 16:30:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 CREATE TRIGGER [dbo].[updateEventsBackup] ON [dbo].[events_backup] --Указывается таблица, для которой будет действовать триггер
 FOR UPDATE --Действие, на которое будет срабатывать триггер
 AS
 IF SYSTEM_USER = 'user' OR  SYSTEM_USER = 'admin'--Проверка текущего пользователя
	ROLLBACK TRAN; --Откат транзакции удаления
GO
ALTER TABLE [dbo].[events_backup] ENABLE TRIGGER [updateEventsBackup]
GO
/****** Object:  Trigger [dbo].[deleteBoss]    Script Date: 15.11.2017 16:30:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE TRIGGER [dbo].[deleteBoss] ON [dbo].[grant_show] --Указывается таблица, для которой будет действовать триггер
 FOR DELETE --Действие, на которое будет срабатывать триггер
 AS
 IF SYSTEM_USER = 'user'--Проверка текущего пользователя
	ROLLBACK TRAN; --Откат транзакции удаления
GO
ALTER TABLE [dbo].[grant_show] ENABLE TRIGGER [deleteBoss]
GO
/****** Object:  Trigger [dbo].[insertBoss]    Script Date: 15.11.2017 16:30:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[insertBoss] ON [dbo].[grant_show]
FOR INSERT
AS
IF SYSTEM_USER = 'user'--Проверка текущего пользователя
	ROLLBACK TRAN; --Откат транзакции
GO
ALTER TABLE [dbo].[grant_show] ENABLE TRIGGER [insertBoss]
GO
/****** Object:  Trigger [dbo].[updateBoss]    Script Date: 15.11.2017 16:30:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE TRIGGER [dbo].[updateBoss] ON [dbo].[grant_show] --Указывается таблица, для которой будет действовать триггер
 FOR UPDATE --Действие, на которое будет срабатывать триггер
 AS
 IF SYSTEM_USER = 'user'--Проверка текущего пользователя
	ROLLBACK TRAN; --Откат транзакции удаления
GO
ALTER TABLE [dbo].[grant_show] ENABLE TRIGGER [updateBoss]
GO
/****** Object:  Trigger [dbo].[deleteShow]    Script Date: 15.11.2017 16:30:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE TRIGGER [dbo].[deleteShow] ON [dbo].[shows] --Указывается таблица, для которой будет действовать триггер
 FOR DELETE --Действие, на которое будет срабатывать триггер
 AS
 IF SYSTEM_USER = 'user' OR  SYSTEM_USER = 'admin'--Проверка текущего пользователя
	ROLLBACK TRAN; --Откат транзакции удаления
GO
ALTER TABLE [dbo].[shows] ENABLE TRIGGER [deleteShow]
GO
/****** Object:  Trigger [dbo].[insertShow]    Script Date: 15.11.2017 16:30:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[insertShow] ON [dbo].[shows]
FOR INSERT
AS
IF SYSTEM_USER = 'admin'--Проверка текущего пользователя
	ROLLBACK TRAN; --Откат транзакции
GO
ALTER TABLE [dbo].[shows] ENABLE TRIGGER [insertShow]
GO
/****** Object:  Trigger [dbo].[updateShow]    Script Date: 15.11.2017 16:30:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE TRIGGER [dbo].[updateShow] ON [dbo].[shows] --Указывается таблица, для которой будет действовать триггер
 FOR UPDATE --Действие, на которое будет срабатывать триггер
 AS
 IF SYSTEM_USER = 'user' OR SYSTEM_USER = 'admin'--Проверка текущего пользователя
	ROLLBACK TRAN; --Откат транзакции удаления
GO
ALTER TABLE [dbo].[shows] ENABLE TRIGGER [updateShow]
GO
/****** Object:  Trigger [dbo].[deleteSubunit]    Script Date: 15.11.2017 16:30:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE TRIGGER [dbo].[deleteSubunit] ON [dbo].[Subunits] --Указывается таблица, для которой будет действовать триггер
 FOR DELETE --Действие, на которое будет срабатывать триггер
 AS
 IF SYSTEM_USER = 'user' OR  SYSTEM_USER = 'admin'--Проверка текущего пользователя
	ROLLBACK TRAN; --Откат транзакции удаления
GO
ALTER TABLE [dbo].[Subunits] ENABLE TRIGGER [deleteSubunit]
GO
/****** Object:  Trigger [dbo].[insertSubunit]    Script Date: 15.11.2017 16:30:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[insertSubunit] ON [dbo].[Subunits]
FOR INSERT
AS
IF SYSTEM_USER = 'admin' OR SYSTEM_USER = 'user'--Проверка текущего пользователя
	ROLLBACK TRAN; --Откат транзакции
GO
ALTER TABLE [dbo].[Subunits] ENABLE TRIGGER [insertSubunit]
GO
/****** Object:  Trigger [dbo].[updateSubunit]    Script Date: 15.11.2017 16:30:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE TRIGGER [dbo].[updateSubunit] ON [dbo].[Subunits] --Указывается таблица, для которой будет действовать триггер
 FOR UPDATE --Действие, на которое будет срабатывать триггер
 AS
 IF SYSTEM_USER = 'user' OR SYSTEM_USER = 'admin'--Проверка текущего пользователя
	ROLLBACK TRAN; --Откат транзакции удаления
GO
ALTER TABLE [dbo].[Subunits] ENABLE TRIGGER [updateSubunit]
GO
/****** Object:  Trigger [dbo].[deleteWorker]    Script Date: 15.11.2017 16:30:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE TRIGGER [dbo].[deleteWorker] ON [dbo].[workers] --Указывается таблица, для которой будет действовать триггер
 FOR DELETE --Действие, на которое будет срабатывать триггер
 AS
 IF SYSTEM_USER = 'user'--Проверка текущего пользователя
	ROLLBACK TRAN; --Откат транзакции удаления
GO
ALTER TABLE [dbo].[workers] ENABLE TRIGGER [deleteWorker]
GO
/****** Object:  Trigger [dbo].[insertWorker]    Script Date: 15.11.2017 16:30:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[insertWorker] ON [dbo].[workers]
FOR INSERT
AS
IF SYSTEM_USER = 'user'--Проверка текущего пользователя
	ROLLBACK TRAN; --Откат транзакции
GO
ALTER TABLE [dbo].[workers] ENABLE TRIGGER [insertWorker]
GO
/****** Object:  Trigger [dbo].[updateWorker]    Script Date: 15.11.2017 16:30:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE TRIGGER [dbo].[updateWorker] ON [dbo].[workers] --Указывается таблица, для которой будет действовать триггер
 FOR UPDATE --Действие, на которое будет срабатывать триггер
 AS
 IF SYSTEM_USER = 'user'--Проверка текущего пользователя
	ROLLBACK TRAN; --Откат транзакции удаления
GO
ALTER TABLE [dbo].[workers] ENABLE TRIGGER [updateWorker]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Номер проверки' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'check', @level2type=N'COLUMN',@level2name=N'id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Сотрудник, регистрирующий проверку' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'check', @level2type=N'COLUMN',@level2name=N'reg_worker'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата регистраци' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'check', @level2type=N'COLUMN',@level2name=N'reg_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата проверки' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'check', @level2type=N'COLUMN',@level2name=N'check_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Сотрудник, проводивший проверку' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'check', @level2type=N'COLUMN',@level2name=N'check_worker'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Обозначение ТД КД' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'check', @level2type=N'COLUMN',@level2name=N'td_kd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Контролируемый показатель' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'check', @level2type=N'COLUMN',@level2name=N'control_indicator'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Количество проверенных операций' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'check', @level2type=N'COLUMN',@level2name=N'count_operations'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Счетчик несоответствий' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'check', @level2type=N'COLUMN',@level2name=N'fail_count'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Описание несоответствия' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'check', @level2type=N'COLUMN',@level2name=N'fail_description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Идентификатор мероприятия' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'events', @level2type=N'COLUMN',@level2name=N'id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Номер несоответствия' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'events', @level2type=N'COLUMN',@level2name=N'check_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Разработчик мероприятия' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'events', @level2type=N'COLUMN',@level2name=N'developer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Сотрудник, предоставивший отчет' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'events', @level2type=N'COLUMN',@level2name=N'report_worker'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Описание мероприятия' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'events', @level2type=N'COLUMN',@level2name=N'description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ответственный' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'events', @level2type=N'COLUMN',@level2name=N'respons_worker'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Срок исполнения' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'events', @level2type=N'COLUMN',@level2name=N'due_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата разработки' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'events', @level2type=N'COLUMN',@level2name=N'develop_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Отчет' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'events', @level2type=N'COLUMN',@level2name=N'report'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Подтверждающая информация' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'events', @level2type=N'COLUMN',@level2name=N'proof_inf'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата отчета' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'events', @level2type=N'COLUMN',@level2name=N'report_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID ознакомления' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'shows', @level2type=N'COLUMN',@level2name=N'id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID проверки' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'shows', @level2type=N'COLUMN',@level2name=N'check_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата ознакомления' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'shows', @level2type=N'COLUMN',@level2name=N'date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID сотрудника' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'shows', @level2type=N'COLUMN',@level2name=N'worker_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Логин учетной записи Windows' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'workers', @level2type=N'COLUMN',@level2name=N'login'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Должность' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'workers', @level2type=N'COLUMN',@level2name=N'post'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Подразделение' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'workers', @level2type=N'COLUMN',@level2name=N'subunit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Права доступа' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'workers', @level2type=N'COLUMN',@level2name=N'access'
GO
