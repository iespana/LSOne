/*
	
	Responsible		: <Partner name>	
	Date created	: <Date created>

	Description		: <Which form profiles have been overwritten>
*/

USE LSPOSNET

/* ************************************************************************************** 
 
 In this script any and all changes that are needed to the system form profiles should be done and maintained.
 As an example:
	If all receipts have been translated into a specific language
	All customers should always have some specific information on header/footer of the customers's receipt that is not in the default data from LS Retail
	..and etc.

This script will never be updated, added to or used by the LS One product team 
so this script can be moved between development pack versions without any merging

 NOTE -> THIS IS AN EXAMPLE, THE RECEIPT WILL BE THE SAME BEFORE AND AFTER THE UPDATE

 SAMPLE CODE:
 UPDATE POSISFORMLAYOUT SET DESCRIPTION = 'New description'
 WHERE ID = 'SYS100001' AND DATAAREAID = 'LSR'

**************************************************************************************  */

