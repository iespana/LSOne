
/*

	Incident No.	: N/A
	Responsible		: Hrólfur Gestsson 
	Description		: Recreated the table with an improved primary index	
							
*/

USE LSPOSNET

if Exists (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[RBOTERMINALGROUPCONNECTION]') AND type in (N'U'))
BEGIN
	drop table [dbo].[RBOTERMINALGROUPCONNECTION]
END
CREATE TABLE [dbo].[RBOTERMINALGROUPCONNECTION](
	[TERMINALGROUPID] [nvarchar](40) NOT NULL,
	[TERMINALID] [nvarchar](20) NOT NULL,
	[STOREID] [nvarchar](20) NOT NULL,
	[DATAAREAID] [nvarchar](4) NOT NULL,
 CONSTRAINT [PK_RBOTERMINALGROUPTERMINALS] PRIMARY KEY CLUSTERED 
(
	[TERMINALGROUPID] ASC,
	[TERMINALID] ASC,
	[STOREID] ASC,
	[DATAAREAID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
