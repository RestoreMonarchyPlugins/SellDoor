using Microsoft.SqlServer.Server;
using RestoreMonarchy.SellDoor.Database;
using SDG.Unturned;
using System.IO;
using UnityEngine;

namespace RestoreMonarchy.SellDoor.Services
{
    public partial class DoorService : MonoBehaviour
    {
        private SellDoorPlugin pluginInstance => SellDoorPlugin.Instance;

        private DoorsDatabase database;

        void Awake()
        {
            string fileName = $"Doors.{Provider.map.Replace(" ", "_")}.json";
            string path = Path.Combine(pluginInstance.Directory, "doors.json");
            if (File.Exists(path))
            {
                string path2 = Path.Combine(pluginInstance.Directory, fileName);
                if (!File.Exists(path2))
                {
                    File.Move(path, path2);
                }
            }

            database = new DoorsDatabase(pluginInstance.Directory, fileName);
        }

        void Start()
        {
            if (Level.isLoaded)
                LoadDoors(0);

            Level.onLevelLoaded += LoadDoors;
            SaveManager.onPostSave += SaveDoors;
            BarricadeManager.onDamageBarricadeRequested += OnDamageBarricadeRequested;
            StructureManager.onDamageStructureRequested += OnDamageStructureRequested;
            BarricadeDrop.OnSalvageRequested_Global += OnSalvageBarricadeRequested;
            StructureDrop.OnSalvageRequested_Global += OnSalvageStructureRequested;
        }

        void OnDestroy()
        {
            SaveDoors();

            Level.onLevelLoaded -= LoadDoors;
            SaveManager.onPostSave -= SaveDoors;
            BarricadeManager.onDamageBarricadeRequested -= OnDamageBarricadeRequested;
            StructureManager.onDamageStructureRequested -= OnDamageStructureRequested;
            BarricadeDrop.OnSalvageRequested_Global -= OnSalvageBarricadeRequested;
            StructureDrop.OnSalvageRequested_Global -= OnSalvageStructureRequested;
        }
    }
}
