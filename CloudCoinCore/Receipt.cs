using Newtonsoft.Json;
namespace Founders
{
    public class Receipt
    {
      [JsonProperty("reciept_id")]
      public string reciept_id { get; set; }
      
	    [JsonProperty("time")]
      public string time { get; set; }
  
	    [JsonProperty("timezone")]
      public string timezone { get; set; }
  
	     [JsonProperty("bank_server")]
       public string bank_server { get; set; }
  
	    [JsonProperty("total_authentic")]
      public string total_authentic { get; set; }
  
     	[JsonProperty("total_fracked")]
       public string total_fracked { get; set; }
  
	     [JsonProperty("total_counterfeit")]
       public string total_counterfeit { get; set; }
  
	     [JsonProperty("total_lost")]
       public string total_lost { get; set; }
  
        [JsonProperty("receipt")]
        public ReceiptDetail[] rd { get; set; }
        
    }
}
