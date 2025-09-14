using System.Collections.Generic;
using UnityEngine;
namespace TechJuego.FruitSliceMerge
{
    public class GameplayDataHolder : ScriptableObject
    {
        public List<MergeItem> mergeItems = new List<MergeItem>();
        public GameplayDataHolder DeepCopy()
        {
            var other = (GameplayDataHolder)MemberwiseClone();
            return other;
        }
    }
}