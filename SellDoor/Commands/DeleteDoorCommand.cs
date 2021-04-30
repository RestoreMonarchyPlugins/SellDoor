using RestoreMonarchy.SellDoor.Helpers;
using RestoreMonarchy.SellDoor.Models;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;

namespace RestoreMonarchy.SellDoor.Commands
{
    public class DeleteDoorCommand : IRocketCommand
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

            if (door == null)
            {
                MessageHelper.Send(caller, "DoorNotForSale");
                return;
            }

            pluginInstance.DoorService.RemoveDoor(door);
            MessageHelper.Send(caller, "DeleteDoorSucccess", door.Id, door.Items.Count);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "deletedoor";

        public string Help => "Delete a door";

        public string Syntax => "";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}
