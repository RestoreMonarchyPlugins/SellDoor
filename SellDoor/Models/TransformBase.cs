using Newtonsoft.Json;
using RestoreMonarchy.SellDoor.Helpers;
using SDG.Unturned;
using Steamworks;
using System;
using UnityEngine;

namespace RestoreMonarchy.SellDoor.Models
{
    public abstract class TransformBase
    {
        public string AssetName { get; set; }
        public Guid? AssetId { get; set; }
        public ConvertableVector3 Position { get; set; }
        public ConvertableVector3 Rotation { get; set; }
        [JsonIgnore]
        public Transform Transform { get; set; }

        public void UpdatePosition()
        {
            if (Transform != null)
            {
                Position = ConvertableVector3.FromVector3(Transform.position);
                Rotation = ConvertableVector3.FromVector3(Transform.rotation.eulerAngles);

                if (AssetId == null)
                {
                    BarricadeDrop drop = BarricadeManager.FindBarricadeByRootTransform(Transform);
                    if (drop != null)
                    {
                        AssetId = drop.asset.GUID;
                        AssetName = drop.asset.itemName;
                    }
                    else
                    {
                        StructureDrop structureDrop = StructureManager.FindStructureByRootTransform(Transform);
                        if (drop != null)
                        {
                            AssetId = structureDrop.asset.GUID;
                            AssetName = structureDrop.asset.itemName;
                        }
                    }
                }
            }   
        }

        public bool Restore(ulong owner, ulong group)
        {
            if (Transform != null)
            {
                return false;
            }

            if (AssetId == null)
            {
                return false;
            }

            Asset asset = Assets.find(AssetId.Value);
            Vector3 position = Position.ToVector3();
            Vector3 rotation = Rotation.ToVector3();
            CSteamID ownerId = new CSteamID(owner);
            CSteamID groupId = new CSteamID(group);

            if (asset is ItemBarricadeAsset barricadeAsset)
            {
                Transform = BarricadeHelper.ForceDropBarricade(barricadeAsset, position, rotation, owner, group).model;                
                ChangeBarricadeOwner(ownerId, groupId);
            } else if (asset is ItemStructureAsset structureAsset)
            {
                Transform = StructureHelper.ForceDropStructure(structureAsset, position, rotation, owner, group).model;
                ChangeStructureOwner(ownerId, groupId);
            }

            return Transform != null;
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
