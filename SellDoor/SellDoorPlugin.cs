using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using System;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;
using RestoreMonarchy.SellDoor.Services;

namespace RestoreMonarchy.SellDoor
{
    public class SellDoorPlugin : RocketPlugin<SellDoorConfiguration>
    {
        public static SellDoorPlugin Instance { get; private set; }
        public Color MessageColor { get; private set; }

        public DoorService DoorService { get; private set; }

        protected override void Load()
        {
            Instance = this;
            MessageColor = UnturnedChat.GetColorFromName(Configuration.Instance.MessageColor, Color.green);

            DoorService = gameObject.AddComponent<DoorService>();

            Logger.Log($"{Name} {Assembly.GetName().Version} has been loaded!", ConsoleColor.Yellow);
        }

        protected override void Unload()
        {
            Destroy(DoorService);
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
            { "BuyDoorSuccess", "You successfully bought this door for {0}" },
            { "BuyDoorCantAfford", "You can't afford to buy this door. It costs: ${0}" },
            { "DoorAlreadyOnSale", "This door is already on sale" },
            { "DoorItemNotLooking", "You are not looking at any barricade or structure" },
            { "LinkDoorFormat", "Use: /linkdoor <doorId>" },
            { "LinkDoorWrongDoorId", "{0} is not a valid doorId" },
            { "DoorNotFound", "Door with Id {0} was not found" },
            { "DoorItemNotOwner", "You are not an owner of this {0}"},
            { "LinkDoorSuccess", "Successfully linked {0} with door #{1}" },
            { "CheckDoorSuccess", "This is door #{0} for {1} owned by {2}" }
        };
    }
}
