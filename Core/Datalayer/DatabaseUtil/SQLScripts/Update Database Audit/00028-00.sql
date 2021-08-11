
/*

	Incident No.	: 11751
	Responsible		: Hörður Kristjánsson
	Sprint			: LS Retail .NET 2012/Tyr
	Date created	: 07.09.2011
	
	Description		: Add fields to POSTRANSACTIONSERVICEPROFILELog

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:	- POSTRANSACTIONSERVICEPROFILELog - fields added
						
*/


USE LSPOSNET_Audit

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSTRANSACTIONSERVICEPROFILELog') AND NAME='ISSUEGIFTCARDOPTION')
ALTER TABLE dbo.POSTRANSACTIONSERVICEPROFILELog ADD ISSUEGIFTCARDOPTION INT NULL

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSTRANSACTIONSERVICEPROFILELog') AND NAME='USEGIFTCARDS')
ALTER TABLE dbo.POSTRANSACTIONSERVICEPROFILELog ADD USEGIFTCARDS TINYINT NULL

GO