

/*

	Incident No.	: [TFS incident no]
	Responsible		: Pétur Þór Sigurðsson
	Sprint			: [N/A]
	Date created	: [29.11.2010]

	Description		: Data needed for Hospitality functionality in LS POS

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	:	POSISHOSPITALITYDININGTABLES - Created
						POSISHospitalityOperations	 - Operations added to the table
						POSISOperations				 - Operations added to the table
						
*/


USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[POSISHOSPITALITYDININGTABLES]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[POSISHOSPITALITYDININGTABLES](
		[RESTAURANTID] [nvarchar](20) NOT NULL,
		[SALESTYPE] [nvarchar](20) NOT NULL,
		[DINEINTABLENO] [int] NOT NULL,
		[SEQUENCE] [int] NOT NULL,
		[DININGTABLELAYOUTID] [nvarchar](20) NOT NULL,
		[DESCRIPTION] [nvarchar](30) NULL,
		[NOOFGUESTS] [int] NULL,
		[STAFFID] [nvarchar](10) NULL,
		[STATUS] [int] NULL,
		[TRANSACTIONXML] [xml] NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
	 CONSTRAINT [PK_POSISHOSPITALITYDININGTABLES] PRIMARY KEY CLUSTERED 
	(
		[RESTAURANTID] ASC,
		[SALESTYPE] ASC,
		[DINEINTABLENO] ASC,
		[SEQUENCE] ASC,
		[DININGTABLELAYOUTID] ASC,
		[DATAAREAID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO


-- Add Hospitality POS operations
IF NOT EXISTS (SELECT * FROM [POSISHOSPITALITYOPERATIONS] WHERE [OPERATIONID] = 800 and [DATAAREAID] = 'LSR')
INSERT POSISHOSPITALITYOPERATIONS(OPERATIONID,OPERATIONNAME,PERMISSIONID,CHECKUSERACCESS,ALLOWPARAMETER,DATAAREAID) VALUES('800','Seat Guests','0','0','0','LSR')
IF NOT EXISTS (SELECT * FROM [POSISHOSPITALITYOPERATIONS] WHERE [OPERATIONID] = 801 and [DATAAREAID] = 'LSR')
INSERT POSISHOSPITALITYOPERATIONS(OPERATIONID,OPERATIONNAME,PERMISSIONID,CHECKUSERACCESS,ALLOWPARAMETER,DATAAREAID) VALUES('801','Split Bill','0','0','0','LSR')
IF NOT EXISTS (SELECT * FROM [POSISHOSPITALITYOPERATIONS] WHERE [OPERATIONID] = 802 and [DATAAREAID] = 'LSR')
INSERT POSISHOSPITALITYOPERATIONS(OPERATIONID,OPERATIONNAME,PERMISSIONID,CHECKUSERACCESS,ALLOWPARAMETER,DATAAREAID) VALUES('802','Transfer','0','0','0','LSR')
IF NOT EXISTS (SELECT * FROM [POSISHOSPITALITYOPERATIONS] WHERE [OPERATIONID] = 803 and [DATAAREAID] = 'LSR')
INSERT POSISHOSPITALITYOPERATIONS(OPERATIONID,OPERATIONNAME,PERMISSIONID,CHECKUSERACCESS,ALLOWPARAMETER,DATAAREAID) VALUES('803','Change Staff','0','0','0','LSR')
IF NOT EXISTS (SELECT * FROM [POSISHOSPITALITYOPERATIONS] WHERE [OPERATIONID] = 804 and [DATAAREAID] = 'LSR')
INSERT POSISHOSPITALITYOPERATIONS(OPERATIONID,OPERATIONNAME,PERMISSIONID,CHECKUSERACCESS,ALLOWPARAMETER,DATAAREAID) VALUES('804','Run POS','0','0','0','LSR')
IF NOT EXISTS (SELECT * FROM [POSISHOSPITALITYOPERATIONS] WHERE [OPERATIONID] = 805 and [DATAAREAID] = 'LSR')
INSERT POSISHOSPITALITYOPERATIONS(OPERATIONID,OPERATIONNAME,PERMISSIONID,CHECKUSERACCESS,ALLOWPARAMETER,DATAAREAID) VALUES('805','Print PreReceipt','0','0','0','LSR')
IF NOT EXISTS (SELECT * FROM [POSISHOSPITALITYOPERATIONS] WHERE [OPERATIONID] = 806 and [DATAAREAID] = 'LSR')
INSERT POSISHOSPITALITYOPERATIONS(OPERATIONID,OPERATIONNAME,PERMISSIONID,CHECKUSERACCESS,ALLOWPARAMETER,DATAAREAID) VALUES('806','Log Off','0','0','0','LSR')
GO


-- Add POS operations
IF NOT EXISTS (SELECT * FROM [POSISOPERATIONS] WHERE [OPERATIONID] = 1301 and [DATAAREAID] = 'LSR')
INSERT POSISOPERATIONS(OPERATIONID,OPERATIONNAME,CHECKUSERACCESS,USEROPERATION,DATAAREAID) VALUES('1301','Exit Hospitality POS','1','1','LSR')
IF NOT EXISTS (SELECT * FROM [POSISOPERATIONS] WHERE [OPERATIONID] = 1302 and [DATAAREAID] = 'LSR')
INSERT POSISOPERATIONS(OPERATIONID,OPERATIONNAME,CHECKUSERACCESS,USEROPERATION,DATAAREAID) VALUES('1302','Print Hospitality Menu Type','1','1','LSR')
IF NOT EXISTS (SELECT * FROM [POSISOPERATIONS] WHERE [OPERATIONID] = 1303 and [DATAAREAID] = 'LSR')
INSERT POSISOPERATIONS(OPERATIONID,OPERATIONNAME,CHECKUSERACCESS,USEROPERATION,DATAAREAID) VALUES('1303','Set Hospitality Menu Type','1','1','LSR')
IF NOT EXISTS (SELECT * FROM [POSISOPERATIONS] WHERE [OPERATIONID] = 1304 and [DATAAREAID] = 'LSR')
INSERT POSISOPERATIONS(OPERATIONID,OPERATIONNAME,CHECKUSERACCESS,USEROPERATION,DATAAREAID) VALUES('1304','Change Hospitality Menu Type','1','1','LSR')
IF NOT EXISTS (SELECT * FROM [POSISOPERATIONS] WHERE [OPERATIONID] = 130 and [DATAAREAID] = 'LSR')
INSERT POSISOPERATIONS(OPERATIONID,OPERATIONNAME,CHECKUSERACCESS,USEROPERATION,DATAAREAID) VALUES('130','Clear Item Comments','1','1','LSR')
GO
