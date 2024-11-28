using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RestoreMonarchy.SellDoor.Helpers
{
    public static class BarricadeHelper
    {
        public static BarricadeDrop FindBarricadeDrop(Guid assetGuid, Vector3 point)
        {
            List<RegionCoordinate> regions = new();
            Regions.getRegionsInRadius(point, 0.1f, regions);

            List<Transform> results = new();
            BarricadeManager.getBarricadesInRadius(point, 0.1f, regions, results);

            foreach (Transform result in results)
            {
                BarricadeDrop barricadeDrop = BarricadeManager.FindBarricadeByRootTransform(result);
                BarricadeData barricadeData = barricadeDrop.GetServersideData();

                if (barricadeData.point == point && barricadeDrop.asset.GUID == assetGuid)
                {
                    return barricadeDrop;
                }
            }

            return null;
        }

        public static BarricadeDrop ForceDropBarricade(ItemBarricadeAsset asset, Vector3 point, Vector3 angle, ulong owner, ulong group)
        {
            byte[] state = asset.getState(true);
            Barricade barricade = new(asset, asset.health, state);
            Quaternion rotation = Quaternion.Euler(angle.x, angle.y, angle.z);
            //Quaternion rotation = BarricadeManager.getRotation(barricade.asset, angleX, angleY, angleZ);

            Transform transform = BarricadeManager.dropNonPlantedBarricade(barricade, point, rotation, owner, group);

            return BarricadeManager.FindBarricadeByRootTransform(transform);
        }  
    }
}
