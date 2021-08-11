/*

	Incident No.	: 
	Responsible		: Tobias Helmer
	Sprint			: LS Retail .NET 2011.1\Sprint 2
	Date created	: 02.05.2011

	Description		: Update TaxCollectLimit table information

	Logic scripts   : 
	
	Tables affected	: TAXCOLLECTLIMIT
						
*/

-- To Document table description
exec spDB_SetTableDescription_1_0 'TAXCOLLECTLIMIT','Table that contains information about tax collection limits. Multiple limits can exist for a sales tax code.'

-- To Document field description
exec spDB_SetFieldDescription_1_0 'TAXCOLLECTLIMIT','TAXCODE','The taxcode ID of the tax collection limit record'
exec spDB_SetFieldDescription_1_0 'TAXCOLLECTLIMIT','TAXMAX','Maximal tax amount collected.'
exec spDB_SetFieldDescription_1_0 'TAXCOLLECTLIMIT','TAXMIN','Tax amounts below this amount will be zeroed.'
exec spDB_SetFieldDescription_1_0 'TAXCOLLECTLIMIT','TAXTODATE','Specifies when the collect limit period ends. Empty value means indefinite.'
exec spDB_SetFieldDescription_1_0 'TAXCOLLECTLIMIT','TAXFROMDATE','Specifies when the collect limit period starts. Empty value means indefinite.'
exec spDB_SetFieldDescription_1_0 'TAXCOLLECTLIMIT','LINENUM','Counter that is a part of the primary key. Needed when more than one limit for a tax code exist.'
exec spDB_SetFieldDescription_1_0 'TAXCOLLECTLIMIT','RECVERSION','Currently not in use.'
exec spDB_SetFieldDescription_1_0 'TAXCOLLECTLIMIT','DATAAREAID','DATAAREAID'