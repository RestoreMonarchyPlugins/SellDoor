using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Framework.Utilities;
using SDG.Unturned;
using RestoreMonarchy.SellDoor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using RestoreMonarchy.SellDoor.Helpers;

namespace RestoreMonarchy.SellDoor.Commands
{
    public class CostDoorCommand : IRocketCommand
    {
        private SellDoorPlugin pluginInstance => SellDoorPlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            Transform transform = RaycastHelper.GetBarricadeTransform(player.Player, out _, out BarricadeDrop drop);

            if (transform == null || drop.interactable as InteractableDoor == null)
            {
                MessageHelper.Send(caller, "DoorNotLooking");
                return;
            }

            Door door = pluginInstance.DoorService.GetDoor(transform);

            if (door == null || door.IsSold)
            {
                MessageHelper.Send(caller, "DoorNotForSale");
                return;
            }

            MessageHelper.Send(caller, "CostDoorPrice", door.PriceString);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "costdoor";

        public string Help => "Replies to player with the selected door price";

        public string Syntax => string.Empty;

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}
