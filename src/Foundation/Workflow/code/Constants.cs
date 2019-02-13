namespace Sitecore.Foundation.Workflow
{
    using Sitecore.Data;

    public struct Constants
    {
        public static readonly string ItemUrlFormat = "<a href=\"{0}\" target=\"_blank\">Preview Item Page</a>";
        public static readonly string WorkflowNameFormat = "{0} Item";
        public static readonly string PreviewUrlHasPresentation = "{0}://{1}/?sc_itemid=%7b{2}%7d&sc_lang={3}&sc_mode=preview";
        public static readonly string PreviewUrlNoPresentation = "{0}://{1}/sitecore/shell/Applications/Content Editor.aspx?la={3}&fo={2}";

        public static readonly string NextState = "Next State";
        public static readonly string Comments = "Comments";
        public static readonly string NoComment = "---";
        public static readonly string DomainToSend = "frasersproperty";

        public struct EmailToken
        {
            public static readonly string ItemName = "[ItemName]";
            public static readonly string ItemUrl = "[ItemURL]";
            public static readonly string WorkflowName = "[WorkflowName]";
            public static readonly string NextStep = "[NextStep]";
            public static readonly string SubmitComment = "[SubmitComment]";
            public static readonly string Receiver = "[Receiver]";
            public static readonly string CurrentActionUser = "[CurrentActionUser]";
        }

        public struct Role
        {
            public static readonly string MainAdmin = @"frasersproperty\Main Site Admin";
            public static readonly string MallAdmin = @"frasersproperty\Mall Site Admin";
            public static readonly string MainAuthor = @"frasersproperty\Main Site Author";
            public static readonly string MallAuthor = @"frasersproperty\Mall Site Author";
        }

        public struct States
        {
            public static readonly ID Draft = new ID("{6C811026-F174-4E3D-AE92-031A87F1F011}");
            public static readonly ID WaitingForMallSiteApproval = new ID("{0CD981C2-2148-4159-A810-8EB6343A1124}");
            public static readonly ID WaitingForMainSiteApproval = new ID("{52FC6AC9-F0C4-4FC0-8508-E6D1D429061D}");
            public static readonly ID Approved = new ID("{9635A6C2-0C1D-48C9-918C-D15D71613566}");
        }
    }
}