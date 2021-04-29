using HarmonyLib;
using RestoreMonarchy.SellDoor.Models;
using SDG.Unturned;

namespace RestoreMonarchy.SellDoor.Patches
{
    [HarmonyPatch(typeof(BarricadeManager))]
    class BarricadeManagerPatches
    {
        [HarmonyPatch("ServerSetSignTextInternal")]
        [HarmonyPrefix]
        static bool ServerSetSignTextInternalPrefix(InteractableSign sign, ref string trimmedText)
        {
            Door door = SellDoorPlugin.Instance.DoorService.GetDoorOrItem(sign.transform);

            if (door != null)
            {
                trimmedText = SellDoorPlugin.Instance.DoorService.GetSignText(door);
            }

            return true;
        }
    }
}
