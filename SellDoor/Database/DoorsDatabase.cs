using RestoreMonarchy.SellDoor.Models;
using RestoreMonarchy.SellDoor.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestoreMonarchy.SellDoor.Database
{
    public class DoorsDatabase
    {
        public IEnumerable<Door> Doors => doors;

        private DataStorage<List<Door>> dataStorage;
        private List<Door> doors;

        private int GetNextID() => doors.OrderByDescending(x => x.Id).FirstOrDefault()?.Id + 1 ?? 1;

        public DoorsDatabase(string dir, string fileName)
        {
            dataStorage = new DataStorage<List<Door>>(dir, fileName);
        }

        public void Reload()
        {
            doors = dataStorage.Read();
            if (doors == null)
                doors = new List<Door>();
        }

        public void Save()
        {
            foreach (Door door in doors)
            {
                door.UpdatePosition();
                foreach (DoorItem item in door.Items)
                { 
                    item.UpdatePosition();
                }
            }

            dataStorage.Save(doors);
        }

        public void AddDoor(Door door)
        {
            door.Id = GetNextID();
            doors.Add(door);
            Save();
        }

        public void RemoveDoor(Door door)
        {
            doors.Remove(door);
            Save();
        }

        public void DeleteBrokenDoors()
        {
            doors.RemoveAll(x => x.Transform == null);
            Save();
        }
    }
}
