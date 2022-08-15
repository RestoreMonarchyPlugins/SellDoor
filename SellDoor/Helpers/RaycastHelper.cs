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
            Ray ray = new Ray(player.look.aim.position, player.look.aim.forward);
            if (Physics.Raycast(ray, out hit, 3, RayMasks.BARRICADE_INTERACT))
            {
                Transform transform = hit.transform;
                InteractableDoorHinge doorHinge = hit.transform.GetComponent<InteractableDoorHinge>();
                if (doorHinge != null)
                {
                    transform = doorHinge.door.transform;
                }

                drop = BarricadeManager.FindBarricadeByRootTransform(transform);
                if (drop != null)
                {
                    barricadeData = drop.GetServersideData();
                    return drop.model;
                }
            }

            return null;
        }

        public static Transform GetStructureTransform(Player player, out StructureData structureData)
        {
            structureData = null;
            RaycastHit hit;
            Ray ray = new Ray(player.look.aim.position, player.look.aim.forward);
            if (Physics.Raycast(ray, out hit, 3, RayMasks.STRUCTURE_INTERACT))
            {
                StructureDrop drop = StructureManager.FindStructureByRootTransform(hit.transform);
                if (drop != null)
                {
                    structureData = drop.GetServersideData();
                    return drop.model;
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
                    return BarricadeManager.FindBarricadeByRootTransform(transform)?.model ?? null;
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
                    return StructureManager.FindStructureByRootTransform(transform)?.model ?? null;
                }
            }

            return null;
        }
    }
}
