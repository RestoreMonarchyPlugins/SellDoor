using HarmonyLib;
using RestoreMonarchy.SellDoor.Services;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using System;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace RestoreMonarchy.SellDoor
{
    public partial class SellDoorPlugin : RocketPlugin<SellDoorConfiguration>
    {
        public static SellDoorPlugin Instance { get; private set; }
        public Color MessageColor { get; private set; }

        public DoorService DoorService { get; private set; }
        public UIService UIService { get; private set; }

        public const string HarmonyId = "com.restoremonarchy.selldoor";
        private Harmony harmony;

        protected override void Load()
        {
            Instance = this;
            MessageColor = UnturnedChat.GetColorFromName(Configuration.Instance.MessageColor, Color.green);

            DoorService = gameObject.AddComponent<DoorService>();
            if (Configuration.Instance.EnableUI)
            {
                UIService = gameObject.AddComponent<UIService>();
            }

            harmony = new Harmony(HarmonyId);
            harmony.PatchAll(Assembly);

            Logger.Log($"{Name} {Assembly.GetName().Version} has been loaded!", ConsoleColor.Yellow);
        }

        protected override void Unload()
        {   
            Destroy(DoorService);
            if (UIService != null)
            {
                Destroy(UIService);
            }            

            Logger.Log($"{Name} has been unloaded!", ConsoleColor.Yellow);
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            { "DoorNotLooking", "You are not looking at any door" },
            { "DoorNotForSale", "This door is not on sale" },
            { "SellDoorFormat", "Use: /selldoor <price>" },
            { "SellDoorWrongPrice", "{0} is not a valid price" },
            { "SellDoorSuccess", "Successfully put door #{0} on sale for ${1}" },
            { "DoorNotOwner", "You are not an owner of that door" },
            { "CostDoorPrice", "You can buy this door for ${0}" },
            { "BuyDoorSuccess", "You successfully bought this door for ${0}" },
            { "BuyDoorCantAfford", "You can't afford to buy this door. It costs: ${0}" },
            { "BuyDoorLimit", "You can't own more than {0} doors!" },
            { "DoorAlreadyOnSale", "This door is already on sale" },
            { "DoorItemNotLooking", "You are not looking at any barricade or structure" },
            { "LinkDoorFormat", "Use: /linkdoor <doorId>" },
            { "WrongDoorId", "{0} is not a valid doorId" },
            { "DoorNotFound", "Door with Id {0} was not found" },
            { "DoorItemNotOwner", "You are not an owner of this {0}"},
            { "LinkDoorSuccess", "Successfully linked {0} with door #{1}" },
            { "CheckDoorSuccess", "This is door #{0} for {1} owned by {2}" },
            { "DoorItemAlready", "This {0} is already linked to door" },
            { "NotDoorItem", "This {0} is not linked to any door" },
            { "UnlinkDoorSuccess", "Successfully unlinked {0} with door #{1}" },
            { "SellDoorNoPermission", "You don't have permission to add a new door on sale" },
            { "DeleteDoorSucccess", "Successfully deleted door #{0} with {1} items" },

            { "SignPropertyOwner", "Property owned by {0}" },
            { "SignForSale", "For sale for ${0}" },
            { "SignNotForSale", "Not for sale" },

            { "UI_Title", "Door #{0}" },
            { "UI_Owner_Key", "Owner" },
            { "UI_Price_Key", "Price" },
            { "UI_BuyButton_Text", "Buy Property" },
            { "UI_SellButton_Text", "Abandon Property" },
            { "UI_Owner_Value_You", "You" },
            { "UI_Owner_Value_Unkown", "Dusk Property Group" },
            { "UI_Price_Value_NotForSale", "Not for sale" },
            { "UI_Price_Value", "${0}" }
        };
    }
}
