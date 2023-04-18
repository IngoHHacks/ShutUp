using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace ShutUp
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "IngoH.WrestlingEmpire.ShutUp";
        public const string PluginName = "ShutUp";
        public const string PluginVer = "1.1.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);
        
        internal static ConfigEntry<bool> LockAfterSkip;
        internal static ConfigEntry<int> LockDelay;

        internal static string PluginPath;
        

        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
            
            LockAfterSkip = Config.Bind("General", "LockAfterSkip", true, "Prevents skipping dialog too fast by holding the skip button.");
            LockDelay = Config.Bind("General", "LockDelay", 200, "Delay in milliseconds after skipping dialog before the skip button can be used again if LockAfterSkip is enabled and the skip button is held down.");
        }

        private void OnEnable()
        {
            Harmony.PatchAll();
            Logger.LogInfo($"Loaded {PluginName}!");
        }

        private void OnDisable()
        {
            Harmony.UnpatchSelf();
            Logger.LogInfo($"Unloaded {PluginName}!");
        }
        
        private static bool _lock = false;
        private static long _lastSkip = 0;
        
        [HarmonyPatch(typeof(ANKJMHMKLPJ), nameof(ANKJMHMKLPJ.AOOPBBMLNMB))]
        [HarmonyPostfix]
        private static void ANKJMHMKLPJ_EKPJDDCHBFC()
        {
            FMOKFGNFBEL obj = KDAEEMPNIHH.PIMOFAAHKMI[KDAEEMPNIHH.HGPLPDFJEBC];
            if (ANKJMHMKLPJ.DNNAOLIENKK > 0 && LAHGBLEJCEO.GLDIFJOEOIO == 50 && DGCPHFIBPHC.KHMKIGPJPHN[ANKJMHMKLPJ.DNNAOLIENKK].KEMOICEBDOF.DPDEPJEGFMC >= 5)
            {
                obj = DGCPHFIBPHC.KHMKIGPJPHN[ANKJMHMKLPJ.DNNAOLIENKK].KEMOICEBDOF;
            }
            if (ANKJMHMKLPJ.JECKILHMDHE == 0)
            {
                int num2 = 0;
                if (obj.EKJFOHFFANP[1] != 0 || obj.EKJFOHFFANP[2] != 0 || obj.EKJFOHFFANP[3] != 0 || obj.EKJFOHFFANP[4] != 0 || obj.KNOFLDDCJMA != 0 || obj.JGLBABPLBNG != 0)
                {
                    num2 = 1;
                }
                if (((Math.Abs(KDAEEMPNIHH.EPGMPBANIGI - 1f) < 0.0001f && KDAEEMPNIHH.JJNHKOIGFMO < Screen.height * 0.3f) || num2 != 0 || Input.GetKey(KeyCode.Escape) || Input.GetMouseButton(0)) && KDAEEMPNIHH.PKCABBMAGNE == 0f)
                {
                    if (!LockAfterSkip.Value || !_lock ||
                        (DateTime.Now.Ticks / 10000) - _lastSkip > LockDelay.Value)
                    {
                        ANKJMHMKLPJ.AGAGHGBLCDA++;
                        ANKJMHMKLPJ.AHKCECADCAM = 0f;
                        if (LockAfterSkip.Value)
                        {
                            _lock = true;
                            _lastSkip = DateTime.Now.Ticks / 10000;
                        }
                    }
                }
                else if (LockAfterSkip.Value)
                {
                    _lock = false;
                }
            }
        }
    }
}