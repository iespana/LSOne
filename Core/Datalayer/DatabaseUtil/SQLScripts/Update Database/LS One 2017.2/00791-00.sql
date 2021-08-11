/*

	Incident No.	: ONE-7399
	Responsible		: Quang Nguyen
	Sprint			: Algot - (11.8-23.8)
	Date created	: 17.08.2017

	Description		: Adding a new operation
	
	
	Tables affected	: OPERATIONS
						
*/


USE LSPOSNET
GO

IF NOT EXISTS (SELECT 'x' FROM OPERATIONS WHERE ID='307')
Begin
	insert into OPERATIONS (MASTERID, ID, DESCRIPTION, TYPE, LOOKUPTYPE, AUDIT)
	values ('E2DA0489-E671-4128-8E59-DE8E181D9BA2', 307, 'Clear discounts',2,0,1)
End
GO