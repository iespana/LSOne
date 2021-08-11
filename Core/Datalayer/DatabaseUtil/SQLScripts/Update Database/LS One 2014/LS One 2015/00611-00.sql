/*
	Incident No.	: N/A
	Responsible		: Indriði Ingi Stefánsson
	Sprint			: LS One 2015
	Date created	: 06.02.2015

	Description		: Adding new operation Item sale report
						
*/
USE LSPOSNET
GO

IF NOT EXISTS (SELECT * FROM POSISOPERATIONS WHERE OPERATIONID = 1800)
Begin
 insert into POSISOPERATIONS values(1800, 'Run Job', null, null, 0, 1, 'LSR', 18, null)
End
