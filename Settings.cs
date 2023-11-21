using UnityModManagerNet;

namespace dvLocoSwapMod
{
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Draw("Steam Only (ON) or Diesel Only (OFF)")]
        public bool SteamOnly = true;

        [Draw("Ensure steam license in \"Steam Only\" mode")]
        public bool EnsureSteamLicense = true;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

        public void OnChange()
        {
        }
    }
}
