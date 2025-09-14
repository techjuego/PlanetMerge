using UnityEngine;
namespace TechJuego.FruitSliceMerge
{
    public static class ExtensionMethods
    {
        public static string ToJson(this object value)
        {
            return JsonUtility.ToJson(value);
        }
        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
        public static void DestroyChildren(this GameObject parent)
        {
            Transform[] children = new Transform[parent.transform.childCount];
            for (int i = 0; i < parent.transform.childCount; i++)
                children[i] = parent.transform.GetChild(i);
            for (int i = 0; i < children.Length; i++)
                GameObject.Destroy(children[i].gameObject);
        }
        public static Transform FindRecursive(this Transform trm)
        {
            Transform child = null;
            // Loop through top level
            foreach (Transform t in trm)
            {
                if (t.name.Contains("(Clone)"))
                {
                    t.name = t.name.Replace("(Clone)", "");
                    child = t;
                    return child;
                }
                if (t.childCount > 0)
                {
                    child = t.FindRecursive();
                    if (child)
                    {
                        return child;
                    }
                }
            }
            return child;
        }

    }
}