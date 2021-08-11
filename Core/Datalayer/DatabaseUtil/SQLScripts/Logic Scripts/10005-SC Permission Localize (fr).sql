
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
set @locale = 'fr'


-- Permission group localizations
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'DF4AFBD0-B35C-11DE-8A39-0800200C9A66','Profil',@dataAreaID --Profiles
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'1DDEEF60-D9AE-11DE-8A39-0800200C9A66','Général',@dataAreaID --General
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'78535D50-04E7-11DF-8A39-0800200C9A66','Élément maître d''article',@dataAreaID --Item master
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'0CA8E620-E997-11DA-8AD9-0800200C9A66','Gestion des utilisateurs et de la sécurité',@dataAreaID --User and security management
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'CEAC1AD0-E997-11DA-8AD9-0800200C9A66','Rapports',@dataAreaID --Reports
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'808ED9F0-E997-11DA-8AD9-0800200C9A66','Administration du système',@dataAreaID --System Administration
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'a54c7dc0-a922-11df-94e2-0800200c9a66','Restauration',@dataAreaID --Hospitality
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'32246200-f0bb-11df-98cf-0800200c9a66','Stock',@dataAreaID --Inventory
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'7d750220-a99f-11e1-afa6-0800200c9a66','Permissions PDV',@dataAreaID --POS permissions
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'2c562010-f660-11e0-be50-0800200c9a66','Autorisations de fidélité',@dataAreaID --Loyalty permissions
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'7ccab506-b497-403e-99e3-7141655e5bfc','Réplication',@dataAreaID --Replication
exec spSECURITY_AddPermissionGroupLocalization_1_0 @locale,'9D0F831D-7B2B-4A0E-B9E6-349B50A235BE','Commandes client',@dataAreaID --Customer orders



-- Permission localizations
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'e13c05d0-cf89-11de-8a39-0800200c9a66','Afficher les autorisations d''actions',@dataAreaID --View action permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'e694e380-cf89-11de-8a39-0800200c9a66','Modifier les autorisations d''actions',@dataAreaID --Edit action permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1d608220-fa2c-11da-974d-0800200c9a66','Créer de nouveaux utilisateurs',@dataAreaID --Create new users
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'d32f5960-554d-11db-b0de-0800200c9a66','Modifier l''utilisateur',@dataAreaID --Edit user
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'9b82f9b0-face-11da-974d-0800200c9a66','Supprimer des utilisateurs',@dataAreaID --Delete users
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'e1cf2610-6049-11db-b0de-0800200c9a66','Activer/Désactiver les utilisateurs',@dataAreaID --Enable/Disable users
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'a0ae94a0-fa04-11da-974d-0800200c9a66','Rétablir le mot de passe pour d''autres utilisateurs',@dataAreaID --Reset password for other users
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'4d3f3f70-5d02-11db-b0de-0800200c9a66','Créer des groupes d''utilisateurs',@dataAreaID --Create user groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'fdab47a0-5d02-11db-b0de-0800200c9a66','Modifier les groupes d''utilisateurs',@dataAreaID --Edit user groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'762df510-5d03-11db-b0de-0800200c9a66','Supprimer les groupes d''utilisateurs',@dataAreaID --Delete user groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1906ecc0-fac0-11da-974d-0800200c9a66','Affecter des utilisateurs à des groupes',@dataAreaID --Assign users to groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'39f09b00-fac2-11da-974d-0800200c9a66','Accorder/Refuser/Hériter des autorisations',@dataAreaID --Grant/Deny/Inherit permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'eca62890-fac3-11da-974d-0800200c9a66','Accorder des autorisations d''utilisateur plus élevées',@dataAreaID --Grant higher user permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'8c7c3250-5eb6-11db-b0de-0800200c9a66','Gérer les autorisations d''utilisateurs',@dataAreaID --Manage user permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'628e9c80-5d03-11db-b0de-0800200c9a66','Gérer les autorisations de groupe',@dataAreaID --Manage group permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'48078c50-6d7f-11db-9fe1-0800200c9a66','Afficher les journaux d''audit',@dataAreaID --View audit logs
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'fb0d8940-fb74-11de-8a39-0800200c9a66','Afficher le calendrier du service de mise à jour',@dataAreaID --View update service schedule
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'13d44f40-fb75-11de-8a39-0800200c9a66','Gérer le calendrier du service de mise à jour',@dataAreaID --Manage update service schedule
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'a6164dc0-a229-11db-8ab9-0800200c9a66','Maintenir les réglages administratifs',@dataAreaID --Maintain administrative settings
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'F20F86F0-AC18-11DE-8A39-0800200C9A66','Gérer les journaux d''audit',@dataAreaID --Manage audit logs
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'31dfe220-b35d-11de-8a39-0800200c9a66','Afficher les profils visuels',@dataAreaID --View visual profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'60876530-b35d-11de-8a39-0800200c9a66','Modifier les profils visuels',@dataAreaID --Edit visual profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'9a413920-db3c-11de-8a39-0800200c9a66','Afficher les profils de fonctionnalités',@dataAreaID --View functionality profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'a1372500-db3c-11de-8a39-0800200c9a66','Modifier les profils de fonctionnalités',@dataAreaID --Edit functionality profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'4929cca0-d44d-11de-8a39-0800200c9a66','Modifier les dispositions de boutons tactiles',@dataAreaID --Edit touch button layouts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'40a5f690-dffa-11de-8a39-0800200c9a66','Afficher les profils de services de transaction',@dataAreaID --View Site service profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'47556390-dffa-11de-8a39-0800200c9a66','Modifier les profils de services de transaction',@dataAreaID --Edit Site service profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1ce28d90-e3ec-11de-8a39-0800200c9a66','Afficher les profils matériel',@dataAreaID --View hardware profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'24414d60-e3ec-11de-8a39-0800200c9a66','Modifier les profils matériel',@dataAreaID --Edit hardware profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'004a89b0-c53f-11de-8a39-0800200c9a66','Afficher les types de cartes',@dataAreaID --View card types
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'08427c40-c53f-11de-8a39-0800200c9a66','Modifier les types de cartes',@dataAreaID --Edit card types
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'d4e6db50-c941-11de-8a39-0800200c9a66','Afficher les modes de paiement',@dataAreaID --View payment methods
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'dc259000-c941-11de-8a39-0800200c9a66','Modifier les modes de paiement',@dataAreaID --Edit payment methods
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'651897b0-b72f-11de-8a39-0800200c9a66','Afficher les magasins',@dataAreaID --View stores
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'6b7db540-b72f-11de-8a39-0800200c9a66','Modifier les magasins',@dataAreaID --Edit stores
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'24faeb80-df2d-11de-8a39-0800200c9a66','Afficher les reçus',@dataAreaID --View receipts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'f7b3fcb0-e95a-11de-8a39-0800200c9a66','Afficher les formulaires',@dataAreaID --View forms
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ff6dadc0-e95a-11de-8a39-0800200c9a66','Modifier les formulaires',@dataAreaID --Edit forms
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'36f689f0-d9ad-11de-8a39-0800200c9a66','Afficher les clients',@dataAreaID --View customers
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'9e542310-d9ac-11de-8a39-0800200c9a66','Modifier les clients',@dataAreaID --Edit customers
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'BE2722F4-242B-474C-918B-73B2413F4CE1','Afficher les catégories', @dataAreaID -- View Categories
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'BC8A8926-CD9E-4562-92F7-8711ED378910','Modifier les catégories', @dataAreaID -- Edit Categories
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'5245D6BA-3F89-439F-A792-00613D5FE15A','Afficher les plages d''exonération fiscale', @dataAreaID --  View tax free ranges
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C1D8415A-8425-4CF1-9CC1-D81D33D669FC','Gérer les paramètres de prix',@dataAreaID --Manage price settings
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'AB7B8DFC-A18A-4303-88E5-2C11EC575FCF','Gérer les paramètres de suspension',@dataAreaID --Manage suspension settings
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1A4FE3AB-35F7-41FC-A166-C562651DF175','Gérer les paramètres de carburant',@dataAreaID --Manage fuel settings
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'5B9150B2-30D6-4267-83F4-8EE83AC9CDEC','Gérer les paramètres de paiements autorisés',@dataAreaID --Manage allowed payment settings
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'EBE2BF19-17C0-4B1E-88F5-FFD46883C15D','Gérer les groupes de clients',@dataAreaID --Manage customer groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'e35842f0-ba3c-11de-8a39-0800200c9a66','Afficher les terminaux',@dataAreaID --View terminals
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'e8a33df0-ba3c-11de-8a39-0800200c9a66','Modifier les terminaux',@dataAreaID --Edit terminals
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'a0e9a5a0-09bd-11df-8a39-0800200c9a66','Afficher les articles de vente au détail',@dataAreaID --View retail items
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'a6c71ca0-09bd-11df-8a39-0800200c9a66','Modifier les articles de vente au détail',@dataAreaID --Edit retail items
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'365B452C-0520-40CA-9D73-1C97D8E366E2','Modifier plusieurs articles',@dataAreaID --Multi edit items
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C40E5050-9A39-11DF-981C-0800200C9A66','Gérer les départements de vente au détail',@dataAreaID --Manage retail departments
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'f16aa510-0bf7-11df-8a39-0800200c9a66','Gérer les codes barres des articles de vente au détail',@dataAreaID --Manage retail item bar codes
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'6C12501A-0229-4067-A3E3-C0A175E48789','Gérer le paramétrage des codes barres',@dataAreaID -- Manage bar code setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'0438a1a0-118f-11df-8a39-0800200c9a66','Gérer les masques de code barres',@dataAreaID --Manage bar code masks
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'176c1b40-0bf8-11df-8a39-0800200c9a66','Gérer les dimensions d''un article de vente au détail',@dataAreaID --Manage retail item dimensions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ab59b550-2080-11df-8a39-0800200c9a66','Gérer les prix d''accords commerciaux',@dataAreaID --Manage trade agreement prices
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'87ea40f0-6cb9-11df-be2b-0800200c9a66','Afficher les groupes de prix clients/remises',@dataAreaID --View customer price/discount groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'8edd6db0-6cb9-11df-be2b-0800200c9a66','Modifier les groupes de prix client/remises',@dataAreaID --Edit customer price/discount groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'82e990f0-7241-11df-93f2-0800200c9a66','Afficher les groupes de remise d''articles',@dataAreaID --View item discount groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'b8ce1b00-7241-11df-93f2-0800200c9a66','Modifier les groupes de remise d''articles',@dataAreaID --Edit item discount groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'78056f10-7890-11df-93f2-0800200c9a66','Afficher le paramétrage de la taxe de vente',@dataAreaID --View sales tax setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'7c841960-7890-11df-93f2-0800200c9a66','Modifier le paramétrage de la taxe de vente',@dataAreaID --Edit sales tax setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ac55d1a0-85e3-11df-a4ee-0800200c9a66','Gérer les licences POS',@dataAreaID --Manage POS licenses
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'90886df0-89aa-11df-a4ee-0800200c9a66','Comptages de caisse',@dataAreaID --Tender declarations
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'b5d318d0-9643-11df-981c-0800200c9a66','Gérer les remises',@dataAreaID --Manage discounts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'828E4BB0-9A69-4D30-A2C4-BB34DEA25DDA','Gérer les divisions de vente au détail',@dataAreaID --Manage retail divisions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ecee9350-89ad-11df-a4ee-0800200c9a66','Modifier les devises',@dataAreaID --Edit currencies
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'992a90f0-89af-11df-a4ee-0800200c9a66','Afficher les devises',@dataAreaID --View currencies
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'15c8b5b0-a6de-11df-981c-0800200c9a66','Gérer les groupes de vente au détail',@dataAreaID --Manage retail groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'b93ae980-a3b8-11df-981c-0800200c9a66','Gérer le paramétrage de la restauration',@dataAreaID --Manage hospitality setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'62abdc60-ae93-11df-94e2-0800200c9a66','Gérer les groupes spéciaux',@dataAreaID --Manage special groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'8e1c4e00-b4ee-11df-8d81-0800200c9a66','Afficher le paramétrage de codes infos',@dataAreaID --View infocode setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'cfbe2320-b506-11df-8d81-0800200c9a66','Modifier le paramétrage de codes infos',@dataAreaID --Edit infocode setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'21294200-bc03-11df-851a-0800200c9a66','Gérer les unités de l''article',@dataAreaID --Manage item units
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'3ff1a240-c0b8-11df-851a-0800200c9a66','Afficher les groupes de prix',@dataAreaID --View price groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'FF5CECCE-80E9-402B-8720-F65AF31DA820','Modifier les groupes de prix',@dataAreaID --Edit price groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'4991ec80-c72d-11df-bd3b-0800200c9a66','Gérer les séquences de numéros',@dataAreaID --Manage number sequences
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1eec0bd0-dac6-11df-937b-0800200c9a66','Gérer les types de ventes',@dataAreaID --Manage sales types
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'329f05f0-e8c8-11df-9492-0800200c9a66','Gérer les types de restauration',@dataAreaID --Manage hospitality types
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ea1c5150-f0b7-11df-98cf-0800200c9a66','Gérer le calcul de relevés',@dataAreaID --Manage Calculate Statement
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'fb1ce320-f0b7-11df-98cf-0800200c9a66','Gérer la publication d''un relevé',@dataAreaID --Manage Post Statement
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'0b79c730-f624-11df-98cf-0800200c9a66','Afficher les fournisseurs',@dataAreaID --View vendors
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'6bca4d80-f624-11df-98cf-0800200c9a66','Modifier les fournisseurs',@dataAreaID --Edit vendors
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'55b825c0-f3eb-11df-98cf-0800200c9a66','Gérer la disposition de tables',@dataAreaID --Manage dining table layouts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'34bd0c00-f3e1-11df-98cf-0800200c9a66','Gérer les commandes d''achat',@dataAreaID --Manage purchase orders
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'36D84296-316B-4A06-8E9B-DA6D5BA3D2B5','Gérer l''ajustement de stock pour tous les magasins',@dataAreaID --Manage inventory adjustments for all stores
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ad735180-f6df-11df-98cf-0800200c9a66','Afficher les ajustements de stock',@dataAreaID --View inventory adjustments
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'4f4eb240-f6e7-11df-98cf-0800200c9a66','Modifier les ajustements de stock',@dataAreaID --Edit inventory adjustments
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'b13a3ed0-02b7-11e0-a976-0800200c9a66','Gérer les types de menu restaurant',@dataAreaID --Manage restaurant menu types
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'c07b5430-03a5-11e0-a976-0800200c9a66','Gérer l''impression de postes',@dataAreaID --Manage station printing
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'6ed29390-09f0-11e0-81e0-0800200c9a66','Afficher et modifier le comptage du stock',@dataAreaID --View and edit stock counting
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'b737bf90-0c3c-11e0-81e0-0800200c9a66','Modifier les menus PDV',@dataAreaID --Edit POS menus
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'0da1ad40-0c3e-11e0-81e0-0800200c9a66','Afficher les menus PDV',@dataAreaID --View POS menus
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'04AD0711-49C3-464C-9440-06B05F8B59C6','Modifier le paramétrage du mappage TEF',@dataAreaID --Edit EFT mapping setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'CA74D61C-7894-4364-BD91-55A3F98E6A0F','Afficher le paramétrage du mappage TEF',@dataAreaID --View EFT mapping setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'c73861f0-0d0a-11e0-81e0-0800200c9a66','Gérer les documents de réception de marchandises',@dataAreaID --Manage goods receiving documents
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'66323910-19b4-11e0-ac64-0800200c9a66','Gérer les groupes de menus',@dataAreaID --Manage menu groups
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'53956c90-27cf-11e0-91fa-0800200c9a66','Gérer les hôtes distants',@dataAreaID --Manage remote hosts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'6b2d3670-643e-11e0-ae3e-0800200c9a66','Comptages de caisse',@dataAreaID --Tender declarations
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'29a9f4b0-7fe4-11e0-b278-0800200c9a66','Afficher le rapport de niveau des stocks',@dataAreaID --View stock level report
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'81caa200-a232-11e0-8264-0800200c9a66','Gérer les articles associés',@dataAreaID --Manage linked items
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'88e35e80-a23a-11e0-8264-0800200c9a66','Gérer les cartes cadeau',@dataAreaID --Manage gift cards
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'23329470-d232-11e0-9572-0800200c9a66','Gérer les documents de crédit',@dataAreaID --Manage credit vouchers
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'15de9180-e078-11e0-9572-0800200c9a66','Gérer les interruptions centrales',@dataAreaID --Manage central suspensions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'F61938B8-DB15-4714-94F9-1AD16DDDF5FB','Valider et recevoir une commande d''achat',@dataAreaID --Post and receive purchase order
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'8E677D9F-EE02-4A52-9768-0DF78F985126','Auto-remplir les lignes du document de réception de marchandises',@dataAreaID --Auto populate goods receiving document lines
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'c26aa590-f65f-11e0-be50-0800200c9a66','Gérer les liens de cartes LBM',@dataAreaID --Manage msr card links
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'2007e06d-60c6-45cb-9053-3c47a34bc33a','Afficher les types de revenus ou de dépenses',@dataAreaID --View Income and Expense Types
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'dc287d10-9842-11e1-a8b0-0800200c9a66','Gérer les rapports',@dataAreaID --Manage reports
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'F7BC3A76-0911-4A2B-9632-55EFC7B18682','Gérer les profils de gestionnaire de cuisine',@dataAreaID --Manage Kitchen Manager profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'9D520883-3C20-4F56-903A-6233897BA26B','Gérer les stations d''affichage de cuisine',@dataAreaID --Manage Kitchen Display Stations
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'19C10A2A-B228-47FF-8F47-558CD1E8DFCE','Gérer les profils d''affichage de cuisine',@dataAreaID --Manage Kitchen Display Profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'7d1ef450-6edb-11e2-bcfd-0800200c9a66','Gérer les jetons d''authentification',@dataAreaID --Manage authentication tokens
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'d541ded2-a8a4-4e26-934a-fa87c21036ae','Gérer les imprimantes de cuisine',@dataAreaID --Manage Kitchen printers

exec spSECURITY_AddPermissionLocalization_1_0 @locale,'799DD5EB-6906-4955-BFD4-AC4304DE4EA3','Afficher les modèles de l''assistant de configuration',@dataAreaID --View configuration wizard templates
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'500329C3-054C-43AF-9281-B312909BFADA','Modifier les modèles de l''assistant de configuration',@dataAreaID --Edit configuration wizard templates

exec spSECURITY_AddPermissionLocalization_1_0 @locale,'8649312D-14F8-4255-BA94-C02712B903D0','Afficher les demandes de transfert de stock',@dataAreaID --View inventory transfer requests
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'EB9693BB-B825-4010-AFDE-B56CF2DD3183','Modifier les demandes de transfert de stock',@dataAreaID --Edit inventory transfer requests
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'E329EB84-B449-4239-AED8-C6AEC374AD67','Afficher les commandes de transfert de stock',@dataAreaID --View inventory transfer orders
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'D404EDDE-1303-46FD-9FF2-5A9D4247DFE8','Êtes-vous sûr(e) de vouloir supprimer la traduction choisie ?',@dataAreaID --Edit inventory transfer orders

exec spSECURITY_AddPermissionLocalization_1_0 @locale,'aab3bc37-dca6-4407-9941-e1692c7ccf80','Afficher les écritures article',@dataAreaID --View item ledger

exec spSECURITY_AddPermissionLocalization_1_0 @locale,'4342D98A-CDEB-409D-80F2-FCA069DA2782','Gérer le réapprovisionnement',@dataAreaID --Manage replenishment
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'B1F9E246-5EAB-4FC5-BDB4-E1F769787447','Gérer l''inventaire parqué',@dataAreaID --Manage parked inventory
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C3109275-6E3F-491A-A37C-CA32BD3014F1','Gérer les ajustements d''inventaire',@dataAreaID --Manage inventory adjustments
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'AA1822B1-0953-4B90-9DAC-307E5B88DB3F','Gérer les réservations de stock',@dataAreaID --Manage stock reservations
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'411B0B74-BBD7-4F01-94CB-D8294376E0F9','Gérer les réservations de stock pour tous les magasins',@dataAreaID --Manage stock reservations for all stores
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'B50FFD7C-5A65-4871-8DDC-DB3039F6029E','Gérer l''inventaire parqué pour tous les magasins',@dataAreaID --Manage parked inventory for all stores

exec spSECURITY_AddPermissionLocalization_1_0 @locale,'8DECDF32-753D-4ECF-A64D-E93722D3C3CF', 'Afficher les entrées des écritures client',@dataAreaID --View customer ledger entries
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'10C08A81-CB1F-4FC9-8C9C-86B715F1F83C', 'Modifier les entrées des écritures client', @dataAreaID --Edit customer ledger entries'
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'80921901-b919-452f-9c97-6029acabc053', 'Afficher les programmes de fidélité', @dataAreaID --View loyalty schemes
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'80921902-b919-452f-9c97-6029acabc053', 'Modifier les programmes de fidélité', @dataAreaID --Edit loyalty schemes
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'80921903-b919-452f-9c97-6029acabc053', 'Afficher les cartes de fidélité', @dataAreaID --View loyalty cards
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'80921904-b919-452f-9c97-6029acabc053', 'Modifier les cartes de fidélité', @dataAreaID --Edit loyalty cards
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'D0E05BAE-4AD7-4E3E-A35C-D9B63D8F4039', 'Afficher les transactions de fidélité', @dataAreaID --View loyalty transactions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'26542533-9583-4648-8b2d-847eb5bddad1', 'Gérer l''importation de profils', @dataAreaID --Manage import profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'E8C7C5C0-FA5D-4BDE-8516-E616D0B86B87', 'Gérer le paramétrage des styles', @dataAreaID --Manage styles setup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'47EC274D-B7F0-4F24-80C6-2E1FF42F144F', 'Gérer la banque d''images', @dataAreaID --Manage image bank

-- Label templates
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'735C60D3-CBCF-443E-81FD-19904F9232C5','Afficher les modèles d''étiquettes',@dataAreaID --View label templates
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'49940BC5-A609-40D1-A3FB-729E8BA4284B','Modifier les modèles d''étiquettes',@dataAreaID --Edit label templates
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'DBFBB019-E273-40E3-896C-F217C88FDD2B','Imprimer des étiquettes',@dataAreaID --Print labels

--Pos Permissions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9a0-a9a8-11e1-afa6-0800200c9a66','Vente d''article',@dataAreaID --Item Sale
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9a1-a9a8-11e1-afa6-0800200c9a66','Vérification du prix',@dataAreaID --Price Check
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9a2-a9a8-11e1-afa6-0800200c9a66','Annuler l''article',@dataAreaID --Void Item
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9a3-a9a8-11e1-afa6-0800200c9a66','Commentaire d''article',@dataAreaID --Item Comment
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9a4-a9a8-11e1-afa6-0800200c9a66','Prix manuel',@dataAreaID --Price Override
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9a5-a9a8-11e1-afa6-0800200c9a66','Régler la quantité',@dataAreaID --Set quantity
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9a6-a9a8-11e1-afa6-0800200c9a66','Supprimer la quantité',@dataAreaID --Clear quantity
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9a7-a9a8-11e1-afa6-0800200c9a66','Recherche d''article',@dataAreaID --Item Search
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9a8-a9a8-11e1-afa6-0800200c9a66','Retourner l''article ',@dataAreaID --Return Item
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9a9-a9a8-11e1-afa6-0800200c9a66','Transaction de retour ',@dataAreaID --Return Transaction
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9aa-a9a8-11e1-afa6-0800200c9a66','Afficher le journal',@dataAreaID --Show Journal
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9ab-a9a8-11e1-afa6-0800200c9a66','Demande de fidélité',@dataAreaID --Loyalty Request
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'F42F5BB2-E92D-46A6-87EE-1DBF9784A648','Remise de points de fidélité',@dataAreaID --Loyalty points discount

exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9ac-a9a8-11e1-afa6-0800200c9a66','Effacer le vendeur',@dataAreaID --Clear Salesperson
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9ad-a9a8-11e1-afa6-0800200c9a66','Commentaire de facture',@dataAreaID --Invoice Comment
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9ae-a9a8-11e1-afa6-0800200c9a66','Modifier l''unité de mesure',@dataAreaID --Change Unit of Measure
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9b1-a9a8-11e1-afa6-0800200c9a66','Code info sur demande',@dataAreaID --Infocode On Request
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9b3-a9a8-11e1-afa6-0800200c9a66','Modifier les commentaires relatifs à l''article',@dataAreaID --Change Item Comments
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9b4-a9a8-11e1-afa6-0800200c9a66','Paiement en espèces',@dataAreaID --Pay Cash
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9b5-a9a8-11e1-afa6-0800200c9a66','Paiement par carte',@dataAreaID --Pay Card
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9b6-a9a8-11e1-afa6-0800200c9a66','Payer le compte client',@dataAreaID --Pay Customer Account
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9b7-a9a8-11e1-afa6-0800200c9a66','Paiement en devises',@dataAreaID --Pay Currency
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9b8-a9a8-11e1-afa6-0800200c9a66','Paiement en chèque',@dataAreaID --Pay Check
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9b9-a9a8-11e1-afa6-0800200c9a66','Paiement rapide en espèces',@dataAreaID --Pay Cash Quick
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9ba-a9a8-11e1-afa6-0800200c9a66','Paiement par carte de société',@dataAreaID --Pay Corporate Card
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9bb-a9a8-11e1-afa6-0800200c9a66','Annuler le paiement ',@dataAreaID --Void Payment
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9bc-a9a8-11e1-afa6-0800200c9a66','Payer la note de crédit',@dataAreaID --Pay Credit Memo
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9bd-a9a8-11e1-afa6-0800200c9a66','Payer la carte cadeau',@dataAreaID --Pay gift card
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528c9be-a9a8-11e1-afa6-0800200c9a66','Montant de remise par ligne',@dataAreaID --Line Discount Amount
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0b0-a9a8-11e1-afa6-0800200c9a66','Pourcentage de remise par ligne',@dataAreaID --Line Discount Percent
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0b1-a9a8-11e1-afa6-0800200c9a66','Montant total de la remise',@dataAreaID --Total Discount Amount
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0b2-a9a8-11e1-afa6-0800200c9a66','Pourcentage total de la remise',@dataAreaID --Total Discount Percent
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'8BFE0874-54B3-4AAE-9ACB-09926BC3A31B','Déclencher une remise périodique manuellement',@dataAreaID --Manually trigger periodic discount
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0b5-a9a8-11e1-afa6-0800200c9a66','Annuler la transaction ',@dataAreaID --Void Transaction
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0b6-a9a8-11e1-afa6-0800200c9a66','Commentaire sur la transaction',@dataAreaID --Transaction Comment
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0b7-a9a8-11e1-afa6-0800200c9a66','Vendeur',@dataAreaID --Sales Person
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0b8-a9a8-11e1-afa6-0800200c9a66','Suspendre la transaction',@dataAreaID --Suspend Transaction
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0b9-a9a8-11e1-afa6-0800200c9a66','Rappeler la transaction',@dataAreaID --Recall Transaction
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0ba-a9a8-11e1-afa6-0800200c9a66','Annulation de l''ordonnance pharmacie',@dataAreaID --Pharmacy Prescription Cancel
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0bb-a9a8-11e1-afa6-0800200c9a66','Ordonnances pharmacie',@dataAreaID --Pharmacy Prescriptions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0bc-a9a8-11e1-afa6-0800200c9a66','Émettre une note de crédit',@dataAreaID --Issue Credit Memo
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0bd-a9a8-11e1-afa6-0800200c9a66','Émettre une carte cadeau',@dataAreaID --Issue Gift Certificate
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'3957A752-254D-4705-8857-3418E4F15C69','Obtenir le solde de la carte cadeau',@dataAreaID --Get gift card balance
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0be-a9a8-11e1-afa6-0800200c9a66','Afficher total',@dataAreaID --Display Total
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0bf-a9a8-11e1-afa6-0800200c9a66','Commande client',@dataAreaID --Sales Order
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0c0-a9a8-11e1-afa6-0800200c9a66','Facture vente',@dataAreaID --Sales Invoice
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0c1-a9a8-11e1-afa6-0800200c9a66','Montant du revenu',@dataAreaID --Income Account
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0c2-a9a8-11e1-afa6-0800200c9a66','Compte de dépenses ',@dataAreaID --Expense Account
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0c3-a9a8-11e1-afa6-0800200c9a66','Retourner les comptes de revenu',@dataAreaID --Return Income Accounts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1528f0c4-a9a8-11e1-afa6-0800200c9a66','Retourner les comptes de dépense',@dataAreaID --Return Expense Accounts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'39edafe0-a9a2-11e1-afa6-0800200c9a66','Recherche de client',@dataAreaID --Customer Search
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'80e83310-a9a3-11e1-afa6-0800200c9a66','Effacer client',@dataAreaID --Customer Clear
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'5c1b01b0-a9a4-11e1-afa6-0800200c9a66','Transactions client',@dataAreaID --Customer Transactions
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ee006700-a9a4-11e1-afa6-0800200c9a66','Rapport de transactions client',@dataAreaID --Customer Transactions Report
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'432b8890-a9a5-11e1-afa6-0800200c9a66','Se déconnecter',@dataAreaID --Log Off
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'cc68e530-a9a5-11e1-afa6-0800200c9a66','Verrouiller le terminal',@dataAreaID --Lock Terminal
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'563d2aa0-a9a6-11e1-afa6-0800200c9a66','Forcer la déconnexion',@dataAreaID --Log Off Force
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'c24bd340-a9a6-11e1-afa6-0800200c9a66','Recherche de stock',@dataAreaID --Inventory Lookup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'32216ef0-a9a7-11e1-afa6-0800200c9a66','Initialiser l''état Z de caisse',@dataAreaID --Initialize Z Report
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'808bbe60-a9a7-11e1-afa6-0800200c9a66','Imprimer X',@dataAreaID --Print X
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'cee37030-a9a7-11e1-afa6-0800200c9a66','Imprimer Z',@dataAreaID --Print Z
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1B4B77E2-A287-4219-B068-C4463C1DD32D','Imprimer l''état des ventes d''articles',@dataAreaID --Print item sales report
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'4cc27dc0-a9a8-11e1-afa6-0800200c9a66','Activer le mode conception',@dataAreaID --Design Mode Enable
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'b3ff6280-aa36-11e1-afa6-0800200c9a66','Minimiser la fenêtre POS',@dataAreaID --Minimize POS Window
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1396aaf0-aa37-11e1-afa6-0800200c9a66','Opération vide ',@dataAreaID --Blank Operation
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'018BC3EB-19F6-441A-9295-64A0468C108E','Exécuter une commande externe',@dataAreaID --Run external command
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'0E7B1B16-B4A8-4FAB-B8CD-71B51696D431','Exécuter le plug-in PDV',@dataAreaID --Execute POS plugin
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'67f1d250-aa37-11e1-afa6-0800200c9a66','Ouvrir le tiroir',@dataAreaID --Open Drawer
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'c61cb980-aa37-11e1-afa6-0800200c9a66','Fin de journée',@dataAreaID --End Of Day
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'30685560-aa38-11e1-afa6-0800200c9a66','Fin d''équipe',@dataAreaID --End Of Shift
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'7a5dbe80-aa38-11e1-afa6-0800200c9a66','Comptage de caisse',@dataAreaID --Tender Declaration
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'d0120d40-aa38-11e1-afa6-0800200c9a66','Dépôt compte client',@dataAreaID --Customer Account Deposit
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'1e2eb1e0-aa39-11e1-afa6-0800200c9a66','Déclarer le montant initial',@dataAreaID --Declare Start Amount
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'6cc36cb0-aa39-11e1-afa6-0800200c9a66','Entrée de fond de caisse',@dataAreaID --Float Entry
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'c9154e20-aa39-11e1-afa6-0800200c9a66','Retrait de mode de paiement',@dataAreaID --Tender Removal
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'0d3cd0a0-aa3a-11e1-afa6-0800200c9a66','Mise en coffre-fort',@dataAreaID --Safe Drop
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'891e8790-aa3a-11e1-afa6-0800200c9a66','Remise en banque',@dataAreaID --Bank Drop
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'2d7c78b0-aa3b-11e1-afa6-0800200c9a66','Contrepassation de la mise en coffre-fort',@dataAreaID --Safe Drop Reversal
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'97105d50-aa3b-11e1-afa6-0800200c9a66','Contrepassation de la remise en banque',@dataAreaID --Bank Drop Reversal
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ed067000-aa3b-11e1-afa6-0800200c9a66','Fractionner la facture',@dataAreaID --Split Bill
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'48857110-aa3c-11e1-afa6-0800200c9a66','Quitter le PDV de restauration',@dataAreaID --Exit Hospitality POS
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'ad42ee70-aa3c-11e1-afa6-0800200c9a66','Imprimer le type de menu de restaurant',@dataAreaID --Print Hospitality Menu Type
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'0b1136b0-aa3d-11e1-afa6-0800200c9a66','Paramétrer le type de menu de restaurant',@dataAreaID --Set Hospitality Menu Type
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'3f89fbc0-aa3d-11e1-afa6-0800200c9a66','Modifier le type de menu du restaurant',@dataAreaID --Change Hospitality Menu Type
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'7906eed0-aa3d-11e1-afa6-0800200c9a66','Transaction exempte de taxe',@dataAreaID --Tax Exempt Transaction
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'b7d1c310-aa3d-11e1-afa6-0800200c9a66','Effacer l''exemption de taxe de la transaction',@dataAreaID --Clear Transaction Tax Exemption
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'fc8bc190-aa3d-11e1-afa6-0800200c9a66','Modification du groupe de taxes du code info',@dataAreaID --Infocode Tax Group Change
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'f5215570-bbb6-11e1-afa7-0800200c9a66','Modifier les profils de styles',@dataAreaID --Edit Style profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'3134f8f0-bbb7-11e1-afa7-0800200c9a66','Afficher les profils de styles',@dataAreaID --View Style profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'76078220-d1a8-11e1-9b23-0800200c9a66','Afficher les profils de formulaires',@dataAreaID --View Form Profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'12f12fa0-d1a9-11e1-9b23-0800200c9a66','Modifier les profils de formulaires',@dataAreaID --Edit Form Profiles
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'59270E24-FF01-4C88-9AEA-11BDED40A953','Afficher les contextes',@dataAreaID --View Context
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'3F1D120B-207A-4A0A-A56D-2EAD1B2814EE','Modifier les contextes',@dataAreaID --Edit Contexts
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'B15D8168-F2E8-4093-95EB-582E068AD9EC','Ajouter un client',@dataAreaID --Customer add
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'38402373-1A92-483F-B77B-E18C986BB586','Ralentir la commande',@dataAreaID --Bump order
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'20E06A62-BAE4-4EEC-BB04-A4A708956904','Paiement à l''aide du programme de fidélité',@dataAreaID --Pay loyalty
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'214B99CE-0C26-4D16-B65B-EA555708B4F0','Ajouter un client à la carte de fidélité',@dataAreaID --Add customer to loyalty card
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'17B02E4B-ABCB-46A0-BE29-B8FACAB2F434','Afficher le mappage d''intégration',@dataAreaID --View integration mapping
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'DACEDF0B-0709-4B3B-9A27-0FC7DB1FD6BD','Modifier le mappage d''intégration',@dataAreaID --Edit integration mapping
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'5E00968F-DEF1-461C-995C-034DFAAF2619','Afficher la carte de la base de données',@dataAreaID --View database map
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'3a6ed366-3d24-4263-b8d4-6e2a74629d94','Rapport financier',@dataAreaID --Financial report
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'9E435E02-17C8-4BA4-9365-B1A1EA3C8D30','Afficher le journal d''intégration',@dataAreaID --View integration log
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'B2FE62A8-ECA5-4EB7-8C05-36A0E2E290AD','Début de journée',@dataAreaID --Start of day
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'235EE80D-2B89-4CA3-BA0C-5527BFF93ACC','Gérer les profils matériel sur POS',@dataAreaID --Manage hardware profile on POS
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'41E5E014-9DAA-481E-95F3-0BD3E3896556','Commandes client',@dataAreaID --Customer orders
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'2504F28E-2210-4E65-BA6A-3D611F63197E','Devis',@dataAreaID --Quotes
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C35A6965-FAE3-45DB-AACF-F0606F6C4838','Gestion des commandes client',@dataAreaID --Customer order management
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'DD054719-5FF9-4F41-81F2-B282921F1924','Remplacer le dépôt minimum',@dataAreaID --Override minimum deposit
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'DF5E32BF-C726-4A24-B2A6-2104F06D590D','Configurer le code raison',@dataAreaID --Set reason code
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C3A5300F-593E-488C-B080-04025A3BC253','Manage POS customer blocking',@dataAreaID -- Manage POS customer blocking

exec spSECURITY_AddPermissionLocalization_1_0 @locale,'BF61E0E6-AEF1-4AF3-B95F-B6C68DC91D64','Quitter l''application PDV',@dataAreaID --Exit POS application
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'2279A999-A5CB-44B8-805F-8684E6E9FCD7','Relancer l''ordinateur',@dataAreaID --Restart computer
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'5C3CCB66-2EEB-4252-9E91-30C2C603EBBC','Éteindre l''ordinateur',@dataAreaID --Shutdown computer
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'B7464BC4-47B2-48BA-B82A-306E658ECD96','Activer le mode formation',@dataAreaID --Activate training mode

-- User and security management Scheduler (anyNewGuid, description, permissionGroupGUID, sortCode, PermissionCodeGUID, CodeIsEncrypted)
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'58428E18-BFAC-4DAC-9054-1F159AC9CCEA','Accéder à la réplication',@dataAreaID --Access replication
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'E9BD9613-0AB0-4502-951D-6A4701EDFD6F','Modifier les lieux de distribution',@dataAreaID --Edit distribution locations
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C92401C4-7717-4F6F-806C-9130666A896C','Afficher les lieux de distribution',@dataAreaID --View distribution locations

-- 
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'493ED36B-EFDA-4604-BA7D-3DDC7D7381CF','Afficher les projets et les sous-projets',@dataAreaID --View jobs and subjobs
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'A7D518EE-1AD7-49FC-B30B-2D266D0D4D67','Créer plusieurs sous-projets',@dataAreaID --Create multiple subjobs
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'D02A5A6C-0C02-414E-9B42-2E2C1DE658D5','Modifier les paramètres',@dataAreaID --Edit settings
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'19A1CF87-8394-4456-B034-5018665A1AD8','Afficher la conception de la base de données',@dataAreaID --View database design
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'C47B6771-F61A-4EDE-9062-5244CCCD06D2','Modifier les projets et les sous-projets',@dataAreaID --Edit jobs and subjobs
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'B8169851-A8B9-44CB-AF45-88CD6AE321A2','Modifier la carte de la base de données',@dataAreaID --Edit database map
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'50E152C0-907B-44D8-ACCB-9B900785FD85','Modifier la conception de la base de données',@dataAreaID --Edit database design
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'818ACA49-4542-48BD-813F-A6A8E3450F31','Afficher les paramètres',@dataAreaID --View settings
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'660C23AD-3D5D-4758-B80D-E8E539B56E73','Exécuter un projet manuellement',@dataAreaID --Manually run a job


--Backup
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'21EA337E-31BC-4709-B018-D468FD77F492','Sauvegarder base de données',@dataAreaID --Backup database

--Reprint
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'7B11B93C-D7EB-4EC5-BC0E-CBC66BB61E2B','Réimprimer un reçu',@dataAreaID --Reprint receipt

--Customer orders
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'2559EBBC-0175-4E57-9F1A-8DA7A0C621E5','Gérer les paramètres de la commande client',@dataAreaID --Manage customer order settings
exec spSECURITY_AddPermissionLocalization_1_0 @locale,'70729423-230E-46BC-844C-BEE84A3E2358','Gérer les commandes client',@dataAreaID --Manage customer orders

exec spSECURITY_AddPermissionLocalization_1_0 @locale,'d8cd437d-d27c-4cb9-a4b9-af0a6cfd3549','Afficher l''audit des opérations sur terminal',@dataAreaID --View terminal operations audit
