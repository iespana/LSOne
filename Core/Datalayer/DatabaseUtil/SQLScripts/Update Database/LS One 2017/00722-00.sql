/*

	Incident No.	: ONE-5518
	Responsible		: Simona Avornicesei
	Sprint			: Cinnamon 15.12 - 29.12
	Date created	: 21.12.2016

	Description		: Adding new system reason code for inventory transfer
	
	
	Tables affected	: INVENTTRANSREASON
						
*/

use LSPOSNET
GO

IF NOT EXISTS (SELECT REASONID FROM INVENTTRANSREASON WHERE REASONID = 'ADJ001')
BEGIN
	INSERT INTO INVENTTRANSREASON ([REASONID]
      ,[REASONTEXT]
      ,[DATAAREAID])
	VALUES ('ADJ001', 'Changed to service item', 'LSR')
END
GO