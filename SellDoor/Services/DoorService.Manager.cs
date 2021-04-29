using RestoreMonarchy.SellDoor.Helpers;
using RestoreMonarchy.SellDoor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
                if (door.IsBarricade)
                    door.Transform = RaycastHelper.GetBarricadeTransform(pos);
                else
                    door.Transform = RaycastHelper.GetStructureTransform(pos);

                foreach (DoorItem item in door.Items)
                {
                    pos = item.Position.ToVector3();

                    if (item.IsBarricade)
                        item.Transform = RaycastHelper.GetBarricadeTransform(pos);
                    else
                        item.Transform = RaycastHelper.GetStructureTransform(pos);
                }
            }
        }

        private void SaveDoors()
        {
            database.Save();
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

        public void AddDoorItem(Door door, DoorItem item)
        {
            if (door.Items == null)
                door.Items = new List<DoorItem>();

            door.Items.Add(item);
        }
    }
}
