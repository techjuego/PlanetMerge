using System.Collections.Generic;
using UnityEngine;
namespace TechJuego.PlanetMerge.Monetization
{
    public class MobileAdsData : ScriptableObject
    {
        public bool isUnityPresent;
        public bool isAdmobPresent;
        public List<string> providerAdded = new List<string>();
        public List<AdEvents> adsEvents = new List<AdEvents>();
        public List<MonitizationAds> monitizationAds = new List<MonitizationAds>();

        public string AdmobAppID_Android;
        public string AdmobAppID_IOS;

        public string UnityAppID_Android;
        public string UnityAppID_IOS;
    }
}