using Newtonsoft.Json;
using SDG.Unturned;
using Steamworks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RestoreMonarchy.SellDoor.Models
{
    public class Door : TransformBase
    {
        public Door() { }

        public int Id { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public decimal Price { get; set; }
        public bool IsSold { get; set; }

        public List<DoorItem> Items { get; set; }

        [JsonIgnore]
        public string PriceString => Price.ToString("N");

        public void UpdateSigns()
        {
            foreach (DoorItem item in Items)
            {
                item.UpdateSign(string.Empty);                    
            }
        }

        public DoorItem GetDoorItem(Transform transform)
        {
            return Items.FirstOrDefault(i => i.Transform == transform);
        }

        public bool TryGetDoorOwners(out CSteamID steamID, out CSteamID groupID)
        {
            steamID = CSteamID.Nil;
            groupID = CSteamID.Nil;
            BarricadeDrop drop = BarricadeManager.FindBarricadeByRootTransform(Transform);
            if (drop == null)
                return false;

            steamID = new CSteamID(drop.GetServersideData().owner);
            groupID = new CSteamID(drop.GetServersideData().group);
            return true;
        }
    }
}
