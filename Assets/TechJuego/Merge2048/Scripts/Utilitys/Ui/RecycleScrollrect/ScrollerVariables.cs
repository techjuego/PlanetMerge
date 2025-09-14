using UnityEngine;

namespace TechJuego.FruitSliceMerge.Utils
{
    /// <summary>
    /// This Script used to add starting value for scroll
    /// </summary>
    [DisallowMultipleComponent]
    public class ScrollerVariables : MonoBehaviour
    {
        /// <summary>
        /// show data from ented starting index
        /// </summary>
        public int m_StartIndex;
        /// <summary>
        /// paddin in elements
        /// </summary>
        public RectOffset m_Padding;
        /// <summary>
        /// space between scroll elements
        /// </summary>
        public float ElementSpacing = 10;
        public float MaxVelocity;
        public bool SnapUseCellSpacing;
        public bool SnapWhileDragging;
        public float ScrollPosition;
        public float SnapVelocityThreshold;
        public float SnapWatchOffset, SnapJumpToOffset, SnapCellCenterOffset, SnapTweenTime;
    }
}