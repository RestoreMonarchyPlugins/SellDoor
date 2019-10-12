using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SellDoor.Models
{
    public class DoorData
    {
        public DoorData() { }

        public DoorData(ulong owner, decimal price, ConvertablePosition position = null)
        {
            SellerID = owner;
            Price = price;
            Position = position;
        }

        public ulong SellerID { get; set; }
        public decimal Price { get; set; }
        public ConvertablePosition Position { get; set; }
    }
}
