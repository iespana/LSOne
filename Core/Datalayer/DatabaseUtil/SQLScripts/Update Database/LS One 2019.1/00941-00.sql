﻿/*
	Incident No.	: ONE-10523
	Responsible		: Ovidiu Caba
	Sprint			: Bellatrix
	Date created	: 23.09.2019

	Description		: Add column FORCEREFUNDTOTHISPAYMENTTYPE to RBOSTORETENDERTYPETABLE
*/
USE LSPOSNET

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOSTORETENDERTYPETABLE' AND COLUMN_NAME = 'FORCEREFUNDTOTHISPAYMENTTYPE')
BEGIN
	ALTER TABLE RBOSTORETENDERTYPETABLE ADD FORCEREFUNDTOTHISPAYMENTTYPE BIT
	EXECUTE spDB_SetFieldDescription_1_0 'RBOSTORETENDERTYPETABLE', 'FORCEREFUNDTOTHISPAYMENTTYPE', 'If this property is true, then the refund is only allowed with the same tender type';
END
GO