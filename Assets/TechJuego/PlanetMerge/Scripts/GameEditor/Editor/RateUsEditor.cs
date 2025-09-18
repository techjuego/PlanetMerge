using System.Collections;
using System.Collections.Generic;
using TechJuego.PlanetMerge.Rateus;
using UnityEditor;
using UnityEngine;
namespace TechJuego.PlanetMerge
{
    public class RateUsEditor
    {
        private static RateUsData rateSettings;
        public static void ShowRateUs()
        {
            rateSettings = Resources.Load<RateUsData>("Rateus/RateUsSetting");
            if (rateSettings == null)
            {
                CreateRateusSettings();
                rateSettings = Resources.Load<RateUsData>("Rateus/RateUsSetting");
            }
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Your App IDs:", EditorStyles.boldLabel);
            rateSettings.iosAppID = EditorGUILayout.TextField("iOS App ID", rateSettings.iosAppID, GUILayout.Width(500));
            rateSettings.googlePlayBundleID = EditorGUILayout.TextField("Google Play bundle ID", rateSettings.googlePlayBundleID, GUILayout.Width(500));
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("When To Show Popup", GUILayout.Width(150));
            rateSettings.WhenToShow = (GameState)EditorGUILayout.EnumPopup(rateSettings.WhenToShow, GUILayout.Width(140));
            EditorGUILayout.LabelField("||", GUILayout.Width(20));
            EditorGUILayout.LabelField("Call on every", GUILayout.Width(100));
            rateSettings.CallOnEvery = EditorGUILayout.IntPopup(rateSettings.CallOnEvery, new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" }, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, GUILayout.Width(70));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button(new GUIContent("Save"), GUILayout.Width(80)))
            {
                EditorUtility.SetDirty(rateSettings);
                AssetDatabase.SaveAssets();
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }
        private static void CreateRateusSettings()
        {
            RateUsData asset = ScriptableObject.CreateInstance<RateUsData>();
            if (!AssetDatabase.IsValidFolder(GamePath.ResourcePath + "/Rateus/"))
            {
                AssetDatabase.CreateFolder(GamePath.ResourcePath, "Rateus");
                AssetDatabase.Refresh();
            }
            AssetDatabase.CreateAsset(asset, GamePath.ResourcePath + "/Rateus/RateUsSetting.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}