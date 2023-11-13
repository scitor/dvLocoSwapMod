using System.Reflection;
using HarmonyLib;
using UnityModManagerNet;

namespace dvSteamOnly;

public static class Main
{
    public static SteamOnlySettings Settings { get; private set; }

    // Unity Mod Manage Wiki: https://wiki.nexusmods.com/index.php/Category:Unity_Mod_Manager
    private static bool Load(UnityModManager.ModEntry modEntry)
    {
        Settings = UnityModManager.ModSettings.Load<SteamOnlySettings>(modEntry);
        modEntry.OnGUI = DrawGUI;
        modEntry.OnSaveGUI = SaveGUI;
        modEntry.OnToggle = OnToggle;

        return true;
    }

    static void DrawGUI(UnityModManager.ModEntry entry)
    {
        Settings.Draw(entry);
    }

    static void SaveGUI(UnityModManager.ModEntry entry)
    {
        Settings.Save(entry);
    }

    private static bool OnToggle(UnityModManager.ModEntry modEntry, bool active)
    {
        Harmony harmony = new Harmony(modEntry.Info.Id);
        if (active) {
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        } else {
            harmony.UnpatchAll(modEntry.Info.Id);
        }
        return true;
    }
}
