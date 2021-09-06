using RestoreMonarchy.SellDoor.Database;
using SDG.Unturned;
using UnityEngine;

namespace RestoreMonarchy.SellDoor.Services
{
    public partial class DoorService : MonoBehaviour
    {
        private SellDoorPlugin pluginInstance => SellDoorPlugin.Instance;

        private DoorsDatabase database;

        void Awake()
        {
            database = new DoorsDatabase(pluginInstance.Directory, "doors.json");
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
