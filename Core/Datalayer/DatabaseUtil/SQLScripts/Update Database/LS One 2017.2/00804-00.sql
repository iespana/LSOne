/*
	Incident No.	: ONE-9156
	Responsible		: Hörður Kristjánsson
	Sprint			: Edosian
	Date created	: 25.10.2018

	Description		: Add index to LINENUM to speed up LINEUM sequence generation. INVENTJOURNALTRANS can in some implementations grow fairly large, so having an index 
	                  improves performance significantly. 
*/

USE LSPOSNET

IF NOT EXISTS(SELECT * FROM SYS.INDEXES WHERE NAME = N'LS_INVENTJOURNALTRANS_LINENUM')
BEGIN
	CREATE NONCLUSTERED INDEX [LS_INVENTJOURNALTRANS_LINENUM] ON [dbo].[INVENTJOURNALTRANS]
	(
		[LINENUM] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END

GO