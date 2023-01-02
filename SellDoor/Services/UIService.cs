using fr34kyn01535.Uconomy;
using RestoreMonarchy.SellDoor.Commands;
using RestoreMonarchy.SellDoor.Extensions;
using RestoreMonarchy.SellDoor.Helpers;
using RestoreMonarchy.SellDoor.Models;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using UnityEngine;

namespace RestoreMonarchy.SellDoor.Services
{
    public class UIService : MonoBehaviour
    {
        private SellDoorPlugin pluginInstance => SellDoorPlugin.Instance;

        private DoorService doorService => pluginInstance.DoorService;

        private const ushort EffectId = 54600;
        private const short EffectKey = 27300;

        void Awake()
        {

        }

        void Start()
        {
            UnturnedPlayerEvents.OnPlayerUpdateGesture += OnPlayerUpdateGesture;
            EffectManager.onEffectButtonClicked += OnEffectButtonClicked;
        }

        void OnDestroy()
        {
            UnturnedPlayerEvents.OnPlayerUpdateGesture -= OnPlayerUpdateGesture;
            EffectManager.onEffectButtonClicked -= OnEffectButtonClicked;
        }

        private void OnPlayerUpdateGesture(UnturnedPlayer unturnedPlayer, UnturnedPlayerEvents.PlayerGesture gesture)
        {
            Player player = unturnedPlayer.Player;

            if (gesture != UnturnedPlayerEvents.PlayerGesture.PunchRight)
            {
                return;
            }

            Transform transform = RaycastHelper.GetBarricadeTransform(player, out _, out BarricadeDrop drop);

            if (transform == null || drop.interactable as InteractableDoor == null)
            {
                return;
            }

            Door door = doorService.GetDoor(transform);

            if (door == null)
            {
                return;
            }

            string text1 = door.OwnerId != null ? door.OwnerName : "N/A";
            string text2 = door.IsSold ? "N/A" : "$" + door.PriceString;

            EffectManager.sendUIEffect(EffectId, EffectKey, player.TransportConnection(), true, text1, text2);

            if (door.OwnerId.Equals(unturnedPlayer.Id))
            {
                EffectManager.sendUIEffectVisibility(EffectKey, player.TransportConnection(), true, "LP", true);
            }

            if (!door.OwnerId.Equals(unturnedPlayer.Id) && !door.IsSold)
            {
                EffectManager.sendUIEffectVisibility(EffectKey, player.TransportConnection(), true, "BP", true);
            }

            player.enablePluginWidgetFlag(EPluginWidgetFlags.Modal);
        }

        private void OnEffectButtonClicked(Player player, string buttonName)
        {
            MessageHelper.Send(buttonName);
            switch (buttonName)
            {
                case "LP":
                    HandleLPButtonClick(player);
                    break;
                case "BP":
                    HandleBPButtonClick(player);
                    break;
                case "Close":
                    HandleCloseButtonClick(player);
                    break;
            }
        }

        private void HandleLPButtonClick(Player player)
        {
            MessageHelper.Send("HandleLPButtonClick");

            Transform transform = RaycastHelper.GetBarricadeTransform(player, out _, out BarricadeDrop drop);

            if (transform == null || drop.interactable as InteractableDoor == null)
            {
                MessageHelper.Send("You are not looking at any door");
                return;
            }

            Door door = doorService.GetDoor(transform);

            door.OwnerId = null;
            door.IsSold = false;
            door.OwnerName = "N/A";

            door.ChangeBarricadeOwner(CSteamID.Nil, CSteamID.Nil);

            foreach (DoorItem item in door.Items)
            {
                item.ChangeTransformOwner(CSteamID.Nil, CSteamID.Nil);
            }

            EffectManager.askEffectClearByID(EffectId, player.TransportConnection());
            player.disablePluginWidgetFlag(EPluginWidgetFlags.Modal);
        }

        private void HandleBPButtonClick(Player player)
        {
            MessageHelper.Send("HandleBPButtonClick");

            UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromPlayer(player);

            if (BuyDoorCommand.BuyDoor(unturnedPlayer))
            {
                EffectManager.askEffectClearByID(EffectId, player.TransportConnection());
                player.disablePluginWidgetFlag(EPluginWidgetFlags.Modal);
            }
        }

        private void HandleCloseButtonClick(Player player)
        {
            MessageHelper.Send("HandleCloseButtonClick");

            EffectManager.askEffectClearByID(EffectId, player.TransportConnection());
            player.disablePluginWidgetFlag(EPluginWidgetFlags.Modal);
        }
    }
}
