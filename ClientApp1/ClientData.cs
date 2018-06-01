using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp1
{
    public class ClientData
    {
        [JsonProperty("sender")]
        public string Sender { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
       // [JsonProperty("time")]
       // public DateTime Time { get; set; }
       
    }
}
