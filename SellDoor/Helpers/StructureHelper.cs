using SDG.Unturned;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RestoreMonarchy.SellDoor.Helpers
{
    public class StructureHelper
    {
        public static StructureDrop FindStructureDropByPosition(Guid assetGuid, Vector3 point)
        {
            List<RegionCoordinate> regions = new();
            Regions.getRegionsInRadius(point, 0.1f, regions);

            List<Transform> results = new();
            StructureManager.getStructuresInRadius(point, 0.1f, regions, results);

            foreach (Transform result in results)
            {
                StructureDrop structureDrop = StructureManager.FindStructureByRootTransform(result);
                StructureData structureData = structureDrop.GetServersideData();

                if (structureData.point == point && structureDrop.asset.GUID == assetGuid)
                {
                    return structureDrop;
                }
            }

            return null;
        }

        public static StructureDrop ForceDropStructure(ItemStructureAsset asset, Vector3 point, Vector3 angle, ulong owner, ulong group)
        {
            Structure structure = new(asset, asset.health);
            Quaternion rotation = Quaternion.Euler(angle.x, angle.y, angle.z);

            StructureManager.dropReplicatedStructure(structure, point, rotation, owner, group);

            return FindStructureDropByPosition(asset.GUID, point);
        }
    }
}
