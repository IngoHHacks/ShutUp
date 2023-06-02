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
        public const string PluginVer = "1.2.0";

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
        
        [HarmonyPatch(typeof(DNMADBBLNDC), nameof(DNMADBBLNDC.CFFHMHNAKLF))]
        [HarmonyPostfix]
        private static void DNMADBBLNDC_CFFHMHNAKLF()
        {
            KDOHFMKNHOB obj = IODAJCMGILB.LEGPGICAAJI[IODAJCMGILB.FOOBAJOCOAK];
            if (DNMADBBLNDC.PDMDFGNJCPN > 0 && DNDIEGNJOKN.OBNLIIMODBI == 50 && AMJONEKIAID.NCPIJJFEDFL[DNMADBBLNDC.PDMDFGNJCPN].LKGAHNBHMAE.AHPNDLJNCFK >= 5)
            {
                obj = AMJONEKIAID.NCPIJJFEDFL[DNMADBBLNDC.PDMDFGNJCPN].LKGAHNBHMAE;
            }
            if (DNMADBBLNDC.IGIEBCJHBLP == 0)
            {
                int num2 = 0;
                if (obj.HFEHAADPDHP[1] != 0 || obj.HFEHAADPDHP[2] != 0 || obj.HFEHAADPDHP[3] != 0 || obj.HFEHAADPDHP[4] != 0 || obj.IIJHBGKCENJ != 0 || obj.ALAAHPKCGJG != 0)
                {
                    num2 = 1;
                }
                if (((Math.Abs(IODAJCMGILB.DHLELODFJHB - 1f) < 0.0001f && IODAJCMGILB.NIALJKKHMNI < Screen.height * 0.3f) || num2 != 0 || Input.GetKey(KeyCode.Escape) || Input.GetMouseButton(0)) && IODAJCMGILB.DBEEFCPGGCP == 0f)
                {
                    if (!LockAfterSkip.Value || !_lock ||
                        (DateTime.Now.Ticks / 10000) - _lastSkip > LockDelay.Value)
                    {
                        DNMADBBLNDC.BABHEGOMNLJ++;
                        DNMADBBLNDC.GKDOOPDCBMD = 0f;
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