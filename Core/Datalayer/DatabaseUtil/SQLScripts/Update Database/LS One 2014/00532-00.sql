
/*
	Incident No.	: N/A
	Responsible		: Hörður Kristjánsson	
	Sprint			: LS One 2014
	Date created	: 15.04.2014

	Description		: Added STOREID to terminal table primary key
						
*/


USE LSPOSNET
GO


IF EXISTS (SELECT * FROM RBOTERMINALTABLE WHERE STOREID IS NULL OR STOREID = '')
BEGIN
	declare @defaultStore nvarchar(20)
	set @defaultStore = ISNULL((select top(1) STOREID from RBOTERMINALTABLE where STOREID <> '' and STOREID is not null), '')	

	update RBOTERMINALTABLE
	set STOREID = @defaultStore
	where STOREID IS NULL
END

if exists (select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS  where TABLE_NAME = 'RBOTERMINALTABLE' and CONSTRAINT_NAME = 'I_20156TERMINALIDX' and CONSTRAINT_TYPE = 'PRIMARY KEY')
begin
	alter table RBOTERMINALTABLE
	drop constraint I_20156TERMINALIDX	

	alter table RBOTERMINALTABLE
	alter column STOREID nvarchar(20) NOT NULL

	alter table RBOTERMINALTABLE
	add constraint I_20156TERMINALIDX PRIMARY KEY CLUSTERED 	
	(
		TERMINALID asc,
		STOREID asc,
		DATAAREAID asc
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
end

GO
