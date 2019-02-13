using System.Linq;
using System.Web;

namespace Sitecore.Foundation.Device.Repositories
{
    public static class DeviceRepository
    {
        public static DeviceType RetrieveContext()
        {
            //GETS THE CURRENT USER CONTEXT
            var context = HttpContext.Current;

            //FIRST TRY BUILT IN ASP.NET CHECK
            if (context.Request.Browser.IsMobileDevice)
            {
                return DeviceType.Mobile;
            }

            //THEN TRY CHECKING FOR THE HTTP_X_WAP_PROFILE HEADER
            if (context.Request.ServerVariables["HTTP_X_WAP_PROFILE"] != null)
            {
                return DeviceType.Mobile;
            }

            //THEN TRY CHECKING THAT HTTP_ACCEPT EXISTS AND CONTAINS WAP
            if (context.Request.ServerVariables["HTTP_ACCEPT"] != null &&
                context.Request.ServerVariables["HTTP_ACCEPT"].ToLower().Contains("wap"))
            {
                return DeviceType.Mobile;
            }

            //AND FINALLY CHECK THE HTTP_USER_AGENT
            //HEADER VARIABLE FOR ANY ONE OF THE FOLLOWING
            if (context.Request.ServerVariables["HTTP_USER_AGENT"] != null)
            {
                var userAgent = context.Request.ServerVariables["HTTP_USER_AGENT"].ToLower();

                var tablets =
                  new[]
                    {
                        "ipad", "android 3", "xoom", "sch-i800", "tablet", "kindle", "playbook"
                    };

                //Loop through each item in the list created above
                //and check if the header contains that text
                if (tablets.Any(userAgent.Contains) || (userAgent.Contains("android") && !userAgent.Contains("mobile")))
                {
                    return DeviceType.Tablet;
                }

                //Create a list of all mobile types
                var mobiles =
                  new[]
                    {
                      "midp", "j2me", "avant", "docomo",
                      "novarra", "palmos", "palmsource",
                      "240x320", "opwv", "chtml",
                      "pda", "windows ce", "mmp/",
                      "blackberry", "mib/", "symbian",
                      "wireless", "nokia", "hand", "mobi",
                      "phone", "cdm", "up.b", "audio",
                      "SIE-", "SEC-", "samsung", "HTC",
                      "mot-", "mitsu", "sagem", "sony"
                      , "alcatel", "lg", "eric", "vx",
                      "NEC", "philips", "mmm", "xx",
                      "panasonic", "sharp", "wap", "sch",
                      "rover", "pocket", "benq", "java",
                      "pt", "pg", "vox", "amoi",
                      "bird", "compal", "kg", "voda",
                      "sany", "kdd", "dbt", "sendo",
                      "sgh", "gradi", "jb", "dddi",
                      "moto", "iphone", "Opera Mini"
                    };

                //Loop through each item in the list created above
                //and check if the header contains that text
                if (mobiles.Any(userAgent.Contains))
                {
                    return DeviceType.Mobile;
                }
            }

            return DeviceType.Default;
        }

        public static bool IsMobileOrTablet
        {
            get
            {
                var device = RetrieveContext();
                return (device == DeviceType.Mobile) || (device == DeviceType.Tablet);
            }
        } 
    }
}