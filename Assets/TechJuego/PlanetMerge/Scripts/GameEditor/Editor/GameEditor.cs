using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using System;
using TechJuego.FruitSliceMerge.Monetization;
using TechJuego.FruitSliceMerge.Sound;

namespace TechJuego.FruitSliceMerge
{
    /// <summary>
    /// Custom Editor Window for managing game settings like sound, ads, and about section.
    /// </summary>
    [InitializeOnLoad]
    public class GameEditor : EditorWindow
    {
        private Vector2 scrollViewVector;
        private static int selected;
        public Texture SameRotate;
        private string[] toolbarStrings = { "Gameplay","Sound", "Ads", "Rateus", "About" };
        private static GameEditor window;
        /// <summary>
        /// Initializes and opens the Game Editor window.
        /// </summary>
        [MenuItem("Tech Juego/Game editor and settings")]
        public static void Init()
        {
            // Get existing open window or if none, make a new one:
            window = (GameEditor)GetWindow(typeof(GameEditor), false, "Game editor");
            window.Show();
        }
        /// <summary>
        /// Draws the custom editor UI.
        /// </summary>
        private void OnGUI()
        {
            GUI.changed = false;
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            int oldSelected = selected;
            selected = GUILayout.Toolbar(selected, toolbarStrings, GUILayout.Width(450));
            GUILayout.EndHorizontal();
            scrollViewVector = GUI.BeginScrollView(new Rect(0, 45, position.width, position.height), scrollViewVector, new Rect(0, 0, 500, 1600));
            GUILayout.Space(-30);
            if (oldSelected != selected)
                scrollViewVector = Vector2.zero;
            if (toolbarStrings[selected] == "Gameplay")
            {
                GameplayDataEditor.ShowGameplay();
            }
            if (toolbarStrings[selected] == "Sound")
            {
                SoundEditor.ShowSound();
            }
            else if (toolbarStrings[selected] == "About")
            {
                AboutEditor.ShowAbout();
            }
            else if (toolbarStrings[selected] == "Ads")
            {

#if UNITY_WEBGL
                WebglAdsEditor.ShowMonetization();
#endif
#if UNITY_ANDROID || UNITY_IPHONE
                MobileAdsEditor.ShowMonetization();
#endif

            }
            else if (toolbarStrings[selected] == "Rateus")
            {
                RateUsEditor.ShowRateUs();
            }
            GUI.EndScrollView();
            if (GUI.changed && !EditorApplication.isPlaying)
                EditorSceneManager.MarkAllScenesDirty();
        }


    }
}