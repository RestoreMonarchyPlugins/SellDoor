using RestoreMonarchy.SellDoor.Helpers;
using RestoreMonarchy.SellDoor.Models;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;

namespace RestoreMonarchy.SellDoor.Commands
{
    public class UnlinkDoorCommand : IRocketCommand
    {
        private SellDoorPlugin pluginInstance => SellDoorPlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            string name = string.Empty;
            Transform transform = RaycastHelper.GetBarricadeTransform(player.Player, out BarricadeData barricadeData, out BarricadeDrop drop);

            if (transform == null)
            {
                transform = RaycastHelper.GetStructureTransform(player.Player, out StructureData structureData);
                if (transform != null)
                {
                    name = structureData.structure.asset.itemName;
                }
            }
            else
            {
                name = barricadeData.barricade.asset.itemName;
            }

            if (transform == null)
            {
                MessageHelper.Send(caller, "DoorItemNotLooking");
                return;
            }

            Door door = pluginInstance.DoorService.GetDoorFromItem(transform);

            if (door == null)
            {
                MessageHelper.Send(caller, "NotDoorItem", name);
                return;
            }

            pluginInstance.DoorService.RemoveDoorItem(door, transform);
            MessageHelper.Send(caller, "UnlinkDoorSuccess", name, door.Id);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "unlinkdoor";

        public string Help => "Unlinks the selected barricade or structure from the door";

        public string Syntax => "";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}
