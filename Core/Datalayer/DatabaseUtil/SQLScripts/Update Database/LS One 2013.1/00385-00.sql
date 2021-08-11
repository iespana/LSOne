
/*

	Incident No.	: N/A
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: LS One 2013.1\Sprint One
	Date created	: 12.7.2013

	Description		: Goes through all items and finds items that have only one vendor on them and sets that vendor as the default (primary) vendor
	
						
*/

USE LSPOSNET

DECLARE @ITEMID NVARCHAR(100)
DECLARE @COUNT INT
DECLARE @SQL NVARCHAR(255)
DECLARE @VENDORID NVARCHAR(100)
DECLARE ITEM CURSOR FOR


SELECT RETAILITEMID, COUNT(*) AS NOOFVENDORS 
FROM VENDORITEMS
GROUP BY RETAILITEMID

OPEN ITEM

FETCH FROM ITEM INTO @ITEMID, @COUNT

WHILE @@FETCH_STATUS = 0
BEGIN

      IF @COUNT = 1
      BEGIN
            SELECT TOP 1 @VENDORID = VENDORID 
            FROM VENDORITEMS 
            WHERE RETAILITEMID = @ITEMID
            AND DATAAREAID = 'LSR'
            
            UPDATE INVENTTABLE
            SET PRIMARYVENDORID = @VENDORID
            WHERE ITEMID = @ITEMID
            AND DATAAREAID = 'LSR'
      
      END   

    
      FETCH NEXT FROM ITEM INTO @ITEMID, @COUNT

END

CLOSE ITEM
DEALLOCATE ITEM

GO



