
/*

	Incident No.	: ONE-3195
	Responsible		: Helgi Rúnar Gunnarsson
	Sprint			: Paris

	Description		: Inserting gift receipt form type into POSFORMTYPE
	
						
*/

/*
	
	NB!!

	**** When a script is added to any folder the "Build Action" on the script needs to be set to "Embedded resource" in properties (F4) ****

	If this is not done the DatabaseUtility will not find the script and will therefore not run it

*/

USE LSPOSNET
GO

if not exists (select ID from POSFORMTYPE where ID = '4f5db772-1337-4104-a294-13590928bb1d')
begin
	insert into POSFORMTYPE 
	values ('4f5db772-1337-4104-a294-13590928bb1d', 'Gift receipt', 30, 'LSR')
end

go