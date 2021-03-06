
/*

	Incident No.	: N/A
	Responsible		: Hrólfur Gestsson 
	Description		: Update as a result of the new Fiscal service	
							
*/

USE LSPOSNET

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOTRANSACTIONFISCALTRANS'))
BEGIN
	CREATE TABLE [dbo].[RBOTRANSACTIONFISCALTRANS](
	[RECEIPTID] [nvarchar](20) NOT NULL,
	[TRANSACTIONID] [nvarchar](20) NOT NULL,
	[STORE] [nvarchar](20) NOT NULL,
	[TERMINAL] [nvarchar](20) NOT NULL,
	[FISCALUNITID] [nvarchar](100) NOT NULL,
	[FISCALCONTROLID] [nvarchar](100) NOT NULL,
	[DATAAREAID] [nvarchar](4) NOT NULL,
	[REPLICATIONCOUNTER] [int] IDENTITY(1,1) NOT NULL,
	[REPLICATED] [tinyint] NOT NULL,
 CONSTRAINT [PK_RBOTRANSACTIONFISCALTRANS] PRIMARY KEY CLUSTERED 
(
	[RECEIPTID] ASC,
	[STORE] ASC,
	[TERMINAL] ASC,
	[DATAAREAID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [dbo].[RBOTRANSACTIONFISCALTRANS] ADD  CONSTRAINT [DF_RBOTRANSACTIONFISCALTRANS_REPLICATED]  DEFAULT ((0)) FOR [REPLICATED]
end

GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSHARDWAREPROFILE' AND COLUMN_NAME = 'FISCALPRINTERDEVICENAME')
BEGIN
	BEGIN TRANSACTION
	EXECUTE sp_rename N'dbo.POSHARDWAREPROFILE.FISCALPRINTERDEVICENAME', N'Tmp_FISCALPRINTERCONNECTION_2', 'COLUMN' 
	EXECUTE sp_rename N'dbo.POSHARDWAREPROFILE.Tmp_FISCALPRINTERCONNECTION_2', N'FISCALPRINTERCONNECTION', 'COLUMN' 
	ALTER TABLE dbo.POSHARDWAREPROFILE SET (LOCK_ESCALATION = TABLE)
	COMMIT
END

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOTRANSACTIONFISCALLOG'))
BEGIN
	CREATE TABLE [dbo].[RBOTRANSACTIONFISCALLOG](
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[EntryDate] [datetime] NULL,
		[PrintString] [text] NULL,
		[OperationID] [int] NULL,
	 CONSTRAINT [PK_RBOTRANSACTIONFISCALLOG] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
end

GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSHARDWAREPROFILE' AND COLUMN_NAME = 'FISCALPRINTERCONNECTION')
BEGIN
	ALTER TABLE POSHARDWAREPROFILE ALTER COLUMN FISCALPRINTERCONNECTION varchar(200)
END