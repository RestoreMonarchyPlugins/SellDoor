using RestoreMonarchy.SellDoor.Models;
using Rocket.API;

namespace RestoreMonarchy.SellDoor
{
    public class SellDoorConfiguration : IRocketPluginConfiguration
    {
        public string MessageColor { get; set; }
        public int DefaultMaxDoors { get; set; }
        public SellDoorLimit[] Limits { get; set; }

        public void LoadDefaults()
        {
            MessageColor = "yellow";
            DefaultMaxDoors = 2;
            Limits = new SellDoorLimit[]
            {
                new SellDoorLimit()
                {
                    Permission = "selldoor.vip",
                    MaxDoors = 3
                }
            };
        }
    }
}
