using Sitecore.Data;

namespace Sitecore.Feature.WebApi
{
    public static class Templates
    {
        public struct StoreOffers
        {
            public static readonly ID HalalCertified = new ID("{FE2265B5-E478-439B-ACA8-CE2223F6420A}");
            public static readonly ID EarnFrasersPoints = new ID("{55F759E2-0FCC-4FAF-A46B-CDF16D08FCEF}");
            public static readonly ID AcceptsGiftCards = new ID("{80FE60BA-90EE-4B25-BFE3-8A7D3EA0F59D}");
        }

        public struct Store
        {
            public static readonly ID StoreName = new ID("{B9D4826F-56C7-4F2B-9520-648769FA42DF}");
            public static readonly ID UnitNo = new ID("{24685BA6-1A7D-4907-B7D2-BDF13A538E03}");
            public static readonly ID PhoneNo = new ID("{8F83BC03-E92D-4461-94A4-7B77DC6AFED9}");
            public static readonly ID Logo = new ID("{40A19311-9EE7-4CDE-A9CE-2747B338B2B5}");
            public static readonly ID OpeningHours = new ID("{1D8EC12C-2F42-4813-8013-1C28B8B88315}");
            public static readonly ID Description = new ID("{842869B0-46F9-4B25-8B3F-EC7CE1D6C8E6}");
            public static readonly ID StoreOffers = new ID("{6D6ECBB5-C929-4F47-ACC6-A185AAF49AAB}");
        }

        public struct Article
        {
            public static readonly ID Title = new ID("{30525C09-F338-4417-880A-27107C3FBF71}");
            public static readonly ID Thumbnail = new ID("{DEA8455B-1F74-4E64-A4FF-6454109A7A84}");
            public static readonly ID Summary = new ID("{4E997E31-CDF9-4D49-AA84-105B00C0286D}");
            public static readonly ID Description = new ID("{5507CFAC-7561-4B74-86E5-A1C821EC6182}");
        }

        public struct Identity
        {
            public static readonly ID ID = new ID("{FA8DE5B9-D5D8-40A7-866A-23AF4F5A9629}");

            public struct Fields
            {
                public static readonly ID SiteName = new ID("{D390B56F-6F6C-4DA7-832C-2ED4C44733E5}");
                public static readonly ID SiteCode = new ID("{12614B52-3914-4054-A3CC-AFEE27D363BC}");
            }
        }
    }
}