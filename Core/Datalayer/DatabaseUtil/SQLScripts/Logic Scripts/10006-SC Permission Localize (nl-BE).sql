
/*
	Incident No.	: 
	Responsible		: 
	Sprint			: 
	Date created	: 

	Description		: Auto generated language file for permissions

	Logic scripts   : 
	
	Tables affected	: None
						
*/

Use LSPOSNET 

GO

declare @dataAreaID nvarchar(10)
declare @locale nvarchar(10)

set @dataAreaID = 'LSR'
set @locale = 'nl-BE'


-- Permission group localizations
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'DF4AFBD0-B35C-11DE-8A39-0800200C9A66','Profielen',@dataAreaID --Profiles
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'1DDEEF60-D9AE-11DE-8A39-0800200C9A66','Algemeen',@dataAreaID --General
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'78535D50-04E7-11DF-8A39-0800200C9A66','Artikel Master',@dataAreaID --Item master
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'0CA8E620-E997-11DA-8AD9-0800200C9A66','Gebruikers- en veiligheidsbeheer',@dataAreaID --User and security management
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'CEAC1AD0-E997-11DA-8AD9-0800200C9A66','Verslagen',@dataAreaID --Reports
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'808ED9F0-E997-11DA-8AD9-0800200C9A66','Systeembeheer',@dataAreaID --System Administration
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'a54c7dc0-a922-11df-94e2-0800200c9a66','Hospitality',@dataAreaID --Hospitality
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'32246200-f0bb-11df-98cf-0800200c9a66','Inventaris',@dataAreaID --Inventory
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'7d750220-a99f-11e1-afa6-0800200c9a66','POS-machtigingen',@dataAreaID --POS permissions
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'2c562010-f660-11e0-be50-0800200c9a66','Getrouwheidsmachtigingen',@dataAreaID --Loyalty permissions
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'7ccab506-b497-403e-99e3-7141655e5bfc','Replicatie',@dataAreaID --Replication
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'9D0F831D-7B2B-4A0E-B9E6-349B50A235BE','Klantbestellingen',@dataAreaID --Customer orders



-- Permission localizations
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'e13c05d0-cf89-11de-8a39-0800200c9a66','Actievergunningen bekijken',@dataAreaID --View action permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'e694e380-cf89-11de-8a39-0800200c9a66','Actievergunningen bewerken',@dataAreaID --Edit action permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1d608220-fa2c-11da-974d-0800200c9a66','Nieuwe gebruikers aanmaken',@dataAreaID --Create new users
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'d32f5960-554d-11db-b0de-0800200c9a66','Gebruiker bewerken',@dataAreaID --Edit user
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'9b82f9b0-face-11da-974d-0800200c9a66','Gebruikers verwijderen',@dataAreaID --Delete users
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'e1cf2610-6049-11db-b0de-0800200c9a66','Gebruikers inschakelen/uitschakelen',@dataAreaID --Enable/Disable users
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'a0ae94a0-fa04-11da-974d-0800200c9a66','Wachtwoord voor andere gebruikers resetten',@dataAreaID --Reset password for other users
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'4d3f3f70-5d02-11db-b0de-0800200c9a66','Gebruikersgroepen aanmaken',@dataAreaID --Create user groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'fdab47a0-5d02-11db-b0de-0800200c9a66','Gebruikersgroepen bewerken',@dataAreaID --Edit user groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'762df510-5d03-11db-b0de-0800200c9a66','Gebruikersgroepen verwijderen',@dataAreaID --Delete user groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1906ecc0-fac0-11da-974d-0800200c9a66','Gebruikers aan groepen toewijzen',@dataAreaID --Assign users to groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'39f09b00-fac2-11da-974d-0800200c9a66','Machtigingen toekennen/weigeren/overnemen',@dataAreaID --Grant/Deny/Inherit permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'eca62890-fac3-11da-974d-0800200c9a66','Hogere gebruikersmachtigingen toekennen',@dataAreaID --Grant higher user permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'8c7c3250-5eb6-11db-b0de-0800200c9a66','Gebruikersmachtigingen beheren',@dataAreaID --Manage user permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'628e9c80-5d03-11db-b0de-0800200c9a66','Groepsmachtigingen beheren',@dataAreaID --Manage group permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'48078c50-6d7f-11db-9fe1-0800200c9a66','Auditlogs bekijken',@dataAreaID --View audit logs
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'fb0d8940-fb74-11de-8a39-0800200c9a66','Schema updateservice bekijken',@dataAreaID --View update service schedule
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'13d44f40-fb75-11de-8a39-0800200c9a66','Schema updateservice beheren',@dataAreaID --Manage update service schedule
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'a6164dc0-a229-11db-8ab9-0800200c9a66','Beheersinstellingen onderhouden',@dataAreaID --Maintain administrative settings
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'F20F86F0-AC18-11DE-8A39-0800200C9A66','Auditlogs beheren',@dataAreaID --Manage audit logs
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'31dfe220-b35d-11de-8a39-0800200c9a66','Visuele profielen bekijken',@dataAreaID --View visual profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'60876530-b35d-11de-8a39-0800200c9a66','Visuele profielen bewerken',@dataAreaID --Edit visual profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'9a413920-db3c-11de-8a39-0800200c9a66','Functionaliteitsprofielen bekijken',@dataAreaID --View functionality profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'a1372500-db3c-11de-8a39-0800200c9a66','Functionaliteitsprofielen bewerken',@dataAreaID --Edit functionality profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'4929cca0-d44d-11de-8a39-0800200c9a66','Lay-out aanraakknoppen bewerken',@dataAreaID --Edit touch button layouts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'40a5f690-dffa-11de-8a39-0800200c9a66','Transactieserviceprofielen bekijken',@dataAreaID --View Site service profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'47556390-dffa-11de-8a39-0800200c9a66','Transactieserviceprofielen bewerken',@dataAreaID --Edit Site service profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1ce28d90-e3ec-11de-8a39-0800200c9a66','Hardwareprofielen bekijken',@dataAreaID --View hardware profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'24414d60-e3ec-11de-8a39-0800200c9a66','Hardwareprofielen bewerken',@dataAreaID --Edit hardware profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'004a89b0-c53f-11de-8a39-0800200c9a66','Kaarttypes bekijken',@dataAreaID --View card types
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'08427c40-c53f-11de-8a39-0800200c9a66','Kaarttypes bewerken',@dataAreaID --Edit card types
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'d4e6db50-c941-11de-8a39-0800200c9a66','Betalingsmethoden bekijken',@dataAreaID --View payment methods
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'dc259000-c941-11de-8a39-0800200c9a66','Betalingsmethoden bewerken',@dataAreaID --Edit payment methods
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'651897b0-b72f-11de-8a39-0800200c9a66','Winkels bekijken',@dataAreaID --View stores
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'6b7db540-b72f-11de-8a39-0800200c9a66','Winkels bewerken',@dataAreaID --Edit stores
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'24faeb80-df2d-11de-8a39-0800200c9a66','Ontvangstbewijzen bekijken',@dataAreaID --View receipts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'f7b3fcb0-e95a-11de-8a39-0800200c9a66','Formulieren bekijken',@dataAreaID --View forms
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ff6dadc0-e95a-11de-8a39-0800200c9a66','Formulieren bewerken',@dataAreaID --Edit forms
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'36f689f0-d9ad-11de-8a39-0800200c9a66','Klanten bekijken',@dataAreaID --View customers
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'9e542310-d9ac-11de-8a39-0800200c9a66','Klanten bewerken',@dataAreaID --Edit customers
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'BE2722F4-242B-474C-918B-73B2413F4CE1','Categorieën bekijken', @dataAreaID -- View Categories
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'BC8A8926-CD9E-4562-92F7-8711ED378910','Categorieën bewerken', @dataAreaID -- Edit Categories
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'5245D6BA-3F89-439F-A792-00613D5FE15A','Tax free bereiken bekijken', @dataAreaID --  View tax free ranges
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C1D8415A-8425-4CF1-9CC1-D81D33D669FC','Prijsinstellingen beheren',@dataAreaID --Manage price settings
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'AB7B8DFC-A18A-4303-88E5-2C11EC575FCF','Instellingen wachtticket beheren',@dataAreaID --Manage suspension settings
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1A4FE3AB-35F7-41FC-A166-C562651DF175','Brandstofinstellingen beheren',@dataAreaID --Manage fuel settings
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'5B9150B2-30D6-4267-83F4-8EE83AC9CDEC','Toegelaten betalingsinstellingen beheren',@dataAreaID --Manage allowed payment settings
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'EBE2BF19-17C0-4B1E-88F5-FFD46883C15D','Klantgroepen beheren',@dataAreaID --Manage customer groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'e35842f0-ba3c-11de-8a39-0800200c9a66','Terminals bekijken',@dataAreaID --View terminals
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'e8a33df0-ba3c-11de-8a39-0800200c9a66','Terminals bewerken',@dataAreaID --Edit terminals
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'a0e9a5a0-09bd-11df-8a39-0800200c9a66','Retailartikelen bekijken',@dataAreaID --View retail items
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'a6c71ca0-09bd-11df-8a39-0800200c9a66','Retailartikelen bewerken',@dataAreaID --Edit retail items
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'365B452C-0520-40CA-9D73-1C97D8E366E2','Meerdere bewerkingen artikelen',@dataAreaID --Multi edit items
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C40E5050-9A39-11DF-981C-0800200C9A66','Retailafdelingen beheren',@dataAreaID --Manage retail departments
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'f16aa510-0bf7-11df-8a39-0800200c9a66','Barcodes retailartikel beheren',@dataAreaID --Manage retail item bar codes
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'6C12501A-0229-4067-A3E3-C0A175E48789','Barcode-instelling beheren',@dataAreaID -- Manage bar code setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'0438a1a0-118f-11df-8a39-0800200c9a66','Barcodemaskers beheren',@dataAreaID --Manage bar code masks
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'176c1b40-0bf8-11df-8a39-0800200c9a66','Afmetingen retailartikel beheren',@dataAreaID --Manage retail item dimensions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ab59b550-2080-11df-8a39-0800200c9a66','Prijzen handelsovereenkomst beheren',@dataAreaID --Manage trade agreement prices
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'87ea40f0-6cb9-11df-be2b-0800200c9a66','Klantprijs/kortingsgroepen bekijken',@dataAreaID --View customer price/discount groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'8edd6db0-6cb9-11df-be2b-0800200c9a66','Klantprijs/kortingsgroepen bewerken',@dataAreaID --Edit customer price/discount groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'82e990f0-7241-11df-93f2-0800200c9a66','Kortingsgroepen artikel bekijken',@dataAreaID --View item discount groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'b8ce1b00-7241-11df-93f2-0800200c9a66','Kortingsgroepen artikel bewerken',@dataAreaID --Edit item discount groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'78056f10-7890-11df-93f2-0800200c9a66','Btw-instellingen bekijken',@dataAreaID --View sales tax setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'7c841960-7890-11df-93f2-0800200c9a66','Btw-instellingen bewerken',@dataAreaID --Edit sales tax setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ac55d1a0-85e3-11df-a4ee-0800200c9a66','POS-licenties beheren',@dataAreaID --Manage POS licenses
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'90886df0-89aa-11df-a4ee-0800200c9a66','Kascontroles',@dataAreaID --Tender declarations
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'b5d318d0-9643-11df-981c-0800200c9a66','Kortingen beheren',@dataAreaID --Manage discounts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'828E4BB0-9A69-4D30-A2C4-BB34DEA25DDA','Retaildivisies beheren',@dataAreaID --Manage retail divisions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ecee9350-89ad-11df-a4ee-0800200c9a66','Valuta''s bewerken',@dataAreaID --Edit currencies
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'992a90f0-89af-11df-a4ee-0800200c9a66','Valuta''s bekijken',@dataAreaID --View currencies
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'15c8b5b0-a6de-11df-981c-0800200c9a66','Retailgroepen beheren',@dataAreaID --Manage retail groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'b93ae980-a3b8-11df-981c-0800200c9a66','Instelling hospitality beheren',@dataAreaID --Manage hospitality setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'62abdc60-ae93-11df-94e2-0800200c9a66','Speciale groepen beheren',@dataAreaID --Manage special groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'8e1c4e00-b4ee-11df-8d81-0800200c9a66','Instelling infocode bekijken',@dataAreaID --View infocode setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'cfbe2320-b506-11df-8d81-0800200c9a66','Instelling infocode bewerken',@dataAreaID --Edit infocode setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'21294200-bc03-11df-851a-0800200c9a66','Artikeleenheden beheren',@dataAreaID --Manage item units
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'3ff1a240-c0b8-11df-851a-0800200c9a66','Prijsgroepen bekijken',@dataAreaID --View price groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'FF5CECCE-80E9-402B-8720-F65AF31DA820','Prijsgroepen bewerken',@dataAreaID --Edit price groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'4991ec80-c72d-11df-bd3b-0800200c9a66','Getallenreeksen beheren',@dataAreaID --Manage number sequences
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1eec0bd0-dac6-11df-937b-0800200c9a66','Verkooptypes beheren',@dataAreaID --Manage sales types
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'329f05f0-e8c8-11df-9492-0800200c9a66','Hospitalitytypes beheren',@dataAreaID --Manage hospitality types
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ea1c5150-f0b7-11df-98cf-0800200c9a66','Berekening dagafsluiting beheren',@dataAreaID --Manage Calculate Statement
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'fb1ce320-f0b7-11df-98cf-0800200c9a66','Boeking dagafsluiting beheren',@dataAreaID --Manage Post Statement
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'0b79c730-f624-11df-98cf-0800200c9a66','Leveranciers bekijken',@dataAreaID --View vendors
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'6bca4d80-f624-11df-98cf-0800200c9a66','Leveranciers bewerken',@dataAreaID --Edit vendors
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'55b825c0-f3eb-11df-98cf-0800200c9a66','Indelingen tafels beheren',@dataAreaID --Manage dining table layouts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'34bd0c00-f3e1-11df-98cf-0800200c9a66','Aankooporders beheren',@dataAreaID --Manage purchase orders
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'36D84296-316B-4A06-8E9B-DA6D5BA3D2B5','Inventarisaanpassingen voor alle winkels beheren',@dataAreaID --Manage inventory adjustments for all stores
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ad735180-f6df-11df-98cf-0800200c9a66','Inventarisaanpassingen bekijken',@dataAreaID --View inventory adjustments
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'4f4eb240-f6e7-11df-98cf-0800200c9a66','Inventarisaanpassingen bewerken',@dataAreaID --Edit inventory adjustments
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'b13a3ed0-02b7-11e0-a976-0800200c9a66','Menutypes restaurant beheren',@dataAreaID --Manage restaurant menu types
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'c07b5430-03a5-11e0-a976-0800200c9a66','Afdrukken station beheren',@dataAreaID --Manage station printing
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'6ed29390-09f0-11e0-81e0-0800200c9a66','Voorraadtelling bekijken en bewerken',@dataAreaID --View and edit stock counting
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'b737bf90-0c3c-11e0-81e0-0800200c9a66','POS-menu''s bewerken',@dataAreaID --Edit POS menus
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'0da1ad40-0c3e-11e0-81e0-0800200c9a66','POS-menu''s bekijken',@dataAreaID --View POS menus
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'04AD0711-49C3-464C-9440-06B05F8B59C6','Instellingen EFT-mapping bewerken',@dataAreaID --Edit EFT mapping setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'CA74D61C-7894-4364-BD91-55A3F98E6A0F','Instellingen EFT-mapping bekijken',@dataAreaID --View EFT mapping setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'c73861f0-0d0a-11e0-81e0-0800200c9a66','Ontvangstdocumenten beheren',@dataAreaID --Manage goods receiving documents
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'66323910-19b4-11e0-ac64-0800200c9a66','Menugroepen beheren',@dataAreaID --Manage menu groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'53956c90-27cf-11e0-91fa-0800200c9a66','Remote hosts beheren',@dataAreaID --Manage remote hosts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'6b2d3670-643e-11e0-ae3e-0800200c9a66','Kascontroles',@dataAreaID --Tender declarations
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'29a9f4b0-7fe4-11e0-b278-0800200c9a66','Voorraadniveauverslag bekijken',@dataAreaID --View stock level report
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'81caa200-a232-11e0-8264-0800200c9a66','Verbonden artikelen beheren',@dataAreaID --Manage linked items
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'88e35e80-a23a-11e0-8264-0800200c9a66','Cadeaukaarten beheren',@dataAreaID --Manage gift cards
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'23329470-d232-11e0-9572-0800200c9a66','Kredietbonnen beheren',@dataAreaID --Manage credit vouchers
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'15de9180-e078-11e0-9572-0800200c9a66','Centraal wachtticket beheren',@dataAreaID --Manage central suspensions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'F61938B8-DB15-4714-94F9-1AD16DDDF5FB','Aankooporder boeken en ontvangen',@dataAreaID --Post and receive purchase order
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'8E677D9F-EE02-4A52-9768-0DF78F985126','Lijnen ontvangstdocument automatisch invullen',@dataAreaID --Auto populate goods receiving document lines
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'c26aa590-f65f-11e0-be50-0800200c9a66','Msr-kaartlinks beheren',@dataAreaID --Manage msr card links
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'2007e06d-60c6-45cb-9053-3c47a34bc33a','Inkomsten- en uitgaventypes bekijken',@dataAreaID --View Income and Expense Types
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'dc287d10-9842-11e1-a8b0-0800200c9a66','Verslagen beheren',@dataAreaID --Manage reports
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'F7BC3A76-0911-4A2B-9632-55EFC7B18682','Profielen Kitchen Manager beheren',@dataAreaID --Manage Kitchen Manager profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'9D520883-3C20-4F56-903A-6233897BA26B','Stations keukendisplay beheren',@dataAreaID --Manage Kitchen Display Stations
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'19C10A2A-B228-47FF-8F47-558CD1E8DFCE','Profielen keukendisplay beheren',@dataAreaID --Manage Kitchen Display Profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'7d1ef450-6edb-11e2-bcfd-0800200c9a66','Authenticatietokens beheren',@dataAreaID --Manage authentication tokens
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'d541ded2-a8a4-4e26-934a-fa87c21036ae','Keukenprinters beheren',@dataAreaID --Manage Kitchen printers

exec spSECURITY_AddPermissionLocalization_1_0 @locale,'799DD5EB-6906-4955-BFD4-AC4304DE4EA3','Templates configuratiewizard bekijken',@dataAreaID --View configuration wizard templates
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'500329C3-054C-43AF-9281-B312909BFADA','Templates configuratiewizard bewerken',@dataAreaID --Edit configuration wizard templates

exec spSECURITY_AddPermissionLocalization_1_0 @locale,'8649312D-14F8-4255-BA94-C02712B903D0','Aanvragen voor inventarisoverdracht bekijken',@dataAreaID --View inventory transfer requests
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'EB9693BB-B825-4010-AFDE-B56CF2DD3183','Aanvragen voor inventarisoverdracht bewerken',@dataAreaID --Edit inventory transfer requests
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'E329EB84-B449-4239-AED8-C6AEC374AD67','Inventarisoverdrachtsorders bekijken',@dataAreaID --View inventory transfer orders
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'D404EDDE-1303-46FD-9FF2-5A9D4247DFE8','Weet u zeker dat u de geselecteerde vertaling wilt verwijderen?',@dataAreaID --Edit inventory transfer orders

exec spSECURITY_AddPermissionLocalization_1_0 @locale,'aab3bc37-dca6-4407-9941-e1692c7ccf80','Artikelgrootboek bekijken',@dataAreaID --View item ledger

exec spSECURITY_AddPermissionLocalization_1_0 @locale,'4342D98A-CDEB-409D-80F2-FCA069DA2782','Aanvulling beheren',@dataAreaID --Manage replenishment
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'B1F9E246-5EAB-4FC5-BDB4-E1F769787447','Gereserveerde inventaris beheren',@dataAreaID --Manage parked inventory
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C3109275-6E3F-491A-A37C-CA32BD3014F1','Inventarisaanpassingen beheren',@dataAreaID --Manage inventory adjustments
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'AA1822B1-0953-4B90-9DAC-307E5B88DB3F','Voorraadreserveringen beheren',@dataAreaID --Manage stock reservations
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'411B0B74-BBD7-4F01-94CB-D8294376E0F9','Voorraadreserveringen voor alle winkels beheren',@dataAreaID --Manage stock reservations for all stores
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'B50FFD7C-5A65-4871-8DDC-DB3039F6029E','Gereserveerde inventaris voor alle winkels beheren',@dataAreaID --Manage parked inventory for all stores

exec spSECURITY_AddPermissionLocalization_1_0 @locale,'8DECDF32-753D-4ECF-A64D-E93722D3C3CF', 'Lijnen grootboek klant bekijken',@dataAreaID --View customer ledger entries
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'10C08A81-CB1F-4FC9-8C9C-86B715F1F83C', 'Lijnen grootboek klant bewerken', @dataAreaID --Edit customer ledger entries'
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'80921901-b919-452f-9c97-6029acabc053', 'Getrouwheidsplannen bekijken', @dataAreaID --View loyalty schemes
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'80921902-b919-452f-9c97-6029acabc053', 'Getrouwheidsplannen bewerken', @dataAreaID --Edit loyalty schemes
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'80921903-b919-452f-9c97-6029acabc053', 'Getrouwheidskaarten bekijken', @dataAreaID --View loyalty cards
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'80921904-b919-452f-9c97-6029acabc053', 'Getrouwheidskaarten bewerken', @dataAreaID --Edit loyalty cards
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'D0E05BAE-4AD7-4E3E-A35C-D9B63D8F4039', 'Getrouwheidstransacties bekijken', @dataAreaID --View loyalty transactions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'26542533-9583-4648-8b2d-847eb5bddad1', 'Importprofielen beheren', @dataAreaID --Manage import profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'E8C7C5C0-FA5D-4BDE-8516-E616D0B86B87', 'Instelling stijlen beheren', @dataAreaID --Manage styles setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'47EC274D-B7F0-4F24-80C6-2E1FF42F144F', 'Afbeeldingsdatabase beheren', @dataAreaID --Manage image bank

-- Label templates
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'735C60D3-CBCF-443E-81FD-19904F9232C5','Labeltemplates bekijken',@dataAreaID --View label templates
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'49940BC5-A609-40D1-A3FB-729E8BA4284B','Labeltemplates bewerken',@dataAreaID --Edit label templates
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'DBFBB019-E273-40E3-896C-F217C88FDD2B','Labels afdrukken',@dataAreaID --Print labels

--Pos Permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9a0-a9a8-11e1-afa6-0800200c9a66','Artikel verkoop',@dataAreaID --Item Sale
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9a1-a9a8-11e1-afa6-0800200c9a66','Prijscontrole',@dataAreaID --Price Check
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9a2-a9a8-11e1-afa6-0800200c9a66','Artikel ongeldig maken',@dataAreaID --Void Item
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9a3-a9a8-11e1-afa6-0800200c9a66','Item nota',@dataAreaID --Item Comment
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9a4-a9a8-11e1-afa6-0800200c9a66','Prijs overschrijven',@dataAreaID --Price Override
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9a5-a9a8-11e1-afa6-0800200c9a66','Hoeveelheid instellen',@dataAreaID --Set quantity
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9a6-a9a8-11e1-afa6-0800200c9a66','Hoeveelheid wissen',@dataAreaID --Clear quantity
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9a7-a9a8-11e1-afa6-0800200c9a66','Artikel zoeken',@dataAreaID --Item Search
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9a8-a9a8-11e1-afa6-0800200c9a66','Artikel teruggeven',@dataAreaID --Return Item
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9a9-a9a8-11e1-afa6-0800200c9a66','Retour transactie',@dataAreaID --Return Transaction
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9aa-a9a8-11e1-afa6-0800200c9a66','Journaal tonen',@dataAreaID --Show Journal
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9ab-a9a8-11e1-afa6-0800200c9a66','Aanvraag getrouwheid',@dataAreaID --Loyalty Request
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'F42F5BB2-E92D-46A6-87EE-1DBF9784A648','Loyalty puntenkorting',@dataAreaID --Loyalty points discount

exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9ac-a9a8-11e1-afa6-0800200c9a66','Verkoper wissen',@dataAreaID --Clear Salesperson
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9ad-a9a8-11e1-afa6-0800200c9a66','Factuur nota',@dataAreaID --Invoice Comment
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9ae-a9a8-11e1-afa6-0800200c9a66','Meeteenheid wijzigen',@dataAreaID --Change Unit of Measure
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9b1-a9a8-11e1-afa6-0800200c9a66','Infocode op verzoek',@dataAreaID --Infocode On Request
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9b3-a9a8-11e1-afa6-0800200c9a66','Commentaar bij artikel wijzigen',@dataAreaID --Change Item Comments
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9b4-a9a8-11e1-afa6-0800200c9a66','Cash betalen',@dataAreaID --Pay Cash
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9b5-a9a8-11e1-afa6-0800200c9a66','Met kaart betalen',@dataAreaID --Pay Card
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9b6-a9a8-11e1-afa6-0800200c9a66','Rekening klant betalen',@dataAreaID --Pay Customer Account
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9b7-a9a8-11e1-afa6-0800200c9a66','Valuta betalen',@dataAreaID --Pay Currency
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9b8-a9a8-11e1-afa6-0800200c9a66','Met cheque betalen',@dataAreaID --Pay Check
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9b9-a9a8-11e1-afa6-0800200c9a66','Cash Quick betalen',@dataAreaID --Pay Cash Quick
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9ba-a9a8-11e1-afa6-0800200c9a66','Bedrijfskaart betalen',@dataAreaID --Pay Corporate Card
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9bb-a9a8-11e1-afa6-0800200c9a66','Betaling ongeldig maken',@dataAreaID --Void Payment
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9bc-a9a8-11e1-afa6-0800200c9a66','Credit memo betalen',@dataAreaID --Pay Credit Memo
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9bd-a9a8-11e1-afa6-0800200c9a66','Betaal cadeaubon',@dataAreaID --Pay gift card
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9be-a9a8-11e1-afa6-0800200c9a66','Lijnkorting aantal',@dataAreaID --Line Discount Amount
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0b0-a9a8-11e1-afa6-0800200c9a66','Lijnkorting percentage',@dataAreaID --Line Discount Percent
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0b1-a9a8-11e1-afa6-0800200c9a66','Totale kortingshoeveelheid',@dataAreaID --Total Discount Amount
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0b2-a9a8-11e1-afa6-0800200c9a66','Totale kortingsprocent',@dataAreaID --Total Discount Percent
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'8BFE0874-54B3-4AAE-9ACB-09926BC3A31B','Periodieke korting handmatig activeren',@dataAreaID --Manually trigger periodic discount
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0b5-a9a8-11e1-afa6-0800200c9a66','Transactie ongeldig maken',@dataAreaID --Void Transaction
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0b6-a9a8-11e1-afa6-0800200c9a66','Commentaar bij transactie',@dataAreaID --Transaction Comment
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0b7-a9a8-11e1-afa6-0800200c9a66','Verkoper',@dataAreaID --Sales Person
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0b8-a9a8-11e1-afa6-0800200c9a66','Transactie opschorten',@dataAreaID --Suspend Transaction
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0b9-a9a8-11e1-afa6-0800200c9a66','Transactie herroepen',@dataAreaID --Recall Transaction
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0ba-a9a8-11e1-afa6-0800200c9a66','Geneesmiddelenvoorschrift annuleren',@dataAreaID --Pharmacy Prescription Cancel
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0bb-a9a8-11e1-afa6-0800200c9a66','Geneesmiddelenvoorschriften',@dataAreaID --Pharmacy Prescriptions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0bc-a9a8-11e1-afa6-0800200c9a66','Credit memo uitgeven',@dataAreaID --Issue Credit Memo
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0bd-a9a8-11e1-afa6-0800200c9a66','Cadeaucertificaat uitgeven',@dataAreaID --Issue Gift Certificate
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'3957A752-254D-4705-8857-3418E4F15C69','Saldo cadeaukaart ophalen',@dataAreaID --Get gift card balance
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0be-a9a8-11e1-afa6-0800200c9a66','Totaal weergeven',@dataAreaID --Display Total
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0bf-a9a8-11e1-afa6-0800200c9a66','Verkooporder',@dataAreaID --Sales Order
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0c0-a9a8-11e1-afa6-0800200c9a66','Verkoopfactuur',@dataAreaID --Sales Invoice
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0c1-a9a8-11e1-afa6-0800200c9a66','Inkomstenrekening',@dataAreaID --Income Account
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0c2-a9a8-11e1-afa6-0800200c9a66','Onkostenrekening',@dataAreaID --Expense Account
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0c3-a9a8-11e1-afa6-0800200c9a66','Terugkerende inkomstenrekeningen',@dataAreaID --Return Income Accounts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0c4-a9a8-11e1-afa6-0800200c9a66','Terugkerende uitgavenrekeningen',@dataAreaID --Return Expense Accounts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'39edafe0-a9a2-11e1-afa6-0800200c9a66','Klant zoeken',@dataAreaID --Customer Search
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'80e83310-a9a3-11e1-afa6-0800200c9a66','Klant wissen',@dataAreaID --Customer Clear
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'5c1b01b0-a9a4-11e1-afa6-0800200c9a66','Klant transacties',@dataAreaID --Customer Transactions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ee006700-a9a4-11e1-afa6-0800200c9a66','Verslag klant transacties',@dataAreaID --Customer Transactions Report
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'432b8890-a9a5-11e1-afa6-0800200c9a66','Afmelden',@dataAreaID --Log Off
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'cc68e530-a9a5-11e1-afa6-0800200c9a66','Terminal vergrendelen',@dataAreaID --Lock Terminal
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'563d2aa0-a9a6-11e1-afa6-0800200c9a66','Afmelden forceren',@dataAreaID --Log Off Force
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'c24bd340-a9a6-11e1-afa6-0800200c9a66','Opzoeken inventaris',@dataAreaID --Inventory Lookup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'32216ef0-a9a7-11e1-afa6-0800200c9a66','Z-verslag initialiseren',@dataAreaID --Initialize Z Report
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'808bbe60-a9a7-11e1-afa6-0800200c9a66','Afdrukken X',@dataAreaID --Print X
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'cee37030-a9a7-11e1-afa6-0800200c9a66','Afdrukken Z',@dataAreaID --Print Z
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1B4B77E2-A287-4219-B068-C4463C1DD32D','Verslag artikelverkoop afdrukken',@dataAreaID --Print item sales report
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'4cc27dc0-a9a8-11e1-afa6-0800200c9a66','Ontwerpmodus inschakelen',@dataAreaID --Design Mode Enable
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'b3ff6280-aa36-11e1-afa6-0800200c9a66','POS-venster minimaliseren',@dataAreaID --Minimize POS Window
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1396aaf0-aa37-11e1-afa6-0800200c9a66','Blanco handeling',@dataAreaID --Blank Operation
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'018BC3EB-19F6-441A-9295-64A0468C108E','Externe opdracht uitvoeren',@dataAreaID --Run external command
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'0E7B1B16-B4A8-4FAB-B8CD-71B51696D431','POS plugin uitvoeren',@dataAreaID --Execute POS plugin
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'67f1d250-aa37-11e1-afa6-0800200c9a66','Lade openen',@dataAreaID --Open Drawer
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'c61cb980-aa37-11e1-afa6-0800200c9a66','End of Day',@dataAreaID --End Of Day
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'30685560-aa38-11e1-afa6-0800200c9a66','End of Shift',@dataAreaID --End Of Shift
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'7a5dbe80-aa38-11e1-afa6-0800200c9a66','Kascontrole',@dataAreaID --Tender Declaration
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'d0120d40-aa38-11e1-afa6-0800200c9a66','Deposito rekening klant',@dataAreaID --Customer Account Deposit
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1e2eb1e0-aa39-11e1-afa6-0800200c9a66','Startbedrag aangeven',@dataAreaID --Declare Start Amount
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'6cc36cb0-aa39-11e1-afa6-0800200c9a66','Float entry',@dataAreaID --Float Entry
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'c9154e20-aa39-11e1-afa6-0800200c9a66','Verwijdering van de offerte',@dataAreaID --Tender Removal
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'0d3cd0a0-aa3a-11e1-afa6-0800200c9a66','Safe Drop',@dataAreaID --Safe Drop
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'891e8790-aa3a-11e1-afa6-0800200c9a66','Bank Drop',@dataAreaID --Bank Drop
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'2d7c78b0-aa3b-11e1-afa6-0800200c9a66','Safe Drop Reversal',@dataAreaID --Safe Drop Reversal
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'97105d50-aa3b-11e1-afa6-0800200c9a66','Bank Drop Reversal',@dataAreaID --Bank Drop Reversal
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ed067000-aa3b-11e1-afa6-0800200c9a66','Factuur opsplitsen',@dataAreaID --Split Bill
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'48857110-aa3c-11e1-afa6-0800200c9a66','Uit Hospitality POS',@dataAreaID --Exit Hospitality POS
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ad42ee70-aa3c-11e1-afa6-0800200c9a66','Hospitalitymenutype afdrukken',@dataAreaID --Print Hospitality Menu Type
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'0b1136b0-aa3d-11e1-afa6-0800200c9a66','Hospitalitymenutype instellen',@dataAreaID --Set Hospitality Menu Type
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'3f89fbc0-aa3d-11e1-afa6-0800200c9a66','Hospitalitymenutype wijzigen',@dataAreaID --Change Hospitality Menu Type
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'7906eed0-aa3d-11e1-afa6-0800200c9a66','Transactie met belastingsvrijstelling',@dataAreaID --Tax Exempt Transaction
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'b7d1c310-aa3d-11e1-afa6-0800200c9a66','Belastingsvrijstelling transactie wissen',@dataAreaID --Clear Transaction Tax Exemption
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'fc8bc190-aa3d-11e1-afa6-0800200c9a66','Infocode belastingsgroep wijziging',@dataAreaID --Infocode Tax Group Change
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'f5215570-bbb6-11e1-afa7-0800200c9a66','Stijlprofielen bewerken',@dataAreaID --Edit Style profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'3134f8f0-bbb7-11e1-afa7-0800200c9a66','Stijlprofielen bekijken',@dataAreaID --View Style profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'76078220-d1a8-11e1-9b23-0800200c9a66','Formulierprofielen bekijken',@dataAreaID --View Form Profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'12f12fa0-d1a9-11e1-9b23-0800200c9a66','Formulierprofielen bewerken',@dataAreaID --Edit Form Profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'59270E24-FF01-4C88-9AEA-11BDED40A953','Contexten bekijken',@dataAreaID --View Context
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'3F1D120B-207A-4A0A-A56D-2EAD1B2814EE','Contexten bewerken',@dataAreaID --Edit Contexts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'B15D8168-F2E8-4093-95EB-582E068AD9EC','Klant toevoegen',@dataAreaID --Customer add
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'38402373-1A92-483F-B77B-E18C986BB586','Bestelling verplaatsen',@dataAreaID --Bump order
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'20E06A62-BAE4-4EEC-BB04-A4A708956904','Getrouwheid betalen',@dataAreaID --Pay loyalty
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'214B99CE-0C26-4D16-B65B-EA555708B4F0','Klant aan getrouwheidskaart toevoegen',@dataAreaID --Add customer to loyalty card
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'17B02E4B-ABCB-46A0-BE29-B8FACAB2F434','Integratie-kartering bekijken',@dataAreaID --View integration mapping
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'DACEDF0B-0709-4B3B-9A27-0FC7DB1FD6BD','Integratie-kartering bewerken',@dataAreaID --Edit integration mapping
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'5E00968F-DEF1-461C-995C-034DFAAF2619','Databasekaart bekijken',@dataAreaID --View database map
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'3a6ed366-3d24-4263-b8d4-6e2a74629d94','Financieel verslag',@dataAreaID --Financial report
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'9E435E02-17C8-4BA4-9365-B1A1EA3C8D30','Integratielog bekijken',@dataAreaID --View integration log
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'B2FE62A8-ECA5-4EB7-8C05-36A0E2E290AD','Begin werkdag',@dataAreaID --Start of day
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'235EE80D-2B89-4CA3-BA0C-5527BFF93ACC','Hardwareprofiel op POS beheren',@dataAreaID --Manage hardware profile on POS
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'41E5E014-9DAA-481E-95F3-0BD3E3896556','Klantbestellingen',@dataAreaID --Customer orders
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'2504F28E-2210-4E65-BA6A-3D611F63197E','Offertes',@dataAreaID --Quotes
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C35A6965-FAE3-45DB-AACF-F0606F6C4838','Beheer klantbestelling',@dataAreaID --Customer order management
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'DD054719-5FF9-4F41-81F2-B282921F1924','Minimaal deposito overschrijven',@dataAreaID --Override minimum deposit
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'DF5E32BF-C726-4A24-B2A6-2104F06D590D','Redencode instellen',@dataAreaID --Set reason code
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C3A5300F-593E-488C-B080-04025A3BC253','Manage POS customer blocking',@dataAreaID -- Manage POS customer blocking

exec spSECURITY_AddPermissionLocalization_1_0 @locale,'BF61E0E6-AEF1-4AF3-B95F-B6C68DC91D64','POS-app afsluiten',@dataAreaID --Exit POS application
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'2279A999-A5CB-44B8-805F-8684E6E9FCD7','Computer opnieuw starten',@dataAreaID --Restart computer
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'5C3CCB66-2EEB-4252-9E91-30C2C603EBBC','Computer uitschakelen',@dataAreaID --Shutdown computer
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'B7464BC4-47B2-48BA-B82A-306E658ECD96','Leermodus inschakelen',@dataAreaID --Activate training mode

-- User and security management Scheduler (anyNewGuid, description, permissionGroupGUID, sortCode, PermissionCodeGUID, CodeIsEncrypted)
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'58428E18-BFAC-4DAC-9054-1F159AC9CCEA','Toegangsreplicatie',@dataAreaID --Access replication
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'E9BD9613-0AB0-4502-951D-6A4701EDFD6F','Distributielocaties bewerken',@dataAreaID --Edit distribution locations
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C92401C4-7717-4F6F-806C-9130666A896C','Distributielocaties bekijken',@dataAreaID --View distribution locations

-- 
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'493ED36B-EFDA-4604-BA7D-3DDC7D7381CF','Jobs en subjobs bekijken',@dataAreaID --View jobs and subjobs
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'A7D518EE-1AD7-49FC-B30B-2D266D0D4D67','Meerdere subjobs aanmaken',@dataAreaID --Create multiple subjobs
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'D02A5A6C-0C02-414E-9B42-2E2C1DE658D5','Instellingen bewerken',@dataAreaID --Edit settings
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'19A1CF87-8394-4456-B034-5018665A1AD8','Databaseontwerp bekijken',@dataAreaID --View database design
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C47B6771-F61A-4EDE-9062-5244CCCD06D2','Jobs en subjobs bewerken',@dataAreaID --Edit jobs and subjobs
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'B8169851-A8B9-44CB-AF45-88CD6AE321A2','Databasekaart bewerken',@dataAreaID --Edit database map
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'50E152C0-907B-44D8-ACCB-9B900785FD85','Databaseontwerp bewerken',@dataAreaID --Edit database design
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'818ACA49-4542-48BD-813F-A6A8E3450F31','Instellingen bekijken',@dataAreaID --View settings
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'660C23AD-3D5D-4758-B80D-E8E539B56E73','Een job handmatig uitvoeren',@dataAreaID --Manually run a job


--Backup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'21EA337E-31BC-4709-B018-D468FD77F492','Back-up database',@dataAreaID --Backup database

--Reprint
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'7B11B93C-D7EB-4EC5-BC0E-CBC66BB61E2B','Ontvangstbewijs opnieuw afdrukken',@dataAreaID --Reprint receipt

--Customer orders
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'2559EBBC-0175-4E57-9F1A-8DA7A0C621E5','Instellingen klantbestelling beheren',@dataAreaID --Manage customer order settings
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'70729423-230E-46BC-844C-BEE84A3E2358','Klantbestellingen beheren',@dataAreaID --Manage customer orders

exec spSECURITY_AddPermissionLocalization_1_0 @locale,'d8cd437d-d27c-4cb9-a4b9-af0a6cfd3549','Controle handelingen terminal bekijken',@dataAreaID --View terminal operations audit
