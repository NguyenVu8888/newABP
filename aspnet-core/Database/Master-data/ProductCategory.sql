USE [vu_ecommerce]
GO

INSERT INTO [dbo].[AppProductCategories]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Slug]
           ,[SortOrder]
           ,[CoverPucture]
           ,[Visibility]
           ,[IsActive]
           ,[ParentId]
           ,[SeoMetaDescreption]
           ,[ExtraProperties]
           ,[ConcurrencyStamp]
           ,[CreationTime]
           ,[CreatorId])
     VALUES
           (NEWID()
           ,N'Điện thoại'
           ,'C1'
           ,'dien-thoai'
           ,1
           ,null
           ,1
           ,1
           ,null
           ,null
           ,1
           ,1
           ,GETDATE()
           ,null)
GO


INSERT INTO [dbo].[AppProductCategories]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Slug]
           ,[SortOrder]
           ,[CoverPucture]
           ,[Visibility]
           ,[IsActive]
           ,[ParentId]
           ,[SeoMetaDescreption]
           ,[ExtraProperties]
           ,[ConcurrencyStamp]
           ,[CreationTime]
           ,[CreatorId])
     VALUES
           (NEWID()
           ,N'Máy tính bảng'
           ,'C2'
           ,'lap-top'
           ,1
           ,null
           ,1
           ,1
           ,null
           ,null
           ,1
           ,1
           ,GETDATE()
           ,null)
GO

