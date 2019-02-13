using Newtonsoft.Json;

namespace Sitecore.Feature.WebApi.Formatters
{
    public class JsonOutput
    { 
        [JsonProperty(Order=-2)]
        public string Status { get; set; }

        [JsonProperty(Order=-1)]
        public string Reason { get; set; }

        public JsonOutput()
        {
            this.Status = null;
            this.Reason = null;
        }

        public JsonOutput(string status)
        {
            this.Status = status;
        }

        public JsonOutput(string status, string reason)
        {
            this.Status = status;
            this.Reason = reason;
        }
    }
}