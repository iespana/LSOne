/*
	Incident No.	: ONE-12383
	Responsible		: Adrian Chiorean
	Sprint			: Bæjarins beztu 
	Date created	: 07.07.2020

	Description		: Add tables for retail item assemblies
*/

USE LSPOSNET

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RETAILITEMASSEMBLY')
BEGIN
	CREATE TABLE [dbo].[RETAILITEMASSEMBLY](
		[ID] [uniqueidentifier] NOT NULL,
		[DESCRIPTION] [nvarchar](250) NOT NULL,
		[ITEMID] [nvarchar](40) NOT NULL,
		[ENABLED] [bit] NOT NULL,
		[CALCULATEPRICEFROMCOMPS] [bit] NOT NULL,
		[STARTINGDATE] [datetime] NOT NULL,
		[STOREID] [nvarchar](40) NOT NULL,
		[PRICE] [numeric](28, 12) NOT NULL,
		[EXPANDASSEMBLY] [int] NOT NULL
	 CONSTRAINT [PK_RETAILITEMASSEMBLY] PRIMARY KEY CLUSTERED
	(
		[ID] ASC, [ITEMID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	EXEC spDB_SetTableDescription_1_0 'RETAILITEMASSEMBLY','Contains item assemblies';

	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMASSEMBLY', 'ID', 'Unique ID of the assembly';
	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMASSEMBLY', 'DESCRIPTION', 'Description of the assembly';
	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMASSEMBLY', 'ITEMID', 'Item ID linked to the assembly';
	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMASSEMBLY', 'ENABLED', 'True if the assembly is enabled and can be sold on the POS';
	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMASSEMBLY', 'CALCULATEPRICEFROMCOMPS', 'True if the price of the assembly is calculated from the components';
	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMASSEMBLY', 'STOREID', 'The store ID on which the assembly is enabled or empty for all stores';
	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMASSEMBLY', 'PRICE', 'Price of the assembly';
	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMASSEMBLY', 'STARTINGDATE', 'The date from which the assembly is active';
	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMASSEMBLY', 'EXPANDASSEMBLY', 'States if assembly components shall be displayed in the POS, on the receipt and on KDS stations. It can contain 0 (None) or any combination of 1 (POS), 2 (receipt) and 4 (KDS)';
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RETAILITEMASSEMBLYCOMPONENTS')
BEGIN
	CREATE TABLE [dbo].[RETAILITEMASSEMBLYCOMPONENTS](
		[ID] [uniqueidentifier] NOT NULL,
		[ASSEMBLYID] [uniqueidentifier] NOT NULL,
		[ITEMID] [nvarchar](40) NOT NULL,
		[QUANTITY] [numeric](28, 12) NOT NULL,
		[UNITID] [nvarchar](40) NOT NULL
		CONSTRAINT [PK_RETAILITEMASSEMBLYCOMPONENTS] PRIMARY KEY CLUSTERED
		(
			[ID] ASC, [ASSEMBLYID] ASC, [ITEMID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	EXEC spDB_SetTableDescription_1_0 'RETAILITEMASSEMBLYCOMPONENTS','Contains item assembly components';

	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMASSEMBLYCOMPONENTS', 'ID', 'Unique ID of the assembly component';
	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMASSEMBLYCOMPONENTS', 'ASSEMBLYID', 'ID of the assembly';
	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMASSEMBLYCOMPONENTS', 'ITEMID', 'Item ID of the component';
	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMASSEMBLYCOMPONENTS', 'QUANTITY', 'Quantity of the component';
	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMASSEMBLYCOMPONENTS', 'UNITID', 'The unit ID of the component';
END
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE NAME = 'IDX_RETAILITEMASSEMBLYCOMPONENTS_ASSEMBLYID')
BEGIN
	CREATE NONCLUSTERED INDEX IDX_RETAILITEMASSEMBLYCOMPONENTS_ASSEMBLYID
	ON RETAILITEMASSEMBLYCOMPONENTS(ASSEMBLYID)
END
GO
