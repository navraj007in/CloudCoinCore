using Newtonsoft.Json;
namespace Founders
{
    class Stack
    {
        [JsonProperty("cloudcoin")]
        public CloudCoin[] cc { get; set; }
    }
}
