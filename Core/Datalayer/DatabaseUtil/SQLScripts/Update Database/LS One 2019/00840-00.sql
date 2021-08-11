/*
	Incident No.	: ONE-8330 / 8332
	Responsible		: Adrian Chiorean
	Sprint			: Kerla
	Date created	: 11.06.2018

	Description		: Create GetIFTokens unsecure stored procedure
*/

USE LSPOSNET


IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[spGetAccessTokens_1_0_Unsecure]'))
BEGIN
	DROP PROCEDURE [dbo].[spGetAccessTokens_1_0_Unsecure]
END
GO

CREATE PROCEDURE [dbo].[spGetAccessTokens_1_0_Unsecure]
AS
	SELECT [DESCRIPTION], [SENDERDNS], [USERID], [STOREID], [TIMESTAMP], [ACTIVE], COALESCE([TOKEN], '') AS TOKEN FROM ACCESSTOKENTABLE
GO