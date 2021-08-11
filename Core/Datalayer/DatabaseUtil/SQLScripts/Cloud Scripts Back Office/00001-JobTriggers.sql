
/*

	Incident No.	: N/A
	Responsible		: Indriði Ingi Stefánsson
	Sprint			: Cumulus
	Date created	: 2014/11/18

	Description		: Auto generated from a database using the following script 


 SELECT 
  'IF NOT EXISTS(SELECT * FROM JscJobTriggers WHERE ID = '''+CONVERT(VARCHAR(50),jobTriggers.id)+''' ) 
  BEGIN 
  INSERT INTO JscJobTriggers (
	ID,
	Job,
	TriggerKind,
	Enabled,
	RecurrenceType,
	Seconds,
	Minutes,
	Hours,
	Months,
	Years,
	DaysOfMonth,
	DaysOfWeek,
	StartTime,
	EndTime,
	Tag)
  VALUES(
  
'''+CONVERT(VARCHAR(50),jobTriggers.id)+''',
'''+CONVERT(VARCHAR(50),Job)+''',
'''+CONVERT(VARCHAR(50), TriggerKind)+''',
'+CONVERT(VARCHAR(50), Enabled)+',
'''+CONVERT(VARCHAR(50), RecurrenceType)+''',
'+ CASE When Seconds IS not null then  ''''+ CONVERT(VARCHAR(170),Seconds)+'''' else 'null' END+',
'+ CASE When Minutes IS not null then  ''''+ CONVERT(VARCHAR(170),Minutes)+'''' else 'null' END+',
'+ CASE When Hours IS not null then  ''''+ CONVERT(VARCHAR(170),Hours)+'''' else 'null' END+',
'+ CASE When Months IS not null then  ''''+ CONVERT(VARCHAR(170),Months)+'''' else 'null' END+',
'+ CASE When Years IS not null then  ''''+ CONVERT(VARCHAR(170),Years)+'''' else 'null' END+',
'+ CASE When DaysOfMonth IS not null then  ''''+ CONVERT(VARCHAR(170),DaysOfMonth)+'''' else 'null' END+',
'+ CASE When DaysOfWeek IS not null then  ''''+ CONVERT(VARCHAR(170),DaysOfWeek)+'''' else 'null' END+',
'+ CASE When StartTime IS not null then  ''''+ CONVERT(VARCHAR(170),StartTime)+'''' else 'null' END+',
'+ CASE When EndTime IS not null then  ''''+ CONVERT(VARCHAR(170),EndTime)+'''' else 'null' END+',
'+ CASE When Tag IS not null then  ''''+ CONVERT(VARCHAR(170),Tag)+'''' else 'null' END+')
	END
ELSE
	Begin
		Update  JscJobTriggers set
			Job ='''+CONVERT(VARCHAR(50),Job)+''',
			TriggerKind ='+CONVERT(VARCHAR(50), TriggerKind)+',			
			Enabled='+ CONVERT(VARCHAR(50), Enabled)+',
			RecurrenceType='+ CONVERT(VARCHAR(50), RecurrenceType)+',
			Seconds='+CASE When Seconds IS not null then  ''''+ CONVERT(VARCHAR(170),Seconds)+'''' else 'null' END+',
			Minutes='+CASE When Minutes IS not null then  ''''+ CONVERT(VARCHAR(170),Minutes)+'''' else 'null' END+',
			Hours='+CASE When Hours IS not null then  ''''+ CONVERT(VARCHAR(170),Hours)+'''' else 'null' END+',
			Months='+CASE When Months IS not null then  ''''+ CONVERT(VARCHAR(170),Months)+'''' else 'null' END+',
			Years='+CASE When Years IS not null then  ''''+ CONVERT(VARCHAR(170),Years)+'''' else 'null' END+',
			DaysOfMonth='+CASE When DaysOfMonth IS not null then  ''''+ CONVERT(VARCHAR(170),DaysOfMonth)+'''' else 'null' END+',
			DaysOfWeek='+CASE When DaysOfWeek IS not null then  ''''+ CONVERT(VARCHAR(170),DaysOfWeek)+'''' else 'null' END+',
			StartTime='+CASE When StartTime IS not null then  ''''+ CONVERT(VARCHAR(170),StartTime)+'''' else 'null' END+',
			EndTime='+CASE When EndTime IS not null then  ''''+ CONVERT(VARCHAR(170),EndTime)+'''' else 'null' END+',
			Tag='+CASE When Tag IS not null then  ''''+ CONVERT(VARCHAR(170),Tag)+'''' else 'null' END+'			
			WHERE ID = '''+CONVERT(VARCHAR(50), jobTriggers.id)+'''
	END'
  FROM (
  SELECT  --ID -- USE 
  newid() AS id --if you need new ones

      ,[Job]
      ,[TriggerKind]
      ,[Enabled]
      ,[RecurrenceType]
      ,[Seconds]
      ,[Minutes]
      ,[Hours]
      ,[Months]
      ,[Years]
      ,[DaysOfMonth]
      ,[DaysOfWeek]
      ,[StartTime]
      ,[EndTime]
      ,[Tag]
  FROM JscJobTriggers where enabled = 1) jobTriggers 




	
						
*/

/*
	
	NB!!

	**** When a script is added to any folder the "Build Action" on the script needs to be set to "Embedded resource" in properties (F4) ****

	If this is not done the DatabaseUtility will not find the script and will therefore not run it

*/

Use LSPOSNET 
IF NOT EXISTS(SELECT * FROM JscJobTriggers WHERE ID = 'E7460BA8-050B-45D2-A29C-5F8CD60CC429' ) 
  BEGIN 
  INSERT INTO JscJobTriggers (
	ID,
	Job,
	TriggerKind,
	Enabled,
	RecurrenceType,
	Seconds,
	Minutes,
	Hours,
	Months,
	Years,
	DaysOfMonth,
	DaysOfWeek,
	StartTime,
	EndTime,
	Tag)
  VALUES(
  
'E7460BA8-050B-45D2-A29C-5F8CD60CC429',
'EFAF10E4-A7AB-4047-8172-9AF26839F489',
'1',
1,
'2',
'36',
'10',
'0',
'*',
'*',
'*',
'*',
'Jun 11 2015 12:10AM',
null,
'')
	END
ELSE
	Begin
		Update  JscJobTriggers set
			Job ='EFAF10E4-A7AB-4047-8172-9AF26839F489',
			TriggerKind =1,			
			Enabled=1,
			RecurrenceType=2,
			Seconds='36',
			Minutes='10',
			Hours='0',
			Months='*',
			Years='*',
			DaysOfMonth='*',
			DaysOfWeek='*',
			StartTime='Jun 11 2015 12:10AM',
			EndTime=null,
			Tag=''			
			WHERE ID = 'E7460BA8-050B-45D2-A29C-5F8CD60CC429'
	END
IF NOT EXISTS(SELECT * FROM JscJobTriggers WHERE ID = '5D55A622-D388-48AE-BE23-5CAF64570A5E' ) 
  BEGIN 
  INSERT INTO JscJobTriggers (
	ID,
	Job,
	TriggerKind,
	Enabled,
	RecurrenceType,
	Seconds,
	Minutes,
	Hours,
	Months,
	Years,
	DaysOfMonth,
	DaysOfWeek,
	StartTime,
	EndTime,
	Tag)
  VALUES(
  
'5D55A622-D388-48AE-BE23-5CAF64570A5E',
'51A5A057-0243-4771-9223-0B98BB9F54A8',
'0',
1,
'1',
'0',
'*/60',
'*',
'*',
'*',
'*',
'*',
'Jun 11 2015 12:30AM',
null,
'')
	END
ELSE
	Begin
		Update  JscJobTriggers set
			Job ='51A5A057-0243-4771-9223-0B98BB9F54A8',
			TriggerKind =0,			
			Enabled=1,
			RecurrenceType=1,
			Seconds='0',
			Minutes='*/60',
			Hours='*',
			Months='*',
			Years='*',
			DaysOfMonth='*',
			DaysOfWeek='*',
			StartTime='Jun 11 2015 12:30AM',
			EndTime=null,
			Tag=''			
			WHERE ID = '5D55A622-D388-48AE-BE23-5CAF64570A5E'
	END
IF NOT EXISTS(SELECT * FROM JscJobTriggers WHERE ID = '60E191FD-2A4B-471D-9973-93D943D73E00' ) 
  BEGIN 
  INSERT INTO JscJobTriggers (
	ID,
	Job,
	TriggerKind,
	Enabled,
	RecurrenceType,
	Seconds,
	Minutes,
	Hours,
	Months,
	Years,
	DaysOfMonth,
	DaysOfWeek,
	StartTime,
	EndTime,
	Tag)
  VALUES(
  
'60E191FD-2A4B-471D-9973-93D943D73E00',
'E76244B1-3DD7-46CC-9A0B-752ECD050815',
'1',
1,
'2',
'0',
'0',
'0',
'*',
'*',
'*',
'*',
'Jun 11 2015 12:00AM',
null,
'')
	END
ELSE
	Begin
		Update  JscJobTriggers set
			Job ='E76244B1-3DD7-46CC-9A0B-752ECD050815',
			TriggerKind =1,			
			Enabled=1,
			RecurrenceType=2,
			Seconds='0',
			Minutes='0',
			Hours='0',
			Months='*',
			Years='*',
			DaysOfMonth='*',
			DaysOfWeek='*',
			StartTime='Jun 11 2015 12:00AM',
			EndTime=null,
			Tag=''			
			WHERE ID = '60E191FD-2A4B-471D-9973-93D943D73E00'
	END