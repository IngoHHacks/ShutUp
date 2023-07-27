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
        public const string PluginVer = "1.3.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);
        
        internal static ConfigEntry<bool> LockAfterSkip;
        internal static ConfigEntry<int> LockDelay;

        internal static string PluginPath;
        

        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
            
            LockAfterSkip = Config.Bind("General", "LockAfterSkip", true, "Prevents skipping FMHJNNGPMKG too fast by holding the skip button.");
            LockDelay = Config.Bind("General", "LockDelay", 200, "Delay in milliseconds after skipping FMHJNNGPMKG before the skip button can be used again if LockAfterSkip is enabled and the skip button is held down.");
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
        
        [HarmonyPatch(typeof(FMHJNNGPMKG), nameof(FMHJNNGPMKG.HECHPCGFPFI))]
        [HarmonyPostfix]
        private static void FMHJNNGPMKG_HECHPCGFPFI()
        {
            IMBAMKCPLIF obj = MFCAJFKKFFE.FBOPLHBCBFI[MFCAJFKKFFE.OMPNDJNOOIF];
            if (FMHJNNGPMKG.CJGHFHCHDNN > 0 && JJDCNALMPCI.AAAIDOOHBCM == 50 && FFKMIEMAJML.FJCOPECCEKN[FMHJNNGPMKG.CJGHFHCHDNN].GHDPNAGKOCP.FNIDHNNCLBB >= 5)
            {
                obj = FFKMIEMAJML.FJCOPECCEKN[FMHJNNGPMKG.CJGHFHCHDNN].GHDPNAGKOCP;
            }
            if (FMHJNNGPMKG.DFKDIOPDOIN == 0)
            {
                int num2 = 0;
                if (obj.HNGCFDLDGBF[1] != 0 || obj.HNGCFDLDGBF[2] != 0 || obj.HNGCFDLDGBF[3] != 0 || obj.HNGCFDLDGBF[4] != 0 || obj.HGLNBKOFGDO != 0 || obj.HKPDEHMCKIO != 0)
                {
                    num2 = 1;
                }
                if (((Math.Abs(MFCAJFKKFFE.OIOEJJAMHKB - 1f) < 0.0001f && MFCAJFKKFFE.LCNKDGDGJJI < Screen.height * 0.3f) || num2 != 0 || Input.GetKey(KeyCode.Escape) || Input.GetMouseButton(0)) && MFCAJFKKFFE.JOPPDHFINKD == 0f)
                {
                    if (!LockAfterSkip.Value || !_lock ||
                        (DateTime.Now.Ticks / 10000) - _lastSkip > LockDelay.Value)
                    {
                        FMHJNNGPMKG.NJJPPLCPOIA++;
                        FMHJNNGPMKG.AMBGCJOBKFN = 0f;
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