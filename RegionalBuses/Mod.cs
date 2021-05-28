using CitiesHarmony.API;
using ICities;

namespace RegionalBuses
{
    public class Mod : IUserMod
    {
        public static string IntercityBusLine = "Intercity Bus Line";

        public string Name => "Intercity Bus Control";
        public string Description => "Intercity Bus Control";

        
        public void OnEnabled() {
            HarmonyHelper.EnsureHarmonyInstalled();
        }
    }
}