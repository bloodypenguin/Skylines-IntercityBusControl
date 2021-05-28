using System;
using HarmonyLib;

namespace RegionalBuses.HarmonyPatches.NetInfoPatches
{
    [HarmonyPatch(typeof(NetInfo))]
    [HarmonyPatch(nameof(NetInfo.InitializePrefab))]
    internal static class InitializePrefabPatch
    {

        internal static void Postfix(NetInfo __instance)
        {
            try
            {
                if (__instance?.name != Mod.IntercityBusLine)
                {
                    return;
                }

                if (PrefabCollection<BuildingInfo>.FindLoaded("Bus Station")?.m_buildingAI is TransportStationAI ai1)
                {
                    ai1.m_transportLineInfo = __instance;
                }
                if (PrefabCollection<BuildingInfo>.FindLoaded("Monorail Bus Hub")?.m_buildingAI is TransportStationAI ai3)
                {
                    ai3.m_transportLineInfo = __instance;
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
        }
    }
}