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

            int doorId = 0;
            bool flag = true;
            if (command.Length > 0)
            {
                flag = false;
                if (!int.TryParse(command[0], out doorId))
                {
                    MessageHelper.Send(caller, "WrongDoorId", command[0]);
                    return;
                }
            }

            Door door;
            if (flag)
            {
                Transform transform = RaycastHelper.GetBarricadeTransform(player.Player, out _, out BarricadeDrop drop);

                if (transform == null || (drop.interactable as InteractableDoor == null && drop.interactable as InteractableSign == null))
                {
                    MessageHelper.Send(caller, "DoorNotLooking");
                    return;
                }
                door = pluginInstance.DoorService.GetDoorOrItem(transform);
            }
            else
            {
                door = pluginInstance.DoorService.GetDoor(doorId);
            }
             

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

        public string Help => "Deletes the selected or specified door and all its items from database";

        public string Syntax => "[doorId]";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}
