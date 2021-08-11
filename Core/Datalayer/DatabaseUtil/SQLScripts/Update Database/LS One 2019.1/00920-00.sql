﻿/*
	Incident No.	: ONE-9496
	Responsible		: Adrian Chiorean
	Sprint			: Sirius
	Date created	: 07.02.2019

	Description		: Add table for Windows printing configuration
*/

USE LSPOSNET

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'WINDOWSPRINTERCONFIGURATION')
BEGIN
	CREATE TABLE [dbo].[WINDOWSPRINTERCONFIGURATION](
		[ID] [nvarchar](40) NOT NULL,
		[DESCRIPTION] [nvarchar](100) NOT NULL,
		[DEVICENAME] [nvarchar](200) NULL,
		[FONTNAME] [nvarchar](100) NULL,
		[FONTSIZE] [real] NULL,
		[WIDEHIGHFONTSIZE] [real] NULL,
		[LEFTMARGIN] [int] NULL,
		[RIGHTMARGIN] [int] NULL,
		[TOPMARGIN] [int] NULL,
		[BOTTOMMARGIN] [int] NULL,
		[PRINTDESIGNBOXES] [bit] NULL,
		[FOLDERLOCATION] [nvarchar](500) NULL,
	 CONSTRAINT [PK_WINDOWSPRINTERCONFIGURATION] PRIMARY KEY CLUSTERED
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

END
GO

-- Add number sequence
IF NOT EXISTS(SELECT 1 FROM dbo.NUMBERSEQUENCETABLE where DATAAREAID = 'LSR' and NUMBERSEQUENCE = 'WINDOWSPRINTER')
BEGIN
	INSERT INTO dbo.NUMBERSEQUENCETABLE (NUMBERSEQUENCE,TXT,LOWEST,HIGHEST,FORMAT,INUSE,EMBEDSTOREID,CANBEDELETED,STOREID,DATAAREAID)
	VALUES ('WINDOWSPRINTER','Windows printer',1,999999999,'#########',1,0,0,'HO','LSR')
	INSERT INTO dbo.NUMBERSEQUENCEVALUE (NUMBERSEQUENCE,NEXTREC,STOREID,DATAAREAID)
	VALUES ('WINDOWSPRINTER',1,'HO','LSR')
END
GO

CREATE TABLE #TEMP00890 (
HARWAREPROFILECOLUMNADDED BIT NOT NULL,
STATIONPRINTINGCOLUMNADDED BIT NOT NULL)

INSERT INTO #TEMP00890 VALUES (0, 0)

-- Add field for hardware profile and migrate data
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSHARDWAREPROFILE' AND COLUMN_NAME = 'WINDOWSPRINTERCONFIGURATIONID')
BEGIN
	ALTER TABLE POSHARDWAREPROFILE ADD WINDOWSPRINTERCONFIGURATIONID NVARCHAR(40) NULL
	UPDATE #TEMP00890 SET HARWAREPROFILECOLUMNADDED = 1

	-- MARK OLD COLUMS AS OBSOLETE
	EXECUTE spDB_SetFieldDescription_1_0 'POSHARDWAREPROFILE', 'WINPRINT_LEFTMARGIN', '[Obsolete, ONE-9496] Configuration moved to WINDOWSPRINTERCONFIGURATION';
	EXECUTE spDB_SetFieldDescription_1_0 'POSHARDWAREPROFILE', 'WINPRINT_RIGHTMARGIN', '[Obsolete, ONE-9496] Configuration moved to WINDOWSPRINTERCONFIGURATION';
	EXECUTE spDB_SetFieldDescription_1_0 'POSHARDWAREPROFILE', 'WINPRINT_TOPMARGIN', '[Obsolete, ONE-9496] Configuration moved to WINDOWSPRINTERCONFIGURATION';
	EXECUTE spDB_SetFieldDescription_1_0 'POSHARDWAREPROFILE', 'WINPRINT_BOTTOMMARGIN', '[Obsolete, ONE-9496] Configuration moved to WINDOWSPRINTERCONFIGURATION';
	EXECUTE spDB_SetFieldDescription_1_0 'POSHARDWAREPROFILE', 'WINPRINT_FONTSIZE', '[Obsolete, ONE-9496] Configuration moved to WINDOWSPRINTERCONFIGURATION';
	EXECUTE spDB_SetFieldDescription_1_0 'POSHARDWAREPROFILE', 'WINPRINT_PRINTDESIGNBOXES', '[Obsolete, ONE-9496] Configuration moved to WINDOWSPRINTERCONFIGURATION';
	EXECUTE spDB_SetFieldDescription_1_0 'POSHARDWAREPROFILE', 'WINPRINT_FOLDERLOCATION', '[Obsolete, ONE-9496] Configuration moved to WINDOWSPRINTERCONFIGURATION';
END

-- Add field for printing stations and migrate data
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RESTAURANTSTATION' AND COLUMN_NAME = 'WINDOWSPRINTERCONFIGURATIONID')
BEGIN
	ALTER TABLE RESTAURANTSTATION ADD WINDOWSPRINTERCONFIGURATIONID NVARCHAR(40) NULL
	UPDATE #TEMP00890 SET STATIONPRINTINGCOLUMNADDED = 1

		-- MARK OLD COLUMS AS OBSOLETE
	EXECUTE spDB_SetFieldDescription_1_0 'RESTAURANTSTATION', 'FONTSIZE', '[Obsolete, ONE-9496] Configuration moved to WINDOWSPRINTERCONFIGURATION';
	EXECUTE spDB_SetFieldDescription_1_0 'RESTAURANTSTATION', 'FONTNAME', '[Obsolete, ONE-9496] Configuration moved to WINDOWSPRINTERCONFIGURATION';
END
GO

DECLARE @NEWCONFIGID INT = 0
DECLARE @ENTITYID NVARCHAR(40)
DECLARE @DEVICENAME NVARCHAR(200)
DECLARE @FONTNAME NVARCHAR(100)
DECLARE @FONTSIZE REAL
DECLARE @WIDEHIGHFONTSIZE REAL
DECLARE @LEFTMARGIN INT
DECLARE @RIGHTMARGIN INT
DECLARE @TOPMARGIN INT
DECLARE @BOTTOMMARGIN INT
DECLARE @PRINTDESIGNBOXES BIT
DECLARE @FOLDERLOCATION NVARCHAR(500)
DECLARE @DATAAREAID NVARCHAR(4)

IF EXISTS (SELECT 1 FROM #TEMP00890 WHERE HARWAREPROFILECOLUMNADDED = 1)
BEGIN
DECLARE HARDWARECURSOR CURSOR FOR -- We select default font because it was hardcoded before
	SELECT PROFILEID, PRINTERDEVICENAME, 'Courier New' AS FONTNAME, 9.5 AS FONTSIZE, 18 AS WIDEHIGHFONTSIZE, WINPRINT_LEFTMARGIN, WINPRINT_RIGHTMARGIN, WINPRINT_TOPMARGIN, WINPRINT_BOTTOMMARGIN, WINPRINT_PRINTDESIGNBOXES, WINPRINT_FOLDERLOCATION, DATAAREAID
	FROM POSHARDWAREPROFILE WHERE PRINTER = 2 AND PRINTERDEVICENAME != ''

	OPEN HARDWARECURSOR

	FETCH FROM HARDWARECURSOR INTO @ENTITYID, @DEVICENAME, @FONTNAME, @FONTSIZE, @WIDEHIGHFONTSIZE, @LEFTMARGIN, @RIGHTMARGIN, @TOPMARGIN, @BOTTOMMARGIN, @PRINTDESIGNBOXES, @FOLDERLOCATION, @DATAAREAID

	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @NEWCONFIGID = @NEWCONFIGID + 1

		INSERT INTO WINDOWSPRINTERCONFIGURATION VALUES (FORMAT(@NEWCONFIGID, '0########'), 'Hardware - ' + @ENTITYID, @DEVICENAME, @FONTNAME, @FONTSIZE, @WIDEHIGHFONTSIZE, @LEFTMARGIN, @RIGHTMARGIN, @TOPMARGIN, @BOTTOMMARGIN, @PRINTDESIGNBOXES, @FOLDERLOCATION)
		
		UPDATE POSHARDWAREPROFILE SET WINDOWSPRINTERCONFIGURATIONID = FORMAT(@NEWCONFIGID, '0########') WHERE PROFILEID = @ENTITYID AND DATAAREAID = @DATAAREAID

		FETCH NEXT FROM HARDWARECURSOR INTO @ENTITYID, @DEVICENAME, @FONTNAME, @FONTSIZE, @WIDEHIGHFONTSIZE, @LEFTMARGIN, @RIGHTMARGIN, @TOPMARGIN, @BOTTOMMARGIN, @PRINTDESIGNBOXES, @FOLDERLOCATION, @DATAAREAID
	END

	CLOSE HARDWARECURSOR
	DEALLOCATE HARDWARECURSOR

	UPDATE NUMBERSEQUENCEVALUE SET NEXTREC = @NEWCONFIGID + 1 WHERE NUMBERSEQUENCE = 'WINDOWSPRINTER'
END

IF EXISTS (SELECT 1 FROM #TEMP00890 WHERE STATIONPRINTINGCOLUMNADDED = 1)
BEGIN
	DECLARE STATIONCURSOR CURSOR FOR
	SELECT ID, WINDOWSPRINTER, FONTNAME, FONTSIZE, 18 AS WIDEHIGHFONTSIZE, 45 AS LEFTMARGIN, 5 AS RIGHTMARGIN, 5 AS TOPMARGIN, 60 AS BOTTOMMARGIN, 0 AS PRINTDESIGNBOXES, 'C:\ProgramData\LS Retail\LS One POS\' AS FOLDERLOCATION, DATAAREAID
	FROM RESTAURANTSTATION WHERE STATIONTYPE = 0 AND WINDOWSPRINTER != ''

	OPEN STATIONCURSOR

	FETCH FROM STATIONCURSOR INTO @ENTITYID, @DEVICENAME, @FONTNAME, @FONTSIZE, @WIDEHIGHFONTSIZE, @LEFTMARGIN, @RIGHTMARGIN, @TOPMARGIN, @BOTTOMMARGIN, @PRINTDESIGNBOXES, @FOLDERLOCATION, @DATAAREAID

	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @NEWCONFIGID = @NEWCONFIGID + 1

		INSERT INTO WINDOWSPRINTERCONFIGURATION VALUES (FORMAT(@NEWCONFIGID, '0########'), 'Station printing - ' + @ENTITYID, @DEVICENAME, @FONTNAME, @FONTSIZE, @WIDEHIGHFONTSIZE, @LEFTMARGIN, @RIGHTMARGIN, @TOPMARGIN, @BOTTOMMARGIN, @PRINTDESIGNBOXES, @FOLDERLOCATION)
		
		UPDATE RESTAURANTSTATION SET WINDOWSPRINTERCONFIGURATIONID = FORMAT(@NEWCONFIGID, '0########') WHERE ID = @ENTITYID AND DATAAREAID = @DATAAREAID

		FETCH NEXT FROM STATIONCURSOR INTO @ENTITYID, @DEVICENAME, @FONTNAME, @FONTSIZE, @WIDEHIGHFONTSIZE, @LEFTMARGIN, @RIGHTMARGIN, @TOPMARGIN, @BOTTOMMARGIN, @PRINTDESIGNBOXES, @FOLDERLOCATION, @DATAAREAID
	END

	CLOSE STATIONCURSOR
	DEALLOCATE STATIONCURSOR

	UPDATE NUMBERSEQUENCEVALUE SET NEXTREC = @NEWCONFIGID + 1 WHERE NUMBERSEQUENCE = 'WINDOWSPRINTER'
END
GO

IF OBJECT_ID('tempdb..#TEMP00890') IS NOT NULL DROP TABLE #TEMP00890
GO