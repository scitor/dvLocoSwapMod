using UnityModManagerNet;

namespace dvSteamOnly
{
    public class SteamOnlySettings : UnityModManager.ModSettings, IDrawable
    {
        [Draw("Steam Only (ON) or Diesel Only (OFF)")]
        public bool SteamOnly = true;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

        public void OnChange()
        {
        }
    }
}
