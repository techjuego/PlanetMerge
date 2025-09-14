using UnityEngine;
namespace TechJuego.FruitSliceMerge.Utils
{
    public class ElementBase : MonoBehaviour
    {
        [HideInInspector]
        public string ElementID;
        [HideInInspector]
        public int CellIndex;
        [HideInInspector]
        public int ElementIndex;
        [HideInInspector]
        public bool IsElementActive;
        public virtual void RefreshScrollElement() { }
        [HideInInspector]
        public bool Selected;
        public float? LastElementSize;
        public virtual void ResetSize() { LastElementSize = null; }

        public virtual void DeselectElement() { }
    }
}