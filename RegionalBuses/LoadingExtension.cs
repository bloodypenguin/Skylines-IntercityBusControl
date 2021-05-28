using System;
using CitiesHarmony.API;
using ICities;
using UnityEngine;

namespace RegionalBuses
{
    public class LoadingExtension : LoadingExtensionBase
    {
        private static AppMode _loadMode;
        
        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);
            _loadMode = loading.currentMode;
            try
            {
                if (_loadMode == AppMode.Game)
                {
                    if (!HarmonyHelper.IsHarmonyInstalled)
                    {
                        return;
                    }
                    Patcher.PatchAll();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        public override void OnReleased()
        {
            base.OnReleased();
            try
            {
                if (_loadMode == AppMode.Game)
                {
                    if (!HarmonyHelper.IsHarmonyInstalled)
                    {
                        return;
                    }
                    Patcher.UnpatchAll();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}