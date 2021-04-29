using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using RestoreMonarchy.SellDoor.Models;
using System.Collections.Generic;
using UnityEngine;
using RestoreMonarchy.SellDoor.Helpers;

namespace RestoreMonarchy.SellDoor.Commands
{
    public class SellDoorCommand : IRocketCommand
    {
        private SellDoorPlugin pluginInstance => SellDoorPlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length < 1)
            {
                MessageHelper.Send(caller, "SellDoorFormat");
                return;
            }

            if (!decimal.TryParse(command[0], out decimal price))
            {
                MessageHelper.Send(caller, "SellDoorWrongPrice", command[0]);
                return;
            }

            UnturnedPlayer player = (UnturnedPlayer)caller;

            Transform transform = RaycastHelper.GetBarricadeTransform(player.Player, out BarricadeData barricadeData);

            if (transform == null || barricadeData.barricade.asset.build != EBuild.DOOR)
            {
                MessageHelper.Send(caller, "DoorNotLooking");
                return;
            }

            if (barricadeData.owner != player.CSteamID.m_SteamID)
            {
                MessageHelper.Send(caller, "DoorNotOwner");
                return;
            }

            Door door = pluginInstance.DoorService.GetDoor(transform);

            if (door != null)
            {
                MessageHelper.Send(caller, "DoorAlreadyOnSale");
                return;
            }

            door = pluginInstance.DoorService.SellDoor(transform, price, player.Player);
            MessageHelper.Send(caller, "SellDoorSuccess", door.Id, door.PriceString);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "selldoor";

        public string Help => "Puts the door player is pointing at on sale";

        public string Syntax => "<price>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();        
    }
}
