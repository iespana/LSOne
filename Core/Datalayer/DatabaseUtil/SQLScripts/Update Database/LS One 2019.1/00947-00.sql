/*
	Incident No.	: ONE-10097
	Responsible		: Hörður Kristjánsson
	Sprint			: Wezen
	Date created	: 22.10.2019

	Description		: Add split information to sales lines
*/

USE LSPOSNET

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOTRANSACTIONSALESTRANS' AND COLUMN_NAME = 'LIMITATIONSPLITPARENTLINEID')
BEGIN
	ALTER TABLE RBOTRANSACTIONSALESTRANS ADD LIMITATIONSPLITPARENTLINEID INT NOT NULL DEFAULT -1
	EXECUTE spDB_SetFieldDescription_1_0 'RBOTRANSACTIONSALESTRANS', 'LIMITATIONSPLITPARENTLINEID', 'The parent line ID that this line item was created from. This happens when a line item is partially paid with a limited payment and the remaining amount of the item is split into anoter line.';
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOTRANSACTIONSALESTRANS' AND COLUMN_NAME = 'LIMITATIONSPLITCHILDLINEID')
BEGIN
	ALTER TABLE RBOTRANSACTIONSALESTRANS ADD LIMITATIONSPLITCHILDLINEID INT NOT NULL DEFAULT -1
	EXECUTE spDB_SetFieldDescription_1_0 'RBOTRANSACTIONSALESTRANS', 'LIMITATIONSPLITCHILDLINEID', 'The line ID of the child item that was split away from this line. This happens when a line item is partially paid with a limited payment and the remaining amount of the item is split into anoter line.';
END
GO