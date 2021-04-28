using fr34kyn01535.Uconomy;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Framework.Utilities;
using SDG.Unturned;
using RestoreMonarchy.SellDoor.Models;
using RestoreMonarchy.SellDoor.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace RestoreMonarchy.SellDoor.Commands
{
    public class BuyDoorCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "buydoor";

        public string Help => "Buy the door you are pointing at";

        public string Syntax => string.Empty;

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            InteractableDoorHinge doorHinge;

            if (PhysicsUtility.raycast(new Ray(player.Player.look.aim.position, player.Player.look.aim.forward), out RaycastHit hit,
                4, RayMasks.BARRICADE_INTERACT) && (doorHinge = hit.transform.GetComponent<InteractableDoorHinge>()) != null)
            {
                if (SellDoorPlugin.Instance.DoorsCache.TryGetValue(doorHinge.door.transform, out DoorData doorData))
                {
                    decimal balance = Uconomy.Instance.Database.GetBalance(caller.Id);
                    
                    if (balance < doorData.Price)
                    {
                        UnturnedChat.Say(caller, SellDoorPlugin.Instance.Translate("BuyDoorCantAfford"), SellDoorPlugin.Instance.MessageColor);
                    } else
                    {
                        Uconomy.Instance.Database.IncreaseBalance(caller.Id, doorData.Price * -1);
                        Uconomy.Instance.Database.IncreaseBalance(doorData.SellerID.ToString(), doorData.Price);
                        UnturnedUtility.ChangeBarricadeOwner(doorHinge.door.transform, player.CSteamID.m_SteamID, player.SteamGroupID.m_SteamID);
                        SellDoorPlugin.Instance.DoorsCache.Remove(doorHinge.door.transform);
                        UnturnedChat.Say(caller, SellDoorPlugin.Instance.Translate("BuyDoorSuccess", doorData.Price.ToString("C")), SellDoorPlugin.Instance.MessageColor);
                    }
                }
                else
                {
                    UnturnedChat.Say(caller, SellDoorPlugin.Instance.Translate("DoorNotForSale"), SellDoorPlugin.Instance.MessageColor);
                }
            }
            else
            {
                UnturnedChat.Say(caller, SellDoorPlugin.Instance.Translate("DoorNotFound"), SellDoorPlugin.Instance.MessageColor);
            }
        }
    }
}
