USE [master]
GO
/****** Object:  Database [journal]    Script Date: 06.09.2017 15:54:05 ******/
CREATE DATABASE [journal] ON  PRIMARY 
( NAME = N'journal', FILENAME = N'd:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\journal.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'journal_log', FILENAME = N'd:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\journal_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [journal] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [journal].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [journal] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [journal] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [journal] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [journal] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [journal] SET ARITHABORT OFF 
GO
ALTER DATABASE [journal] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [journal] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [journal] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [journal] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [journal] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [journal] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [journal] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [journal] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [journal] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [journal] SET  DISABLE_BROKER 
GO
ALTER DATABASE [journal] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [journal] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [journal] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [journal] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [journal] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [journal] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [journal] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [journal] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [journal] SET  MULTI_USER 
GO
ALTER DATABASE [journal] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [journal] SET DB_CHAINING OFF 
GO
USE [journal]
GO
/****** Object:  Table [dbo].[check]    Script Date: 06.09.2017 15:54:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[check](
	[id] [int] NOT NULL,
	[reg_worker] [int] NOT NULL,
	[delete_worker] [int] NULL,
	[reg_date] [datetime] NOT NULL,
	[check_date] [datetime] NOT NULL,
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
 CONSTRAINT [PK_check] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[events]    Script Date: 06.09.2017 15:54:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[events](
	[id] [int] IDENTITY(1,1) NOT NULL,
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
 CONSTRAINT [PK_events] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[grant_show]    Script Date: 06.09.2017 15:54:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[grant_show](
	[subunit] [varchar](10) NOT NULL,
	[sector] [varchar](100) NOT NULL,
	[boss] [int] NULL,
	[id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_grant_show] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[shows]    Script Date: 06.09.2017 15:54:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[shows](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[check_id] [int] NOT NULL,
	[date] [datetime] NOT NULL,
	[worker_id] [int] NOT NULL,
	[solution_subunit] [varchar](4) NULL,
 CONSTRAINT [PK_shows] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subunits]    Script Date: 06.09.2017 15:54:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subunits](
	[name] [varchar](10) NOT NULL,
	[sectors] [varchar](100) NULL,
 CONSTRAINT [PK_Subunits] PRIMARY KEY CLUSTERED 
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[workers]    Script Date: 06.09.2017 15:54:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[workers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[login] [varchar](20) NOT NULL,
	[family] [varchar](20) NOT NULL,
	[name] [varchar](20) NOT NULL,
	[otch] [varchar](20) NOT NULL,
	[post] [varchar](100) NOT NULL,
	[subunit] [varchar](50) NOT NULL,
	[access] [varchar](100) NULL,
	[passwd] [varchar](100) NULL,
 CONSTRAINT [PK_workers] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[Check_View]    Script Date: 06.09.2017 15:54:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Check_View]
AS
SELECT        dbo.[check].id, dbo.[check].fail_count, dbo.[check].check_subunit, dbo.[check].sector, dbo.[check].check_date, dbo.[check].check_worker, dbo.[check].td_kd, dbo.[check].control_indicator, 
                         dbo.[check].fail_description, dbo.[check].reg_date, dbo.[check].count_operations, dbo.[check].isActive, dbo.[check].isCorrect, SUBSTRING(dbo.workers.name, 1, 1) + '.' + SUBSTRING(dbo.workers.otch, 1, 1) 
                         + '. ' + dbo.workers.family AS reg_worker, dbo.workers.post, dbo.workers.subunit, dbo.[check].delete_date, 
                         CASE WHEN isCorrect = 0 THEN 'Ошибка' WHEN isActive = 0 THEN 'Устранение' ELSE '-' END AS delete_reason, SUBSTRING(delete_worker.name, 1, 1) + '.' + SUBSTRING(delete_worker.otch, 1, 1) 
                         + '. ' + delete_worker.family AS Delete_Worker, delete_worker.post AS Delete_Post, delete_worker.subunit AS Delete_Subunit, dbo.[check].isFail
FROM            dbo.[check] LEFT OUTER JOIN
                         dbo.workers ON dbo.[check].reg_worker = dbo.workers.id LEFT OUTER JOIN
                         dbo.workers AS delete_worker ON dbo.[check].delete_worker = delete_worker.id
GO
/****** Object:  View [dbo].[Events_View]    Script Date: 06.09.2017 15:54:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Events_View]
AS
SELECT        dbo.events.id, dbo.events.check_id, dbo.[check].fail_count, dbo.[check].check_subunit, dbo.[check].sector, dbo.[check].fail_description, dbo.events.fail_reason, dbo.events.description, dbo.events.respons_worker, 
                         dbo.events.due_date, dbo.events.develop_date, devel.subunit AS Devel_subunit, devel.post AS Devel_post, SUBSTRING(devel.name, 1, 1) + '.' + SUBSTRING(devel.otch, 1, 1) + '. ' + devel.family AS Developer, 
                         dbo.events.report, dbo.events.proof_inf, dbo.events.report_date, rep.subunit AS Rep_subunit, rep.post AS Rep_post, SUBSTRING(rep.name, 1, 1) + '.' + SUBSTRING(rep.otch, 1, 1) + '. ' + rep.family AS Rep_worker,
                          dbo.events.isActive, dbo.events.isCorrect, dbo.events.delete_date, CASE WHEN events.isCorrect = 0 THEN 'Ошибка' WHEN events.isActive = 0 THEN 'Устранение' ELSE '-' END AS delete_reason, 
                         SUBSTRING(del.name, 1, 1) + '.' + SUBSTRING(del.otch, 1, 1) + '. ' + del.family AS Delete_Worker, del.post AS Delete_Post, del.subunit AS Delete_Subunit
FROM            dbo.events LEFT OUTER JOIN
                         dbo.workers AS devel ON devel.id = dbo.events.developer LEFT OUTER JOIN
                         dbo.workers AS rep ON rep.id = dbo.events.report_worker LEFT OUTER JOIN
                         dbo.[check] ON dbo.events.check_id = dbo.[check].id LEFT OUTER JOIN
                         dbo.workers AS del ON del.id = dbo.events.delete_worker
GO
/****** Object:  View [dbo].[Shows_View]    Script Date: 06.09.2017 15:54:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Shows_View]
AS
SELECT        dbo.shows.id, dbo.shows.check_id, dbo.shows.date, SUBSTRING(dbo.workers.name, 1, 1) + '.' + SUBSTRING(dbo.workers.otch, 1, 1) + '. ' + dbo.workers.family AS worker, dbo.workers.post, dbo.workers.subunit, 
                         dbo.shows.worker_id
FROM            dbo.shows LEFT OUTER JOIN
                         dbo.workers ON dbo.shows.worker_id = dbo.workers.id
GO
ALTER TABLE [dbo].[check]  WITH CHECK ADD  CONSTRAINT [FK_reg_worker] FOREIGN KEY([reg_worker])
REFERENCES [dbo].[workers] ([id])
GO
ALTER TABLE [dbo].[check] CHECK CONSTRAINT [FK_reg_worker]
GO
ALTER TABLE [dbo].[events]  WITH CHECK ADD  CONSTRAINT [FK_events_check] FOREIGN KEY([check_id])
REFERENCES [dbo].[check] ([id])
GO
ALTER TABLE [dbo].[events] CHECK CONSTRAINT [FK_events_check]
GO
ALTER TABLE [dbo].[events]  WITH CHECK ADD  CONSTRAINT [FK_events_developer] FOREIGN KEY([developer])
REFERENCES [dbo].[workers] ([id])
GO
ALTER TABLE [dbo].[events] CHECK CONSTRAINT [FK_events_developer]
GO
ALTER TABLE [dbo].[events]  WITH CHECK ADD  CONSTRAINT [FK_events_report_worker] FOREIGN KEY([report_worker])
REFERENCES [dbo].[workers] ([id])
GO
ALTER TABLE [dbo].[events] CHECK CONSTRAINT [FK_events_report_worker]
GO
ALTER TABLE [dbo].[shows]  WITH CHECK ADD  CONSTRAINT [FK_shows_check] FOREIGN KEY([check_id])
REFERENCES [dbo].[check] ([id])
GO
ALTER TABLE [dbo].[shows] CHECK CONSTRAINT [FK_shows_check]
GO
ALTER TABLE [dbo].[shows]  WITH CHECK ADD  CONSTRAINT [FK_shows_workers] FOREIGN KEY([worker_id])
REFERENCES [dbo].[workers] ([id])
GO
ALTER TABLE [dbo].[shows] CHECK CONSTRAINT [FK_shows_workers]
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
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[37] 4[24] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "check"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 219
            End
            DisplayFlags = 280
            TopColumn = 13
         End
         Begin Table = "workers"
            Begin Extent = 
               Top = 6
               Left = 257
               Bottom = 136
               Right = 431
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "delete_worker"
            Begin Extent = 
               Top = 6
               Left = 469
               Bottom = 136
               Right = 643
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 19
         Width = 284
         Width = 1500
         Width = 2760
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2580
         Alias = 1290
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Check_View'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Check_View'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "events"
            Begin Extent = 
               Top = 9
               Left = 317
               Bottom = 139
               Right = 491
            End
            DisplayFlags = 280
            TopColumn = 12
         End
         Begin Table = "devel"
            Begin Extent = 
               Top = 53
               Left = 11
               Bottom = 183
               Right = 185
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "rep"
            Begin Extent = 
               Top = 6
               Left = 529
               Bottom = 136
               Right = 703
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "check"
            Begin Extent = 
               Top = 144
               Left = 223
               Bottom = 274
               Right = 404
            End
            DisplayFlags = 280
            TopColumn = 13
         End
         Begin Table = "del"
            Begin Extent = 
               Top = 6
               Left = 741
               Bottom = 136
               Right = 915
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 23
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
    ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Events_View'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'     Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Events_View'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Events_View'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "shows"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 162
               Right = 214
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "workers"
            Begin Extent = 
               Top = 6
               Left = 250
               Bottom = 136
               Right = 424
            End
            DisplayFlags = 280
            TopColumn = 4
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Shows_View'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Shows_View'
GO
USE [master]
GO
ALTER DATABASE [journal] SET  READ_WRITE 
GO
