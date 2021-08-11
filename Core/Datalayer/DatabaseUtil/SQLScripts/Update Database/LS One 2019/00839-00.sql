﻿/*
	Incident No.	: ONE-8330 / 8331
	Responsible		: Helgi Rúnar Gunnarsson
	Sprint			: Kerla
	Date created	: 07.06.2018

	Description		: Create ACCESSTOKENTABLE table to manage the  access tokens for the integration framework
*/

USE LSPOSNET

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ACCESSTOKENTABLE')
BEGIN
	CREATE TABLE [dbo].[ACCESSTOKENTABLE](
		[DESCRIPTION] [nvarchar](60) NOT NULL,
		[USERID] [uniqueidentifier] NOT NULL,
		[STOREID] [nvarchar](20) NOT NULL,
		[SENDERDNS] [nvarchar](100) NOT NULL,
		[ACTIVE] [bit] NOT NULL,
		[TIMESTAMP] [datetime] NOT NULL,
		[TOKEN] [nvarchar](200) NULL,
	 CONSTRAINT [PK_ACCESSTOKENTABLE] PRIMARY KEY CLUSTERED
	(
		[SENDERDNS] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[ACCESSTOKENTABLE] ADD  DEFAULT ((0)) FOR [ACTIVE]

END
GO
