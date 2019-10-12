using Rocket.API;
using SellDoor.Models;
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

        public void LoadDefaults()
        {
            MessageColor = "green";
        }
    }
}
