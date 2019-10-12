namespace SellDoor.Models
{
    public class DoorData
    {
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