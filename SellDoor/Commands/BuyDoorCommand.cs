﻿using fr34kyn01535.Uconomy;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using RestoreMonarchy.SellDoor.Models;
using System.Collections.Generic;
using UnityEngine;
using RestoreMonarchy.SellDoor.Helpers;
using System.Linq;
using Rocket.Core;
using Logger = Rocket.Core.Logging.Logger;

namespace RestoreMonarchy.SellDoor.Commands
{
    public class BuyDoorCommand : IRocketCommand
    {
        private static SellDoorPlugin pluginInstance => SellDoorPlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            BuyDoor(player);
        }

        public static bool BuyDoor(UnturnedPlayer player)
        {
            Transform transform = RaycastHelper.GetBarricadeTransform(player.Player, out _, out BarricadeDrop drop);

            if (transform == null || (drop.interactable as InteractableDoor == null && drop.interactable as InteractableSign == null))
            {
                MessageHelper.Send(player, "DoorNotLooking");
                return false;
            }

            Door door = pluginInstance.DoorService.GetDoorOrItem(transform);

            if (door == null || door.IsSold)
            {
                MessageHelper.Send(player, "DoorNotForSale");
                return false;
            }

            int doorsCount = pluginInstance.DoorService.GetDoorsCount(player.Id);
            int maxDoors = pluginInstance.Configuration.Instance.DefaultMaxDoors;

            if (maxDoors != -1 && doorsCount >= maxDoors && !player.IsAdmin)
            {
                SellDoorLimit limit = pluginInstance.Configuration.Instance.Limits
                    .OrderByDescending(x => x.MaxDoors)
                    .FirstOrDefault(x => player.HasPermission(x.Permission));

                if (limit == null || limit.MaxDoors <= doorsCount)
                {
                    MessageHelper.Send(player, "BuyDoorLimit", doorsCount);
                    return false;
                }
            }

            if (!UconomyHelper.IsUconomyInstalled())
            {
                MessageHelper.Send(player, "You can't buy this door, because Uconomy plugin is not installed on this server.");
                Logger.LogError("Uconomy must be installed and loaded for the Sell Door plugin to work properly!");
                return false;
            }

            decimal balance = UconomyHelper.GetBalance(player.Id);

            if (balance < door.Price)
            {
                MessageHelper.Send(player, "BuyDoorCantAfford", door.PriceString);
                return false;
            }

            UconomyHelper.IncreaseBalance(player.Id, -door.Price);

            if (!string.IsNullOrEmpty(door.OwnerId))
            {
                UconomyHelper.IncreaseBalance(door.OwnerId, door.Price);
            }

            pluginInstance.DoorService.BuyDoor(door, player.Player);

            MessageHelper.Send(player, "BuyDoorSuccess", door.PriceString);

            return true;
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "buydoor";

        public string Help => "Buys the door player is looking at";

        public string Syntax => string.Empty;

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}
