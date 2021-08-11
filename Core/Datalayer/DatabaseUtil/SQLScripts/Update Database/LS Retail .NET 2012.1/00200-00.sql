/*

	Incident No.	: [TFS incident no]
	Responsible		: Höður Sigurdór Heiðarsson
	Sprint			: LS Retail .NET 2013\Merkúr
	Date created	: 30.05.2012

	Description		: Adding POS permissions to permissiongroup

	Logic scripts   : Permissions changed
	
	Tables affected	: PERMISSIONGROUP - data added
						
*/

USE LSPOSNET

IF NOT EXISTS(SELECT * FROM PERMISSIONGROUP WHERE [GUID] = '7d750220-a99f-11e1-afa6-0800200c9a66')
	Insert into PERMISSIONGROUP (GUID,Name,DATAAREAID) 
	values ('7d750220-a99f-11e1-afa6-0800200c9a66','POS permissions','LSR');