using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Framework.Utilities;
using SDG.Unturned;
using SellDoor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SellDoor.Commands
{
    public class SellDoorCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "selldoor";

        public string Help => "Puts the door you are looking at on sale";

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
                UnturnedChat.Say(caller, SellDoorPlugin.Instance.Translate("SellDoorIncorrectPrice"), SellDoorPlugin.Instance.MessageColor);
                return;
            }

            UnturnedPlayer player = (UnturnedPlayer)caller;
            InteractableDoorHinge doorHinge;
            RaycastHit hit;

            if (PhysicsUtility.raycast(new Ray(player.Player.look.aim.position, player.Player.look.aim.forward), out hit, 4, RayMasks.BARRICADE_INTERACT)
                && (doorHinge = hit.transform.GetComponent<InteractableDoorHinge>()) != null)
            {
                if (doorHinge.door.owner == player.CSteamID)
                {
                    SellDoorPlugin.Instance.DoorsCache.Add(doorHinge.door.transform, new DoorData(player.CSteamID.m_SteamID, price,
                        new ConvertablePosition(player.Position)));
                    UnturnedChat.Say(caller, SellDoorPlugin.Instance.Translate("SellDoorSuccess", price.ToString("C")), SellDoorPlugin.Instance.MessageColor);
                } else
                {
                    UnturnedChat.Say(caller, SellDoorPlugin.Instance.Translate("SellDoorNotOwner"), SellDoorPlugin.Instance.MessageColor);
                }

            } else
            {
                UnturnedChat.Say(caller, SellDoorPlugin.Instance.Translate("SellDoorNotFound"), SellDoorPlugin.Instance.MessageColor);
            }
        }
    }
}
