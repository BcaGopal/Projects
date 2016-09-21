﻿CREATE TABLE [dbo].[BuyerAccounts] (
    [BuyerAccountID] [int] NOT NULL IDENTITY,
    [BankName] [nvarchar](max),
    [Currency] [nvarchar](max),
    [BankAccount] [nvarchar](max),
    [BankAddress1] [nvarchar](max),
    [BankAddress2] [nvarchar](max),
    [BankAddress3] [nvarchar](max),
    [CreatedBy] [nvarchar](max),
    [ModifiedBy] [int] NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedDate] [datetime] NOT NULL,
    [Buyer_BuyerID] [int],
    CONSTRAINT [PK_dbo.BuyerAccounts] PRIMARY KEY ([BuyerAccountID])
)
CREATE INDEX [IX_Buyer_BuyerID] ON [dbo].[BuyerAccounts]([Buyer_BuyerID])
CREATE TABLE [dbo].[Buyers] (
    [BuyerID] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](max),
    [Discription] [nvarchar](max),
    [BusinessType] [nvarchar](max),
    [AddressDetails1] [nvarchar](max),
    [AddressDetails2] [nvarchar](max),
    [CreatedBy] [nvarchar](max),
    [ModifiedBy] [int] NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedDate] [datetime] NOT NULL,
    CONSTRAINT [PK_dbo.Buyers] PRIMARY KEY ([BuyerID])
)
CREATE TABLE [dbo].[BuyerDetails] (
    [BuyerDetailID] [int] NOT NULL IDENTITY,
    [Country] [nvarchar](max),
    [City] [nvarchar](max),
    [State] [nvarchar](max),
    [PostalCode] [nvarchar](max),
    [Address1] [nvarchar](max),
    [Address2] [nvarchar](max),
    [Address3] [nvarchar](max),
    [ContectNumber1] [nvarchar](max),
    [AdditionalContectNo] [nvarchar](max),
    [ContectPerson] [nvarchar](max),
    [FaxNo] [nvarchar](max),
    [EmailAddr1] [nvarchar](max),
    [EmailAddr2] [nvarchar](max),
    [CreatedBy] [nvarchar](max),
    [ModifiedBy] [int] NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedDate] [datetime] NOT NULL,
    [Buyer_BuyerID] [int],
    CONSTRAINT [PK_dbo.BuyerDetails] PRIMARY KEY ([BuyerDetailID])
)
CREATE INDEX [IX_Buyer_BuyerID] ON [dbo].[BuyerDetails]([Buyer_BuyerID])
ALTER TABLE [dbo].[BuyerAccounts] ADD CONSTRAINT [FK_dbo.BuyerAccounts_dbo.Buyers_Buyer_BuyerID] FOREIGN KEY ([Buyer_BuyerID]) REFERENCES [dbo].[Buyers] ([BuyerID])
ALTER TABLE [dbo].[BuyerDetails] ADD CONSTRAINT [FK_dbo.BuyerDetails_dbo.Buyers_Buyer_BuyerID] FOREIGN KEY ([Buyer_BuyerID]) REFERENCES [dbo].[Buyers] ([BuyerID])
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'201402091438264_AlterUser1', N'Surya.Planning.Data.Migrations.Configuration',  0x1F8B0800000000000400ED5D4B6FE33812BE2FB0FFC1D06966818913E732DB486690763ABBC1762741DCDDD780911887183D3C129D4D7EDB1EF627ED5F58524F3E2552A264BB914BE0F0F1B1582C168B148BF5BFFFFCF7ECF7D7289CBDC03443497CEE9D1C1D7B3318FB4980E2F5B9B7C54FBFFCEAFDFEDB5FFF72F629885E67DFAB72A7B41CA91967E7DE33C69B0FF379E63FC308644711F2D3244B9EF0919F44731024F3C5F1F1DFE727277348203C82359B9DDD6F638C2298FF43FE5D26B10F37780BC22F4900C3AC4C2739AB1C75760322986D800FCFBDD5367D03477721886342E3D125C0E0A8A8E5CD2E420408452B183E79335220C100137A3F7CCBE00AA749BC5E6D480208BFBE6D2029F704C20C96FDF8D01437EDD2F1827669DE54ECC512AFEE2CE9EE27C216FC46C9CBBB7CEE7DDCBEC1F4C2F713C230B62429FB2FF8C62590A4BB34D9C014BFDDC32745FDEB4B6F36E731E622480DA1A94F493BF7AE637CBAF06637DB30048F21AC39C9B07C859314FE03C6300518067700639812CE5E0730EFA24489D82E88FFA0BFAA16C9F091D1F6665FC0EB6718AFF1F3B9477E7AB32BF40A832AA5A4E25B8C8800934A38DDC2AE8696DB342502FF367A43B447D5404ED25610A430CB4EA66C6C316563A7E3CB460AA9F07E1C5F388802434F886D4A37C58C28264AB19E39F4F757446792255645533FB01BF082D6B92A5069146F760FC33C377B469B42631FE5390FACC6212AFD2A4DA2FB24AC2AF2D90F5F41BA86743E25FA32AB649BFA028167F346D376EBDFDE8AB7B7C69D52D54EA2662F51E6A76853AC9263EB886D8662A2206833A337562AA34B88010A27D0B67C7BE32BDC771D38960E6C749C1B5D58E9B9365D58E94B2B424B59D3D35914782835BB40259BA9D6D75C89E1DABA80EBADB38BEABD3577537D2AFDBDA4439B8E3F3F97392D2337B2C2CC4C1BAD95BB24C3205C92F253E9EBC91686F15784E9CCEF24C6D0C737DBE811A6933010D1F94805A3683899AA8B7730CD26308CAEC0EB047DFA141105488564FC21AB9B7AB7830ED70E726157286D1FA5E5D1CBAEB84D837EBBC0BC621F5BA2AE389915B1CD7012B16D8E386B3761F206A1D4BDF66A394BD402D85EF11EFEB945A94E783BEC1122929BFE55BF2360D5C7AB14A2F5737D4078097D1481D09BDDA5E4577944FFAB375BF9800A41271C256192ED3D6DA8B40B26696B1A5B9434740FD7531C59D0A626344A73168EB26D3056A9955EA2FABA8F66BD0EEC952AADA3EDEDC9E257A3DE5AAE9C8EA65F6BABC64CBFD86C42E4E7EBC5B78CAEA91F41064BDAE8AA5B0D4A9ED96D798D6FB0A234C39328B0CF60A286C8BF4F288477C8FF968ECF40BA569576E0601B90B2E873B2467151DB6E31BCF0317A61C9F89890890F626B2AA8BE68E6B189656CAD92A8F42F4380A2E9F5D204761EEDD82427E3794BDF41B81DA129ED8EA2506B8A0D8534B80F45D16657A12E216D2D34C506ED2F5AF4EE0FB518D2FE4DA3654196FD3B49837F82EC797C6B0AFADB948CDE0A836833A1ACE7D2A73E96EF2DEDE2017DC7A43025355F37BA49CD8BB592DA9468259529664B2A05EBA694966A25B42ED04A67536AD0770FA9EB7D9408AD2C2B1265D1BC0D92FC82E8B988498DAA30A1C45E5555944DADAE846E4EDD3CC7B3D11B1FB6A45ACDDCB6255531C1074F89BE7B4C8B195199A58722DABC19BD0BB1CA07C548CD16255BD46CFEA74DA89A52B61FC08D84DF6231E8A4D25AF22FB22CF1514E1E7BAA2EDC1DE0BBFB290E660617099A8381F208FECB36C4881E21103ACEBDE3A3A3138993EDD0F55A2840D71723F916FE26B280E9AC010FB8EF07AD64AA3F26084496170BDA69ECC0D631A09BB7169DD728661D955D5A5A7D3E54D81916CCE832E334C750420B279EA88C6FE34B18420C67F48C819E902F41E6834056456402057C0AD1DF30A56DE5DF7C339C02146359D9A3D8471B101AF543A86DB85DA3D4D5ED883997700363DAA8D1789910A05DD8E6755B02EBBA38D5533A1BFD6E22350A65AF169A62C5E8279AF25AA16844D5C07E49A6D48D8905531A2B93F6D556D4AEE4D258692A96F791E4F2F035A6D48D5DC8E5C1EA4BE62CC7446654C7986AA129CEBB7B8AA6EAFC686AD9D432B4B09AE9452B32043095BF845D3EE6B7B05E45FFAAA2E20A62F5A5DDC61A571AB212E314605A14B3EAF5BD5C354865AE7640E5D72A5420C5A597AEEAE57199549B5F293B4024616A4364C4D502B60BD10AAC3ACE6C452C0D640BD82E5E364B8800CA88BE2023E21572A664CB4D7371529AED17EBAE35E22D294CB3EDA188D4D02F2A01BEE7A65CE12F966978A2DF3D1AEE1FC55ED4F3B58B2BCA1D6327777B304377822773C4644B69B3A9647AA39A582D1CEAD8466A805D738B39986A6796668763B1C7D1F4A85416869C9277350CAC12CA05834CA449636A5B18DBEE18B40B49623FCFB5734A67FCD9987F9A4E558BAE21B314069F036E55279CB5AD56E79DCD8B7700CA84B3B9E6C180B32F60B341F19A7940A04C99AD8AD70396BFACECBDF4A30263EE732BA36859D62DE124056B28E4D2BBFE01CC2F34D1270B1E013D2B5F069154CCC632AD9A5419A8F248565645558BFE2EBF606BDF54E0AD5A79AF54625D911E4774C395DF4ED12CDF72F5197DE0018420ED7875609984DB2856E78922DB825ABF29C0E1D5A9E648CDA3012C52936A4753BD6710C9D26C263AF16A972309B0CEE985B8D0224A37910D114FB588A756E3D138677003D2249B63B1EE172C189B6E4D59717F5C415B91614F9D0CC8E7C888677361AE4A671992869036DCBCDA31574A6EB5515F3564A97F948AC74EE3C8DAC656D3707EF32C10976131F738DF78BE7B6C8E39A2E400CF824A997D71176DB856DAE75D57ECB7AEA876C92E3586CA253C0735D21BBADAADDAA3F103977488CEC3BC55362A6F0D4E2EAA440B1C844510245F1A6E43283DB4598832C91C8375756181D8746B4DA1563D7D748E5AD9586919B57DD3CBB611BCA07901E0F3ACE8939D9D0552E502D654578ECD0AA2AB2C73CCD27D99C52A93CC3158E76416884DEF81B6D0A0BDAF4C3FC6CA547E2471B42629DC8973B8AED548534FC7E5DA879865B0C623B975F419D7607EBFDBA4DBCC99C6FB979F334DBA391AE3142CF5D2562A793F61168ECFB1582B59EF616EC56433ECF0729762112B4FB4D0A4959B31A74BAB443B7AE43D4E936A87543B108B6075861D9E6CEB34A976489517B08855A5DBA1E9AC1F31CFB2B72A0B91CBD81B6D5A9CCC3B52A67A47E21CB54BA7B657D7319C5E0661F9ACBA1CA2AF6D7E2CB0A301E24FF01D0D94E87CDC7FACD4D5C719ABD2D159B2EEACD46DE3C6CC29DC26D91CAB715466A19A548B4D18EF89CCEDC3F82C8BE32AD6DF983BAE6233EC7ACBF81C8B5D66B22CB63E8D2B32B7E169922D4C86F2C222672C682E31E62864F353ECAB66B967ACC2255F3EE48B500CC84C18433F74CCE6EBFC34F0F6E92703FD4BA9FFF94026356D4B9E444DAAC524E21C4DB939C4E558ACE5BC3729B79AF359938B83A114EC9F1048B38EBFF935E99473B024975FEB47183AC5B307D6E3A7C118672633CF19705BD326D912AB7CB040022BD3F7521ECA5B5323C883C299D85A1E34186DBA599409DDFD6B3D8AE0BACB190EEDCECB7A4CCE1F57309774CECD7B2221A3ECB8D4AEB5D6F261B7F372231E7676DBD803C8DF88527C13136FF19A7FF9126BDADCCFA197BF549FB6D4D77F65AE597E5A7FD07E60CFEFFA5738FD88D43A9B38BF7FD44A6C63885C67D473BAF69A36E686787FAE9F40F117A06D3FA456F52CBE96EA864975D579CF244975B7DB86C45E5F82F75D8A743758FB599C65656BAB523164EDD7540DC74DB5C03CA81DC0ACE4A9FD726E4FE2ECE8EA1E79E91AAE58A45E0ECB94FAFFFA1A6E7905B63B98977427B628421F3C2D4C2A22476F198651213FAB3FC3658820BD125915F80262F40433FC35F903C6E7DEE2F86421C401EB11936B9E6501A7C20E253017A2ACE97C21CEF2AD10311657FC0252FF19A43F45E0F56716AD4FBCAD41608A985AC3F18407FB5D012E5C039EBA190FF15DF34168F2DBE5B9440E7FB93C20BFB1AB97CB7B83096BAAB28703DEC4DD79B0A951D48733D5A18821356C2E29E2440D02D4C4827288E94689BCCFF9E18FC5EE61A8A15166AFF04CF830B943D80D101725681092FCE8BA8BD9EA74EABB99F36E8D0665649EA1F4E9A2EFB820F58E8BB03308918BA23308498E94E306EE7D95F8D12CC39D07A019676D9162CE0C947F31AE4CE70068FADD25122640AAA8337DB114516886403151697A704888511374C6A8B127D0D99641119766309E3B2B468A2F3318CEB139A3881363817720B160AA1EC9CFDF8EB1D17515A0E280F9D4DCB37261C02B5EFA1F26F3AAD7FC87DB656E4C4F311ECE203431E6CDB07150C5B51976DC23C7AEE9BBE82943D7F4059342D93C22EB35947F7EBBC7222C5C531B34430F2940CE3876A8181367D8BE498A7B3308AEFEF638FD18FFF0A1249C2D69CA4811CED0EFE44010538AC1C1844F70C6705E3D3BE6B58307FBDDBCCD2F7C39B57D8D7FD6F33560DD1B2EC69FB566366FFF76BC5FA569B4ED06D1044FFE5A462F70230ED57BB1229669D883431106ED6320FB290B038239ECE689F096A73A87069318FCFAF84EC2331CD613E3E621198AE74F15233A7A7404FD139E039FDE1F24609ABBF0A30A98719B071A5A619F54D8AEE56B170ACC58BEF6467F598648D827012B5FB5D7106418A2E1E0444CE773A89631CD356CE74226DC81AE6F13886FF88AE35A5E5E57DD12EE0A1951DC7D3EF782477ABDA0D857B63F7FAF6C4E1F5442DB802172B51350E357B9DA5634EFDFAB2352E80352A8F0AB20161DC8BC812235C067ABDAB9C8363710ABDF4BD7355628136D63A5EB83B631F52BDA6D8D958AA2B5C5B24C7BB3EA57C1BB23639805C6686F5B1D09A0337C8651F48CF696D5A33B71880D39DE8342EBE9CE7374FE7D1D4E40A61D761E3DA34F67795D24B99F0DEFEA44B131B42FE7B7B2403BE534DED76ED9314AF00B76F6CA2F339932438BD264BA67C518612E86CBC5D4AC183190C57066706BA2C6B9D09C1D16912A645738629412658CA2E2A300318D33B46E20A873670C7DCE1CADCB5CC74F4965160B145545C44B8E441D06C456BD48317A023E26D93ECCB23CDA73F916CAA7E81106D7F1ED166FB6987419468F21F7658D5AD76DEDE7E138789ACF6E73378DCC45170899887EABBF8D3F6E5118D4745F293EB96820A8D95E7E2FA66389E977E3F55B8D7493C4864025FBEADDC657186D420296DDC62BF002F5B475F390E7D8D92502EB14445989D1D427FF12F10BA2D7DFFE0F8B6F423BBFA50000 , N'6.0.1-21010')

