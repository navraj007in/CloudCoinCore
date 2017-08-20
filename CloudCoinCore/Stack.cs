using Newtonsoft.Json;
namespace CloudCoin
{
    public class Stack
    {
        [JsonProperty("cloudcoin")]
        public CloudCoin[] cc { get; set; }
    }
}
