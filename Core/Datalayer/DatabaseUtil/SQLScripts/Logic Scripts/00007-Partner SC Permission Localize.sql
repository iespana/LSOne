/*
-- Use this document to localize Store Controller permissions and permission groups. Remove the outer comments to get the document working.


-- Do not change this section
-- =================================================================================================================================================================
Use LSPOSNET 

declare @dataAreaID nvarchar(10)
declare @locale nvarchar(10)
set @dataAreaID = 'LSR'
-- =================================================================================================================================================================




set @locale = 'is' -- CHANGE 'is' TO YOUR LANGUAGE CODE

/* YOUR TRANSLATIONS GO BEHIND THE GUID PARAMETER, BUT BEFORE PARAMETER @dataAreaID*/
-- Permission group localizations
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'DF4AFBD0-B35C-11DE-8A39-0800200C9A66','Your translation goes here for "Profiles"',@dataAreaID --Profiles
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'1DDEEF60-D9AE-11DE-8A39-0800200C9A66','',@dataAreaID --General
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'78535D50-04E7-11DF-8A39-0800200C9A66','',@dataAreaID --Item master
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'0CA8E620-E997-11DA-8AD9-0800200C9A66','',@dataAreaID --User and security management
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'CEAC1AD0-E997-11DA-8AD9-0800200C9A66','',@dataAreaID --Reports
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'808ED9F0-E997-11DA-8AD9-0800200C9A66','',@dataAreaID --System Administration
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'a54c7dc0-a922-11df-94e2-0800200c9a66','',@dataAreaID --Hospitality
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'32246200-f0bb-11df-98cf-0800200c9a66','',@dataAreaID --Inventory


-- Permission localizations
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'322180d0-cdfd-11de-8a39-0800200c9a66','',@dataAreaID --Create new POS users
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'3ed26790-cdfd-11de-8a39-0800200c9a66','',@dataAreaID --Edit POS users
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'43479c00-cdfd-11de-8a39-0800200c9a66','',@dataAreaID --Delete POS users
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'e13c05d0-cf89-11de-8a39-0800200c9a66','',@dataAreaID --View action permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'e694e380-cf89-11de-8a39-0800200c9a66','',@dataAreaID --Edit action permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1d608220-fa2c-11da-974d-0800200c9a66','',@dataAreaID --Create new users
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'d32f5960-554d-11db-b0de-0800200c9a66','',@dataAreaID --Edit user
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'9b82f9b0-face-11da-974d-0800200c9a66','',@dataAreaID --Delete users
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'e1cf2610-6049-11db-b0de-0800200c9a66','',@dataAreaID --Enable/Disable users
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'a0ae94a0-fa04-11da-974d-0800200c9a66','',@dataAreaID --Reset password for other users
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'4d3f3f70-5d02-11db-b0de-0800200c9a66','',@dataAreaID --Create user groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'fdab47a0-5d02-11db-b0de-0800200c9a66','',@dataAreaID --Edit user groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'762df510-5d03-11db-b0de-0800200c9a66','',@dataAreaID --Delete user groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1906ecc0-fac0-11da-974d-0800200c9a66','',@dataAreaID --Assign users to groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'39f09b00-fac2-11da-974d-0800200c9a66','',@dataAreaID --Grant/Deny/Inherit permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'eca62890-fac3-11da-974d-0800200c9a66','',@dataAreaID --Grant higher user permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'8c7c3250-5eb6-11db-b0de-0800200c9a66','',@dataAreaID --Manage user permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'628e9c80-5d03-11db-b0de-0800200c9a66','',@dataAreaID --Manage group permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'48078c50-6d7f-11db-9fe1-0800200c9a66','',@dataAreaID --View audit logs
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'fb0d8940-fb74-11de-8a39-0800200c9a66','',@dataAreaID --View update service schedule
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'13d44f40-fb75-11de-8a39-0800200c9a66','',@dataAreaID --Manage update service schedule
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'a6164dc0-a229-11db-8ab9-0800200c9a66','',@dataAreaID --Maintain administrative settings
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'F20F86F0-AC18-11DE-8A39-0800200C9A66','',@dataAreaID --Manage audit logs
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'31dfe220-b35d-11de-8a39-0800200c9a66','',@dataAreaID --View visual profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'60876530-b35d-11de-8a39-0800200c9a66','',@dataAreaID --Edit visual profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'9a413920-db3c-11de-8a39-0800200c9a66','',@dataAreaID --View functionality profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'a1372500-db3c-11de-8a39-0800200c9a66','',@dataAreaID --Edit functionality profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'3edc9f70-d44d-11de-8a39-0800200c9a66','',@dataAreaID --View touch button layouts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'4929cca0-d44d-11de-8a39-0800200c9a66','',@dataAreaID --Edit touch button layouts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'40a5f690-dffa-11de-8a39-0800200c9a66','',@dataAreaID --View transaction service profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'47556390-dffa-11de-8a39-0800200c9a66','',@dataAreaID --Edit transaction service profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1ce28d90-e3ec-11de-8a39-0800200c9a66','',@dataAreaID --View hardware profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'24414d60-e3ec-11de-8a39-0800200c9a66','',@dataAreaID --Edit hardware profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'004a89b0-c53f-11de-8a39-0800200c9a66','',@dataAreaID --View card types
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'08427c40-c53f-11de-8a39-0800200c9a66','',@dataAreaID --Edit card types
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'d4e6db50-c941-11de-8a39-0800200c9a66','',@dataAreaID --View payment methods
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'dc259000-c941-11de-8a39-0800200c9a66','',@dataAreaID --Edit payment methods
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'651897b0-b72f-11de-8a39-0800200c9a66','',@dataAreaID --View stores
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'6b7db540-b72f-11de-8a39-0800200c9a66','',@dataAreaID --Edit stores
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'24faeb80-df2d-11de-8a39-0800200c9a66','',@dataAreaID --View receipts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'f7b3fcb0-e95a-11de-8a39-0800200c9a66','',@dataAreaID --View forms
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ff6dadc0-e95a-11de-8a39-0800200c9a66','',@dataAreaID --Edit forms
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'36f689f0-d9ad-11de-8a39-0800200c9a66','',@dataAreaID --View customers
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'9e542310-d9ac-11de-8a39-0800200c9a66','',@dataAreaID --Edit customers
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'e35842f0-ba3c-11de-8a39-0800200c9a66','',@dataAreaID --View terminals
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'e8a33df0-ba3c-11de-8a39-0800200c9a66','',@dataAreaID --Edit terminals
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'cdbf7f80-04e7-11df-8a39-0800200c9a66','',@dataAreaID --Edit Color/Sizes/Styles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'a0e9a5a0-09bd-11df-8a39-0800200c9a66','',@dataAreaID --View retail items
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'a6c71ca0-09bd-11df-8a39-0800200c9a66','',@dataAreaID --Edit retail items
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'f16aa510-0bf7-11df-8a39-0800200c9a66','',@dataAreaID --Manage retail item bar codes
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'0438a1a0-118f-11df-8a39-0800200c9a66','',@dataAreaID --Manage bar code masks
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'176c1b40-0bf8-11df-8a39-0800200c9a66','',@dataAreaID --Manage retail item dimensions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ab59b550-2080-11df-8a39-0800200c9a66','',@dataAreaID --Manage trade agreement prices
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'87ea40f0-6cb9-11df-be2b-0800200c9a66','',@dataAreaID --View customer price/discount groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'8edd6db0-6cb9-11df-be2b-0800200c9a66','',@dataAreaID --Edit customer price/discount groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'82e990f0-7241-11df-93f2-0800200c9a66','',@dataAreaID --View item discount groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'b8ce1b00-7241-11df-93f2-0800200c9a66','',@dataAreaID --Edit item discount groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'78056f10-7890-11df-93f2-0800200c9a66','',@dataAreaID --View sales tax setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'7c841960-7890-11df-93f2-0800200c9a66','',@dataAreaID --Edit sales tax setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ac55d1a0-85e3-11df-a4ee-0800200c9a66','',@dataAreaID --Manage POS licenses
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'90886df0-89aa-11df-a4ee-0800200c9a66','',@dataAreaID --Tender declarations
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'b5d318d0-9643-11df-981c-0800200c9a66','',@dataAreaID --Manage discounts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ecee9350-89ad-11df-a4ee-0800200c9a66','',@dataAreaID --Edit currencies
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'992a90f0-89af-11df-a4ee-0800200c9a66','',@dataAreaID --View currencies
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'15c8b5b0-a6de-11df-981c-0800200c9a66','',@dataAreaID --Manage retail groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'b93ae980-a3b8-11df-981c-0800200c9a66','',@dataAreaID --Manage hospitality setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'62abdc60-ae93-11df-94e2-0800200c9a66','',@dataAreaID --Manage special groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'8e1c4e00-b4ee-11df-8d81-0800200c9a66','',@dataAreaID --View infocode setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'cfbe2320-b506-11df-8d81-0800200c9a66','',@dataAreaID --Edit infocode setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'21294200-bc03-11df-851a-0800200c9a66','',@dataAreaID --Manage item units
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'3ff1a240-c0b8-11df-851a-0800200c9a66','',@dataAreaID --Manage price groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'4991ec80-c72d-11df-bd3b-0800200c9a66','',@dataAreaID --Manage number sequences
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1eec0bd0-dac6-11df-937b-0800200c9a66','',@dataAreaID --Manage sales types
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'329f05f0-e8c8-11df-9492-0800200c9a66','',@dataAreaID --Manage hospitality types
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ea1c5150-f0b7-11df-98cf-0800200c9a66','',@dataAreaID --Manage Calculate Statement
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'fb1ce320-f0b7-11df-98cf-0800200c9a66','',@dataAreaID --Manage Post Statement
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'0b79c730-f624-11df-98cf-0800200c9a66','',@dataAreaID --View vendors
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'6bca4d80-f624-11df-98cf-0800200c9a66','',@dataAreaID --Edit vendors
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'55b825c0-f3eb-11df-98cf-0800200c9a66','',@dataAreaID --Manage dining table layouts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'34bd0c00-f3e1-11df-98cf-0800200c9a66','',@dataAreaID --Manage purchase orders
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ad735180-f6df-11df-98cf-0800200c9a66','',@dataAreaID --View inventory adjustments
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'4f4eb240-f6e7-11df-98cf-0800200c9a66','',@dataAreaID --Edit inventory adjustments
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'b13a3ed0-02b7-11e0-a976-0800200c9a66','',@dataAreaID --Manage restaurant menu types
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'c07b5430-03a5-11e0-a976-0800200c9a66','',@dataAreaID --Manage station printing
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'6ed29390-09f0-11e0-81e0-0800200c9a66','',@dataAreaID --View and edit stock counting
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'b737bf90-0c3c-11e0-81e0-0800200c9a66','',@dataAreaID --Edit POS menus
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'0da1ad40-0c3e-11e0-81e0-0800200c9a66','',@dataAreaID --View POS menus
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'c73861f0-0d0a-11e0-81e0-0800200c9a66','',@dataAreaID --Manage goods receiving documents
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'66323910-19b4-11e0-ac64-0800200c9a66','',@dataAreaID --Manage menu groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'53956c90-27cf-11e0-91fa-0800200c9a66','',@dataAreaID --Manage remote hosts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'6b2d3670-643e-11e0-ae3e-0800200c9a66','',@dataAreaID --Tender declarations
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'29a9f4b0-7fe4-11e0-b278-0800200c9a66','',@dataAreaID --View stock level report
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'81caa200-a232-11e0-8264-0800200c9a66','',@dataAreaID --Manage linked items
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'88e35e80-a23a-11e0-8264-0800200c9a66','',@dataAreaID --Manage gift cards
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'23329470-d232-11e0-9572-0800200c9a66','',@dataAreaID --Manage credit vouchers
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'15de9180-e078-11e0-9572-0800200c9a66','',@dataAreaID --Manage central suspensions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'F61938B8-DB15-4714-94F9-1AD16DDDF5FB','',@dataAreaID --Post and receive purchase order
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'8E677D9F-EE02-4A52-9768-0DF78F985126','',@dataAreaID --Auto populate goods receiving document lines
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'c26aa590-f65f-11e0-be50-0800200c9a66','',@dataAreaID --Manage msr card links

*/





