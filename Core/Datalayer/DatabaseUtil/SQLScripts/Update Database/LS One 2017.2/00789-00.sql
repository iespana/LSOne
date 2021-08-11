/*

	Incident No.	: ONE-7400
	Responsible		: Quang Nguyen
	Sprint			: Algot - (11.8-23.8)
	Date created	: 17.08.2017

	Description		: Adding a new operation
	
	
	Tables affected	: OPERATIONS
						
*/


USE LSPOSNET
GO

IF NOT EXISTS (SELECT 'x' FROM OPERATIONS WHERE ID='107')
Begin
	insert into OPERATIONS (MASTERID, ID, DESCRIPTION, TYPE, LOOKUPTYPE, AUDIT)
	values ('147CEAB7-DCEF-41A2-8A4C-B81F22B11955', 107, 'Clear price override',2,0,1)
End
GO