using RestoreMonarchy.SellDoor.Commands;
using RestoreMonarchy.SellDoor.Extensions;
using RestoreMonarchy.SellDoor.Helpers;
using RestoreMonarchy.SellDoor.Models;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace RestoreMonarchy.SellDoor.Services
{
    public class UIService : MonoBehaviour
    {
        private SellDoorPlugin pluginInstance => SellDoorPlugin.Instance;

        private DoorService doorService => pluginInstance.DoorService;

        private ushort EffectId => pluginInstance.Configuration.Instance.EffectId;
        private const short EffectKey = 27300;

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

            if (gesture != UnturnedPlayerEvents.PlayerGesture.PunchRight 
                && gesture != UnturnedPlayerEvents.PlayerGesture.PunchLeft
                && gesture != UnturnedPlayerEvents.PlayerGesture.Point)
            {
                return;
            }

            Transform transform = RaycastHelper.GetBarricadeTransform(player, out _, out BarricadeDrop drop);

            if (transform == null || (drop.interactable as InteractableDoor == null && drop.interactable as InteractableSign == null))
            {
                return;
            }

            Door door = doorService.GetDoorOrItem(transform);

            if (door == null)
            {
                return;
            }

            EffectManager.sendUIEffect(EffectId, EffectKey, player.TransportConnection(), true);

            EffectManager.sendUIEffectVisibility(EffectKey, player.TransportConnection(), true, "SellDoorUI", false);
            EffectManager.sendUIEffectVisibility(EffectKey, player.TransportConnection(), true, "SellDoorUI_BuyButton", false);
            EffectManager.sendUIEffectVisibility(EffectKey, player.TransportConnection(), true, "SellDoorUI_SellButton", false);

            EffectManager.sendUIEffectText(EffectKey, player.TransportConnection(), true, "Title", pluginInstance.Translate("UI_Title", door.Id));
            EffectManager.sendUIEffectText(EffectKey, player.TransportConnection(), true, "Owner_Key", pluginInstance.Translate("UI_Owner_Key"));
            EffectManager.sendUIEffectText(EffectKey, player.TransportConnection(), true, "Price_Key", pluginInstance.Translate("UI_Price_Key"));

            EffectManager.sendUIEffectText(EffectKey, player.TransportConnection(), true, "BuyButton_Text", pluginInstance.Translate("UI_BuyButton_Text"));
            EffectManager.sendUIEffectText(EffectKey, player.TransportConnection(), true, "SellButton_Text", pluginInstance.Translate("UI_SellButton_Text"));
            
            if (string.IsNullOrEmpty(door.OwnerId))
            {
                EffectManager.sendUIEffectText(EffectKey, player.TransportConnection(), true, "Owner_Value", pluginInstance.Translate("UI_Owner_Value_Unkown"));
            } else if (door.OwnerId == unturnedPlayer.Id)
            {
                EffectManager.sendUIEffectText(EffectKey, player.TransportConnection(), true, "Owner_Value", pluginInstance.Translate("UI_Owner_Value_You"));
                EffectManager.sendUIEffectVisibility(EffectKey, player.TransportConnection(), true, "SellDoorUI_SellButton", true);
            } else
            {
                
                EffectManager.sendUIEffectText(EffectKey, player.TransportConnection(), true, "Owner_Value", door.OwnerName);
            }

            if (door.IsSold)
            {
                EffectManager.sendUIEffectText(EffectKey, player.TransportConnection(), true, "Price_Value", pluginInstance.Translate("UI_Price_Value_NotForSale"));
            } else
            {
                if (door.OwnerId != unturnedPlayer.Id)
                {
                    EffectManager.sendUIEffectVisibility(EffectKey, player.TransportConnection(), true, "SellDoorUI_BuyButton", true);
                }

                EffectManager.sendUIEffectText(EffectKey, player.TransportConnection(), true, "Price_Value", pluginInstance.Translate("UI_Price_Value", door.PriceString));
            }

            EffectManager.sendUIEffectVisibility(EffectKey, player.TransportConnection(), true, "SellDoorUI", true);

            player.enablePluginWidgetFlag(EPluginWidgetFlags.Modal);
            player.disablePluginWidgetFlag(EPluginWidgetFlags.ShowCenterDot);
        }

        private void OnEffectButtonClicked(Player player, string buttonName)
        {
            switch (buttonName)
            {
                case "SellDoorUI_SellButton":
                    HandleSellButtonClick(player);
                    break;
                case "SellDoorUI_BuyButton":
                    HandleBuyButtonClick(player);
                    break;
                case "SellDoorUI_CloseButton":
                    HandleCloseButtonClick(player);
                    break;
            }
        }

        private void HandleSellButtonClick(Player player)
        {
            UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromPlayer(player);
            Transform transform = RaycastHelper.GetBarricadeTransform(player, out _, out BarricadeDrop drop);

            if (transform == null || drop.interactable as InteractableDoor == null)
            {
                MessageHelper.Send(unturnedPlayer, "DoorNotLooking");
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
            
            door.UpdateSigns();

            EffectManager.askEffectClearByID(EffectId, player.TransportConnection());
            player.disablePluginWidgetFlag(EPluginWidgetFlags.Modal);
            player.enablePluginWidgetFlag(EPluginWidgetFlags.ShowCenterDot);
        }

        private void HandleBuyButtonClick(Player player)
        {
            UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromPlayer(player);

            BuyDoorCommand.BuyDoor(unturnedPlayer, () =>
            {
                EffectManager.askEffectClearByID(EffectId, player.TransportConnection());
                player.disablePluginWidgetFlag(EPluginWidgetFlags.Modal);
                player.enablePluginWidgetFlag(EPluginWidgetFlags.ShowCenterDot);
            });
        }

        private void HandleCloseButtonClick(Player player)
        {
            EffectManager.askEffectClearByID(EffectId, player.TransportConnection());
            player.disablePluginWidgetFlag(EPluginWidgetFlags.Modal);
            player.enablePluginWidgetFlag(EPluginWidgetFlags.ShowCenterDot);
        }
    }
}
