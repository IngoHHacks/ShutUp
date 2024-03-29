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
        public const string PluginVer = "1.4.0";

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
        
        [HarmonyPatch(typeof(NEGAFEHECNL), nameof(NEGAFEHECNL.GPPKMBAODNL))]
        [HarmonyPostfix]
        private static void NEGAFEHECNL_GPPKMBAODNL()
        {
            try
            {
                BJMGCKGNCHO obj = HKJOAJOKOIJ.NAADDLFFIHG[HKJOAJOKOIJ.EMLDNFEIKCK];
                if (NEGAFEHECNL.NNMDEFLLNBF > 0 && LIPNHOMGGHF.FAKHAFKOBPB == 50 &&
                    NJBJIIIACEP.OAAMGFLINOB[NEGAFEHECNL.NNMDEFLLNBF].NLOOBNDGIKO.BPJFLJPKKJK >= 5)
                {
                    obj = NJBJIIIACEP.OAAMGFLINOB[NEGAFEHECNL.NNMDEFLLNBF].NLOOBNDGIKO;
                }

                if (NEGAFEHECNL.EJFHLGMHAHB == 0)
                {
                    int num2 = 0;
                    if (obj.IOIJFFLMBCH[1] != 0 || obj.IOIJFFLMBCH[2] != 0 || obj.IOIJFFLMBCH[3] != 0 ||
                        obj.IOIJFFLMBCH[4] != 0 || obj.FHBEOIPFFDA != 0 || obj.OHEIJEDGKLJ != 0)
                    {
                        num2 = 1;
                    }

                    if (((Math.Abs(HKJOAJOKOIJ.EOOBMIDCKIF - 1f) < 0.0001f &&
                          HKJOAJOKOIJ.MINFPCEENFN < Screen.height * 0.3f) || num2 != 0 ||
                         Input.GetKey(KeyCode.Escape) || Input.GetMouseButton(0)) && HKJOAJOKOIJ.LMADDGDMBGB == 0f)
                    {
                        if (!LockAfterSkip.Value || !_lock ||
                            (DateTime.Now.Ticks / 10000) - _lastSkip > LockDelay.Value)
                        {
                            NEGAFEHECNL.ODOAPLMOJPD++;
                            NEGAFEHECNL.IMJHCHECCED = 0f;
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
            } catch (Exception e)
            {
                _lock = false;
            }
        }
    }
}