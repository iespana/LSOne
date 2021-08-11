﻿/*

	Incident No.	: 
	Responsible		: Tobias Helmer
	Sprint			: 
	Date created	: 16.09.2011

	Description		: column CASHCOUNTINGUSESNUMPAD added; 2 operations added 

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: POSFUNCTIONALITYPROFILE; POSISOPERATIONS					  
						
*/


USE LSPOSNET
GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSFUNCTIONALITYPROFILE') AND NAME='CASHCOUNTINGUSESNUMPAD')
BEGIN
	ALTER TABLE [dbo].[POSFUNCTIONALITYPROFILE] ADD CASHCOUNTINGUSESNUMPAD tinyint NULL;
	ALTER TABLE [dbo].[POSFUNCTIONALITYPROFILE] ADD  CONSTRAINT [DF_POSFUNCTIONALITYPROFILE_CASHCOUNTINGUSESNUMPAD]  DEFAULT ((0)) FOR [CASHCOUNTINGUSESNUMPAD]
END
GO

IF NOT EXISTS (SELECT * FROM [POSISOPERATIONS] WHERE [OPERATIONID] = 519 and [DATAAREAID] = 'LSR')
INSERT [POSISOPERATIONS] ([OPERATIONID], [OPERATIONNAME], [PERMISSIONID], [PERMISSIONID2], [CHECKUSERACCESS], [USEROPERATION], [DATAAREAID]) VALUES ('519', N'Return Income Accounts', 1002, 1002, 1, 1, N'LSR')
IF NOT EXISTS (SELECT * FROM [POSISOPERATIONS] WHERE [OPERATIONID] = 520 and [DATAAREAID] = 'LSR')
INSERT [POSISOPERATIONS] ([OPERATIONID], [OPERATIONNAME], [PERMISSIONID], [PERMISSIONID2], [CHECKUSERACCESS], [USEROPERATION], [DATAAREAID]) VALUES ('520', N'Return Expense Accounts', 1002, 1002, 1, 1, N'LSR')
GO