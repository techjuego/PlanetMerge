using UnityEditor;
using UnityEngine;
namespace TechJuego.PlanetMerge
{
    public class GameplayDataEditor
    {
        private static GameplayDataHolder gameplayDataHolder;
        public static void ShowGameplay()
        {
            if (gameplayDataHolder == null)
            {
                gameplayDataHolder = Resources.Load("Gameplay/GameplayDataHolder") as GameplayDataHolder;
                if (gameplayDataHolder == null)
                {
                    CreateGameplayData();
                    gameplayDataHolder = Resources.Load("Gameplay/GameplayDataHolder") as GameplayDataHolder;
                }
            }


            for (int i = 0; i < gameplayDataHolder.mergeItems.Count; i++)
            {
                int no = i;
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(new GUIContent("X"), GUILayout.Width(30)))
                {
                    gameplayDataHolder.mergeItems.RemoveAt(no);
                    EditorUtility.SetDirty(gameplayDataHolder);
                    AssetDatabase.SaveAssets();
                }
                GUILayout.Label(new GUIContent((i + 1).ToString()), GUILayout.Width(30));
                if (no < gameplayDataHolder.mergeItems.Count)
                {
                    gameplayDataHolder.mergeItems[no] = (MergeItem)EditorGUILayout.ObjectField("", gameplayDataHolder.mergeItems[no], typeof(MergeItem), false, GUILayout.Width(100));
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            if (GUILayout.Button(new GUIContent("Add Item"), GUILayout.Width(80)))
            {
                gameplayDataHolder.mergeItems.Add(null);
                EditorUtility.SetDirty(gameplayDataHolder);
                AssetDatabase.SaveAssets();
            }
            if (GUILayout.Button(new GUIContent("Save"), GUILayout.Width(80)))
            {
                EditorUtility.SetDirty(gameplayDataHolder);
                AssetDatabase.SaveAssets();
            }
        }
        private static void CreateGameplayData()
        {
            GameplayDataHolder asset = ScriptableObject.CreateInstance<GameplayDataHolder>();
            if (!AssetDatabase.IsValidFolder(GamePath.ResourcePath + "/Gameplay/"))
            {
                AssetDatabase.CreateFolder(GamePath.ResourcePath, "Gameplay");
                AssetDatabase.Refresh();
            }
            AssetDatabase.CreateAsset(asset, GamePath.ResourcePath + "/Gameplay/GameplayDataHolder.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}