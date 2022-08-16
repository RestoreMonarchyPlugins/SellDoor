using RestoreMonarchy.SellDoor.Models;
using Rocket.API;

namespace RestoreMonarchy.SellDoor
{
    public class SellDoorConfiguration : IRocketPluginConfiguration
    {
        public string MessageColor { get; set; }
        public int DefaultMaxDoors { get; set; } = -1;
        public SellDoorLimit[] Limits { get; set; } = new SellDoorLimit[0];

        public void LoadDefaults()
        {
            MessageColor = "yellow";
            DefaultMaxDoors = -1;
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
