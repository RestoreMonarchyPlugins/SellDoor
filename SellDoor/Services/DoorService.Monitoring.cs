using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace RestoreMonarchy.SellDoor.Services
{
    public partial class DoorService
    {
        private void OnDamageStructureRequested(CSteamID instigatorSteamID, Transform structureTransform, 
            ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            if (GetDoorOrItem(structureTransform) != null)
                shouldAllow = false;
        }

        private void OnDamageBarricadeRequested(CSteamID instigatorSteamID, Transform barricadeTransform, 
            ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            if (GetDoorOrItem(barricadeTransform) != null)
                shouldAllow = false;
        }

        private void OnSalvageStructureRequested(StructureDrop structure, SteamPlayer instigatorClient, ref bool shouldAllow)
        {
            if (GetDoorOrItem(structure.model) != null)
                shouldAllow = false;
        }

        private void OnSalvageBarricadeRequested(BarricadeDrop barricade, SteamPlayer instigatorClient, ref bool shouldAllow)
        {
            if (GetDoorOrItem(barricade.model) != null)
                shouldAllow = false;
        }
    }
}
