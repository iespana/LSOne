/*

	Incident No.	: XXX
	Responsible		: Óskar Bjarnason
	Sprint			: LS Retail .NET 2013\Jupiter
	Date created	: 12.12.2012

	Description		: Adding column
	
	
	Tables affected	: KITCHENDISPLAYVISUALPROFILE
						
*/
USE LSPOSNET
GO
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('KITCHENDISPLAYVISUALPROFILE') AND NAME='SHOWDEALS')
BEGIN
	 ALTER TABLE KITCHENDISPLAYVISUALPROFILE Add SHOWDEALS tinyint NULL 
END	