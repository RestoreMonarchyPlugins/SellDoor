using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellDoor
{
    public class SellDoorConfiguration : IRocketPluginConfiguration
    {
        public string MessageColor { get; set; }
        public bool IsAllowedToDestroy { get; set; }

        public void LoadDefaults()
        {
            MessageColor = "green";
            IsAllowedToDestroy = false;
        }
    }
}
