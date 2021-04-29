using SDG.Framework.Utilities;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;

namespace RestoreMonarchy.SellDoor.Helpers
{
    public class RaycastHelper
    {
        public static Transform GetBarricadeTransform(Player player, out BarricadeData barricadeData, out BarricadeDrop drop)
        {
            barricadeData = null;
            drop = null;
            RaycastHit hit;
            if (PhysicsUtility.raycast(new Ray(player.look.aim.position, player.look.aim.forward), out hit, 3, RayMasks.BARRICADE_INTERACT))
            {
                Transform transform = hit.transform;
                InteractableDoorHinge doorHinge = hit.transform.GetComponent<InteractableDoorHinge>();
                if (doorHinge != null)
                {
                    transform = doorHinge.door.transform;
                }

                if (BarricadeManager.tryGetInfo(transform, out _, out _, out _, out ushort index, out BarricadeRegion region, out drop))
                {
                    barricadeData = region.barricades[index];
                    return region.drops[index].model;
                }
            }

            return null;
        }

        public static Transform GetStructureTransform(Player player, out StructureData structureData)
        {
            structureData = null;
            RaycastHit hit;
            if (PhysicsUtility.raycast(new Ray(player.look.aim.position, player.look.aim.forward), out hit, 3, RayMasks.STRUCTURE_INTERACT))
            {
                if (StructureManager.tryGetInfo(hit.transform, out _, out _, out ushort index, out StructureRegion region))
                {
                    structureData = region.structures[index];
                    return region.drops[index].model;
                }
            }

            return null;
        }

        public static Transform GetBarricadeTransform(Vector3 position)
        {
            List<Transform> list = new List<Transform>();
            List<RegionCoordinate> regions = new List<RegionCoordinate>();
            Regions.getRegionsInRadius(position, 0.1f, regions);
            BarricadeManager.getBarricadesInRadius(position, 1, regions, list);

            foreach (Transform transform in list)
            {
                if (transform.position == position)
                {
                    if (BarricadeManager.tryGetInfo(transform, out _, out _, out _, out ushort index, out BarricadeRegion region))
                    {
                        return region.drops[index].model;
                    }
                }
            }
            return null;
        }

        public static Transform GetStructureTransform(Vector3 position)
        {
            List<Transform> list = new List<Transform>();
            List<RegionCoordinate> regions = new List<RegionCoordinate>();
            Regions.getRegionsInRadius(position, 0.1f, regions);
            StructureManager.getStructuresInRadius(position, 0.1f, regions, list);

            foreach (Transform transform in list)
            {
                if (transform.position == position)
                {
                    if (StructureManager.tryGetInfo(transform, out _, out _, out ushort index, out StructureRegion region))
                    {
                        return region.drops[index].model;
                    }
                }
            }

            return null;
        }
    }
}
