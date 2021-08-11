/*

	Incident No.	: 17729
	Responsible		: Erna Guðrún Sigurðardóttir
	Sprint			: LS Retail .NET 2013\Merkúr
	Date created	: 04.07.2012

	Description		: Add UseKitchenDisplay to PosHardwareProfile
	
	
	Tables affected	: PosHardwareProfile
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSHARDWAREPROFILE') AND NAME='USEKITCHENDISPLAY')
BEGIN
	ALTER TABLE dbo.POSHARDWAREPROFILE ADD USEKITCHENDISPLAY tinyint NULL
END
GO
