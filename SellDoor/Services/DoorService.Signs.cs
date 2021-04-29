using RestoreMonarchy.SellDoor.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestoreMonarchy.SellDoor.Services
{
    public partial class DoorService
    {
        public string GetSignText(Door door)
        {
            List<string> lines = new List<string>
            {
                pluginInstance.Translate("SignPropertyOwner", door.OwnerName)
            };
            if (door.IsSold)
            {
                lines.Add(pluginInstance.Translate("SignNotForSale"));
            } else
            {
                lines.Add(pluginInstance.Translate("SignForSale", door.PriceString));
            }
            return string.Join($"<br>{Environment.NewLine}", lines);
        }
    }
}
