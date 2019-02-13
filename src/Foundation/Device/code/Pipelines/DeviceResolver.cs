namespace Sitecore.Foundation.Device.Pipelines
{
    using Sitecore.Foundation.Device.Repositories;
    using Sitecore.Pipelines.HttpRequest;
    using System.Web;

    public class DeviceResolver : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            HttpContext currentHttpContext = HttpContext.Current;

            if (currentHttpContext == null || Context.Database == null)
                return;

            if (Context.Site.Name.ToLower() != "website")
                return;

            DeviceType deviceType = DeviceRepository.RetrieveContext();
            switch (deviceType)
            {
                case DeviceType.Default:
                    break;

                case DeviceType.Mobile:
                    this.SetDevice("Mobile");
                    break;

                case DeviceType.Tablet:
                    this.SetDevice("Tablet");
                    break;
            }
        }

        private void SetDevice(string deviceName)
        {
            var device = Context.Database.Resources.Devices[deviceName];
            Context.Device = device;
        }
    }
}