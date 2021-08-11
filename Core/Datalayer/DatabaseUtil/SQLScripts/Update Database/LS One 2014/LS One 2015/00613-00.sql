/*
	Incident No.	: N/A
	Responsible		: Indriði Ingi Stefánsson
	Sprint			: LS One 2015
	Date created	: 23.03.2015

	Description		: Adding new operation Item sale report
						
*/
USE LSPOSNET
GO

  delete from PERMISSIONS where guid in ( 
  'FC11F150-C4F9-11E0-962B-0800200C9A66',
  '2B4FE534-E619-4788-93C3-FDAD053E0255',
  'DBC4802E-985B-4FFC-9BD5-F99C9B214E72',
  'E8C7C5C0-FA5D-4BDE-8516-E616D0B86B87',
  'D396C787-443F-4E78-A629-7384525F7FF6',
  '4929cca0-d44d-11de-8a39-0800200c9a66',
  '3edc9f70-d44d-11de-8a39-0800200c9a66')
