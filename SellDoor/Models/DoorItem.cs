﻿using SDG.Unturned;
using Steamworks;

namespace RestoreMonarchy.SellDoor.Models
{
    public class DoorItem : TransformBase
    {
        public bool IsBarricade { get; set; }

        public void ChangeTransformOwner(CSteamID steamID, CSteamID groupID)
        {
            if (Transform == null)
                return;

            if (IsBarricade)
            {
                ChangeBarricadeOwner(steamID, groupID);
            }                
            else
            {
                ChangeStructureOwner(steamID, groupID);
            }                
        }

        public void UpdateSign(string text)
        {
            if (Transform == null)
            {
                return;
            }                

            BarricadeDrop drop = BarricadeManager.FindBarricadeByRootTransform(Transform);
            if (drop == null)
            {
                return;
            }

            InteractableSign interactableSign = drop.interactable as InteractableSign;
            if (interactableSign == null)
            {
                return;
            }   

            BarricadeManager.ServerSetSignText(interactableSign, text);
        }
    }
}
