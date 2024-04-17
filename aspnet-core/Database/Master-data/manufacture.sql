USE [vu_ecommerce]
GO

INSERT INTO [dbo].[AppManufacturers]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Slug]
           ,[CoverPicture]
           ,[Visibility]
           ,[IsActive]
           ,[Country]
           ,[ExtraProperties]
           ,[ConcurrencyStamp]
           ,[CreationTime]
           ,[CreatorId])
     VALUES
           (newid()
           ,N'Apple'
           ,'M1'
           ,'app'
           ,null
           ,1
           ,1
           ,'us'
           ,1
           ,1
           ,GETDATE()
           ,null)
GO

INSERT INTO [dbo].[AppManufacturers]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Slug]
           ,[CoverPicture]
           ,[Visibility]
           ,[IsActive]
           ,[Country]
           ,[ExtraProperties]
           ,[ConcurrencyStamp]
           ,[CreationTime]
           ,[CreatorId])
     VALUES
           (newid()
           ,N'SAMSUMG'
           ,'M2'
           ,'samsung'
           ,null
           ,1
           ,1
           ,'us'
           ,1
           ,1
           ,GETDATE()
           ,null)
GO


