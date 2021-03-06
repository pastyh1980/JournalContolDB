USE [journal]
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPaneCount' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Shows_View'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPane1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Shows_View'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPaneCount' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Events_View'
GO
/*EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPane2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Events_View'
GO*/
EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPane1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Events_View'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPaneCount' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Check_View'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPane2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Check_View'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPane1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Check_View'
GO
/****** Object:  View [dbo].[Shows_View]    Script Date: 02.02.2018 15:32:43 ******/
DROP VIEW [dbo].[Shows_View]
GO
/****** Object:  View [dbo].[Events_View]    Script Date: 02.02.2018 15:32:43 ******/
DROP VIEW [dbo].[Events_View]
GO
/****** Object:  View [dbo].[Check_View]    Script Date: 02.02.2018 15:32:43 ******/
DROP VIEW [dbo].[Check_View]
GO
/****** Object:  View [dbo].[Check_View]    Script Date: 02.02.2018 15:32:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Check_View]
AS
SELECT        dbo.[check].id, dbo.[check].fail_count, dbo.[check].check_subunit, dbo.[check].sector, dbo.[check].check_date, dbo.[check].check_worker, dbo.[check].td_kd, dbo.[check].control_indicator, dbo.[check].fail_description, 
                         dbo.[check].reg_date, dbo.[check].count_operations, dbo.[check].isActive, dbo.[check].isCorrect, dbo.workers.family + ' ' + SUBSTRING(dbo.workers.name, 1, 1) + '.' + SUBSTRING(dbo.workers.otch, 1, 1) + '.' AS reg_worker, 
                         dbo.workers.post, dbo.workers.subunit, dbo.[check].delete_date, CASE WHEN isCorrect = 0 THEN 'Ошибка' WHEN isActive = 0 THEN 'Устранение' ELSE '-' END AS delete_reason, 
                         delete_worker.family + ' ' + SUBSTRING(delete_worker.name, 1, 1) + '.' + SUBSTRING(delete_worker.otch, 1, 1) + '.' AS Delete_Worker, delete_worker.post AS Delete_Post, delete_worker.subunit AS Delete_Subunit, 
                         dbo.[check].isFail
FROM            dbo.[check] LEFT OUTER JOIN
                         dbo.workers ON dbo.[check].reg_worker = dbo.workers.id LEFT OUTER JOIN
                         dbo.workers AS delete_worker ON dbo.[check].delete_worker = delete_worker.id
GO
/****** Object:  View [dbo].[Events_View]    Script Date: 02.02.2018 15:32:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Events_View]
AS
SELECT        dbo.events.id, dbo.events.check_id, dbo.[check].fail_count, dbo.[check].check_subunit, dbo.[check].sector, dbo.[check].fail_description, dbo.events.fail_reason, dbo.events.description, dbo.events.respons_worker, 
                         dbo.events.due_date, dbo.events.develop_date, devel.subunit AS Devel_subunit, devel.post AS Devel_post, devel.family + ' ' + SUBSTRING(devel.name, 1, 1) + '.' + SUBSTRING(devel.otch, 1, 1) + '.' AS Developer, 
                         dbo.events.report, dbo.events.proof_inf, dbo.events.report_date, rep.subunit AS Rep_subunit, rep.post AS Rep_post, rep.family + ' ' + SUBSTRING(rep.name, 1, 1) + '.' + SUBSTRING(rep.otch, 1, 1) + '.' AS Rep_worker,
                          dbo.events.isActive, dbo.events.isCorrect, dbo.events.delete_date, CASE WHEN events.isCorrect = 0 THEN 'Ошибка' WHEN events.isActive = 0 THEN 'Устранение' ELSE '-' END AS delete_reason, 
                         del.family + ' ' + SUBSTRING(del.name, 1, 1) + '.' + SUBSTRING(del.otch, 1, 1) + '.' AS Delete_Worker, del.post AS Delete_Post, del.subunit AS Delete_Subunit, dbo.[check].check_date, dbo.[check].control_indicator, 
                         dbo.events.developer AS dev_id
FROM            dbo.events LEFT OUTER JOIN
                         dbo.workers AS devel ON devel.id = dbo.events.developer LEFT OUTER JOIN
                         dbo.workers AS rep ON rep.id = dbo.events.report_worker LEFT OUTER JOIN
                         dbo.[check] ON dbo.events.check_id = dbo.[check].id LEFT OUTER JOIN
                         dbo.workers AS del ON del.id = dbo.events.delete_worker
GO
/****** Object:  View [dbo].[Shows_View]    Script Date: 02.02.2018 15:32:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Shows_View]
AS
SELECT        dbo.shows.id, dbo.shows.check_id, dbo.shows.date, dbo.workers.family + ' ' + SUBSTRING(dbo.workers.name, 1, 1) + '.' + SUBSTRING(dbo.workers.otch, 1, 1) + '.' AS worker, dbo.workers.post, dbo.workers.subunit, 
                         dbo.shows.worker_id
FROM            dbo.shows LEFT OUTER JOIN
                         dbo.workers ON dbo.shows.worker_id = dbo.workers.id
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
            TopColumn = 0
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
      Begin ColumnWidths = 23
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
         ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Check_View'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Check_View'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Check_View'
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
            TopColumn = 0
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
            TopColumn = 0
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
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'   Width = 1500
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
