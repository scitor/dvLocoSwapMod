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
        private static List<TrainCarLivery> SteamLiveries = new List<TrainCarLivery>();
        private static List<TrainCarLivery> NonSteamLiveries = new List<TrainCarLivery>();

        static void Postfix(StationLocoSpawner __instance)
        {
            if (SteamLiveries.Count == 0) {
                CacheLiveries();
            }
            foreach (ListTrainCarTypeWrapper trainCarTypeWrapper in __instance.locoTypeGroupsToSpawn) {
                ref List<TrainCarLivery> liveries = ref trainCarTypeWrapper.liveries;
                if (Main.Settings.SteamOnly) {
                    if (liveries.RemoveAll(L => L.v1 == TrainCarType.LocoDiesel) > 0) {
                        liveries.Add(SteamLiveries.Find(L => L.v1 == TrainCarType.LocoSteamHeavy));
                        liveries.Add(SteamLiveries.Find(L => L.v1 == TrainCarType.Tender));
                    }
                    if (liveries.RemoveAll(L => L.v1 == TrainCarType.LocoShunter || L.v1 == TrainCarType.LocoDM3 || L.v1 == TrainCarType.LocoDH4) > 0) {
                        liveries.Add(SteamLiveries.Find(L => L.v1 == TrainCarType.LocoS060));
                    }
                } else {
                    List<TrainCarLivery> replacements = new List<TrainCarLivery>();
                    if (liveries.RemoveAll(L => L.v1 == TrainCarType.LocoSteamHeavy) > 0) {
                        liveries.RemoveAll(L => L.v1 == TrainCarType.Tender);
                        replacements = NonSteamLiveries.FindAll(L => L.v1 == TrainCarType.LocoDM3 || L.v1 == TrainCarType.LocoDH4);
                        liveries.Add(replacements[(new Random()).Next(replacements.Count)]);
                    }
                    if (liveries.RemoveAll(L => L.v1 == TrainCarType.LocoS060) > 0) {
                        replacements = NonSteamLiveries.FindAll(L => L.v1 == TrainCarType.LocoShunter || L.v1 == TrainCarType.LocoDM3);
                        liveries.Add(replacements[(new Random()).Next(replacements.Count)]);
                    }
                }
            }
        }

        static void CacheLiveries() 
        {
            foreach (TrainCarLivery livery in Globals.G.Types.Liveries) {
                if (CarTypes.IsSteamLocomotive(livery) || livery.v1 == TrainCarType.Tender) {
                    SteamLiveries.Add(livery);
                } else {
                    NonSteamLiveries.Add(livery);
                }
            }
        }
    }
}
