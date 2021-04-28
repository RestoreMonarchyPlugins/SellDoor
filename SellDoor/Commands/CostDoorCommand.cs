using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Framework.Utilities;
using SDG.Unturned;
using RestoreMonarchy.SellDoor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RestoreMonarchy.SellDoor.Commands
{
    public class CostDoorCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "costdoor";

        public string Help => "Checks the door's you are point at price";

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
                    UnturnedChat.Say(caller, SellDoorPlugin.Instance.Translate("CostDoorPrice", doorData.Price.ToString("C")), SellDoorPlugin.Instance.MessageColor);
                } else
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
