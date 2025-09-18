using System;
using TechJuego.PlanetMerge.Monetization;
using UnityEditor;
using UnityEngine;
namespace TechJuego.PlanetMerge
{
    public class MobileAdsEditor
    {
        private static MobileAdsData m_MobileAdsData;
        public static void ShowMonetization()
        {
            if (m_MobileAdsData == null)
            {
                m_MobileAdsData = Resources.Load("Monetization/MobileAdsData") as MobileAdsData;
                if (m_MobileAdsData == null)
                {
                    CreateMonetizationSettings();
                    m_MobileAdsData = Resources.Load("Monetization/MobileAdsData") as MobileAdsData;
                }
            }
            if (m_MobileAdsData != null)
            {
                ShowProviderToAdd();
                ShowAdProviderAppID();
                MonitizationID();
                ShowAdCallEvent();
            }
        }
        private static void ShowProviderToAdd()
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Select Ad Provider Which you want to Use");
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label("   ||   ", GUILayout.Width(50));
            GUILayout.Label("Unity", GUILayout.Width(40));
            m_MobileAdsData.isUnityPresent = EditorGUILayout.Toggle(m_MobileAdsData.isUnityPresent, GUILayout.Width(40));
            if (m_MobileAdsData.isUnityPresent)
            {
                if (!m_MobileAdsData.providerAdded.Contains("Unity"))
                {
                    m_MobileAdsData.providerAdded.Add("Unity");
                }
                EditorUtility.SetDirty(m_MobileAdsData);
            }
            else
            {
                if (m_MobileAdsData.providerAdded.Contains("Unity"))
                {
                    m_MobileAdsData.providerAdded.Remove("Unity");
                }
                EditorUtility.SetDirty(m_MobileAdsData);
            }
            if (GUILayout.Button(new GUIContent("Add  Symbol"), GUILayout.Width(100)))
            {
                AddDefineSymbolToGroup(BuildTargetGroup.Android, "UnityAds");
                AddDefineSymbolToGroup(BuildTargetGroup.iOS, "UnityAds");
                AddDefineSymbolToGroup(BuildTargetGroup.Standalone, "UnityAds");
            }
            if (GUILayout.Button(new GUIContent("Remove Symbol"), GUILayout.Width(110)))
            {
                RemoveDefineSymbolToGroup(BuildTargetGroup.Android, "UnityAds");
                RemoveDefineSymbolToGroup(BuildTargetGroup.iOS, "UnityAds");
                RemoveDefineSymbolToGroup(BuildTargetGroup.Standalone, "UnityAds");
            }
            GUILayout.Label("||", GUILayout.Width(50));
            GUILayout.Label("Admob", GUILayout.Width(50));
            m_MobileAdsData.isAdmobPresent = EditorGUILayout.Toggle(m_MobileAdsData.isAdmobPresent, GUILayout.Width(40));
            if (m_MobileAdsData.isAdmobPresent)
            {
                if (!m_MobileAdsData.providerAdded.Contains("Admob"))
                {
                    m_MobileAdsData.providerAdded.Add("Admob");
                }

                EditorUtility.SetDirty(m_MobileAdsData);
            }
            else
            {
                if (m_MobileAdsData.providerAdded.Contains("Admob"))
                {
                    m_MobileAdsData.providerAdded.Remove("Admob");
                }
                EditorUtility.SetDirty(m_MobileAdsData);
            }
            if (GUILayout.Button(new GUIContent("Add  Symbol"), GUILayout.Width(150)))
            {
                AddDefineSymbolToGroup(BuildTargetGroup.Android, "ADMOB");
                AddDefineSymbolToGroup(BuildTargetGroup.iOS, "ADMOB");
                AddDefineSymbolToGroup(BuildTargetGroup.Standalone, "ADMOB");
            }
            if (GUILayout.Button(new GUIContent("Remove Symbol"), GUILayout.Width(150)))
            {
                RemoveDefineSymbolToGroup(BuildTargetGroup.Android, "ADMOB");
                RemoveDefineSymbolToGroup(BuildTargetGroup.iOS, "ADMOB");
                RemoveDefineSymbolToGroup(BuildTargetGroup.Standalone, "ADMOB");
            }
            GUILayout.Label("   ||   ", GUILayout.Width(50));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
        /// <summary>
        /// Displays monetization ID input fields.
        /// </summary>
        private static void ShowAdProviderAppID()
        {
            if (m_MobileAdsData.providerAdded.Count > 0)
            {
                GUILayout.Label("=============================================================================================================");
                if (m_MobileAdsData.isAdmobPresent)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Admob App ID:--", GUILayout.Width(100));
                    GUILayout.Label("Android", GUILayout.Width(60));
                    m_MobileAdsData.AdmobAppID_Android = EditorGUILayout.TextField(m_MobileAdsData.AdmobAppID_Android, GUILayout.Width(200));
                    GUILayout.Space(30);
                    GUILayout.Label("IOS", GUILayout.Width(30));
                    m_MobileAdsData.AdmobAppID_IOS = EditorGUILayout.TextField(m_MobileAdsData.AdmobAppID_IOS, GUILayout.Width(200));
                    if (GUILayout.Button(new GUIContent("Get Admob Ad App ID"), GUILayout.Width(150)))
                    {
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.Space(10);
                if (m_MobileAdsData.isUnityPresent)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Unity App ID:--", GUILayout.Width(100));
                    GUILayout.Label("Android", GUILayout.Width(60));
                    m_MobileAdsData.UnityAppID_Android = EditorGUILayout.TextField(m_MobileAdsData.UnityAppID_Android, GUILayout.Width(200));
                    GUILayout.Space(30);
                    GUILayout.Label("IOS", GUILayout.Width(30));
                    m_MobileAdsData.UnityAppID_IOS = EditorGUILayout.TextField(m_MobileAdsData.UnityAppID_IOS, GUILayout.Width(200));
                    if (GUILayout.Button(new GUIContent("Get Unity Ad App ID"), GUILayout.Width(150)))
                    {
                    }
                    GUILayout.EndHorizontal();
                }
            }
        }
        private static void MonitizationID()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Space(20);
                if (m_MobileAdsData.providerAdded.Count > 0)
                {
                    GUILayout.Label("=============================================================================================================");
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("", GUILayout.Width(29));
                    GUILayout.Label("Ad Provider", GUILayout.Width(100));
                    GUILayout.Label("Ad Type", GUILayout.Width(100));
                    GUILayout.Label("Android Ad ID", GUILayout.Width(280));
                    GUILayout.Label("iOS Ad ID", GUILayout.Width(280));
                    GUILayout.EndHorizontal();
                    for (int i = 0; i < m_MobileAdsData.monitizationAds.Count; i++)
                    {
                        int no = i;
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button(new GUIContent("X"), GUILayout.Width(30)))
                        {
                            m_MobileAdsData.monitizationAds.RemoveAt(no);
                            EditorUtility.SetDirty(m_MobileAdsData);
                            AssetDatabase.SaveAssets();
                        }
                        if (no < m_MobileAdsData.monitizationAds.Count)
                        {
                            string[] provider = m_MobileAdsData.providerAdded.ToArray();
                            int selectedIndex = Mathf.Max(0, Array.IndexOf(provider, m_MobileAdsData.monitizationAds[i].providers));
                            selectedIndex = EditorGUILayout.Popup(selectedIndex, provider, GUILayout.Width(100));
                            m_MobileAdsData.monitizationAds[i].providers = provider[selectedIndex];
                            m_MobileAdsData.monitizationAds[no].AdType = (AdType)EditorGUILayout.EnumPopup(m_MobileAdsData.monitizationAds[no].AdType, GUILayout.Width(100));
                            m_MobileAdsData.monitizationAds[no].Android_ID = EditorGUILayout.TextField(m_MobileAdsData.monitizationAds[no].Android_ID, GUILayout.Width(280));
                            m_MobileAdsData.monitizationAds[no].IOS_ID = EditorGUILayout.TextField(m_MobileAdsData.monitizationAds[no].IOS_ID, GUILayout.Width(280));
                        }
                        GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(new GUIContent("Add Ad ID"), GUILayout.Width(80)))
                    {
                        m_MobileAdsData.monitizationAds.Add(new() { AdType = AdType.Interstitial, Android_ID = "", IOS_ID = "" });
                        EditorUtility.SetDirty(m_MobileAdsData);
                        AssetDatabase.SaveAssets();
                    }
                    if (GUILayout.Button(new GUIContent("Save"), GUILayout.Width(80)))
                    {
                        EditorUtility.SetDirty(m_MobileAdsData);
                        AssetDatabase.SaveAssets();
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.Label("=============================================================================================================");
                GUILayout.EndVertical();
            }
        }
        private static void ShowAdCallEvent()
        {
            if (m_MobileAdsData.providerAdded.Count > 0)
            {
                GUILayout.BeginVertical();
                {
                    GUILayout.Label("Add Call Events ============================================================================================");
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(" ", GUILayout.Width(30));
                    GUILayout.Label("When To Call", GUILayout.Width(100));
                    GUILayout.Label("Call Every Level", GUILayout.Width(100));
                    GUILayout.EndHorizontal();
                    if (m_MobileAdsData.monitizationAds.Count > 0)
                    {
                        for (int i = 0; i < m_MobileAdsData.adsEvents.Count; i++)
                        {
                            GUILayout.BeginHorizontal();
                            if (GUILayout.Button(new GUIContent("X"), GUILayout.Width(30)))
                            {
                                m_MobileAdsData.adsEvents.RemoveAt(i);
                                EditorUtility.SetDirty(m_MobileAdsData);
                                AssetDatabase.SaveAssets();
                            }
                            if (i < m_MobileAdsData.adsEvents.Count)
                            {
                                m_MobileAdsData.adsEvents[i].gameEvent = (GameState)EditorGUILayout.EnumPopup(m_MobileAdsData.adsEvents[i].gameEvent, GUILayout.Width(100));
                                if (m_MobileAdsData.adsEvents[i].AddToCall == AdType.Banner)
                                {
                                    m_MobileAdsData.adsEvents[i].AddToCall = AdType.Interstitial;
                                }
                                m_MobileAdsData.adsEvents[i].AddToCall = (AdType)EditorGUILayout.EnumPopup(m_MobileAdsData.adsEvents[i].AddToCall, GUILayout.Width(100));
                                m_MobileAdsData.adsEvents[i].everyLevel = EditorGUILayout.IntPopup(m_MobileAdsData.adsEvents[i].everyLevel, new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" }, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, GUILayout.Width(70));
                            }
                            GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                        }
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button(new GUIContent("Add Call Event"), GUILayout.Width(100)))
                        {
                            m_MobileAdsData.adsEvents.Add(new() { everyLevel = 1, gameEvent = GameState.None });
                            EditorUtility.SetDirty(m_MobileAdsData);
                            AssetDatabase.SaveAssets();
                        }
                        if (GUILayout.Button(new GUIContent("Save"), GUILayout.Width(80)))
                        {
                            EditorUtility.SetDirty(m_MobileAdsData);
                            AssetDatabase.SaveAssets();
                        }
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.Label("========================================================================================================");
                }
                GUILayout.EndVertical();
            }
        }
        private static void AddDefineSymbolToGroup(BuildTargetGroup targetGroup, string symbol)
        {
            // Get existing symbols
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
            // Check if symbol already exists
            if (!defines.Contains(symbol))
            {
                // Add new symbol
                defines += $";{symbol}";
                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defines);
            }
        }
        private static void RemoveDefineSymbolToGroup(BuildTargetGroup targetGroup, string symbol)
        {
            // Get existing symbols
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);

            // Check if the symbol exists
            if (defines.Contains(symbol))
            {
                // Remove the symbol safely
                string updatedDefines = defines.Replace(symbol, "").Replace(";;", ";").Trim(';');
                // Apply the updated define symbols
                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, updatedDefines);
                Debug.Log($"Removed '{symbol}' from {targetGroup}");
            }
        }
        private static void CreateMonetizationSettings()
        {
            MobileAdsData asset = ScriptableObject.CreateInstance<MobileAdsData>();
            if (!AssetDatabase.IsValidFolder(GamePath.ResourcePath + "/Monetization/"))
            {
                AssetDatabase.CreateFolder(GamePath.ResourcePath, "Monetization");
                AssetDatabase.Refresh();
            }
            AssetDatabase.CreateAsset(asset, GamePath.ResourcePath + "/Monetization/MobileAdsData.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
     
    }
}