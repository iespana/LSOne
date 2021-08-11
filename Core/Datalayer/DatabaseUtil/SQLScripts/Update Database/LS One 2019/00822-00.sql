/*
	Incident No.	: ONE-7836
	Responsible		: Adrian Chiorean
	Sprint			: Eket
	Date created	: 01.11.2017

	Description		: Remove unused stored procedures
*/

USE LSPOSNET

IF EXISTS(SELECT 1 FROM sys.procedures WHERE name = 'spPOSGetItamSalesOrReturns_1_0')
BEGIN
	DROP PROCEDURE spPOSGetItamSalesOrReturns_1_0
END
GO

IF EXISTS(SELECT 1 FROM sys.procedures WHERE name = 'spPOSGetItemSalesOrReturns_2_0')
BEGIN
	DROP PROCEDURE spPOSGetItemSalesOrReturns_2_0
END
GO