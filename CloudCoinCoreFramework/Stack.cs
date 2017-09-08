using Newtonsoft.Json;
namespace Founders
{
    public class Stack
    {
        [JsonProperty("cloudcoin")]
        public CloudCoin[] cc { get; set; }
    }
}
