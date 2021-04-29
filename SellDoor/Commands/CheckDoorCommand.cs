using RestoreMonarchy.SellDoor.Helpers;
using RestoreMonarchy.SellDoor.Models;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;

namespace RestoreMonarchy.SellDoor.Commands
{
    public class CheckDoorCommand : IRocketCommand
    {
        private SellDoorPlugin pluginInstance => SellDoorPlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            Transform transform;
            transform = RaycastHelper.GetBarricadeTransform(player.Player, out _, out _);
            if (transform == null)
            {
                transform = RaycastHelper.GetStructureTransform(player.Player, out _);
            }

            if (transform == null)
            {
                MessageHelper.Send(caller, "DoorNotLooking");
                return;
            }

            Door door = pluginInstance.DoorService.GetDoorOrItem(transform);

            if (door == null)
            {
                MessageHelper.Send(caller, "DoorNotForSale");
                return;
            }

            MessageHelper.Send(caller, "CheckDoorSuccess", door.Id, door.PriceString, door.OwnerName);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "checkdoor";

        public string Help => "Checks the door info";

        public string Syntax => "";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        
    }
}
