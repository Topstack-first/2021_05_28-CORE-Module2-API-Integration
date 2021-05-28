--DROP DATABASE CORE
IF db_id('CORE') IS NULL
CREATE DATABASE CORE
GO
DECLARE @dbname nvarchar(128)
SET @dbname = N'CORE'

IF (EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = @dbname OR name = @dbname)))
	BEGIN
		USE [CORE]
		
		CREATE TABLE [User] (
		  [UserId] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
		  [UserName] nvarchar(255),
		  [UserEmail] nvarchar(255),
		  [UserPost] int,
		  [UserAccountStatus] bit,
		  [UserApprovalStatus] nvarchar(100),
		  [FirstName] nvarchar(255),
		  [LastName] nvarchar(255),
		  [JobTitle] nvarchar(255),
		  [UserPassword] nvarchar(255),
		  [UserProfilePic] nvarchar(255),
		  [UserBiography] nvarchar(255),
		  [CreatedAt] timestamp,
		  [CreatedBy] nvarchar(255),
		  [RoleId] int,
		  [DepartmentId] int,
		  [DocumentAttached] int,
		  [DeleteStatus] bit
		)
		

		CREATE TABLE [Role] (
		  [RoleId] int PRIMARY KEY IDENTITY(1, 1),
		  [RoleName] varchar(30),
		  [RolePermission] int,
		  [CreatedAt] timestamp,
		  [CreatedBy] nvarchar(100)
		)
		

		CREATE TABLE [Permission] (
		  [PermissionID] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
		  [PermissionName] nvarchar(255),
		  [BitWiseValue] int
		)
		

		CREATE TABLE [Department] (
		  [DepartmentId] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
		  [DepartmentName] nvarchar(255)
		)

		CREATE TABLE [dbo].[RefreshToken](
			[TokenId] [int] IDENTITY(1,1) NOT NULL,
			[UserId] [int] NOT NULL,
			[Token] [varchar](200) NOT NULL,
			[ExpiryDate] [datetime] NOT NULL,
		 CONSTRAINT [PK_RefreshToken] PRIMARY KEY CLUSTERED 
		(
			[TokenId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY]

		ALTER TABLE [dbo].[RefreshToken]  WITH CHECK ADD FOREIGN KEY([UserId])
		REFERENCES [dbo].[User] ([UserId])
		ON UPDATE CASCADE
		ON DELETE CASCADE
		

		ALTER TABLE [User] ADD FOREIGN KEY ([RoleId]) REFERENCES [Role] ([RoleId])

		ALTER TABLE [User] ADD FOREIGN KEY ([DepartmentId]) REFERENCES [Department] ([DepartmentId])
		

	END
---------------------------------------INSERT TABLE DATA:permission-----------------------------------------------------------------------------------------------

	SET ANSI_NULLS ON
	GO

	SET QUOTED_IDENTIFIER ON
	GO

	CREATE PROCEDURE [dbo].[SpAddPermission] @PermissionName nvarchar(100), @BitwiseValue int
	AS
	INSERT INTO Permission VALUES (@PermissionName,@BitwiseValue)
	GO
	USE [CORE]
	GO
	---------------------------------------INSERT TABLE DATA:role-----------------------------------------------------------------------------------------------

	SET ANSI_NULLS ON
	GO

	SET QUOTED_IDENTIFIER ON
	GO

	CREATE PROCEDURE [dbo].[SpAddRole] @RoleName varchar(100)
	AS
	DECLARE @PermVal AS INT
	IF @RoleName = 'SA'
	BEGIN
		 SET @PermVal = (SELECT sum(BitWiseValue) FROM Permission)
		INSERT INTO [Role] VALUES ('Super Admin', @PermVal, DEFAULT, 'SYSTEM')

	END

	if @RoleName = 'PM'--Project Manager
	BEGIN
		SET @PermVal = (SELECT sum(BitWiseValue) FROM Permission WHERE PermissionID IN ('2','11','3','4','10'))
		INSERT INTO [Role] VALUES ('Project Manager', @PermVal, DEFAULT, 'SYSTEM')
	END

	if @RoleName = 'PU'--Power User
	BEGIN

		SET @PermVal = (SELECT sum(BitWiseValue) FROM Permission WHERE PermissionID IN ('2','11','3','4','10'))
		INSERT INTO [Role] VALUES ('Power User', @PermVal, DEFAULT, 'SYSTEM')
	END

	if @RoleName = 'M'--Member
	BEGIN

		SET @PermVal = (SELECT sum(BitWiseValue) FROM Permission WHERE PermissionID IN ('2','11','3','4','10'))
		INSERT INTO [Role] VALUES ('Member', @PermVal, DEFAULT, 'SYSTEM')
	END

	GO

	---------------------------------------INSERT TABLE DATA:User-----------------------------------------------------------------------------------------------
	SET ANSI_NULLS ON
	GO

	SET QUOTED_IDENTIFIER ON
	GO

	CREATE PROCEDURE [dbo].[SpInsertUsers] @Username nvarchar(100), @Email nvarchar(100), @Post int, @AccountStatus bit, @ApprovalStatus nvarchar(100), @Firstname nvarchar(100), @Lastname nvarchar(100), @JobTitle nvarchar(100), @Password nvarchar(20), @Profile_pic nvarchar(100), @Biography nvarchar(100), @Role_id int, @DepartmentId int, @DocumentAttached int, @DeleteStatus bit
	AS
	INSERT INTO [User] VALUES (@Username,@Email,@Post,@AccountStatus,@ApprovalStatus,@Firstname,@Lastname,@JobTitle,@Password,@Profile_pic,@Biography,DEFAULT,'SYSTEM',@Role_id, @DepartmentId,@DocumentAttached,@DeleteStatus)
	GO

	---------------------------------------INSERT TABLE DATA:Department-----------------------------------------------------------------------------------------------

	SET ANSI_NULLS ON
	GO

	SET QUOTED_IDENTIFIER ON
	GO

	CREATE PROCEDURE [dbo].[SpAddDepartment] @DepartmentName nvarchar(255)
	AS
	INSERT INTO [Department] VALUES (@DepartmentName)
	GO
	

	-----------------------------------------EXEC----------------------------------------
	EXEC [dbo].[SpAddPermission] 'Create User','1'
	EXEC [dbo].[SpAddPermission] 'Sign in Account','2'
	EXEC [dbo].[SpAddPermission] 'View User','4'
	EXEC [dbo].[SpAddPermission] 'Edit User','8'
	EXEC [dbo].[SpAddPermission] 'Sign Up Account','16'
	EXEC [dbo].[SpAddPermission] 'Manage Approval','32'
	EXEC [dbo].[SpAddPermission] 'Manage Status','64'
	EXEC [dbo].[SpAddPermission] 'Assign Role','128'
	EXEC [dbo].[SpAddPermission] 'Remove Role','256'
	EXEC [dbo].[SpAddPermission] 'View Capability','512'
	EXEC [dbo].[SpAddPermission] 'Sign out account','1024'
	EXEC [dbo].[SpAddPermission] 'Delete User','2048'

	EXEC [dbo].[SpAddDepartment] 'Research and Development'
	EXEC [dbo].[SpAddDepartment] 'Human Resources'
	EXEC [dbo].[SpAddDepartment] 'Financial and Accounting'
	EXEC [dbo].[SpAddDepartment] 'Information Technology'

	EXEC [dbo].[SpAddRole] 'SA'
	EXEC [dbo].[SpAddRole] 'PM'
	EXEC [dbo].[SpAddRole] 'PU'
	EXEC [dbo].[SpAddRole] 'M'
	--(@Username,@Email,@Post,@AccountStatus,@ApprovalStatus,@Firstname,@Lastname,@JobTitle,@Password,@Profile_pic,@Biography,DEFAULT,'SYSTEM',@Role_id, @DepartmentId,@DocumentAttached,@DeleteStatus)
	EXEC [dbo].[SpInsertUsers] 'Soleh98','Solehin98@gmail.com',0,0,'Pending','Ahmad Solehin','Bin Shamsul Bahri','System Developer','Soleh@123!','C://Users/Soleh.png','Solehin good!',1,1,1,1
	EXEC [dbo].[SpInsertUsers] 'Hafiz94','Hafizuddin94@gmail.com',0,0,'Pending','Muhammad Hafizuddin','Bin A Rahim','System Developer','Hafiz@123!','C://Users/Hafiz.png','Hafiz likes to eat!',2,2,1,1
	EXEC [dbo].[SpInsertUsers] 'ikhwan12','Ikhwan12@gmail.com',0,0,'Pending','Khairul Ikhwan','Bin Mohd Yusoff','Team Lead','ikhwan@123!','C://Users/ikhwan.png','Ikhwan',3,3,1,1
	EXEC [dbo].[SpInsertUsers] 'Jimmy12','Jimmy12@gmail.com',0,0,'Pending','Jimmy','Neutron','System Developer','jimmy@123!','C://Users/jimmy.png','Jimmy',4,4,1,1

	SELECT * FROM CORE..[User]
	SELECT * FROM CORE..[Role]
	SELECT * FROM CORE..Permission
	SELECT * FROM CORE..Department
	SELECT * FROM CORE..RefreshToken
	