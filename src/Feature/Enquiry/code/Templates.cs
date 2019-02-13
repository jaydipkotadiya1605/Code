namespace Sitecore.Feature.Enquiry
{
    using Sitecore.Data;
    public struct Templates
    {
        public struct EnquiryResources
        {
            public static readonly ID ID = new ID("{E37E9E40-AD4C-4516-8E66-F41F500194EA}");
        }
        public struct EnquiryType
        {
            public static readonly ID ID = new ID("{5B2C6C77-D101-4D64-9030-A55E8A7F4232}");
        }

        public struct LeasingResources
        {
            public static readonly ID ID = new ID("{9B8858EE-A668-4FDF-ADDC-EAFA8FD36AC5}");
        }
        public struct LeasingType
        {
            public static readonly ID ID = new ID("{EDF0C502-A342-4D3B-8F89-51DEFE8931EB}");
        }
        public struct EnquiryTemplate {
            public static readonly ID ID = new ID("{8329244E-B078-423D-AC5B-A394BCC6C441}");
            public struct Fields {
                public static readonly ID Title = new ID("{ED290D78-4241-42A0-8C62-B3E25C4BB48B}");
                public static readonly ID Subject = new ID("{843278D6-2A48-42BE-9393-871521278552}");
                public static readonly ID Message = new ID("{2268E826-33B0-4BD3-A5CF-0660D0CF63DF}");

            }
        }
    }

    public static class Constants {
        public const string ErrorMessage = "ErrorMessage";
        public const string StatusForm = "StatusForm";
        public const string GeneralEnquiry = "General Enquiry";
        public const string LeasingEnquiry = "Leasing Enquiry";
    }
}