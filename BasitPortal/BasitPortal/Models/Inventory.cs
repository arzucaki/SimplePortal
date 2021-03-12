using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasitPortal.Models
{
    public class Inventory
    {
        [JsonProperty("data")]
        public List<QualityInventory> data { get; set; }
        public int count { get; set; }
    }
}