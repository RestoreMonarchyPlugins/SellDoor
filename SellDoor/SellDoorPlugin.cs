using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using RestoreMonarchy.SellDoor.Models;
using RestoreMonarchy.SellDoor.Utilities;
using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace RestoreMonarchy.SellDoor
{
    public class SellDoorPlugin : RocketPlugin<SellDoorConfiguration>
    {
        public static SellDoorPlugin Instance { get; private set; }
        public Color MessageColor { get; private set; }
        public Dictionary<Transform, DoorData> DoorsCache { get; set; }
        public DataManager DataManager { get; private set; }
        protected override void Load()
        {
            Instance = this;
            MessageColor = UnturnedChat.GetColorFromName(Configuration.Instance.MessageColor, Color.green);
            DoorsCache = new Dictionary<Transform, DoorData>();
            DataManager = new DataManager(Directory, "SellDoorData.json");

            Level.onLevelLoaded += (level) => LoadDoors();

            BarricadeManager.onDamageBarricadeRequested += OnDamageBarricadeRequest;
            SaveManager.onPreSave += SaveDoors;

            Logger.Log($"{Name} {Assembly.GetName().Version} has been loaded!", ConsoleColor.Yellow);
        }

        public void LoadDoors()
        {
            if (DataManager.ReadObject<List<DoorData>>(out List<DoorData> doors))
            {
                if (doors != null)
                {
                    foreach (var door in doors)
                    {
                        LoadDoor(door);
                    }
                }                
            } else
            {
                UnloadPlugin(Rocket.API.PluginState.Cancelled);
                return;
            }
            Logger.Log($"{DoorsCache.Count} doors have been loaded!", ConsoleColor.Yellow);
        }

        private void LoadDoor(DoorData door)
        {
            foreach (var region in BarricadeManager.regions)
            {
                for (int i = 0; i < region.drops.Count; i++)
                {
                    var model = region.drops[i].model;
                    if (model.position.x == door.Position.X && model.position.y == door.Position.Y && model.position.z == door.Position.Z)
                    {
                        DoorsCache.Add(region.drops[i].model, door);
                        return;
                    }
                }
            }
        }

        public void SaveDoors()
        {
            List<DoorData> doors = new List<DoorData>();
            foreach (var doorCache in DoorsCache)
            {
                if (doorCache.Key == null)
                {
                    DoorsCache.Remove(doorCache.Key);
                    continue;
                }

                doorCache.Value.Position = new ConvertablePosition(doorCache.Key.position);
                doors.Add(doorCache.Value);
            }
            DataManager.SaveObject(doors);
        }

        private void OnDamageBarricadeRequest(CSteamID instigatorSteamID, Transform barricadeTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            if (DoorsCache.ContainsKey(barricadeTransform))
            {
                shouldAllow = false;
            }
        }

        protected override void Unload()
        {
            BarricadeManager.onDamageBarricadeRequested -= OnDamageBarricadeRequest;
            SaveManager.onPreSave -= SaveDoors;
            Level.onLevelLoaded -= (level) => LoadDoors();
            Logger.Log($"{Name} has been unloaded!", ConsoleColor.Yellow);
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            { "DoorNotFound", "You are not pointing at any door" },
            { "DoorNotForSale", "This door is not on sale" },
            { "SellDoorFormat", "Use: /selldoor <price>" },
            { "SellDoorWrongPrice", "{0} is not a correct price" },
            { "SellDoorSuccess", "Successfully put your door on sale for {0}" },
            { "SellDoorNotOwner", "You are not an owner of that door" },
            { "CostDoorPrice", "You can buy this door for {0}" },
            { "BuyDoorSuccess", "You successfully bought this door for {0}" },
            { "BuyDoorCantAfford", "You can't afford to buy this door" }
        };
    }
}
