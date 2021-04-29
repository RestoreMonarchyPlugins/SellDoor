using Rocket.API;

namespace RestoreMonarchy.SellDoor
{
    public class SellDoorConfiguration : IRocketPluginConfiguration
    {
        public string MessageColor { get; set; }

        public void LoadDefaults()
        {
            MessageColor = "yellow";
        }
    }
}
