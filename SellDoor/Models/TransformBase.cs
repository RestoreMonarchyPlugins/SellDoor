using Newtonsoft.Json;
using SDG.Unturned;
using Steamworks;
using System;
using UnityEngine;

namespace RestoreMonarchy.SellDoor.Models
{
    public abstract class TransformBase
    {
        public ConvertableVector3 Position { get; set; }
        [JsonIgnore]
        public Transform Transform { get; set; }

        public void UpdatePosition()
        {
            if (Transform != null)
                Position = ConvertableVector3.FromVector3(Transform.position);
        }

        public void ChangeBarricadeOwner(CSteamID steamID, CSteamID groupID)
        {
            byte[] arr = new byte[17];
            BitConverter.GetBytes(steamID.m_SteamID).CopyTo(arr, 0);
            BitConverter.GetBytes(groupID.m_SteamID).CopyTo(arr, 8);
            BitConverter.GetBytes(false).CopyTo(arr, 16);
            BarricadeManager.updateReplicatedState(Transform, arr, 17);
            BarricadeManager.changeOwnerAndGroup(Transform, steamID.m_SteamID, groupID.m_SteamID);
        }

        public void ChangeStructureOwner(CSteamID steamID, CSteamID groupID)
        {
            StructureManager.changeOwnerAndGroup(Transform, steamID.m_SteamID, groupID.m_SteamID);
        }
    }
}
