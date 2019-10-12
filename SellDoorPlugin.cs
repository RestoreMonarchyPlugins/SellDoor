using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using SellDoor.Models;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace SellDoor
{
    public class SellDoorPlugin : RocketPlugin<SellDoorConfiguration>
    {
        public Color MessageColor { get; private set; }
        public static SellDoorPlugin Instance { get; private set; }
        public Dictionary<Transform, DoorData> DoorsCache { get; set; }

        protected override void Load()
        {
            Instance = this;
            MessageColor = UnturnedChat.GetColorFromName(Configuration.Instance.MessageColor, Color.green);
            DoorsCache = new Dictionary<Transform, DoorData>();
            BarricadeManager.onDamageBarricadeRequested += OnDamageBarricadeRequested;


            Logger.Log($"{Name} {Assembly.GetName().Version} has been loaded!", ConsoleColor.Yellow);
        }

        private void OnDamageBarricadeRequested(CSteamID instigatorSteamID, Transform barricadeTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            if (!Configuration.Instance.IsAllowedToDestroy && DoorsCache.ContainsKey(barricadeTransform))
            {
                shouldAllow = false;
            }
        }

        protected override void Unload()
        {
            Logger.Log($"{Name} has been unloaded!", ConsoleColor.Yellow);
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            { "SellDoorFormat", "Use: /selldoor <price>" },
            { "SellDoorIncorrectPrice", "The price is not in correct format" },
            { "SellDoorNotFound",  "You are not pointing at any door" },
            { "SellDoorNotOwner", "You are not an owner of that door" },
            { "SellDoorSuccess", "Successfully put your door on sale for {0}" }
        };
    }
}
