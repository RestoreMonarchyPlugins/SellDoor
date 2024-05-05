using RestoreMonarchy.SellDoor.Models;
using Rocket.API;

namespace RestoreMonarchy.SellDoor
{
    public class SellDoorConfiguration : IRocketPluginConfiguration
    {
        public string MessageColor { get; set; }
        public bool EnableUI { get; set; }
        public ushort EffectId { get; set; } = 54600;
        public int DefaultMaxDoors { get; set; } = -1;
        public SellDoorLimit[] Limits { get; set; } = [];

        public void LoadDefaults()
        {
            MessageColor = "yellow";
            EnableUI = false;
            EffectId = 54600;
            DefaultMaxDoors = -1;
            Limits =
            [
                new SellDoorLimit()
                {
                    Permission = "selldoor.vip",
                    MaxDoors = 3
                }
            ];
        }
    }
}
