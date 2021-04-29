using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace RestoreMonarchy.SellDoor.Models
{
    public class Door : TransformBase
    {
        public Door() { }

        public int Id { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public decimal Price { get; set; }
        public bool IsSold { get; set; }

        public List<DoorItem> Items { get; set; }

        [JsonIgnore]
        public string PriceString => Price.ToString("N");
    }
}
