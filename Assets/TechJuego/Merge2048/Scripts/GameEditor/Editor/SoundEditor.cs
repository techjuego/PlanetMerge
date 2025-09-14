using System.Collections;
using System.Collections.Generic;
using TechJuego.FruitSliceMerge.Sound;
using UnityEditor;
using UnityEngine;
namespace TechJuego.FruitSliceMerge
{
    public class SoundEditor
    {
        private static SoundsHolder soundsHolder;
        public static void ShowSound()
        {
            if (soundsHolder == null)
            {
                soundsHolder = Resources.Load("Sounds/SoundsHolder") as SoundsHolder;
                if (soundsHolder == null)
                {
                    CreateSoundSettings();
                    soundsHolder = Resources.Load("Sounds/SoundsHolder") as SoundsHolder;
                }
            }
            if (soundsHolder != null)
            {
                GUILayout.BeginVertical();
                GUILayout.Label("SFX");
                for (int i = 0; i < soundsHolder.soundClips.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label((i + 1) + ".");
                    GUILayout.Label("Clip name");
                    soundsHolder.soundClips[i].clipName = EditorGUILayout.TextField("", soundsHolder.soundClips[i].clipName, GUILayout.Width(100));
                    GUILayout.Label("Clip");
                    soundsHolder.soundClips[i].clip = (AudioClip)EditorGUILayout.ObjectField("", soundsHolder.soundClips[i].clip, typeof(AudioClip), false, GUILayout.Width(100));
                    GUILayout.Label("Volume");
                    soundsHolder.soundClips[i].volume = EditorGUILayout.Slider(soundsHolder.soundClips[i].volume, 0, 1, GUILayout.Width(150));
                    if (GUILayout.Button(new GUIContent("X", "X"), GUILayout.Width(30)))
                    {
                        soundsHolder.soundClips.RemoveAt(i);
                        EditorUtility.SetDirty(soundsHolder);
                        AssetDatabase.SaveAssets();
                    }
                    GUILayout.Space(Screen.width);
                    GUILayout.EndHorizontal();
                }
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(new GUIContent("Add Sound", "Add Sound"), GUILayout.Width(80)))
                {
                    soundsHolder.soundClips.Add(new SoundClips());
                    EditorUtility.SetDirty(soundsHolder);
                    AssetDatabase.SaveAssets();
                }
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                GUILayout.BeginVertical();
                GUILayout.Label("Music");
                for (int i = 0; i < soundsHolder.musicClip.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label((i + 1) + ".");
                    GUILayout.Label("Clip name");
                    soundsHolder.musicClip[i].clipName = EditorGUILayout.TextField("", soundsHolder.musicClip[i].clipName, GUILayout.Width(100));
                    GUILayout.Label("Clip");
                    soundsHolder.musicClip[i].clip = (AudioClip)EditorGUILayout.ObjectField("", soundsHolder.musicClip[i].clip, typeof(AudioClip), false, GUILayout.Width(100));
                    GUILayout.Label("Volume");
                    soundsHolder.musicClip[i].volume = EditorGUILayout.Slider(soundsHolder.musicClip[i].volume, 0, 1, GUILayout.Width(150));
                    if (GUILayout.Button(new GUIContent("X", "X"), GUILayout.Width(30)))
                    {
                        soundsHolder.musicClip.RemoveAt(i);
                        EditorUtility.SetDirty(soundsHolder);
                        AssetDatabase.SaveAssets();
                    }
                    GUILayout.Space(Screen.width);
                    GUILayout.EndHorizontal();
                }
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(new GUIContent("Add Music", "Add Music"), GUILayout.Width(80)))
                {
                    soundsHolder.musicClip.Add(new SoundClips());
                    EditorUtility.SetDirty(soundsHolder);
                    AssetDatabase.SaveAssets();
                }

                GUILayout.EndHorizontal();
                if (GUILayout.Button(new GUIContent("Save", "Save"), GUILayout.Width(80)))
                {
                    EditorUtility.SetDirty(soundsHolder);
                    AssetDatabase.SaveAssets();
                }
                GUILayout.EndVertical();
            }
        }
        private static void CreateSoundSettings()
        {
            SoundsHolder asset = ScriptableObject.CreateInstance<SoundsHolder>();
            if (!AssetDatabase.IsValidFolder(GamePath.ResourcePath + "/Sounds/"))
            {
                AssetDatabase.CreateFolder(GamePath.ResourcePath, "Sounds");
                AssetDatabase.Refresh();
            }
            AssetDatabase.CreateAsset(asset, GamePath.ResourcePath + "/Sounds/SoundsHolder.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}