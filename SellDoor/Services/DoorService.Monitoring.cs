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

        private void OnSalvageStructureRequested(CSteamID steamID, byte x, byte y, ushort index, ref bool shouldAllow)
        {
            if (!StructureManager.tryGetRegion(x, y, out StructureRegion region))
                return;

            if (GetDoorOrItem(region.drops[index].model) != null)
                shouldAllow = false;
        }

        private void OnSalvageBarricadeRequested(CSteamID steamID, byte x, byte y, ushort plant, ushort index, ref bool shouldAllow)
        {
            if (!BarricadeManager.tryGetRegion(x, y, plant, out BarricadeRegion region))
                return;

            if (GetDoorOrItem(region.drops[index].model) != null)
                shouldAllow = false;
        }
    }
}
