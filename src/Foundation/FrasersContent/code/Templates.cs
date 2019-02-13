namespace Sitecore.Foundation.FrasersContent
{
    using Sitecore.Data;

    public partial struct Templates
    {
        public struct Store
        {
            public static readonly ID ID = new ID("{C94A0BBD-ED40-4C0F-AFE2-0039A40ED828}");

            public struct Fields
            {
                public static readonly ID ID = new ID("{C94A0BBD-ED40-4C0F-AFE2-0039A40ED828}");
                public static readonly ID StoreName = new ID("{B9D4826F-56C7-4F2B-9520-648769FA42DF}");
                public static readonly string StoreName_FieldName = "Store Name";
                public static readonly ID Logo = new ID("{40A19311-9EE7-4CDE-A9CE-2747B338B2B5}");
                public static readonly ID PhoneNumber = new ID("{8F83BC03-E92D-4461-94A4-7B77DC6AFED9}");
                public static readonly string PhoneNumber_FieldName = "Phone Number";
                public static readonly ID NewDate = new ID("{1BEB7714-80DC-4642-AB25-BA6B916E29E6}");
                public static readonly string NewDate_FieldName = "New Date";
                public static readonly ID StoreCategories = new ID("{0F1AEF8E-0E98-4D1B-B32A-B37F07658931}");
                public static readonly string StoreCategories_FieldName = "Store Categories";
                public static readonly ID Description = new ID("{842869B0-46F9-4B25-8B3F-EC7CE1D6C8E6}");
                public static readonly string Description_FieldName = "Description";
                public static readonly ID Wing = new ID("{C446B54A-C016-4251-B3E9-E477846CD147}");
                public static readonly string Wing_FieldName = "Wing";
                public static readonly ID UnitNo = new ID("{24685BA6-1A7D-4907-B7D2-BDF13A538E03}");
                public static readonly string UnitNo_FieldName = "Unit No";
                public static readonly ID Contact = new ID("{B45A9BC9-DC37-47D8-9235-3E448CEE2E57}");
                public static readonly string Contact_FieldName = "Contact";
                public static readonly ID OpeningHours = new ID("{1D8EC12C-2F42-4813-8013-1C28B8B88315}");
                public static readonly string OpeningHours_FieldName = "Opening Hours";
                public static readonly ID StoreOffers = new ID("{6D6ECBB5-C929-4F47-ACC6-A185AAF49AAB}");
                public static readonly string StoreOffers_FieldName = "Store Offers";
                public static readonly ID Brands = new ID("{2502DC6E-89E3-4887-80F6-E6747100F821}");
                public static readonly string Brands_FieldName = "Brands";
                public static readonly ID Keywords = new ID("{828E5081-96E1-4D71-AC4B-E43FCA6690D4}");
                public static readonly string Keywords_FieldName = "Keywords";
                public static readonly string Score_FieldName = "score";
            }
        }

        public struct StorePage
        {
            public static ID ID => new ID("{5D406680-B088-46E8-9734-7370F2F6E9ED}");
        }

        public struct StoreCategory
        {
            public static readonly ID ID = new ID("{0E7A3169-332E-4207-8ADD-D9F32D393958}");

            public struct Fields
            {
                public static readonly ID Key = new ID("{587D57DD-CB7B-4C18-B17A-A1C40B7FE669}");
                public static readonly ID Value = new ID("{5BD13AF1-553C-42F2-9E2B-004E695BA975}");
            }
        }

        public struct StoreOffer
        {
            public static readonly ID ID = new ID("{4F231ECB-8F0C-4D73-88F5-EDA3A4BA829C}");

            public struct Fields
            {
                public static readonly ID Key = new ID("{5149BD0D-4544-403F-ABED-EDA3CBFD5017}");
                public static readonly ID Value = new ID("{89DEEB44-72AB-48A7-A040-B92A37068CD0}");
                public static readonly ID IconCssClass = new ID("{3CE55246-1D7B-4A99-862A-A69954A39BAD}");
                public static readonly ID OrderNumber = new ID("{6D254C59-C4D2-42EF-AC65-4E8570DD990A}");
            }
        }
        public struct Wing
        {
            public static readonly ID ID = new ID("{2E49D7BB-6D3A-4B76-8D26-38A8FE30DAC7}");
            public struct Fields
            {
                public static readonly ID Key = new ID("{DC66063A-619A-41E6-A0C9-5BD308CB4850}");
                public static readonly ID Value = new ID("{13726788-0FFC-42B5-BB54-12EE94CD8B31}");
            }
        }
        public struct MallSiteSetting
        {
            public static readonly ID ID = new ID("{C1918D32-A056-41F2-B49A-C6E7965E2CA1}");

            public struct Fields
            {
                public static readonly ID MainSite = new ID("{2722BA68-BC77-4D4F-BC60-BB41F4B0C414}");
            }
        }

        public struct Event
        {
            public static readonly ID ID = new ID("{701DF1A5-082E-46DF-B87A-77B4D1323AC8}");
            public struct Fields
            {
                public static readonly ID Title = new ID("{004BC4AE-0AAD-47BF-B18B-C75CFA9364C8}");
                public static readonly string Title_FieldName = "title_t";
                public static readonly ID Image = new ID("{68D9C908-A52B-4A0D-8BE1-2D44CAF70CA6}");
                public static readonly ID Thumbnail = new ID("{0477B953-4588-4520-AEFE-52B6E87566FD}");
                public static readonly ID Summary = new ID("{DB364BB7-9F54-4264-A25F-3B184FB7D089}");
                public static readonly string Summary_FieldName = "summary_t";
                public static readonly ID Description = new ID("{81B58390-E8E1-4DE1-830B-D3552ABE445E}");
                public static readonly string Description_FieldName = "description_t";
                public static readonly ID StartDate = new ID("{C64AD6DC-CF30-412E-8406-75641E1D15DE}");
                public static readonly ID EndDate = new ID("{013305BB-025D-47EA-A340-A4D7FD8DB00B}");
                public static readonly ID BookingLink = new ID("{DDB676F7-362C-4A4C-8684-6155C1667EE1}");
                public static readonly ID IsEventAllMalls = new ID("{687F32F4-1CF8-495F-A7EC-53A4C7C2C48D}");
                public static readonly ID EventSchedule = new ID("{3C10833F-5C44-46F1-B096-B091918804B8}");
                public static readonly ID Category = new ID("{B595EEF2-9425-4778-91C2-9675606DE8C5}");
            }
        }

        public struct EventCategory
        {
            public static readonly ID ID = new ID("{CB36E1B3-D93E-4A55-B04A-EFC7F11D4502}");

            public struct Fields
            {
                public static readonly ID Key = new ID("{20AC0F58-75DA-44D9-BB69-F248FFB41D5F}");
                public static readonly ID Value = new ID("{7E1224AB-48B3-4859-9562-6519CBFAFC52}");
            }
        }

        public struct Article
        {
            public static ID ID => new ID("{636B4735-A56A-4D0B-8D65-6A18E5B2C91C}");
            public struct Fields
            {
                public static readonly ID Title = new ID("{30525C09-F338-4417-880A-27107C3FBF71}");
                public static readonly string Title_FieldName = "title_t";
                public static readonly ID Thumbnail = new ID("{DEA8455B-1F74-4E64-A4FF-6454109A7A84}");
                public static readonly ID Banner = new ID("{9C1F493F-213F-4603-B4CA-6BEF497F88AE}");
                public static readonly ID Summary = new ID("{4E997E31-CDF9-4D49-AA84-105B00C0286D}");
                public static readonly string Summary_FieldName = "summary_t";
                public static readonly ID Description = new ID("{5507CFAC-7561-4B74-86E5-A1C821EC6182}");
                public static readonly string Description_FieldName = "description_t";
                public static readonly ID StartDate = new ID("{BC03F16F-9D3C-4CD6-B0E7-3F9A3E00742F}");
                public static readonly ID EndDate = new ID("{0E45E08F-5ADC-4E4D-84AE-FFB4CB66E4E0}");
                public static readonly ID BookingId = new ID("{F1263CB3-84E2-46D7-97D4-24848F83A227}");
                public static readonly string BookingId_FieldName = "booking_id_t";
                public static readonly ID Category = new ID("{31C583D0-FD40-47D8-81C7-DF52EDA0ED97}");
                public static readonly ID Store = new ID("{D352D897-0D6E-44BE-ABFD-6901F36E63D8}");
            }
        }
        public struct ArticleCategory
        {
            public static readonly ID ID = new ID("{8960B17C-3732-4A2C-A1DC-AD1EBF65E179}");
            public struct Fields
            {
                public static readonly ID Key = new ID("{9AE5B2DC-7F40-4890-8B00-EE612F40D3D8}");
                public static readonly ID Value = new ID("{79AAB1BE-8792-47E6-B98D-184F81F73273}");
            }
        }
        public struct MallSite
        {
            public static ID ID => new ID("{24F3889D-C63B-452D-B158-7601EB790305}");
            public struct Fields
            {
                public static readonly ID DisplayOnMalls = new ID("{9518C26D-7A6E-4D02-BE50-E9E360C7957E}");
            }
        }
        public struct TabCategory
        {
            public static readonly ID ID = new ID("{F513CF2F-F4D9-4FC5-8FD2-EF670DC7FDDA}");

            public struct Fields
            {
                public static readonly ID Key = new ID("{AB53617B-D46A-40C6-A30D-9AA60C20C31A}");
                public static readonly ID Value = new ID("{13CDC28F-A24A-4B3C-A335-CBB70E41A9C5}");
                public static readonly ID Page = new ID("{51F41CCF-33D4-4E4A-8309-ACF6A14DCD41}");
                public static readonly ID OrderNumber = new ID("{B9026424-5F46-43C9-B4DF-7FD1670B7DDE}");
            }
        }

        public struct SchedulableSetting
        {
            public static readonly ID ID = new ID("{69CC14A8-3AC3-4687-91D6-C548C06A62F3}");

            public struct Fields
            {
                public static readonly ID PostDate = new ID("{84FE7E48-12A5-4F18-840C-C3EC5C98510E}");
                public static readonly ID ExpiryDate = new ID("{9B4E265B-2BFA-4665-85D0-8C15F2F07A14}");
            }
        }

        public struct SpecialEvent
        {
            public static ID ID => new ID("{5D5635DB-F3E3-4171-973B-C83D522C111A}");
        }

        public struct Navigable
        {
            public static readonly ID ID = new ID("{BF668C01-168B-4F4E-BDFD-B7EBA56A644C}");

            public struct Fields
            {
                public static readonly ID ShowInNavigation = new ID("{EA72D68D-4A69-4B2F-BDA1-DC8DFDD560E8}");
                public static readonly ID NavigationTitle = new ID("{50F14A1E-3E83-4186-8A90-399B5C3BDD53}");
                public static readonly ID ShowChildren = new ID("{BDD1BB11-64F7-4DA3-86A2-17E9B2B6B76A}");
                public static readonly ID IsTab = new ID("{084A9C69-64B3-4409-A4E6-55329602B17A}");
                public static readonly ID IsSubTab = new ID("{F6E276A6-DD54-4585-B69A-7C969F511960}");
                public static readonly ID IsTabPage = new ID("{125A4E47-7DC0-451F-B37B-2DD5176EAFA9}");
            }
        }

        public struct Blog
        {
            public static readonly ID ID = new ID("{9B2E84CE-C0F2-4ACD-8282-74ED1B293C19}");
            public struct Fields
            {
                public static readonly ID Title = new ID("{A77FFFED-E1F3-4CC1-95AE-06CE63EC8877}");
                public static readonly string Title_FieldName = "title_t";
                public static readonly ID Summary = new ID("{6EF0D4E4-2FAE-49CE-B150-DC2405A383A4}");
                public static readonly string Summary_FieldName = "summary_t";
                public static readonly ID Body = new ID("{6704AA4E-BFC7-4161-8787-87983D71E9F6}");
                public static readonly string Body_FieldName = "body_t";
                public static readonly ID Author = new ID("{914DD69D-8F23-4D70-A42D-20A631020C2C}");
                public static readonly string Author_FieldName = "author_t";
                public static readonly ID Banner = new ID("{B3AC3362-D195-46BE-B223-9EB630A30DB2}");
                public static readonly ID Thumbnail = new ID("{6B90E446-94D5-493C-9E7E-6956F7DB35D8}");
                public static readonly ID Category = new ID("{594CBB70-CC6F-4B29-A55B-FBA6B1C548FD}");
            }
            public struct BlogCategory
            {
                public static readonly ID ID = new ID("{2A1E2F32-DF3E-4B82-B6B9-4BAA981DA0F7}");
                public struct Fields
                {
                    public static readonly ID Key = new ID("{B61A7758-DEB3-44D4-ABF9-48D85D612C30}");
                    public static readonly ID Value = new ID("{D1170E59-438B-4FF8-874D-0438DFE2DDF7}");
                }
            }
            public struct BlogCategoryFolder
            {
                public static readonly ID ID = new ID("{EA99F9BC-728C-4E1E-AECF-46709BD2CC7D}");
            }
        }

        public struct Banner
        {
            public static readonly ID ID = new ID("{F99D38F2-8B19-461B-89FD-3ED8262CA199}");

            public struct Fields
            {
                public static readonly ID Summary = new ID("{0E196BAC-78A7-42DA-9DFB-1E1DF89EE2A1}");
                public static readonly ID Title = new ID("{6591A883-0CCB-4F93-8978-C7D26AE2254A}");
                public static readonly ID Category = new ID("{4A8E4D82-40A8-4EB1-BC4A-8031867C7527}");
                public static readonly ID Link = new ID("{AFFB581E-17CE-4F73-9894-E47AD6925E3A}");
                public static readonly ID Image = new ID("{3110E64C-0296-4074-B8BA-2C9A486FBA82}");
                public static readonly ID MobileImage = new ID("{943D0FDD-2D6E-439D-A5C6-F2DD9D11BB9C}");
            }
        }

        public struct Link
        {
            public static readonly ID ID = new ID("{F91BA0B2-24E1-42DF-B2E3-2122A7BC71AC}");

            public struct Fields
            {
                public static readonly ID Link = new ID("{E81C5D1D-E1DE-47E7-A5C1-D3C77DB7E75B}");
            }
        }

        public struct ContactFormMainSite
        {
            public static readonly ID ID = new ID("{02C8558C-403F-41EA-AFEC-0169C2DA04E5}");
            public struct Fields
            {
                public static readonly ID ContactPageTitle = new ID("{4CE23CA3-0663-4C79-8C59-EEB5B251C6FE}");
                public static readonly ID ContactPageSubTitle = new ID("{5ED37496-1555-4255-A5ED-B13D39F143AD}");
                public static readonly ID ReceiverName = new ID("{0FF35D56-2384-4845-87E2-EB93768035CC}");
                public static readonly ID ReceiverEmail = new ID("{8CFCE156-A215-42D6-87D2-0F92E1F404F1}");
                public static readonly ID NoReplyEmail = new ID("{6CA85ACC-5DFE-428F-96FA-39A1D37F933F}");
                public static readonly ID GoogleCapchaPublicKey = new ID("{ED78E746-BD29-4A2B-9901-F122D200C36C}");
                public static readonly ID GoogleCapchaPrivateKey = new ID("{26AE7575-792F-4108-A050-FC86762C30E2}");
            }
        }

        public struct AddThisSocialToken
        {
            public static readonly ID ID = new ID("{A9E07F11-C8EE-475B-A8F3-E490955C92AF}");
        }
        public struct TrackingID
        {
            public static readonly ID ID = new ID("{449338CE-CA1E-4D38-A904-CDB4085C84DD}");
        }

        public struct ArticlePage {
            public static readonly ID ID = new ID("{24F3889D-C63B-452D-B158-7601EB790305}");
        }
        
        public struct Identity
        {
            public static readonly ID ID = new ID("{FA8DE5B9-D5D8-40A7-866A-23AF4F5A9629}");

            public struct Fields
            {
                public static readonly ID SiteName = new ID("{D390B56F-6F6C-4DA7-832C-2ED4C44733E5}");
            }
        }
        public struct FrasersRewards
        {
            public static readonly ID ID = new ID("{875A246A-4F98-4F8B-BDA6-477C268B0607}");
        }
    }
}