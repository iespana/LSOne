
/*

	Incident No.	: 14213
	Responsible		: Hörður Kristjánsson
	Sprint			: LS Retail .NET 2012/Askur
	Date created	: 31.01.2012
	
	Description		: Add fields to POSTRANSACTIONSERVICEPROFILELog

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:	- POSTRANSACTIONSERVICEPROFILELog - fields added
						
*/


USE LSPOSNET_Audit

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSTRANSACTIONSERVICEPROFILELog') AND NAME='USECREDITVOUCHERS')
ALTER TABLE dbo.POSTRANSACTIONSERVICEPROFILELog ADD USECREDITVOUCHERS TINYINT NULL

GO