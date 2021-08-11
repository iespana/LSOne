
/*

	Incident No.	: N/A
	Responsible		: Marý B Steingrímsdóttir
	Sprint			: LS One 2013.1\Sprint One
	Date created	: 12.7.2013

	Description		: Inserts the version value of the online help that comes with LS One 2013.1
	
						
*/

USE LSPOSNET

IF NOT EXISTS (SELECT TOP 1 ID FROM POSISINFO WHERE ID = 'HELPVERSION_EN')
BEGIN
	INSERT INTO POSISINFO (ID, TEXT) 
	VALUES ('HELPVERSION_EN', '1.0.0.0')
END

GO






