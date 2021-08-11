/*

	Incident No.	: 8793
	Responsible		: Björn Eiríksson
	Sprint			: Store Controller 2.0 SP1
	Date created	: 21.02.2011
	
	Description		: Table to store states

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:	STATES
						
*/

USE LSPOSNET

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[STATES]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[STATES](
	[FORMATTERID] int NOT NULL,
	[ABBREVIATEDNAME] [nvarchar](4) NOT NULL,
	[NAME] [nvarchar](30) NOT NULL,
	[DATAAREAID] [nvarchar](4) NOT NULL,
 CONSTRAINT [PK_STATES] PRIMARY KEY CLUSTERED 
(
	[FORMATTERID] ASC,
	[ABBREVIATEDNAME] ASC,
	[NAME] ASC,
	[DATAAREAID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'AN',N'Andaman and Nicobar Islands',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'AP',N'Andhra Pradesh',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'AR',N'Arunachal Pradesh',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'AS',N'Assam',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'BR',N'Bihar',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'CH',N'Chandigarh',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'CG',N'Chhattisgarh',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'DN',N'Dadra and Nagar Haveli',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'DD',N'Daman and Diu',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'DL',N'Delhi',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'GA',N'Goa',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'GJ',N'Gujarat',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'HR',N'Haryana',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'HP',N'Himachal Pradesh',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'JK',N'Jammu and Kashmir',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'JH',N'Jharkhand',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'KA',N'Karnataka',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'KL',N'Kerala',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'LD',N'Lakshadweep',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'MP',N'Madhya Pradesh',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'MH',N'Maharashtra',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'MN',N'Manipur',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'ML',N'Meghalaya',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'MZ',N'Mizoram',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'NL',N'Nagaland',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'OR',N'Orissa',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'PY',N'Puducherry',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'PB',N'Punjab',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'RJ',N'Rajasthan',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'SK',N'Sikkim',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'TN',N'Tamil Nadu',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'TR',N'Tripura',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'UP',N'Uttar Pradesh',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'UK',N'Uttarakhand',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (4,N'WB',N'West Bengal',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (3,N'ON',N'Ontario',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (3,N'QC',N'Quebec',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (3,N'NS',N'Nova Scotia',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (3,N'NB',N'New Brunswick',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (3,N'MB',N'Manitoba',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (3,N'BC',N'British Columbia',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (3,N'PE',N'Prince Edward Island',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (3,N'SK',N'Saskatchewan',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (3,N'AB',N'Alberta',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (3,N'NL',N'Newfoundland and Labrador',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (3,N'NT',N'Northwest Territories',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (3,N'YT',N'Yukon',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (3,N'NU',N'Nunavut',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'AL',N'Alabama',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'AK',N'Alaska',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'AZ',N'Arizona',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'AR',N'Arkansas',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'CA',N'California',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'CO',N'Colorado',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'CT',N'Connecticut',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'DE',N'Delaware',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'FL',N'Florida',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'GA',N'Georgia',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'HI',N'Hawaii',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'ID',N'Idaho',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'IL',N'Illinois',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'IN',N'Indiana',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'IA',N'Iowa',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'KS',N'Kansas',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'KY',N'Kentucky',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'LA',N'Louisiana',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'ME',N'Maine',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'MD',N'Maryland',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'MA',N'Massachusetts',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'MI',N'Michigan',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'MN',N'Minnesota',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'MS',N'Mississippi',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'MO',N'Missouri',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'MT',N'Montana',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'NE',N'Nebraska',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'NV',N'Nevada',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'NH',N'New Hampshire',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'NJ',N'New Jersey',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'NM',N'New Mexico',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'NY',N'New York',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'NC',N'North Carolina',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'ND',N'North Dakota',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'OH',N'Ohio',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'OK',N'Oklahoma',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'OR',N'Oregon',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'PA',N'Pennsylvania',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'RI',N'Rhode Island',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'SC',N'South Carolina',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'SD',N'South Dakota',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'TN',N'Tennessee',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'TX',N'Texas',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'UT',N'Utah',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'VT',N'Vermont',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'VA',N'Virginia',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'WA',N'Washington',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'WV',N'West Virginia',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'WI',N'Wisconsin',N'LSR')

insert into [dbo].[STATES] ([FORMATTERID],[ABBREVIATEDNAME],[NAME],[DATAAREAID])
values (2,N'WY',N'Wyoming',N'LSR')

end

GO