
/*
	Incident No.	: N/A
	Responsible		: Sigfus Johannesson	
	Sprint			: LS One 2014
	Date created	: 10.04.2014

	Description		: The trigger [Update_NUMBERSEQUENCETABLE] prevents the running of the lookup value script. This trigger gets recreated correctly in the audit logic script.
						
*/
USE LSPOSNET
GO

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Update_NUMBERSEQUENCETABLE]'))
BEGIN
	DROP TRIGGER [dbo].[Update_NUMBERSEQUENCETABLE]
END
GO
