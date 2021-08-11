
/*

	Incident No.	: 10410
	Responsible		: Marý B. Steingrímsdóttir
	Sprint			: LS Retail .NET 2012\Askur
	Date created	: 23.01.2012

	Description		: New table, new operations

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: PosZReport - create a new table
					  PosisOperations - new operations
						
*/

IF NOT EXISTS (SELECT * FROM [POSISOPERATIONS] WHERE [OPERATIONID] = 902 and [DATAAREAID] = 'LSR')
INSERT POSISOPERATIONS(OPERATIONID,OPERATIONNAME,CHECKUSERACCESS,USEROPERATION,DATAAREAID) VALUES('902','Initialize Z Report','0','1','LSR')
GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSZREPORT'))
BEGIN
	CREATE TABLE [dbo].[POSZREPORT](
		[ZREPORTID] [nvarchar](20) NOT NULL,
		[CREATEDDATE] [datetime] NULL,
		[STOREID] [nvarchar](20) NOT NULL,
		[TERMINALID] [nvarchar](20) NOT NULL,
		[STAFFID] [nvarchar](20) NULL,
		[ZGROSSAMOUNT] [numeric](28, 12) NULL,
		[ZNETAMOUNT] [numeric](28, 12) NULL,
		[TOTALGROSSAMOUNT] [numeric](28, 12) NULL,
		[TOTALNETAMOUNT] [numeric](28, 12) NULL,
		[ENTRYTYPE] [int] NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
	 CONSTRAINT [PK_POSZREPORT] PRIMARY KEY CLUSTERED 
	(
		[ZREPORTID] ASC,
		[STOREID] ASC,
		[TERMINALID] ASC,
		[DATAAREAID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[POSZREPORT] ADD CONSTRAINT [DF_POSZREPORT_CREATEDDATE]  DEFAULT (getdate()) FOR [CREATEDDATE]
	ALTER TABLE [dbo].[POSZREPORT] ADD CONSTRAINT [DF_POSZREPORT_MANUAL] DEFAULT 1 FOR [ENTRYTYPE]
END

GO


exec spDB_SetTableDescription_1_0 'POSZREPORT','Table that stores information about each Z report and the total gross and net amounts in those transactions'
exec spDB_SetFieldDescription_1_0 'POSZREPORT','ZREPORTID','The ID of the Z report'
exec spDB_SetFieldDescription_1_0 'POSZREPORT','CREATEDDATE','Date when Z report was run'
exec spDB_SetFieldDescription_1_0 'POSZREPORT','STOREID','The store the Z report was run for'
exec spDB_SetFieldDescription_1_0 'POSZREPORT','TERMINALID','The terminal the Z report was run for'
exec spDB_SetFieldDescription_1_0 'POSZREPORT','STAFFID','The staff that was logged onto the terminal when the Z report was run'
exec spDB_SetFieldDescription_1_0 'POSZREPORT','ZGROSSAMOUNT','The total gross amount for all the transactions included in the Z report'
exec spDB_SetFieldDescription_1_0 'POSZREPORT','ZNETAMOUNT','The total net amount for all the transactions included in the Z report'
exec spDB_SetFieldDescription_1_0 'POSZREPORT','TOTALGROSSAMOUNT','The total gross amount for all the transactions sold'
exec spDB_SetFieldDescription_1_0 'POSZREPORT','TOTALNETAMOUNT','The total net amount for all the transactions sold'

GO







