using RestoreMonarchy.SellDoor.Helpers;
using RestoreMonarchy.SellDoor.Models;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;

namespace RestoreMonarchy.SellDoor.Commands
{
    public class LinkDoorCommand : IRocketCommand
    {
        private SellDoorPlugin pluginInstance => SellDoorPlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length < 1)
            {
                MessageHelper.Send(caller, "LinkDoorFormat");
                return;
            }
            
            if (!int.TryParse(command[0], out int doorId))
            {
                MessageHelper.Send(caller, "WrongDoorId", command[0]);
                return;
            }

            DoorItem doorItem = new();
            string name = string.Empty;
            ulong ownerId = 0;
            doorItem.Transform = RaycastHelper.GetBarricadeTransform(player.Player, out BarricadeData barricadeData, out BarricadeDrop drop);
            
            if (doorItem.Transform == null)
            {
                doorItem.Transform = RaycastHelper.GetStructureTransform(player.Player, out StructureData structureData);
                if (doorItem.Transform != null)
                {
                    ownerId = structureData.owner;
                    name = structureData.structure.asset.itemName;
                    doorItem.IsBarricade = false;
                    doorItem.AssetId = structureData.structure.asset.GUID;
                    doorItem.AssetName = structureData.structure.asset.itemName;
                }
            } else
            {
                ownerId = barricadeData.owner;
                name = barricadeData.barricade.asset.itemName;
                doorItem.IsBarricade = true;
                doorItem.AssetId = barricadeData.barricade.asset.GUID;
                doorItem.AssetName = barricadeData.barricade.asset.itemName;
            }

            if (doorItem.Transform == null)
            {
                MessageHelper.Send(caller, "DoorItemNotLooking");
                return;
            }

            if (ownerId != player.CSteamID.m_SteamID)
            {
                MessageHelper.Send(caller, "DoorItemNotOwner", name);
                return;
            }

            if (pluginInstance.DoorService.IsDoor(doorItem.Transform))
            {
                MessageHelper.Send(caller, "DoorItemAlready", name);
                return;
            }

            Door door = pluginInstance.DoorService.GetDoor(doorId);

            if (door == null)
            {
                MessageHelper.Send(caller, "DoorNotFound", doorId);
                return;
            }

            pluginInstance.DoorService.AddDoorItem(door, doorItem);
            MessageHelper.Send(caller, "LinkDoorSuccess", name, doorId);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "linkdoor";

        public string Help => "Links the selected barricade or structure to the door";

        public string Syntax => "<doorId>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}
