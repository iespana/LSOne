
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
set @locale = 'is'


-- Permission group localizations
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'DF4AFBD0-B35C-11DE-8A39-0800200C9A66','Prófílar',@dataAreaID --Profiles
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'1DDEEF60-D9AE-11DE-8A39-0800200C9A66','Almennt',@dataAreaID --General
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'78535D50-04E7-11DF-8A39-0800200C9A66','Vörumaster',@dataAreaID --Item master
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'0CA8E620-E997-11DA-8AD9-0800200C9A66','Notanda og öryggisumsjón',@dataAreaID --User and security management
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'CEAC1AD0-E997-11DA-8AD9-0800200C9A66','Skýrslur',@dataAreaID --Reports
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'808ED9F0-E997-11DA-8AD9-0800200C9A66','Kerfisstjórnun',@dataAreaID --System Administration
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'a54c7dc0-a922-11df-94e2-0800200c9a66','Veitingakerfi',@dataAreaID --Hospitality
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'32246200-f0bb-11df-98cf-0800200c9a66','Birgðir',@dataAreaID --Inventory
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'7d750220-a99f-11e1-afa6-0800200c9a66','POS réttindi',@dataAreaID --POS permissions
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'2c562010-f660-11e0-be50-0800200c9a66','Vildarkerfi',@dataAreaID --Loyalty permissions
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'7ccab506-b497-403e-99e3-7141655e5bfc','Gagnaflutningur',@dataAreaID --Replication
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'9D0F831D-7B2B-4A0E-B9E6-349B50A235BE','Sölupantanir',@dataAreaID --Customer orders



-- Permission localizations
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'E13C05D0-CF89-11DE-8A39-0800200C9A66','Skoða aðgerðaréttindi',@dataAreaID --View action permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'E694E380-CF89-11DE-8A39-0800200C9A66','Breyta aðgerðaréttindum',@dataAreaID --Edit action permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1D608220-FA2C-11DA-974D-0800200C9A66','Búa til nýja notendur',@dataAreaID --Create new users
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'D32F5960-554D-11DB-B0DE-0800200C9A66','Breyta notendum',@dataAreaID --Edit user
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'BA589886-68D0-420B-B495-BE0426514EBA','Skoða notendur',@dataAreaID --View users
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'9B82F9B0-FACE-11DA-974D-0800200C9A66','Eyða notendum',@dataAreaID --Delete users
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'E1CF2610-6049-11DB-B0DE-0800200C9A66','Virkja/afvirkja notendur',@dataAreaID --Enable/Disable users
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'A0AE94A0-FA04-11DA-974D-0800200C9A66','Breyta lykilorði fyrir aðra notendur',@dataAreaID --Reset password for other users
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'4D3F3F70-5D02-11DB-B0DE-0800200C9A66','Búa til notendahópa',@dataAreaID --Create user groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'FDAB47A0-5D02-11DB-B0DE-0800200C9A66','Breyta notendahópum',@dataAreaID --Edit user groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'762DF510-5D03-11DB-B0DE-0800200C9A66','Eyða notendahópum',@dataAreaID --Delete user groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1906ECC0-FAC0-11DA-974D-0800200C9A66','Setja notendur í hópa',@dataAreaID --Assign users to groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'39F09B00-FAC2-11DA-974D-0800200C9A66','Gefa notendum réttindi',@dataAreaID --Grant/Deny/Inherit permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ECA62890-FAC3-11DA-974D-0800200C9A66','Gefa notendum hærri réttindi',@dataAreaID --Grant higher user permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'8C7C3250-5EB6-11DB-B0DE-0800200C9A66','Umsjón notendaréttinda',@dataAreaID --Manage user permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'882DF07B-4F3C-4202-A7F4-932616A6D41B','Umsjón með notendaprófílum',@dataAreaID --Manage user profile
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'628E9C80-5D03-11DB-B0DE-0800200C9A66','Umsjón hóparéttinda',@dataAreaID --Manage group permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'48078C50-6D7F-11DB-9FE1-0800200C9A66','Skoða eftirlitsskrá',@dataAreaID --View audit logs
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'FB0D8940-FB74-11DE-8A39-0800200C9A66','Skoða uppfærsluþjónustustillingar',@dataAreaID --View update service schedule
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'13D44F40-FB75-11DE-8A39-0800200C9A66','Umsjón uppfærsluþjónustuáætlunar',@dataAreaID --Manage update service schedule
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'A6164DC0-A229-11DB-8AB9-0800200C9A66','Breyta kerfisstillingum',@dataAreaID --Maintain administrative settings
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'F20F86F0-AC18-11DE-8A39-0800200C9A66','Umsjón eftirlitsskráar',@dataAreaID --Manage audit logs
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'31DFE220-B35D-11DE-8A39-0800200C9A66','Skoða útlitsprófíla',@dataAreaID --View visual profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'60876530-B35D-11DE-8A39-0800200C9A66','Breyta útlitsprófílum',@dataAreaID --Edit visual profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'9A413920-DB3C-11DE-8A39-0800200C9A66','Skoða virkniprófíla',@dataAreaID --View functionality profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'A1372500-DB3C-11DE-8A39-0800200C9A66','Breyta virkniprófílum',@dataAreaID --Edit functionality profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'4929CCA0-D44D-11DE-8A39-0800200C9A66','Breyta snertihnappauppsetningum',@dataAreaID --Edit touch button layouts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'40A5F690-DFFA-11DE-8A39-0800200C9A66','Skoða færsluþjónustuprófíla',@dataAreaID --View Site service profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'47556390-DFFA-11DE-8A39-0800200C9A66','Breyta færsluþjónustuprófílum',@dataAreaID --Edit Site service profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1CE28D90-E3EC-11DE-8A39-0800200C9A66','Skoða vélbúnaðarprófíla',@dataAreaID --View hardware profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'24414D60-E3EC-11DE-8A39-0800200C9A66','Breyta vélbúnaðarprófílum',@dataAreaID --Edit hardware profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'004A89B0-C53F-11DE-8A39-0800200C9A66','Skoða kortategundir',@dataAreaID --View card types
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'08427C40-C53F-11DE-8A39-0800200C9A66','Breyta kortategundum',@dataAreaID --Edit card types
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'D4E6DB50-C941-11DE-8A39-0800200C9A66','Skoða greiðslumáta',@dataAreaID --View payment methods
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'DC259000-C941-11DE-8A39-0800200C9A66','Breyta greiðslumátum',@dataAreaID --Edit payment methods
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'651897B0-B72F-11DE-8A39-0800200C9A66','Skoða búðir',@dataAreaID --View stores
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'6B7DB540-B72F-11DE-8A39-0800200C9A66','Breyta búðum',@dataAreaID --Edit stores
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'24FAEB80-DF2D-11DE-8A39-0800200C9A66','Skoða kvittanir',@dataAreaID --View receipts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'F7B3FCB0-E95A-11DE-8A39-0800200C9A66','Skoða eyðublöð',@dataAreaID --View forms
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'FF6DADC0-E95A-11DE-8A39-0800200C9A66','Breyta eyðublöðum',@dataAreaID --Edit forms
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'36F689F0-D9AD-11DE-8A39-0800200C9A66','Skoða viðskiptamenn',@dataAreaID --View customers
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'9E542310-D9AC-11DE-8A39-0800200C9A66','Breyta viðskiptamönnum',@dataAreaID --Edit customers
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'BE2722F4-242B-474C-918B-73B2413F4CE1','Skoða flokka', @dataAreaID -- View Categories
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'BC8A8926-CD9E-4562-92F7-8711ED378910','Breyta flokkum', @dataAreaID -- Edit Categories
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'5245D6BA-3F89-439F-A792-00613D5FE15A','Skoða tax free bil', @dataAreaID --  View tax free ranges
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C1D8415A-8425-4CF1-9CC1-D81D33D669FC','Umsjón með verðstillingum',@dataAreaID --Manage price settings
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'AB7B8DFC-A18A-4303-88E5-2C11EC575FCF','Umsjón með biðfærslu stillingum',@dataAreaID --Manage suspension settings
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1A4FE3AB-35F7-41FC-A166-C562651DF175','Umsjón með eldsneytisstillingum',@dataAreaID --Manage fuel settings
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'5B9150B2-30D6-4267-83F4-8EE83AC9CDEC','Umsjón með leyfðum greiðslustillingum',@dataAreaID --Manage allowed payment settings
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'EBE2BF19-17C0-4B1E-88F5-FFD46883C15D','Umsjón með viðskiptamannahópum',@dataAreaID --Manage customer groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'E35842F0-BA3C-11DE-8A39-0800200C9A66','Skoða útstöðvar',@dataAreaID --View terminals
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'E8A33DF0-BA3C-11DE-8A39-0800200C9A66','Breyta útstöðvum',@dataAreaID --Edit terminals
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'A0E9A5A0-09BD-11DF-8A39-0800200C9A66','Skoða kassavörur',@dataAreaID --View retail items
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'A6C71CA0-09BD-11DF-8A39-0800200C9A66','Breyta kassavörum',@dataAreaID --Edit retail items
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'365B452C-0520-40CA-9D73-1C97D8E366E2','Magnbreyta kassavörum',@dataAreaID --Multi edit items
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C40E5050-9A39-11DF-981C-0800200C9A66','Umsjón smásöludeilda',@dataAreaID --Manage retail departments
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'F16AA510-0BF7-11DF-8A39-0800200C9A66','Umsjón strikamerkja',@dataAreaID --Manage retail item bar codes
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C2882498-E950-423E-8A81-2DD331EF7659','Umsjón með raðnúmerum',@dataAreaID --Manage serial numbers
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'6C12501A-0229-4067-A3E3-C0A175E48789','Umsjón með uppsetningu strikamerkja',@dataAreaID -- Manage bar code setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'0438A1A0-118F-11DF-8A39-0800200C9A66','Umsjón strikamerkja maska',@dataAreaID --Manage bar code masks
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'176C1B40-0BF8-11DF-8A39-0800200C9A66','Umsjón vörubreytileika',@dataAreaID --Manage retail item dimensions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'AB59B550-2080-11DF-8A39-0800200C9A66','Umsjón verðsamninga verða',@dataAreaID --Manage trade agreement prices
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'87EA40F0-6CB9-11DF-BE2B-0800200C9A66','Skoða viðskiptamanna verð/afslátta hópa',@dataAreaID --View customer price/discount groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'8EDD6DB0-6CB9-11DF-BE2B-0800200C9A66','Breyta viðskiptamanna verð/afslátta hópum',@dataAreaID --Edit customer price/discount groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'82E990F0-7241-11DF-93F2-0800200C9A66','Skoða vöruafsláttahópa',@dataAreaID --View item discount groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'B8CE1B00-7241-11DF-93F2-0800200C9A66','Breyta vöruafsláttahópum',@dataAreaID --Edit item discount groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'78056F10-7890-11DF-93F2-0800200C9A66','Skoða söluskattsuppsetningar',@dataAreaID --View sales tax setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'7C841960-7890-11DF-93F2-0800200C9A66','Breyta söluskattsuppsetningum',@dataAreaID --Edit sales tax setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'AC55D1A0-85E3-11DF-A4EE-0800200C9A66','Umsjón POS leyfa',@dataAreaID --Manage POS licenses
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'90886DF0-89AA-11DF-A4EE-0800200C9A66','Greiðslutalning',@dataAreaID --Tender declarations
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'B5D318D0-9643-11DF-981C-0800200C9A66','Umsjón afslátta',@dataAreaID --Manage discounts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'828E4BB0-9A69-4D30-A2C4-BB34DEA25DDA','Umsjón með smásölusviði',@dataAreaID --Manage retail divisions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ECEE9350-89AD-11DF-A4EE-0800200C9A66','Breyta gjaldmiðlum',@dataAreaID --Edit currencies
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'992A90F0-89AF-11DF-A4EE-0800200C9A66','Skoða gjaldmiðla',@dataAreaID --View currencies
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'15C8B5B0-A6DE-11DF-981C-0800200C9A66','Umsjón smásöluflokka',@dataAreaID --Manage retail groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'B93AE980-A3B8-11DF-981C-0800200C9A66','Umsjón með veitingakerfisuppsetningu',@dataAreaID --Manage hospitality setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'62ABDC60-AE93-11DF-94E2-0800200C9A66','Umsjón sérstakra flokka',@dataAreaID --Manage special groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'8E1C4E00-B4EE-11DF-8D81-0800200C9A66','Skoða upplýsingakóta',@dataAreaID --View infocode setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'CFBE2320-B506-11DF-8D81-0800200C9A66','Breyta upplýsingakóta',@dataAreaID --Edit infocode setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'21294200-BC03-11DF-851A-0800200C9A66','Umsjón með einingum',@dataAreaID --Manage item units
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'248B5B82-43C0-48DC-8F7A-BAF69DE9AABE','Umsjón með vörutegundum',@dataAreaID --Manage item types
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'3FF1A240-C0B8-11DF-851A-0800200C9A66','Skoða verðhópa',@dataAreaID --View price groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'FF5CECCE-80E9-402B-8720-F65AF31DA820','Breyta verð hópum',@dataAreaID --Edit price groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'4991EC80-C72D-11DF-BD3B-0800200C9A66','Umsjón með númeraraðir',@dataAreaID --Manage number sequences
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1EEC0BD0-DAC6-11DF-937B-0800200C9A66','Umsjón með sölutegundum',@dataAreaID --Manage sales types
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'329F05F0-E8C8-11DF-9492-0800200C9A66','Umsjón með veitingategundum',@dataAreaID --Manage hospitality types
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'EA1C5150-F0B7-11DF-98CF-0800200C9A66','Umsjón með reikning á uppgjöri',@dataAreaID --Manage Calculate Statement
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'FB1CE320-F0B7-11DF-98CF-0800200C9A66','Umsjón með bókun á uppgjöri',@dataAreaID --Manage Post Statement
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'0B79C730-F624-11DF-98CF-0800200C9A66','Skoða birgja',@dataAreaID --View vendors
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'6BCA4D80-F624-11DF-98CF-0800200C9A66','Breyta birgjum',@dataAreaID --Edit vendors
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'495712EC-0340-4C02-AF5B-0F8E7BA2F35F','Umsjón með byrgjum á vörum',@dataAreaID --Manage vendors on items
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'55B825C0-F3EB-11DF-98CF-0800200C9A66','Umsjón með uppsetningu borða',@dataAreaID --Manage dining table layouts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'34BD0C00-F3E1-11DF-98CF-0800200C9A66','Umsjón með pöntunum',@dataAreaID --Manage purchase orders
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'0D77BB0E-4AC1-4DBC-9F3A-0B32CC8CE3C3','Umsjón með pöntunum í öllum verslunum',@dataAreaID --Manage purchase orders for all stores
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'36D84296-316B-4A06-8E9B-DA6D5BA3D2B5','Umsjón með leiðréttingum á birgðum fyrir allar búðir',@dataAreaID --Manage inventory adjustments for all stores
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'AD735180-F6DF-11DF-98CF-0800200C9A66','Skoða leiðréttingar birgða',@dataAreaID --View inventory adjustments
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'4F4EB240-F6E7-11DF-98CF-0800200C9A66','Breyta leiðréttingum birgða',@dataAreaID --Edit inventory adjustments
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'B13A3ED0-02B7-11E0-A976-0800200C9A66','Umsjón með matseðlategundum vetingastaða',@dataAreaID --Manage restaurant menu types
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C07B5430-03A5-11E0-A976-0800200C9A66','Umsjón með stöðvaprentun',@dataAreaID --Manage station printing
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'6ED29390-09F0-11E0-81E0-0800200C9A66','Skrá vörutalningu',@dataAreaID --View and edit stock counting
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'B737BF90-0C3C-11E0-81E0-0800200C9A66','Breyta POS valmyndum',@dataAreaID --Edit POS menus
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'0DA1AD40-0C3E-11E0-81E0-0800200C9A66','Skoða kassavalmyndir',@dataAreaID --View POS menus
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'04AD0711-49C3-464C-9440-06B05F8B59C6','Breyta EFT vörpunar uppsetningu',@dataAreaID --Edit EFT mapping setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'CA74D61C-7894-4364-BD91-55A3F98E6A0F','Skoða EFT vörpunar uppsetningu',@dataAreaID --View EFT mapping setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C73861F0-0D0A-11E0-81E0-0800200C9A66','Umsjón með vörumóttökuskjölum',@dataAreaID --Manage goods receiving documents
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'0D77BB0E-4AC1-4DBC-9F3A-0B32CC8CE3C4','Umsjón með vörumóttöku í öllum verslunum',@dataAreaID --Manage goods receiving for all stores
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'66323910-19B4-11E0-AC64-0800200C9A66','Umsjón með valmyndahópum',@dataAreaID --Manage menu groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'53956C90-27CF-11E0-91FA-0800200C9A66','Umsjón með fjartengdum hýslum',@dataAreaID --Manage remote hosts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'6B2D3670-643E-11E0-AE3E-0800200C9A66','Greiðslutalning',@dataAreaID --Tender declarations
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'29A9F4B0-7FE4-11E0-B278-0800200C9A66','Skoða birgðarstöðu skýrslu',@dataAreaID --View stock level report
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'81CAA200-A232-11E0-8264-0800200C9A66','Umsjón með tengdum vörum',@dataAreaID --Manage linked items
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'88E35E80-A23A-11E0-8264-0800200C9A66','Umsjón gjafakorta',@dataAreaID --Manage gift cards
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'23329470-D232-11E0-9572-0800200C9A66','Umsjón inneignarnótna',@dataAreaID --Manage credit vouchers
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'15DE9180-E078-11E0-9572-0800200C9A66','Umsjón með geymdum afgreiðslum',@dataAreaID --Manage central suspensions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'F61938B8-DB15-4714-94F9-1AD16DDDF5FB','Bóka og móttaka innkaupapöntun',@dataAreaID --Post and receive purchase order
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'8E677D9F-EE02-4A52-9768-0DF78F985126','Fylla út vörumóttökulínur sjálfkrafa',@dataAreaID --Auto populate goods receiving document lines
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C26AA590-F65F-11E0-BE50-0800200C9A66','Umsjón með kortatengslum',@dataAreaID --Manage msr card links
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'2007E06D-60C6-45CB-9053-3C47A34BC33A','Skoða innkomu- og útgjaldareikninga',@dataAreaID --View Income and Expense Types
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'9202E377-C014-4B04-A234-2BD3664D5A6F','Skoða skýrslur',@dataAreaID --View reports
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'DC287D10-9842-11E1-A8B0-0800200C9A66','Skýrsluumsjón',@dataAreaID --Manage reports
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'06CE13B5-5286-4DBC-BBBA-384CA1247B86','Skoða viðskiptamannaskýrslur',@dataAreaID --View customer reports
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'56BAF216-203D-4A27-9DB2-9FE17D62884F','Skoða vöruskýrslur',@dataAreaID --View item reports
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'2CABA2ED-D9E0-4C2E-B715-21F798869FDA','Skoða söluskýrslur',@dataAreaID --View sales reports
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'9CFD981F-01CA-48D7-8192-2B58DFE7B69C','Skoða uppsetningarskýrslu',@dataAreaID --View setup reports
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'E10AA29D-38BA-407E-B3BA-41FE29BD10A1','Skoða notendaskýrslur',@dataAreaID --View user reports
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'675B26D6-9F69-427A-B451-5CE5F89B9E6A','Skoða veitingaskýrslur',@dataAreaID --View hospitality reports
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'F7BC3A76-0911-4A2B-9632-55EFC7B18682','Umsjón með Eldhússtjóra prófílum',@dataAreaID --Manage Kitchen Manager profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'9D520883-3C20-4F56-903A-6233897BA26B','Umsjón með Eldhússkjástöðvum',@dataAreaID --Manage Kitchen Display Stations
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'19C10A2A-B228-47FF-8F47-558CD1E8DFCE','Umsjón með Eldhússkjáprófílum',@dataAreaID --Manage Kitchen Display Profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'7D1EF450-6EDB-11E2-BCFD-0800200C9A66','Umsjón auðkennislykla',@dataAreaID --Manage authentication tokens
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'D541DED2-A8A4-4E26-934A-FA87C21036AE','Umsjón með eldhúsprenturum',@dataAreaID --Manage Kitchen printers

exec spSECURITY_AddPermissionLocalization_1_0 @locale,'799DD5EB-6906-4955-BFD4-AC4304DE4EA3','Skoða sniðmát stillingarleiðbeinanda',@dataAreaID --View configuration wizard templates
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'500329C3-054C-43AF-9281-B312909BFADA','Breyta sniðmátum stillingarleiðbeinanda',@dataAreaID --Edit configuration wizard templates

exec spSECURITY_AddPermissionLocalization_1_0 @locale,'8649312D-14F8-4255-BA94-C02712B903D0','Skoða birgðartilfærslubeiðnir',@dataAreaID --View inventory transfer requests
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'EB9693BB-B825-4010-AFDE-B56CF2DD3183','Breyta birgðartilfærslubeiðnum',@dataAreaID --Edit inventory transfer requests
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'E329EB84-B449-4239-AED8-C6AEC374AD67','Skoða birgðartilfærslur',@dataAreaID --View inventory transfer orders
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'D404EDDE-1303-46FD-9FF2-5A9D4247DFE8','Breyta birgðartilfærslum',@dataAreaID --Edit inventory transfer orders
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'0D77BB0E-4AC1-4DBC-9F3A-0B32CC8CE3C2','Skoða birgðir í öllum verslunum',@dataAreaID --View inventory for all stores
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'75991D8F-8A93-4BEE-9620-400E1312D9E8','Sjálfkrafa setja fjölda á birgðartilfærslu',@dataAreaID --Auto set quantity on transfer order
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'0D77BB0E-4AC1-4DBC-9F3A-0B32CC8CE3C5','Búa til birgðartilfærslur fyrir allar verslanir',@dataAreaID --Create store transfers for all stores
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'0D77BB0E-4AC1-4DBC-9F3A-0B32CC8CE3C6','Umsjón með birgðartilfærslum fyrir allar verslanir',@dataAreaID --Manage transfers for all stores
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'76C929DB-5D0E-4905-8AFD-F6A7F0B7FB62','Umsjón með birgðasniðmátum',@dataAreaID --Manage inventory templates
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'D280E6AF-4EA8-486E-99ED-EEAA0250240F','Skoða birgðastöðu',@dataAreaID --View inventory on hand

exec spSECURITY_AddPermissionLocalization_1_0 @locale,'9EE4A4C7-F1A2-42CB-845B-89C9CC26F06A','Innlestur gagnapakka',@dataAreaID --Import data packages

exec spSECURITY_AddPermissionLocalization_1_0 @locale,'B336E292-46A0-4B33-980B-6E67774B57DE','Endursetja vinnslustöðu dagbókar',@dataAreaID --Reset journal processing status

exec spSECURITY_AddPermissionLocalization_1_0 @locale,'AAB3BC37-DCA6-4407-9941-E1692C7CCF80','Skoða birgðahreyfingar',@dataAreaID --View item ledger

exec spSECURITY_AddPermissionLocalization_1_0 @locale,'4342D98A-CDEB-409D-80F2-FCA069DA2782','Umsjón með áfyllingum',@dataAreaID --Manage replenishment
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'B1F9E246-5EAB-4FC5-BDB4-E1F769787447','Breyta ótiltækum birgðum',@dataAreaID --Manage parked inventory
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C3109275-6E3F-491A-A37C-CA32BD3014F1','Breyta birgða leiðréttingum',@dataAreaID --Manage inventory adjustments
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'AA1822B1-0953-4B90-9DAC-307E5B88DB3F','Umsjón með fráteknum birgðum',@dataAreaID --Manage stock reservations
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'411B0B74-BBD7-4F01-94CB-D8294376E0F9','Umsjón með fráteknum birgðum fyrir allar verslanir',@dataAreaID --Manage stock reservations for all stores
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'B50FFD7C-5A65-4871-8DDC-DB3039F6029E','Breyta ótiltækum birgðum fyrir allar verslanir',@dataAreaID --Manage parked inventory for all stores

exec spSECURITY_AddPermissionLocalization_1_0 @locale,'8DECDF32-753D-4ECF-A64D-E93722D3C3CF', 'Skoða viðskiptamannahreyfingar',@dataAreaID --View customer ledger entries
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'10C08A81-CB1F-4FC9-8C9C-86B715F1F83C', 'Breyta viðskiptamannaheyfingum', @dataAreaID --Edit customer ledger entries'
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'80921901-B919-452F-9C97-6029ACABC053', 'Skoða vildarkerfi', @dataAreaID --View loyalty schemes
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'80921902-B919-452F-9C97-6029ACABC053', 'Breyta vildarkerfum', @dataAreaID --Edit loyalty schemes
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'80921903-B919-452F-9C97-6029ACABC053', 'Skoða vildarkort', @dataAreaID --View loyalty cards
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'80921904-B919-452F-9C97-6029ACABC053', 'Breyta vildarkortum', @dataAreaID --Edit loyalty cards
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'D0E05BAE-4AD7-4E3E-A35C-D9B63D8F4039', 'Skoða vildarkerfisfærslur', @dataAreaID --View loyalty transactions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'26542533-9583-4648-8B2D-847EB5BDDAD1', 'Umsjón með innlestrar prófílum', @dataAreaID --Manage import profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'E8C7C5C0-FA5D-4BDE-8516-E616D0B86B87', 'Skoða uppsetingu stíla', @dataAreaID --Manage styles setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'47EC274D-B7F0-4F24-80C6-2E1FF42F144F', 'Umsjón með myndabanka', @dataAreaID --Manage image bank

-- Label templates
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'735C60D3-CBCF-443E-81FD-19904F9232C5','Skoða merkimiða sniðmát',@dataAreaID --View label templates
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'49940BC5-A609-40D1-A3FB-729E8BA4284B','Breyta merkimiða sniðmát',@dataAreaID --Edit label templates
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'DBFBB019-E273-40E3-896C-F217C88FDD2B','Prenta merkimiða',@dataAreaID --Print labels

--Pos Permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9A0-A9A8-11E1-AFA6-0800200C9A66','Vörusala',@dataAreaID --Item Sale
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9A1-A9A8-11E1-AFA6-0800200C9A66','Verðathugun',@dataAreaID --Price Check
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9A2-A9A8-11E1-AFA6-0800200C9A66','Leiðrétta vöru',@dataAreaID --Void Item
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9A3-A9A8-11E1-AFA6-0800200C9A66','Vöruathugasemd',@dataAreaID --Item Comment
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9A4-A9A8-11E1-AFA6-0800200C9A66','Breyta verði',@dataAreaID --Price Override
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'9C60EF55-60A1-4930-8D58-E1D2411250E5','Endurstilla verð',@dataAreaID --Clear price Override
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9A5-A9A8-11E1-AFA6-0800200C9A66','Breyta verði',@dataAreaID --Set quantity
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9A6-A9A8-11E1-AFA6-0800200C9A66','Hreinsa magn',@dataAreaID --Clear quantity
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9A7-A9A8-11E1-AFA6-0800200C9A66','Vöruleit',@dataAreaID --Item Search
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9A8-A9A8-11E1-AFA6-0800200C9A66','Vöruskil',@dataAreaID --Return Item
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9A9-A9A8-11E1-AFA6-0800200C9A66','Skila afgreiðslu',@dataAreaID --Return Transaction
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9AA-A9A8-11E1-AFA6-0800200C9A66','Rafræn dagbók',@dataAreaID --Show Journal
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9AB-A9A8-11E1-AFA6-0800200C9A66','Bæta við vildakorti',@dataAreaID --Loyalty Request
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'F42F5BB2-E92D-46A6-87EE-1DBF9784A648','Vildarpunkta afsláttur',@dataAreaID --Loyalty points discount

exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9AC-A9A8-11E1-AFA6-0800200C9A66','Fjarlægja starfsmann',@dataAreaID --Clear Salesperson
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9AD-A9A8-11E1-AFA6-0800200C9A66','Athugasemd við reikning',@dataAreaID --Invoice Comment
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9AE-A9A8-11E1-AFA6-0800200C9A66','Breyta einingu',@dataAreaID --Change Unit of Measure
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9B1-A9A8-11E1-AFA6-0800200C9A66','Birta upplýsingarkóða',@dataAreaID --Infocode On Request
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9B3-A9A8-11E1-AFA6-0800200C9A66','Breyta vöruathugasemd',@dataAreaID --Change Item Comments
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9B4-A9A8-11E1-AFA6-0800200C9A66','Greiða með peningum',@dataAreaID --Pay Cash
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9B5-A9A8-11E1-AFA6-0800200C9A66','Greiða með korti',@dataAreaID --Pay Card
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9B6-A9A8-11E1-AFA6-0800200C9A66','Greiða í reikning',@dataAreaID --Pay Customer Account
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9B7-A9A8-11E1-AFA6-0800200C9A66','Greiða með gjaldmiðli',@dataAreaID --Pay Currency
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9B8-A9A8-11E1-AFA6-0800200C9A66','Greiða með ávísun',@dataAreaID --Pay Check
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9B9-A9A8-11E1-AFA6-0800200C9A66','Greiða allt með peningum',@dataAreaID --Pay Cash Quick
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9BA-A9A8-11E1-AFA6-0800200C9A66','Greiða með fyrirtækja korti',@dataAreaID --Pay Corporate Card
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9BB-A9A8-11E1-AFA6-0800200C9A66','Skila greiðslu',@dataAreaID --Void Payment
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9BC-A9A8-11E1-AFA6-0800200C9A66','Greiða í kreditreikning',@dataAreaID --Pay Credit Memo
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9BD-A9A8-11E1-AFA6-0800200C9A66','Greiða með gjafakorti',@dataAreaID --Pay gift card
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528C9BE-A9A8-11E1-AFA6-0800200C9A66','Línuafsláttur kr.',@dataAreaID --Line Discount Amount
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528F0B0-A9A8-11E1-AFA6-0800200C9A66','Línuafsláttur %',@dataAreaID --Line Discount Percent
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528F0B1-A9A8-11E1-AFA6-0800200C9A66','Heildarafsláttur kr.',@dataAreaID --Total Discount Amount
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528F0B2-A9A8-11E1-AFA6-0800200C9A66','Heildarafsláttur %',@dataAreaID --Total Discount Percent
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'61572931-A0B4-4CDB-AB72-A78A2C27B328','Endurstilla afslætti',@dataAreaID --Clear Discounts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'8BFE0874-54B3-4AAE-9ACB-09926BC3A31B','Virkja tímabundinn afslátt handvirkt',@dataAreaID --Manually trigger periodic discount
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528F0B5-A9A8-11E1-AFA6-0800200C9A66','Skila afgreiðslu',@dataAreaID --Void Transaction
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528F0B6-A9A8-11E1-AFA6-0800200C9A66','Athugasemd við afgreiðslu',@dataAreaID --Transaction Comment
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528F0B7-A9A8-11E1-AFA6-0800200C9A66','Sölumaður',@dataAreaID --Sales Person
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528F0B8-A9A8-11E1-AFA6-0800200C9A66','Setja í biðskrá',@dataAreaID --Suspend Transaction
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528F0B9-A9A8-11E1-AFA6-0800200C9A66','Sækja úr biðskrá',@dataAreaID --Recall Transaction
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528F0BA-A9A8-11E1-AFA6-0800200C9A66','Fjarlægja lyfseðil úr afgreiðslu',@dataAreaID --Pharmacy Prescription Cancel
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528F0BB-A9A8-11E1-AFA6-0800200C9A66','Sækja lyfseðil',@dataAreaID --Pharmacy Prescriptions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528F0BC-A9A8-11E1-AFA6-0800200C9A66','Gefa út inneignarnótu',@dataAreaID --Issue Credit Memo
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528F0BD-A9A8-11E1-AFA6-0800200C9A66','Gefa út gjafakort',@dataAreaID --Issue Gift Certificate
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'3957A752-254D-4705-8857-3418E4F15C69','Sækja stöðu gjafakorts',@dataAreaID --Get gift card balance
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528F0BE-A9A8-11E1-AFA6-0800200C9A66','Sýna heildarupphæð',@dataAreaID --Display Total
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528F0BF-A9A8-11E1-AFA6-0800200C9A66','Sölupöntun',@dataAreaID --Sales Order
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528F0C0-A9A8-11E1-AFA6-0800200C9A66','Sölureikningur',@dataAreaID --Sales Invoice
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528F0C1-A9A8-11E1-AFA6-0800200C9A66','Tekjureikningur',@dataAreaID --Income Account
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528F0C2-A9A8-11E1-AFA6-0800200C9A66','Gjaldareikningur',@dataAreaID --Expense Account
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528F0C3-A9A8-11E1-AFA6-0800200C9A66','Skil af tekjureikningum',@dataAreaID --Return Income Accounts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528F0C4-A9A8-11E1-AFA6-0800200C9A66','Skil af gjaldareikningum',@dataAreaID --Return Expense Accounts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'39EDAFE0-A9A2-11E1-AFA6-0800200C9A66','Viðskiptamannaleit',@dataAreaID --Customer Search
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'80E83310-A9A3-11E1-AFA6-0800200C9A66','Fjarlægja viðskiptamann',@dataAreaID --Customer Clear
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'5C1B01B0-A9A4-11E1-AFA6-0800200C9A66','Afgreiðslur viðskiptamanns',@dataAreaID --Customer Transactions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'EE006700-A9A4-11E1-AFA6-0800200C9A66','Afgreiðsluskýrsla viðskiptamanns',@dataAreaID --Customer Transactions Report
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'432B8890-A9A5-11E1-AFA6-0800200C9A66','Skrá út',@dataAreaID --Log Off
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'CC68E530-A9A5-11E1-AFA6-0800200C9A66','Læsa útstöð',@dataAreaID --Lock Terminal
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'563D2AA0-A9A6-11E1-AFA6-0800200C9A66','Skrá sig út með valdi',@dataAreaID --Log Off Force
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C24BD340-A9A6-11E1-AFA6-0800200C9A66','Birgðauppfletting',@dataAreaID --Inventory Lookup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'32216EF0-A9A7-11E1-AFA6-0800200C9A66','Frumstilla Z skýrslu',@dataAreaID --Initialize Z Report
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'808BBE60-A9A7-11E1-AFA6-0800200C9A66','Prenta X',@dataAreaID --Print X
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'CEE37030-A9A7-11E1-AFA6-0800200C9A66','Prenta Z',@dataAreaID --Print Z
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1B4B77E2-A287-4219-B068-C4463C1DD32D','Prenta vörusöluskýrslu',@dataAreaID --Print item sales report
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'4CC27DC0-A9A8-11E1-AFA6-0800200C9A66','Virkja hönnunarham',@dataAreaID --Design Mode Enable
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'B3FF6280-AA36-11E1-AFA6-0800200C9A66','Fela afgreiðsluglugga',@dataAreaID --Minimize POS Window
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1396AAF0-AA37-11E1-AFA6-0800200C9A66','Sérsniðin aðgerð',@dataAreaID --Blank Operation
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'018BC3EB-19F6-441A-9295-64A0468C108E','Keyra utanaðkomandi skipanir',@dataAreaID --Run external command
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'0E7B1B16-B4A8-4FAB-B8CD-71B51696D431','Keyra POS viðbót',@dataAreaID --Execute POS plugin
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'67F1D250-AA37-11E1-AFA6-0800200C9A66','Opna skúffu',@dataAreaID --Open Drawer
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C61CB980-AA37-11E1-AFA6-0800200C9A66','Uppgjör dags',@dataAreaID --End Of Day
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'30685560-AA38-11E1-AFA6-0800200C9A66','Klára vakt',@dataAreaID --End Of Shift
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'7A5DBE80-AA38-11E1-AFA6-0800200C9A66','Greiðslutalning',@dataAreaID --Tender Declaration
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'D0120D40-AA38-11E1-AFA6-0800200C9A66','Innborgun á viðskm. reikning',@dataAreaID --Customer Account Deposit
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1E2EB1E0-AA39-11E1-AFA6-0800200C9A66','Tilgreinið byrjunarupphæð dags/vaktar',@dataAreaID --Declare Start Amount
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'6CC36CB0-AA39-11E1-AFA6-0800200C9A66','Skiptimynt',@dataAreaID --Float Entry
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C9154E20-AA39-11E1-AFA6-0800200C9A66','Fjarlægja peninga úr kassa',@dataAreaID --Tender Removal
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'0D3CD0A0-AA3A-11E1-AFA6-0800200C9A66','Færa í öryggishólf',@dataAreaID --Safe Drop
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'891E8790-AA3A-11E1-AFA6-0800200C9A66','Færa í banka',@dataAreaID --Bank Drop
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'2D7C78B0-AA3B-11E1-AFA6-0800200C9A66','Bakfæra færslu í öryggishólf',@dataAreaID --Safe Drop Reversal
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'97105D50-AA3B-11E1-AFA6-0800200C9A66','Bakfæra færslu í banka',@dataAreaID --Bank Drop Reversal
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ED067000-AA3B-11E1-AFA6-0800200C9A66','Skipta upp reikningi',@dataAreaID --Split Bill
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'48857110-AA3C-11E1-AFA6-0800200C9A66','Loka kassa veitingakerfis',@dataAreaID --Exit Hospitality POS
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'AD42EE70-AA3C-11E1-AFA6-0800200C9A66','Prenta matseðlategund',@dataAreaID --Print Hospitality Menu Type
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'0B1136B0-AA3D-11E1-AFA6-0800200C9A66','Setja matseðlategund',@dataAreaID --Set Hospitality Menu Type
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'3F89FBC0-AA3D-11E1-AFA6-0800200C9A66','Breyta matseðlategund',@dataAreaID --Change Hospitality Menu Type
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'7906EED0-AA3D-11E1-AFA6-0800200C9A66','Gera færslu undanþegna skatti',@dataAreaID --Tax Exempt Transaction
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'B7D1C310-AA3D-11E1-AFA6-0800200C9A66','Eyða skattaundanþágu af færslu',@dataAreaID --Clear Transaction Tax Exemption
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'FC8BC190-AA3D-11E1-AFA6-0800200C9A66','Breyta skattflokki samkvæmt upplýsingakóta',@dataAreaID --Infocode Tax Group Change
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'F5215570-BBB6-11E1-AFA7-0800200C9A66','Breyta uppsetningu stíls',@dataAreaID --Edit Style profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'3134F8F0-BBB7-11E1-AFA7-0800200C9A66','Skoða uppsetningu stíls',@dataAreaID --View Style profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'76078220-D1A8-11E1-9B23-0800200C9A66','Skoða eyðublaða prófíla',@dataAreaID --View Form Profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'12F12FA0-D1A9-11E1-9B23-0800200C9A66','Breyta eyðublaða prófílum',@dataAreaID --Edit Form Profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'59270E24-FF01-4C88-9AEA-11BDED40A953','Skoða samhengi',@dataAreaID --View Context
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'3F1D120B-207A-4A0A-A56D-2EAD1B2814EE','Breyta samhengi',@dataAreaID --Edit Contexts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'B15D8168-F2E8-4093-95EB-582E068AD9EC','Bæta við viðskipamanni',@dataAreaID --Customer add
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'38402373-1A92-483F-B77B-E18C986BB586','Klára pöntun',@dataAreaID --Bump order
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'20E06A62-BAE4-4EEC-BB04-A4A708956904','Greiða með vildarkjörum',@dataAreaID --Pay loyalty
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'214B99CE-0C26-4D16-B65B-EA555708B4F0','Skrá viðskiptamann á vildarkort',@dataAreaID --Add customer to loyalty card
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'17B02E4B-ABCB-46A0-BE29-B8FACAB2F434','Skoða innleiðingar vörpun',@dataAreaID --View integration mapping
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'DACEDF0B-0709-4B3B-9A27-0FC7DB1FD6BD','Breyta innleiðingar vörpun',@dataAreaID --Edit integration mapping
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'5E00968F-DEF1-461C-995C-034DFAAF2619','Skoða gagnagrunnsvörpun',@dataAreaID --View database map
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'3A6ED366-3D24-4263-B8D4-6E2A74629D94','Fjárstreymisskýrsla',@dataAreaID --Financial report
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'9E435E02-17C8-4BA4-9365-B1A1EA3C8D30','Skoða innleiðingar skráningu',@dataAreaID --View integration log
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'B2FE62A8-ECA5-4EB7-8C05-36A0E2E290AD','Byrjunarupphæð dags',@dataAreaID --Start of day
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'235EE80D-2B89-4CA3-BA0C-5527BFF93ACC','Umsjón með vélbúnaðarprófílum útstöðva',@dataAreaID --Manage hardware profile on POS
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'41E5E014-9DAA-481E-95F3-0BD3E3896556','Sölupantanir',@dataAreaID --Customer orders
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'2504F28E-2210-4E65-BA6A-3D611F63197E','Tilboð',@dataAreaID --Quotes
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C35A6965-FAE3-45DB-AACF-F0606F6C4838','Umsjón með sölupöntunum',@dataAreaID --Customer order management
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'DD054719-5FF9-4F41-81F2-B282921F1924','Yfirskrifa lágmarks innborgun',@dataAreaID --Override minimum deposit
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'DF5E32BF-C726-4A24-B2A6-2104F06D590D','Setja ástæðukóta',@dataAreaID --Set reason code
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C3A5300F-593E-488C-B080-04025A3BC253','Breyta læsingu viðskiptavina á kassa',@dataAreaID -- Manage POS customer blocking

exec spSECURITY_AddPermissionLocalization_1_0 @locale,'BF61E0E6-AEF1-4AF3-B95F-B6C68DC91D64','Loka kassa',@dataAreaID --Exit POS application
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'2279A999-A5CB-44B8-805F-8684E6E9FCD7','Endurræsa tölvu',@dataAreaID --Restart computer
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'5C3CCB66-2EEB-4252-9E91-30C2C603EBBC','Slökkva á tölvu',@dataAreaID --Shutdown computer
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'B7464BC4-47B2-48BA-B82A-306E658ECD96','Virkja þjálfunarham',@dataAreaID --Activate training mode

-- User and security management Scheduler (anyNewGuid, description, permissionGroupGUID, sortCode, PermissionCodeGUID, CodeIsEncrypted)
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'58428E18-BFAC-4DAC-9054-1F159AC9CCEA','Aðgangur að gagnaflutning',@dataAreaID --Access replication
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'E9BD9613-0AB0-4502-951D-6A4701EDFD6F','Breyta staðsetningum',@dataAreaID --Edit distribution locations
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C92401C4-7717-4F6F-806C-9130666A896C','Skoða staðsetningar',@dataAreaID --View distribution locations

-- 
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'493ED36B-EFDA-4604-BA7D-3DDC7D7381CF','Skoða verk og undirverk',@dataAreaID --View jobs and subjobs
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'A7D518EE-1AD7-49FC-B30B-2D266D0D4D67','Búa til mörg undirverk',@dataAreaID --Create multiple subjobs
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'D02A5A6C-0C02-414E-9B42-2E2C1DE658D5','Breyta stillingum',@dataAreaID --Edit settings
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'19A1CF87-8394-4456-B034-5018665A1AD8','Skoða hönnun gagnagrunns',@dataAreaID --View database design
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C47B6771-F61A-4EDE-9062-5244CCCD06D2','Breyta verkum og undirverkum',@dataAreaID --Edit jobs and subjobs
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'B8169851-A8B9-44CB-AF45-88CD6AE321A2','Breyta gagnagrunnsvörpun',@dataAreaID --Edit database map
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'50E152C0-907B-44D8-ACCB-9B900785FD85','Breyta hönnun gagnagrunns',@dataAreaID --Edit database design
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'818ACA49-4542-48BD-813F-A6A8E3450F31','Skoða stillingar',@dataAreaID --View settings
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'660C23AD-3D5D-4758-B80D-E8E539B56E73','Handvirk keyrsla á verkum',@dataAreaID --Manually run a job


--Backup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'21EA337E-31BC-4709-B018-D468FD77F492','Afrita gagnagrunn',@dataAreaID --Backup database

--Reprint
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'7B11B93C-D7EB-4EC5-BC0E-CBC66BB61E2B','Endurprenta kvittun',@dataAreaID --Reprint receipt

--Customer orders
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'2559EBBC-0175-4E57-9F1A-8DA7A0C621E5','Breyta stillingum sölupantana',@dataAreaID --Manage customer order settings
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'70729423-230E-46BC-844C-BEE84A3E2358','Umsjón sölupantana',@dataAreaID --Manage customer orders

exec spSECURITY_AddPermissionLocalization_1_0 @locale,'D8CD437D-D27C-4CB9-A4B9-AF0A6CFD3549','Skoða eftirlitsskrá aðgerða á útstöðvum',@dataAreaID --View terminal operations audit
