using RestoreMonarchy.SellDoor.Helpers;
using RestoreMonarchy.SellDoor.Models;
using Steamworks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace RestoreMonarchy.SellDoor.Services
{
    public partial class DoorService
    {
        private void LoadDoors(int i)
        {
            database.Reload();

            foreach (Door door in database.Doors)
            {
                Vector3 pos = door.Position.ToVector3();
                door.Transform = RaycastHelper.GetBarricadeTransform(pos);

                if (door.Transform == null)
                {
                    Logger.LogWarning($"Door #{door.Id} transform not found");
                }

                foreach (DoorItem item in door.Items)
                {
                    pos = item.Position.ToVector3();

                    if (item.IsBarricade)
                        item.Transform = RaycastHelper.GetBarricadeTransform(pos);
                    else
                        item.Transform = RaycastHelper.GetStructureTransform(pos);

                    if (item.Transform == null)
                    {
                        Logger.LogWarning($"Door #{door.Id} item transform not found");
                    }
                }
            }
        }

        private void SaveDoors()
        {
            database.Save();
        }

        public int GetDoorsCount(string steamId)
        {
            return database.Doors.Count(x => x.OwnerId == steamId);
        }

        public Door GetDoor(int doorId)
        {
            return database.Doors.FirstOrDefault(x => x.Id == doorId);
        }

        public Door GetDoor(Transform transform)
        {
            return database.Doors.FirstOrDefault(d => d.Transform == transform);
        }

        public Door GetDoorOrItem(Transform transform)
        {
            return database.Doors.FirstOrDefault(d => d.Transform == transform || d.Items.Exists(i => i.Transform == transform));
        }

        public Door GetDoorFromItem(Transform transform)
        {
            return database.Doors.FirstOrDefault(d => d.Items.Exists(i => i.Transform == transform));
        }

        public bool IsDoor(Transform transform)
        {
            if (database.Doors.Any(d => d.Transform == transform))
                return true;

            return database.Doors.Any(d => d.Items.Any(i => i.Transform == transform));
        }

        public void AddDoorItem(Door door, DoorItem item)
        {
            if (door.Items == null)
                door.Items = new List<DoorItem>();

            door.Items.Add(item);
            if (door.TryGetDoorOwners(out CSteamID steamID, out CSteamID groupID))
            {
                item.ChangeTransformOwner(steamID, groupID);
            }
            item.UpdateSign(string.Empty);
        }

        public void RemoveDoorItem(Door door, Transform transform)
        {
            DoorItem item = door.GetDoorItem(transform);
            door.Items.Remove(item);
            item.UpdateSign(string.Empty);
        }

        public void RemoveDoor(Door door)
        {
            database.RemoveDoor(door);
            door.UpdateSigns();
        }
    }
}
