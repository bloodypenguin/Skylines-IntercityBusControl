using System;
using HarmonyLib;

namespace RegionalBuses.HarmonyPatches.BuildingInfoPatches
{
    [HarmonyPatch(typeof(BuildingInfo))]
    [HarmonyPatch(nameof(BuildingInfo.InitializePrefab))]
    internal static class InitializePrefabPatch
    {

        internal static void Postfix(BuildingInfo __instance)
        {
            try
            {
                if (__instance.m_buildingAI is not TransportStationAI transportStationAi)
                {
                    return;
                }

                var transportLineInfo1 = transportStationAi.GetTransportLineInfo();
                var transportLineInfo2 = transportStationAi.GetSecondaryTransportLineInfo();
                var intercityTrains =
                    transportLineInfo1 != null && transportLineInfo1.m_class.m_subService ==
                    ItemClass.SubService.PublicTransportTrain || transportLineInfo2 != null &&
                    transportLineInfo2.m_class.m_subService == ItemClass.SubService.PublicTransportTrain;
                var ships =
                    transportLineInfo1 != null && transportLineInfo1.m_class.m_subService ==
                    ItemClass.SubService.PublicTransportShip || transportLineInfo2 != null &&
                    transportLineInfo2.m_class.m_subService == ItemClass.SubService.PublicTransportShip;
                var intercityBus1 = transportLineInfo1 != null &&
                                    transportLineInfo1.m_class.m_subService ==
                                    ItemClass.SubService.PublicTransportBus;
                var intercityBus2 = transportLineInfo2 != null &&
                                    transportLineInfo2.m_class.m_subService ==
                                    ItemClass.SubService.PublicTransportBus;
                var shouldPatch = !ships && !intercityTrains && (intercityBus1 ^ intercityBus2) &&
                                  transportStationAi.m_transportLineInfo == null;
                UnityEngine.Debug.Log($"Intercity Bus Control - {__instance.name} - trains: {intercityTrains}, bus1: {intercityBus1}, bus2: {intercityBus2}");
                if (!shouldPatch)
                {
                    UnityEngine.Debug.Log($"Intercity Bus Control - {__instance.name} does not require patching");
                    return;
                }

                if (__instance?.name != "Bus Station" && __instance?.name != "Ferry Bus Hub" && __instance?.name != "Monorail Bus Hub")
                {
                    var lineInfo = PrefabCollection<NetInfo>.FindLoaded(Mod.IntercityBusLine);
                    if (lineInfo == null)
                    {
                        UnityEngine.Debug.LogWarning($"Intercity Bus Control - {Mod.IntercityBusLine} NetInfo not found!");
                        return;
                    }
                    transportStationAi.m_transportLineInfo = lineInfo;
                }
                if (intercityBus1)
                {
                    transportStationAi.m_maxVehicleCount = 100000;
                    UnityEngine.Debug.Log($"Intercity Bus Control - patched {__instance.name} primary transport with intercity bus support");
                } else if (intercityBus2)
                {
                    transportStationAi.m_maxVehicleCount2 = 100000;
                    UnityEngine.Debug.Log($"Intercity Bus Control - patched {__instance.name} secondary transport with intercity bus support");
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
        }
    }
}