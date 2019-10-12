using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SellDoor.Utilities
{
    public class UnturnedUtility
    {
        public static void ChangeBarricadeOwner(Transform barricadeTransform, ulong newOwner, ulong newOwnerGroup)
        {
            byte[] arr = new byte[17];
            BitConverter.GetBytes(newOwner).CopyTo(arr, 0);
            BitConverter.GetBytes(newOwnerGroup).CopyTo(arr, 8);
            BitConverter.GetBytes(false).CopyTo(arr, 16);
            BarricadeManager.updateReplicatedState(barricadeTransform, arr, 17);
            BarricadeManager.changeOwnerAndGroup(barricadeTransform, newOwner, newOwnerGroup);
        }
    }
}