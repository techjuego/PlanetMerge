using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace TechJuego.FruitSliceMerge
{

    public class AboutEditor : MonoBehaviour
    {
        private static Texture techjuegoIcon;

        public static void ShowAbout()
        {
            if (techjuegoIcon == null)
            {
                techjuegoIcon = Resources.Load("Graphics/techjuegoIcon") as Texture;
            }
            GUILayout.Label(techjuegoIcon);
            GUILayout.Label("Connect with us:");
            EditorGUILayout.SelectableLabel("techjuego@gmail.com");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Website", GUILayout.Width(200)))
            {
                Application.OpenURL("techjuego.com");
            }
            if (GUILayout.Button("Youtube", GUILayout.Width(200)))
            {
                Socialmedia.SubscribeOnYoutube();
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Discord", GUILayout.Width(200)))
            {
                Socialmedia.ConnectOnDiscord();
            }
            if (GUILayout.Button(" Twitter", GUILayout.Width(200)))
            {
                Socialmedia.FollowOnTweeter();
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(" Facebook", GUILayout.Width(200)))
            {
                Socialmedia.FollowOnFacebook();
            }
            if (GUILayout.Button("Instagram", GUILayout.Width(200)))
            {
                Socialmedia.FollowOnInstagram();
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
            if (GUILayout.Button("Open Asset Store Publisher Page", GUILayout.Width(400)))
            {
                Application.OpenURL("https://assetstore.unity.com/publishers/46402");
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }
    }
}