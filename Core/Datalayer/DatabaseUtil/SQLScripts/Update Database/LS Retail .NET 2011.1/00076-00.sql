/*

	Incident No.	: N/A
	Responsible		: Tobias Helmer
	Sprint			: POS Release 8.0.?; for U.S. tax
	Date created	: 15.04.2011

	Description		: Add a field to RBOSTORETABLE to indicate whether tax is included in the price that an operator enters (-> price overwrite, -> key-in price)

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: RBOSTORETABLE
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOSTORETABLE') AND NAME='KEYEDINPRICECONTAINSTAX')
begin
	ALTER TABLE RBOSTORETABLE ADD KEYEDINPRICECONTAINSTAX tinyint NULL
end


GO