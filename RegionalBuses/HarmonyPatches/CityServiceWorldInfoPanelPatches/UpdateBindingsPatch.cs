using ColossalFramework.UI;
using HarmonyLib;

namespace RegionalBuses.HarmonyPatches.CityServiceWorldInfoPanelPatches
{
    
    [HarmonyPatch(typeof(CityServiceWorldInfoPanel))]
    [HarmonyPatch("UpdateBindings")]
    internal static class UpdateBindingsPatch
    {
        private static string _originalLabel;
        
        
        internal static void Postfix(CityServiceWorldInfoPanel __instance, InstanceID ___m_InstanceID, UIPanel ___m_intercityTrainsPanel)
        {
            var label = ___m_intercityTrainsPanel.Find<UILabel>("Label");
            _originalLabel ??= label.text;
            
            var building1 = ___m_InstanceID.Building;
            var instance = BuildingManager.instance;
            var building2 = instance.m_buildings.m_buffer[building1];
            var info = building2.Info;
            var buildingAi = info.m_buildingAI;
            var transportStationAi = buildingAi as TransportStationAI;
            if (transportStationAi == null)
            {
                label.text = _originalLabel;
                return;
            }
            var transportLineInfo1 = transportStationAi.GetTransportLineInfo();
            var transportLineInfo2 = transportStationAi.GetSecondaryTransportLineInfo();
            var ships =
                transportLineInfo1 != null && transportLineInfo1.m_class.m_subService ==
                ItemClass.SubService.PublicTransportShip || transportLineInfo2 != null &&
                transportLineInfo2.m_class.m_subService == ItemClass.SubService.PublicTransportShip;
            var intercityTrains = transportLineInfo1 != null && transportLineInfo1.m_class.m_subService == ItemClass.SubService.PublicTransportTrain || transportLineInfo2 != null && transportLineInfo2.m_class.m_subService == ItemClass.SubService.PublicTransportTrain;
            var intercityBus1 = transportLineInfo1 != null && transportLineInfo1.m_class.m_subService == ItemClass.SubService.PublicTransportBus && transportStationAi.m_maxVehicleCount > 0;
            var intercityBus2 = transportLineInfo2 != null && transportLineInfo2.m_class.m_subService == ItemClass.SubService.PublicTransportBus && transportStationAi.m_maxVehicleCount2 > 0;
            var intercityBuses = (intercityBus1 || intercityBus2) && transportStationAi.m_transportLineInfo != null && transportStationAi.m_transportLineInfo?.name == Mod.IntercityBusLine;
            var isVisible = !ships && !intercityTrains && intercityBuses;
            if (isVisible)
            {
                ___m_intercityTrainsPanel.isVisible = true;
            }

            label.text = intercityBuses ? "Allow Intercity Buses" : _originalLabel;
        }
    }
}