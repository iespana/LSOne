﻿USE LSPOSNET_Audit
GO
  
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSTRANSACTIONSERVICEPROFILELog' AND COLUMN_NAME = 'CENTRALTABLESERVER' AND CHARACTER_MAXIMUM_LENGTH = 20)
BEGIN
alter TABLE POSTRANSACTIONSERVICEPROFILELog alter column CENTRALTABLESERVER nvarchar(50) NOT NULL 
	
END