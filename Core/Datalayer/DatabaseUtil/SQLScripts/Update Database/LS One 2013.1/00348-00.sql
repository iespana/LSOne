/*

	Incident No.	: 23681
	Responsible		: Höður Sigurdór Heiðarsson
	Sprint			: LS One 2013.1\S1
	Date created	: 21.05.2013

	Description		: Added "Get gift card balance" operation
	
	Tables affected	: POSISOPERATIONS
						
*/
USE LSPOSNET

GO
IF NOT EXISTS (SELECT * FROM [POSISOPERATIONS] WHERE [OPERATIONID] = 521 and [DATAAREAID] = 'LSR')
INSERT [POSISOPERATIONS] ([OPERATIONID], [OPERATIONNAME], [PERMISSIONID], [PERMISSIONID2], [CHECKUSERACCESS], [USEROPERATION], [DATAAREAID]) VALUES ('521', N'Get gift card balance', null, null, 1, 1, N'LSR')
 

GO