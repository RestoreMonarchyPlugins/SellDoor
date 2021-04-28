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
    public class SellDoorCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "selldoor";

        public string Help => "Puts the door player is pointing at on sale";

        public string Syntax => "<price>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length < 1)
            {
                UnturnedChat.Say(caller, SellDoorPlugin.Instance.Translate("SellDoorFormat"), SellDoorPlugin.Instance.MessageColor);
                return;
            }

            if (!decimal.TryParse(command[0], out decimal price))
            {
                UnturnedChat.Say(caller, SellDoorPlugin.Instance.Translate("SellDoorWrongPrice", command[0]), SellDoorPlugin.Instance.MessageColor);
                return;
            }

            UnturnedPlayer player = (UnturnedPlayer)caller;
            InteractableDoorHinge doorHinge;

            if (PhysicsUtility.raycast(new Ray(player.Player.look.aim.position, player.Player.look.aim.forward), out RaycastHit hit, 
                4, RayMasks.BARRICADE_INTERACT) && (doorHinge = hit.transform.GetComponent<InteractableDoorHinge>()) != null)
            {
                if (doorHinge.door.owner == player.CSteamID)
                {
                    UnturnedUtility.ChangeBarricadeOwner(doorHinge.door.transform, 0, 0);
                    SellDoorPlugin.Instance.DoorsCache.Add(doorHinge.door.transform, new DoorData(player.CSteamID.m_SteamID, price));
                    UnturnedChat.Say(caller, SellDoorPlugin.Instance.Translate("SellDoorSuccess", price.ToString("C")), SellDoorPlugin.Instance.MessageColor);
                }
                else
                {
                    UnturnedChat.Say(caller, SellDoorPlugin.Instance.Translate("SellDoorNotOwner"), SellDoorPlugin.Instance.MessageColor);
                    return;
                }
            } else
            {
                UnturnedChat.Say(caller, SellDoorPlugin.Instance.Translate("SellDoorNotFound"), SellDoorPlugin.Instance.MessageColor);
            }
        }
    }
}
