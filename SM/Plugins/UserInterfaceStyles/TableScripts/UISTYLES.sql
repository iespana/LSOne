﻿GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('UISTYLES'))
BEGIN
CREATE TABLE [dbo].[UISTYLES](
	[ID] uniqueidentifier NOT NULL,
	[DATAAREAID] nvarchar(4) NOT NULL,
	[CONTEXTID] uniqueidentifier NOT NULL,
	[PARENTSTYLE] uniqueidentifier NULL,
	[DESCRIPTION] nvarchar(60) NOT NULL,
	[STYLEDATA] nvarchar(MAX) NOT NULL,
CONSTRAINT [PK_UISTYLES] PRIMARY KEY NONCLUSTERED ([ID],[DATAAREAID])) ON [PRIMARY]
end

go

if not Exists(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[UISTYLES]') AND name = N'IX_UISTYLES_CONTEXTID')
begin
CREATE INDEX IX_UISTYLES_CONTEXTID
ON dbo.UISTYLES (CONTEXTID)
end

GO

 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'UISTYLES' AND COLUMN_NAME = 'DELETED')
 BEGIN
	ALTER TABLE dbo.UISTYLES ADD DELETED bit NOT NULL CONSTRAINT DF_UISTYLES_DELETED DEFAULT 0
 END

GO