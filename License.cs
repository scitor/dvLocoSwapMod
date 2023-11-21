using DV.Booklets;
using DV.ThingTypes.TransitionHelpers;
using DV.ThingTypes;
using System.Collections;
using UnityEngine;

namespace dvLocoSwapMod
{
    class License
    {
        internal static IEnumerator CheckLicenseCoro()
        {
            yield return new WaitForSeconds(1);

            LicenseManager instance = LicenseManager.Instance;
            if (!instance.IsLicensedForCar(TrainCarType.LocoS060.ToV2()) &&
                !instance.IsLicensedForCar(TrainCarType.LocoSteamHeavy.ToV2())
            ) {
                instance.AcquireGeneralLicense(GeneralLicenseType.S060.ToV2());
                BookletCreator.CreateLicense(GeneralLicenseType.S060.ToV2(), Vector3.zero, Quaternion.identity);
            }
        }
    }
}
