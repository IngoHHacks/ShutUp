using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
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
        public const string PluginVer = "1.0.0";

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
        
        [HarmonyPatch(typeof(HJBJPCDFLGL), "EKPJDDCHBFC")]
        [HarmonyPostfix]
        private static void HJBJPCDFLGL_EKPJDDCHBFC()
        {
            AMPJKOHMNJK aMPJKOHMNJK = JINPJBLJOMA.NDCGFCAFBIB[JINPJBLJOMA.JHPHBHFGAMD];
            if (HJBJPCDFLGL.NAGCDENHJNE > 0 && KPGIEHHDIDA.LHOICDLLMID == 50 && NODLGBMEKCI.NMOJJPKABCC[HJBJPCDFLGL.NAGCDENHJNE].MHCGLBLHDML.GACPHBMEHOE >= 5)
            {
                aMPJKOHMNJK = NODLGBMEKCI.NMOJJPKABCC[HJBJPCDFLGL.NAGCDENHJNE].MHCGLBLHDML;
            }
            if (HJBJPCDFLGL.IMJHHKJLPBC == 0)
            {
                int num2 = 0;
                if (aMPJKOHMNJK.PGCCLPMAFMA[1] != 0 || aMPJKOHMNJK.PGCCLPMAFMA[2] != 0 || aMPJKOHMNJK.PGCCLPMAFMA[3] != 0 || aMPJKOHMNJK.PGCCLPMAFMA[4] != 0 || aMPJKOHMNJK.CCPDCCAPCEG != 0 || aMPJKOHMNJK.EHIEBJHCNIB != 0)
                {
                    num2 = 1;
                }
                if (((JINPJBLJOMA.OIDLNCJOFLC == 1f && JINPJBLJOMA.LFNHGLIBNAK < Screen.height * 0.3f) || num2 != 0 || Input.GetKey(KeyCode.Escape) || Input.GetMouseButton(0)) && JINPJBLJOMA.ALBIPGOEJLM == 0f)
                {
                    if (!LockAfterSkip.Value || !_lock ||
                        (System.DateTime.Now.Ticks / 10000) - _lastSkip > LockDelay.Value)
                    {
                        HJBJPCDFLGL.OKILOINLLAO++;
                        HJBJPCDFLGL.IAOODIBGNFI = 0f;
                        if (LockAfterSkip.Value)
                        {
                            _lock = true;
                            _lastSkip = System.DateTime.Now.Ticks / 10000;
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