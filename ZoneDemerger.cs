using Colossal.Entities;
using Game;
using Game.Prefabs;
using Game.UI;
using Game.UI.InGame;
using Game.Zones;
using System.Collections.Generic;
using System.Reflection;
using Unity.Collections;
using Unity.Entities;

namespace ZoneDemerger
{
    public partial class ZoneDemerger : GameSystemBase
    {
        private PrefabSystem _prefabSystem;
        private EntityQuery _prefabQuery;
        private EntityQuery _zoneQuery;
        private NameSystem _nameSystem;
        //public ZoneDemerger()
        //{

        //}
        protected override void OnCreate()
        {
            base.OnCreate();
            _prefabSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<PrefabSystem>();
            _nameSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<NameSystem>();

            _prefabQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = [ComponentType.ReadOnly<PrefabData>()],
                Any = [ComponentType.ReadWrite<SpawnableBuildingData>()]
            });
            //RequireForUpdate(_prefabQuery);


            _zoneQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = [ComponentType.ReadOnly<ZoneData>()]
            });
            //RequireForUpdate(_zoneQuery);
            NativeArray<Entity> zoneArray = _zoneQuery.ToEntityArray(Allocator.Temp);
            Dictionary<string, Entity> zoneEntityDict = new Dictionary<string, Entity>();
            Mod.log.Info(zoneArray.Length);
            foreach (var entity in zoneArray)
            {
                var zoneName = _prefabSystem.GetPrefabName(entity);
                zoneEntityDict[zoneName] = entity;
            }

            object[][] newZones =
            {
                [1, "Industrial Manufacturing", "Industrial Warehouse", "IndustrialManufacturingWarehouse0"],
                [2, "EU Commercial Low", "EU Commercial Gas", "EU_CommercialGasStation0"],
                [3, "EU Commercial Low", "EU Commercial Motel", "EU_CommercialMotel0"],
                [4, "EU Commercial High", "EU Commercial Hotel", "EU_CommercialHotel0"],
                [5, "NA Commercial Low", "NA Commercial Gas", "NA_CommercialGasStation0"],
                [6, "NA Commercial Low", "NA Commercial Motel", "NA_CommercialMotel0"],
                [7, "NA Commercial High", "NA Commercial Hotel", "NA_CommercialHotel0"]
            };

            foreach (var zone in newZones)
            {
                string oldZoneName = zone[1].ToString();
                string newZoneName = zone[2].ToString();
                string prefix = zone[3].ToString();


                Entity oldZoneEntity = Entity.Null;
                if (zoneEntityDict.ContainsKey(oldZoneName))
                {
                    oldZoneEntity = zoneEntityDict[oldZoneName];
                    Mod.log.Info(oldZoneEntity);
                }

                if (oldZoneEntity != Entity.Null)
                {
                    Mod.log.Info($"Found {oldZoneName}");
                    //var collection = EntityManager.GetComponentTypes(oldZoneEntity);
                    //Mod.log.Info(collection.Length);
                    //foreach (var item in collection)
                    //{
                    //    Mod.log.Info(item.GetManagedType());
                    //}
                    EntityManager.TryGetComponent(oldZoneEntity, out UIObjectData oldUIObjectData);
                    EntityManager.TryGetComponent(oldZoneEntity, out GroupAmbienceData oldGroupAmbienceData);
                    EntityManager.TryGetComponent(oldZoneEntity, out CrimeAccumulationData oldCrimeAccumulationData);
                    EntityManager.TryGetComponent(oldZoneEntity, out MailAccumulationData oldMailAccumulationData);
                    EntityManager.TryGetComponent(oldZoneEntity, out ZoneServiceConsumptionData oldZoneServiceConsumptionData);
                    EntityManager.TryGetComponent(oldZoneEntity, out ZonePollutionData oldZonePollutionData);
                    EntityManager.TryGetComponent(oldZoneEntity, out ZonePropertiesData oldZonePropertiesData);
                    EntityManager.TryGetComponent(oldZoneEntity, out ZoneData oldZoneData);
                    EntityManager.TryGetComponent(oldZoneEntity, out PrefabData oldPrefabData);
                    //EntityManager.TryGetComponent(oldZoneEntity, out ObjectRequirementElement oldObjectRequirementElement);
                    //EntityManager.TryGetComponent(oldZoneEntity, out PlaceableInfoviewItem oldPlaceableInfoviewItem);
                    //EntityManager.TryGetComponent(oldZoneEntity, out LoadedIndex oldLoadedIndex);
                    //EntityManager.TryGetComponent(oldZoneEntity, out UnlockRequirement oldUIObject);
                    //EntityManager.TryGetComponent(oldZoneEntity, out Locked oldZoneData);
                    //EntityManager.TryGetComponent(oldZoneEntity, out UIObjectData oldUIObject);
                    //EntityManager.TryGetComponent(oldZoneEntity, out MailAccumulationData oldMailAccumulationData);
                    //Mod.log.Info("=================================================================>>>>>>>>>>>>>>>>>>>>>");
                    //Mod.log.Info($"m_AccumulationRate: {oldMailAccumulationData.m_AccumulationRate}");
                    //Mod.log.Info($"m_RequireCollect: {oldMailAccumulationData.m_RequireCollect}");
                    //Mod.log.Info("=================================================================>>>>>>>>>>>>>>>>>>>>>");
                    //Mod.log.Info($"m_AreaType: {oldZoneData.m_AreaType}");
                    //Mod.log.Info($"m_MaxHeight: {oldZoneData.m_MaxHeight}");
                    //Mod.log.Info($"m_MinEvenHeight: {oldZoneData.m_MinEvenHeight}");
                    //Mod.log.Info($"m_MinOddHeight: {oldZoneData.m_MinOddHeight}");
                    //Mod.log.Info($"m_ZoneFlags: {oldZoneData.m_ZoneFlags}");
                    //Mod.log.Info($"m_ZoneType: {oldZoneData.m_ZoneType.m_Index}");
                    //Mod.log.Info("*****************************************************************");

                    var oldZonePrefab = _prefabSystem.GetPrefab<PrefabBase>(oldZoneEntity);
                    var newZonePrefab = oldZonePrefab.Clone(newZoneName);
                    _prefabSystem.AddPrefab(newZonePrefab);
                    var newZoneEntity = _prefabSystem.GetEntity(newZonePrefab);
                    EntityManager.TryGetComponent(newZoneEntity, out UIObjectData newUIObjectData);
                    EntityManager.TryGetComponent(newZoneEntity, out GroupAmbienceData newGroupAmbienceData);
                    EntityManager.TryGetComponent(newZoneEntity, out CrimeAccumulationData newCrimeAccumulationData);
                    EntityManager.TryGetComponent(newZoneEntity, out MailAccumulationData newMailAccumulationData);
                    EntityManager.TryGetComponent(newZoneEntity, out ZoneServiceConsumptionData newZoneServiceConsumptionData);
                    EntityManager.TryGetComponent(newZoneEntity, out ZonePollutionData newZonePollutionData);
                    EntityManager.TryGetComponent(newZoneEntity, out ZonePropertiesData newZonePropertiesData);
                    EntityManager.TryGetComponent(newZoneEntity, out ZoneData newZoneData);
                    EntityManager.TryGetComponent(newZoneEntity, out PrefabData newPrefabData);
                    newZonePrefab.TryGet(out UIObject newUIObject);
                    //foreach (var item in newZonePrefab.components)
                    //{
                    //    Mod.log.Info(item.name);
                    //}
                    newUIObjectData = oldUIObjectData;
                    newGroupAmbienceData = oldGroupAmbienceData;
                    newCrimeAccumulationData = oldCrimeAccumulationData;
                    newMailAccumulationData = oldMailAccumulationData;
                    newZoneServiceConsumptionData = oldZoneServiceConsumptionData;
                    newZonePollutionData = oldZonePollutionData;
                    newZonePropertiesData = oldZonePropertiesData;
                    newZoneData = oldZoneData;
                    newPrefabData = oldPrefabData;
                    if (newZoneName == "Industrial Warehouse")
                    {
                        newZonePropertiesData.m_AllowedStored = oldZonePropertiesData.m_AllowedManufactured;
                        newZonePropertiesData.m_AllowedManufactured = (ulong)0;
                        newUIObject.m_Icon = "Media/Game/Icons/ZoneIndustrialWarehouses.svg";
                        newUIObject.m_Priority++;
                    }
                    ModifyVanillaBuildings(newZonePrefab, prefix);
                    //Mod.log.Info("=================================================================>>>>>>>>>>>>>>>>>>>>>");
                    //Mod.log.Info($"m_AccumulationRate: {newZonePropertiesData.m_AllowedStored}");
                    //Mod.log.Info($"m_RequireCollect: {newZonePropertiesData.m_AllowedManufactured}");
                    //Mod.log.Info("=================================================================>>>>>>>>>>>>>>>>>>>>>");
                    //Mod.log.Info($"m_AccumulationRate: {oldZonePropertiesData.m_AllowedManufactured}");
                    //Mod.log.Info($"m_RequireCollect: {oldZonePropertiesData.m_AllowedStored}");
                    //Mod.log.Info("=================================================================>>>>>>>>>>>>>>>>>>>>>");
                    //Mod.log.Info($"m_AreaType: {newZoneData.m_AreaType}");
                    //Mod.log.Info($"m_MaxHeight: {oldZoneData.m_MaxHeight} => {newZoneData.m_MaxHeight}");
                    //Mod.log.Info($"m_MinEvenHeight: {oldZoneData.m_MinEvenHeight} => {newZoneData.m_MinEvenHeight}");
                    //Mod.log.Info($"m_MinOddHeight: {oldZoneData.m_MinOddHeight} => {newZoneData.m_MinOddHeight}");
                    //Mod.log.Info($"m_ZoneFlags: {newZoneData.m_ZoneFlags}");
                    //Mod.log.Info($"m_ZoneType: {newZoneData.m_ZoneType.m_Index}");
                    //Mod.log.Info("=================================================================");
                    //_prefabSystem.UpdatePrefab(newZonePrefab);
                    //Mod.log.Info("*****************************************************************");
                }
                else
                {
                }

                //if (_prefabSystem.TryGetPrefab(new PrefabID(nameof(ZonePrefab), oldZoneName), out var originalPrefab))
                //{

                //    PrefabBase clonedPrefab = originalPrefab.Clone(newZoneName);
                //    originalPrefab.TryGet(out UIObject oldUI);
                //    //_prefabSystem.TryGetComponentData<ZoneData>(clonedPrefab, out var newUI);
                //    clonedPrefab.TryGet(out UIObject newUI);
                //    clonedPrefab.TryGet(out ZoneProperties newZoneProp); 
                //    ZoneData _zoneData;
                //    _zoneData.m_AreaType = clonedPrefab.
                //    Mod.log.Info(oldUI.CompareTo(newUI));
                //    if (newZoneName == "Industrial Warehouse")
                //    {
                //        newUI.m_Icon = "Media/Game/Icons/ZoneIndustrialWarehouses.svg";
                //        newZoneProp.m_AllowedStored = newZoneProp.m_AllowedManufactured;
                //        newZoneProp.m_AllowedManufactured = [];

                //    }
                //    newUI.m_Priority++;


                //    Mod.log.Info(oldUI.CompareTo(newUI));
                //    _prefabSystem.AddPrefab(clonedPrefab);
                //    Entity _zoneEntity = _prefabSystem.GetEntity(clonedPrefab);
                //    Mod.log.Info($"Successfully cloned {oldZoneName} to {newZoneName}");
                //    Mod.log.Info($"Zone Entity: {_zoneEntity}");
                //    // ***************** Zones show up successfully in game. Both in game and in editor. *****************
                //    ModifyVanillaBuildings(_zoneEntity, prefix);
                //}
                //else
                //{
                //    Mod.log.Error($"Something is wrong; can't find {oldZoneName}");
                //}
            }
        }

        void ModifyVanillaBuildings(PrefabBase newZonePrefab, string prefix)
        {

            foreach (var entity in _prefabQuery.ToEntityArray(Allocator.Temp))
            {
                //PrefabBase prefab = 
                if (_prefabSystem.GetPrefabName(entity).StartsWith(prefix) && EntityManager.TryGetComponent(entity, out SpawnableBuildingData spawnableBuildingData))
                {
                    _prefabSystem.TryGetPrefab(entity, out PrefabBase prefab);
                    spawnableBuildingData.m_ZonePrefab = _prefabSystem.GetEntity(newZonePrefab);
                    EntityManager.SetComponentData(entity, spawnableBuildingData);
                    //_prefabSystem.UpdatePrefab(prefab);
                }
            }
            // ***************** Code 2 *****************
            //var prefabs = typeof(PrefabSystem).GetField("m_Prefabs", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_prefabSystem) as List<PrefabBase>;
            //foreach (var item in prefabs)
            //{
            //    if (item.name.StartsWith(prefix) && item.Has<SpawnableBuilding>())
            //    {

            //        item.TryGet<SpawnableBuilding>(out var spawnableBuilding);
            //        var _old = spawnableBuilding.m_ZoneType;
            //        spawnableBuilding.m_ZoneType = _prefabSystem.GetPrefab<ZonePrefab>(_zoneEntity);
            //        var _new = spawnableBuilding.m_ZoneType;

            //        Mod.log.Info($"{item.name}: {_old} was changed to {_new} (1)");
            //        _prefabSystem.AddPrefab(item);
            //    }
            //}
            //Mod.log.Info("*****************************************************************");

            // ***************** Code 1 *****************
            // W
            //foreach (Entity entity in _prefabQuery.ToEntityArray(Allocator.Temp))
            //{
            //    if (EntityManager.TryGetComponent(entity, out SpawnableBuildingData spawnableBuilding) && _prefabSystem.GetPrefab<PrefabBase>(entity).name.StartsWith(prefix))
            //    {
            //        var _old = spawnableBuilding.m_ZonePrefab;
            //        spawnableBuilding.m_ZonePrefab = _zoneEntity;
            //        EntityManager.SetComponentData(entity, spawnableBuilding);
            //        var _new = spawnableBuilding.m_ZonePrefab;
            //        Mod.log.Info($"{_prefabSystem.GetPrefab<PrefabBase>(entity).name}: {_old} was changed to {_new} (2)");
            //        _prefabSystem.Update();
            //    }
            //}
            //Mod.log.Info("*****************************************************************");
        }

        protected override void OnUpdate()
        {
        }
    }

}