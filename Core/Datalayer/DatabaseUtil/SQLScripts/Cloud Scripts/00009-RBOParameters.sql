
/*

	Incident No.	: N/A
	Responsible		: Hörður Kristjánsson
	Sprint			: Pileus
	Date created	: 2015/01/09

	Description		: Adding cloud info to RBOPARAMETERS

*/

Use LSPOSNET 

GO

IF NOT EXISTS(SELECT * FROM RBOPARAMETERS WHERE KEY_ = 0 ) 
BEGIN 
  INSERT INTO RBOPARAMETERS (
	KEY_,
	DATAAREAID,
	SITESERVICEPROFILE,
	SCSTORESERVERHOST,
	SCSTORESERVERPORT)
  VALUES(
	0,
	'LSR',
	'CloudService',
	'<PARTNERCLOUDSERVER>',
	9101)		
END

GO