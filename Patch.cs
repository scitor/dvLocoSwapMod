using DV;
using DV.ThingTypes;
using HarmonyLib;
using System;
using System.Collections.Generic;

namespace dvSteamOnly
{
    [HarmonyPatch(typeof(StationLocoSpawner), "Awake")]
    class StationLocoSpawner_Awake_Patch
    {
        private static Dictionary<TrainCarType, TrainCarLivery> Cache = new Dictionary<TrainCarType, TrainCarLivery>();

        static void Postfix(StationLocoSpawner __instance)
        {
            if (Cache.Count == 0) {
                CacheLiveries();
            }
            foreach (ListTrainCarTypeWrapper trainCarTypeWrapper in __instance.locoTypeGroupsToSpawn) {
                ref List<TrainCarLivery> liveries = ref trainCarTypeWrapper.liveries;
                if (Main.Settings.SteamOnly) {
                    if (liveries.RemoveAll(L => L.v1 == TrainCarType.LocoDiesel) > 0) {
                        liveries.Add(Cache.GetValueSafe(TrainCarType.LocoSteamHeavy));
                        liveries.Add(Cache.GetValueSafe(TrainCarType.Tender));
                    }
                    if (liveries.RemoveAll(L => L.v1 == TrainCarType.LocoShunter || L.v1 == TrainCarType.LocoDM3 || L.v1 == TrainCarType.LocoDH4) > 0) {
                        liveries.Add(Cache.GetValueSafe(TrainCarType.LocoS060));
                    }
                } else {
                    List<TrainCarLivery> replacements = new List<TrainCarLivery>();
                    if (liveries.RemoveAll(L => L.v1 == TrainCarType.LocoSteamHeavy) > 0) {
                        liveries.RemoveAll(L => L.v1 == TrainCarType.Tender);
                        replacements.Add(Cache.GetValueSafe(TrainCarType.LocoDM3));
                        replacements.Add(Cache.GetValueSafe(TrainCarType.LocoDH4));
                        liveries.Add(replacements[(new Random()).Next(replacements.Count)]);
                    }
                    if (liveries.RemoveAll(L => L.v1 == TrainCarType.LocoS060) > 0) {
                        replacements.Add(Cache.GetValueSafe(TrainCarType.LocoShunter));
                        replacements.Add(Cache.GetValueSafe(TrainCarType.LocoDM3));
                        liveries.Add(replacements[(new Random()).Next(replacements.Count)]);
                    }
                }
            }
        }

        static void CacheLiveries() 
        {
            foreach (TrainCarLivery livery in Globals.G.Types.Liveries) {
                if (CarTypes.IsLocomotive(livery) || livery.v1 == TrainCarType.Tender) {
                    Cache.Add(livery.v1, livery);
                }
            }
        }
    }
}
