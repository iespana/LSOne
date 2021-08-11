
/*

	Incident No.	: ONE-3183
	Responsible		: Hörður Kristjánsson
	Sprint			: Paris

	Description		: Removing old pos operation
	
						
*/

/*
	
	NB!!

	**** When a script is added to any folder the "Build Action" on the script needs to be set to "Embedded resource" in properties (F4) ****

	If this is not done the DatabaseUtility will not find the script and will therefore not run it

*/

USE LSPOSNET
GO

if exists(select * from POSISOPERATIONS where OPERATIONID = 903)
begin
	delete from POSISOPERATIONS where OPERATIONID = 903
end

go