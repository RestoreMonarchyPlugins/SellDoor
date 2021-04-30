using RestoreMonarchy.SellDoor.Extensions;
using RestoreMonarchy.SellDoor.Models;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;

namespace RestoreMonarchy.SellDoor.Services
{
    public partial class DoorService
    {
        public void UpdateDoorPrice(Door door, decimal price, Player player)
        {
            door.OwnerName = player.DisplayName();
            door.IsSold = false;
            door.Price = price;
            door.UpdateSigns();
        }

        public void BuyDoor(Door door, Player player)
        {
            door.OwnerId = player.ID();
            door.OwnerName = player.DisplayName();
            door.IsSold = true;

            door.ChangeBarricadeOwner(player.CSteamID(), player.GroupID());            

            foreach (DoorItem item in door.Items)
            {
                if (item.Transform == null)
                    continue;

                item.ChangeTransformOwner(player.CSteamID(), player.GroupID());
            }

            door.UpdateSigns();

            database.Save();
        }

        public Door SellDoor(Transform transform, decimal price, Player player)
        {
            Door door = new Door()
            {
                Price = price,
                OwnerId = player.ID(),
                OwnerName = player.DisplayName(),
                IsSold = false,
                Items = new List<DoorItem>(),
                Transform = transform
            };

            database.AddDoor(door);
            return door;
        }

        
    }
}
