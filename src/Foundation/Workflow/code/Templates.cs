using Sitecore.Data;

namespace Sitecore.Foundation.Workflow
{
    public struct Templates
    {
        public struct EmailTemplate
        {
            public static readonly ID Id = new ID("{2F6F9983-5CBF-4B47-91F6-8F5DFBDCACA7}");
            public struct Fields
            {
                public static readonly ID Message = new ID("{A4E04ABD-D56C-4E4E-A190-7EA638845831}");
                public static readonly ID Subject = new ID("{C0D436A3-7E25-4D14-BF04-41753F7967E2}");
                public static readonly ID Title = new ID("{37013884-0850-4493-823C-FA2B5548F635}");
            }
        }

        public struct NotifyNextStepUser
        {
            public static readonly ID Id = new ID("{7592365A-50D4-4FC3-9CBA-2905DBEEABD2}");
            public struct Fields
            {
                public static readonly ID EmailTemplate = new ID("{3CE817FE-66BE-44C2-B263-672165CE1A13}");
                public static readonly ID SentToItemAuthorOnly = new ID("{71AA25D8-DE5F-4C2E-907C-29DEB16FC49C}");
                public static readonly ID From = new ID("{D9E9B907-11AA-4609-9D6C-6D15F3CF6AB2}");
            }
        }
    }
}