﻿/*

	Incident No.	: x
	Responsible		: Erna Guðrún Sigurðardóttir
	Sprint			: LS Retail .NET 2013\Neptune
	Date created	: 26.02.2013

	Description		: Changed datatype for KITCHENMANAGERPORT from string to integer in KITCHENDISPLAYTRANSACTIONPROFILE
	
	
	Tables affected	: KITCHENDISPLAYTRANSACTIONPROFILE
						
*/
USE LSPOSNET

GO

 IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='KITCHENDISPLAYTRANSACTIONPROFILE' AND COLUMN_NAME='KITCHENMANAGERPORT' AND DATA_TYPE='nvarchar')
  BEGIN
		ALTER TABLE KITCHENDISPLAYTRANSACTIONPROFILE ALTER COLUMN KITCHENMANAGERPORT INT NOT NULL
  END

GO