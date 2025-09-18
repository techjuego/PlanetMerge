using System.Collections;
using System.Collections.Generic;
using TechJuego.PlanetMerge.Monetization;
using UnityEditor;
using UnityEngine;

namespace TechJuego.PlanetMerge
{
    public class WebglAdsEditor
    {
        private static WebglAdsData m_WebglAdsData;

        // Entry point for the editor window
        public static void ShowMonetization()
        {
            if (m_WebglAdsData == null)
            {
                m_WebglAdsData = Resources.Load("Monetization/WebglAdsData") as WebglAdsData;
                if (m_WebglAdsData == null)
                {
                    CreateMonetizationSettings();
                    m_WebglAdsData = Resources.Load("Monetization/WebglAdsData") as WebglAdsData;
                }
            }

            if (m_WebglAdsData != null)
            {
                ShowWebglSettings();
            }
        }

        // Create WebglAdsData asset if it doesn't exist
        private static void CreateMonetizationSettings()
        {
            WebglAdsData asset = ScriptableObject.CreateInstance<WebglAdsData>();
            string folderPath = GamePath.ResourcePath + "/Monetization/";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder(GamePath.ResourcePath, "Monetization");
                AssetDatabase.Refresh();
            }
            AssetDatabase.CreateAsset(asset, folderPath + "WebglAdsData.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        // Add a define symbol to the WebGL build target group
        private static void AddDefineSymbolToGroup(BuildTargetGroup targetGroup, string symbol)
        {
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
            if (!defines.Contains(symbol))
            {
                defines += $";{symbol}";
                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defines);
            }
        }

        // Remove a define symbol from the WebGL build target group
        private static void RemoveDefineSymbolFromGroup(BuildTargetGroup targetGroup, string symbol)
        {
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
            if (defines.Contains(symbol))
            {
                string updatedDefines = defines.Replace(symbol, "").Replace(";;", ";").Trim(';');
                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, updatedDefines);
                Debug.Log($"Removed '{symbol}' from {targetGroup}");
            }
        }

        // Show WebGL Settings in the editor
        static void ShowWebglSettings()
        {
            GUILayout.Space(10);

            GUILayout.BeginVertical("box"); // Wrap content in a box for better structure
            {
                // Define Ad Platforms Section
                DrawAdPlatformSection("GamePix", WebglSymbols.GamePix);
                DrawAdPlatformSection("CrazyGames", WebglSymbols.CrazyGames);
                DrawAdPlatformSection("Yandex", WebglSymbols.Yandex);
                DrawAdPlatformSection("GameMonetize", WebglSymbols.GameMonetize);
                DrawAdPlatformSection("WGPlayground", WebglSymbols.WGPlayground);
                DrawAdPlatformSection("GameArter", WebglSymbols.GameArter);
                DrawAdPlatformSection("GameDISTRIBUTION", WebglSymbols.GAMEDISTRIBUTION);

                GUILayout.Space(20); // Space for separation between sections
                GUILayout.Label("Add Call Events", EditorStyles.boldLabel);
                GUILayout.Label("========================================================================================================");

                // Ad Events Section
                DrawAdEventsSection();

                GUILayout.Space(20); // Padding
            }
            GUILayout.EndVertical();
        }

        // Helper function to draw each ad platform section
        private static void DrawAdPlatformSection(string platformName, string symbol)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(platformName, GUILayout.Width(120));

            if (GUILayout.Button(new GUIContent("Add"), GUILayout.Width(100)))
            {
                AddDefineSymbolToGroup(BuildTargetGroup.WebGL, symbol);
            }
            if (GUILayout.Button(new GUIContent("Remove"), GUILayout.Width(100)))
            {
                RemoveDefineSymbolFromGroup(BuildTargetGroup.WebGL, symbol);
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        // Draw the section for adding and managing Ad Events
        private static void DrawAdEventsSection()
        {
            GUILayout.BeginVertical("box"); // Wrap the ad event content in a box for a cleaner look

            // Column headers
            GUILayout.BeginHorizontal();
            GUILayout.Label("Game Event", GUILayout.Width(120));
            GUILayout.Label("Ad Type", GUILayout.Width(100));
            GUILayout.Label("Call Every Level", GUILayout.Width(120));
            GUILayout.EndHorizontal();

            // List of Ad Events
            if (m_WebglAdsData.webglAdEvents.Count > 0)
            {
                for (int i = 0; i < m_WebglAdsData.webglAdEvents.Count; i++)
                {
                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button(new GUIContent("X"), GUILayout.Width(30)))
                    {
                        m_WebglAdsData.webglAdEvents.RemoveAt(i);
                        EditorUtility.SetDirty(m_WebglAdsData);
                        AssetDatabase.SaveAssets();
                    }

                    if (i < m_WebglAdsData.webglAdEvents.Count)
                    {
                        m_WebglAdsData.webglAdEvents[i].gameEvent = (GameState)EditorGUILayout.EnumPopup(m_WebglAdsData.webglAdEvents[i].gameEvent, GUILayout.Width(120));
                        m_WebglAdsData.webglAdEvents[i].AddToCall = (AdType)EditorGUILayout.EnumPopup(m_WebglAdsData.webglAdEvents[i].AddToCall, GUILayout.Width(100));
                        m_WebglAdsData.webglAdEvents[i].everyLevel = EditorGUILayout.IntPopup(m_WebglAdsData.webglAdEvents[i].everyLevel,
                            new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" },
                            new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                            GUILayout.Width(100));
                    }

                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
            }

            // Button to add new ad event
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Add Call Event"), GUILayout.Width(120)))
            {
                m_WebglAdsData.webglAdEvents.Add(new() { everyLevel = 1, gameEvent = GameState.None });
                EditorUtility.SetDirty(m_WebglAdsData);
                AssetDatabase.SaveAssets();
            }

            // Save button for all changes
            if (GUILayout.Button(new GUIContent("Save"), GUILayout.Width(80)))
            {
                EditorUtility.SetDirty(m_WebglAdsData);
                AssetDatabase.SaveAssets();
            }

            GUILayout.EndHorizontal();

            GUILayout.Label("========================================================================================================");
            GUILayout.EndVertical();
        }
    }
}
