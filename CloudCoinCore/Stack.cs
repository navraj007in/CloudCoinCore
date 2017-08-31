using Newtonsoft.Json;
namespace CloudCoinCore
{
    public class Stack
    {
        [JsonProperty("cloudcoin")]
        public CloudCoin[] cc { get; set; }
    }
}
