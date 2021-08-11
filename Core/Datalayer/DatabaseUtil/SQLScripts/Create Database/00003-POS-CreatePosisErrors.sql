USE LSPOSNET
GO
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('1','1','Could not initialize the application, e.g. loading all settings','POS.Program.cs','1','5.0.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('2','2','A critical error has occurred.  The application will terminate.','POS.Program.cs','1','5.0.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('3','5','Another instance of the application is running.','POS.Program.cs','1','5.0.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('1000','1000','Could not initialize the operation and run the operation thread.','POSProcesses.POSApp.cs','1','5.0.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('1001','1001','The operation thread timed out.','POSProcesses.POSProcessThread.cs','1','5.0.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('1002','1002','Error executing the operation thread.','POSProcesses.POSProcessThread.cs','1','5.0.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('1003','1003','The supplied error number could not be found in the database','POSProcesses.frmError','1','5.0.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('1004','1004','The user has insufficient rights to run the operation.','POSProcesses.Operation','1','5.0.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('1007','1020','Trying to run an unknown operation.','POSProcesses.POSProcessThread','1','5.0.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('1008','1010','Could not display a transaction selected in the journal.','POSProcesses.frmTransactionViewer','1','5.0.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('1009','1820','Error concluding the transaction','POSProcesses.POSApp.cs','1','5.0.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('1010','1824','Error saving the transaction','POSProcesses.POSApp.cs','1','5.3.0.0004',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('1650','1650','Error processing the Tax Free operation','POSProcesses.frmJournal','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('2066','2066','Error intializing the fourcourt.','POSProcesses.WinControls.ForecourtControl','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('2067','2067','Error clearing a fuelling transaction from a fuelling point','POSProcesses.WinControls.ForecourtControl','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('2068','2068','Error when unlocking a fuelling transaction','POSProcesses.WinControls.ForecourtControl','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('2069','2069','Error when preseting the fuelling amount','POSProcesses.WinControls.ForecourtControl','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('2070','2070','Error when preseting the fuelling volume','POSProcesses.WinControls.ForecourtControl','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('2071','2071','Error changing the forecourt to day mode','POSProcesses.WinControls.ForecourtControl','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('2072','2072','Error changing the forecourt to night mode','POSProcesses.WinControls.ForecourtControl','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('2073','2073','Error when calling the emergency stop','POSProcesses.WinControls.ForecourtControl','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('2074','2074','Error when recalling the emergency stop.','POSProcesses.WinControls.ForecourtControl','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('2075','2075','Error when printing the fuel totals','POSProcesses.WinControls.ForecourtControl','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('2076','2076','Error when changing the fuel price.','POSProcesses.WinControls.ForecourtControl','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('2077','2077','Error when locking a fuelling transaction','POSProcesses.WinControls.ForecourtControl','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('3180','3186','Illeagal price entry','POSProcesses.PayCash','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('3540','3540','The user has insufficient rights to run the operation.','POSProcesses.VerifyUserRights','1','5.0.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('3600','3601','Error processing the EOD operation','POSProcesses.EOD','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('3620','3621','Error processing the end of shift operation','POSProcesses.EOS','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('3840','3841','Error processing the DeclareStartAmount operation','POSProcesses.DeclareStartAmount','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('3860','3862','Error processing the FloatEntry operation','POSProcesses.FloatEntry','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('3880','3881','Error processing the TenderRemoval operation','POSProcesses.TenderRemoval ','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('3900','3901','Error processing the SafeDrop operation','POSProcesses.SafeDrop','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('3920','3921','Error processing the BankDrop operation','POSProcesses.BankDrop','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('4023','4023','Error processing a customercard.','POSProcesses.CustomerCard','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('4043','4043','Error processing a customer barcode.','POSProcess.CustomerBarcode','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('4062','4062','Error processing employee card.','POSProcess.EmployeeCard','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('4082','4082','Error processing  a employee barcode.','POSProcess.EmployeeBarcode','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('4103','4103','Error processing a salesperson''s card.','POSProcess.SalespersonCard','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('4122','4122','Error processing a salesperson''s barcode.','POSProcess.SalespersonBarcode','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('4142','4142','Error when clearing a salesperson''s info from an item','POSProcess.SalespersonClear','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('4160','4172','Illeagal quantity entry','POSProcesses.SetQuantity','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('4181','4181','Error processing the Pay Credit Memo operation','POSProcesses.PayCreditMemo','1','5.2.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('10000','10000','The barcode was not found in the database','BusinessLogic.BarcodeSystem','1','5.0.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('10001','10001','Error in processing the barcode','BusinessLogic.BarcodeSystem','1','5.0.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('13000','13000','Could not load all external service and module dlls','SystemFramework.ApplicationServices','1','5.0.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('13010','13010','Could not establish a link to the transaction services','SystemFramework.TransactionServices','1','5.0.0.0',0,0)
INSERT POSISERRORS(ERRORID,ERRORMESSAGEID,DESCRIPTION,CODEUNIT,ACTIVE,FIRSTINVERSION,DATECREATED,DATEUPDATED)       VALUES('50000','50000','Could not increment the EFT batch number','Services.EFT','1','5.0.0.0',0,0)
GO