using fr34kyn01535.Uconomy;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Framework.Utilities;
using SDG.Unturned;
using RestoreMonarchy.SellDoor.Models;
using System.Collections.Generic;
using UnityEngine;
using RestoreMonarchy.SellDoor.Helpers;

namespace RestoreMonarchy.SellDoor.Commands
{
    public class BuyDoorCommand : IRocketCommand
    {
        private SellDoorPlugin pluginInstance => SellDoorPlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            Transform transform = RaycastHelper.GetBarricadeTransform(player.Player, out BarricadeData barricadeData);

            if (transform == null || barricadeData.barricade.asset.build != EBuild.DOOR)
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

            decimal balance = Uconomy.Instance.Database.GetBalance(caller.Id);

            if (balance < door.Price)
            {
                MessageHelper.Send(caller, "BuyDoorCantAfford", door.PriceString);
                return;
            }

            Uconomy.Instance.Database.IncreaseBalance(caller.Id, -door.Price);
            Uconomy.Instance.Database.IncreaseBalance(door.OwnerId, door.Price);
            pluginInstance.DoorService.BuyDoor(door, player.Player);

            MessageHelper.Send(caller, "BuyDoorSuccess", door.PriceString);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "buydoor";

        public string Help => "Buy the door you are pointing at";

        public string Syntax => string.Empty;

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}
