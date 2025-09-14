using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace TechJuego.FruitSliceMerge.Utils
{
    public interface IUGetListDataInterface
    {
        int GetElementCount(RecycleScroll scroller);
        float GetRectSize(RecycleScroll scroller, int dataIndex);
        ElementBase GetScrollElement(RecycleScroll scroller, int dataIndex, int cellIndex);
    }

    [DisallowMultipleComponent]
    [RequireComponent(typeof(ScrollerVariables))]
    public class RecycleScroll : ScrollRect
    {
        /// <summary>
        /// Events to access if any extra operation to be perfrom 
        /// </summary>
        public Action<ElementBase> OnSelect;
        public Action<int> OnSelectint;
        public Action<int> OnSelectString;
        public Action<ElementBase> OnElementWillPool;
        public Action<RecycleScroll, Vector2, float> OnScrolled;
        public Action<ElementBase> OnActivityChange;
        public Action<RecycleScroll, int, int, ElementBase> OnScrollerSnapped;
        public Action<RecycleScroll, bool> Onscrollchanged;
        public Action<RecycleScroll, bool> OnScrollValue;
        public Action<RecycleScroll, ElementBase> OnElementInitiate;
        public Action<RecycleScroll, ElementBase> OnElementReused;
        /// Public variable //////////////////////////////////////////////////////////////////////////////////////////////
        public ScrollerVariables ScrollValue { get { return m_ScollValues; } set { m_ScollValues = value; } }
        public enum ElementPosition { Before, After }
        public int VisiableStartIndex { get { return m_ActiveElementStartIndex % NumberOfElements; } }
        public int VisiableEndIndex { get { return m_ActiveElementEndIndex % NumberOfElements; } }
        public int NumberOfElements { get { return (m_ElementEvents != null ? m_ElementEvents.GetElementCount(this) : 0); } }
        public int? Selected;
        public float ScrollRectSize { get { return vertical ? m_ScrollRectTransform.rect.height : m_ScrollRectTransform.rect.width; } }
        public float ScrollPosition;

        /// <summary>
        /// Get Scroll rect container size depend on layout
        /// </summary>
        public float ScrollSize
        {
            get
            {
                if (vertical)
                    return Mathf.Max(m_Container.rect.height - m_ScrollRectTransform.rect.height, 0);
                else
                    return Mathf.Max(m_Container.rect.width - m_ScrollRectTransform.rect.width, 0);
            }
        }
        public IUGetListDataInterface ElementInterface
        {
            get { return m_ElementEvents; }
            set { m_ElementEvents = value; m_ReloadData = true; }
        }

        /// private variable //////////////////////////////////////////////////////////////////////////////////////////////
        private ScrollerVariables m_ScollValues;
        private float m_ScrollTimeRemaining;
        private int m_SnapElementIndex, m_SnapDataIndex, m_ActiveElementStartIndex, m_ActiveElementEndIndex;
        private bool m_RouteToParent = false, m_Initialized = false, m_UpdateSpacing = false;
        private bool m_IsScrolling, m_SnapJumping, m_SnapInertia, m_SnapBeforeDrag, m_ReloadData, m_Snapping;
        private LayoutElement m_StartSpaceFiller, m_EndSpaceFiller;
        private List<ElementBase> m_PooledElements = new List<ElementBase>();
        private List<ElementBase> m_ActiveElements = new List<ElementBase>();
        private List<float> m_ElementSizeList = new List<float>();
        private List<float> m_ElementOffsetList = new List<float>();
        private enum ListPosition { First, Last }
        private HorizontalOrVerticalLayoutGroup m_LayoutGroup;
        private RectTransform m_ScrollRectTransform, m_PoolElementParent, m_Container;
        private IUGetListDataInterface m_ElementEvents;
        public float m_NormalizedScrollPosition
        {
            get
            {
                var scrollPosition = m_CurrentScrollPosition;
                return (scrollPosition <= 0 ? 0 : scrollPosition / ScrollSize);
            }
        }
        private float m_LinearVelocity
        {
            get { return (vertical ? velocity.y : velocity.x); }
            set
            {
                velocity = vertical ? new Vector2(0, value) : new Vector2(value, 0);
            }
        }
        private float m_CurrentScrollPosition
        {
            get { return ScrollPosition; }
            set
            {
                value = Mathf.Clamp(value, 0, ScrollSize); if (ScrollPosition != value)
                {
                    ScrollPosition = value;
                    if (vertical)
                    {
                        verticalNormalizedPosition = 1f - (ScrollPosition / ScrollSize);
                    }
                    else
                    {
                        horizontalNormalizedPosition = (ScrollPosition / ScrollSize);
                    }
                }
            }
        }
        // Create all recttransfrom need for scroll rect
        protected override void Awake()
        {
            if (!Application.isPlaying) return;
            m_ScollValues = GetComponent<ScrollerVariables>();
            GameObject go;
            m_ScrollRectTransform = GetComponent<RectTransform>();
            if (content != null)
            {
                DestroyImmediate(content.gameObject);
            }
            foreach (Transform item in transform)
            {
                if (item.name == "PooledElements")
                {
                    DestroyImmediate(item.gameObject);
                }
            }
            go = new GameObject("Container", typeof(RectTransform));
            go.transform.SetParent(m_ScrollRectTransform);
            if (vertical)
                go.AddComponent<VerticalLayoutGroup>();
            else
                go.AddComponent<HorizontalLayoutGroup>();
            m_Container = go.GetComponent<RectTransform>();
            m_Container.anchorMin = vertical ? new Vector2(0, 1) : Vector2.zero;
            m_Container.anchorMax = vertical ? Vector2.one : new Vector2(0, 1f);
            m_Container.pivot = vertical ? new Vector2(0.5f, 1f) : new Vector2(0, 0.5f);
            m_Container.offsetMax = Vector2.zero;
            m_Container.offsetMin = Vector2.zero;
            m_Container.localPosition = Vector3.zero;
            m_Container.localRotation = Quaternion.identity;
            m_Container.localScale = Vector3.one;
            content = m_Container;
            m_LayoutGroup = m_Container.GetComponent<HorizontalOrVerticalLayoutGroup>();
            m_LayoutGroup.spacing = m_ScollValues.ElementSpacing;
            m_LayoutGroup.padding = m_ScollValues.m_Padding;
            m_LayoutGroup.childAlignment = TextAnchor.UpperLeft;
            m_LayoutGroup.childForceExpandHeight = true;
            m_LayoutGroup.childForceExpandWidth = true;
            //Add space filler RectTransfrom
            go = new GameObject("StartSpaceFiller", typeof(RectTransform), typeof(LayoutElement));
            go.transform.SetParent(m_Container, false);
            m_StartSpaceFiller = go.GetComponent<LayoutElement>();
            go = new GameObject("EndSpaceFiller", typeof(RectTransform), typeof(LayoutElement));
            go.transform.SetParent(m_Container, false);
            m_EndSpaceFiller = go.GetComponent<LayoutElement>();
            go = new GameObject("PooledElements", typeof(RectTransform));
            go.transform.SetParent(transform, false);
            m_PoolElementParent = go.GetComponent<RectTransform>();
            m_PoolElementParent.gameObject.SetActive(false);
            m_Initialized = true;
        }
        protected override void OnEnable()
        {
            if (!Application.isPlaying) return;
            base.OnEnable();
            onValueChanged.RemoveAllListeners();
            onValueChanged.AddListener(ScrollValueChange);
        }
        void Update()
        {
            if (m_UpdateSpacing)
            {
                AddSpaceBetweenElements(m_ScollValues.ElementSpacing);
                m_ReloadData = false;
            }
            if (m_ReloadData)
            {
                Refreshdata();
            }
            if (m_LinearVelocity != 0 && !m_IsScrolling)
            {
                m_IsScrolling = true;
                if (Onscrollchanged != null) Onscrollchanged(this, true);
            }
            else if (m_LinearVelocity == 0 && m_IsScrolling)
            {
                m_IsScrolling = false;
                if (Onscrollchanged != null) Onscrollchanged(this, false);
            }
        }
        protected override void LateUpdate()
        {
            if (!Application.isPlaying) return;
            base.LateUpdate();
            if (m_ScollValues.MaxVelocity > 0)
            {
                if (horizontal)
                {
                    velocity = new Vector2(Mathf.Clamp(Mathf.Abs(velocity.x), 0, m_ScollValues.MaxVelocity) * Mathf.Sign(velocity.x), velocity.y);
                }
                else
                {
                    velocity = new Vector2(velocity.x, Mathf.Clamp(Mathf.Abs(velocity.y), 0, m_ScollValues.MaxVelocity) * Mathf.Sign(velocity.y));
                }
            }
        }
        /// <summary>
        /// Spawn elemnts if already present reuse it
        /// </summary>
        /// <param name="cellPrefab"></param>
        /// <returns></returns>
        public ElementBase GetElement(ElementBase cellPrefab)
        {
            var element = GetPooledElement(cellPrefab);
            if (element == null)
            {
                var go = Instantiate(cellPrefab.gameObject);
                element = go.GetComponent<ElementBase>();
                element.transform.SetParent(m_Container);
                element.transform.localPosition = Vector3.zero;
                element.transform.localRotation = Quaternion.identity;
                OnElementInitiate?.Invoke(this, element);
            }
            else
            {
                OnElementReused?.Invoke(this, element);
            }
            return element;
        }
        public void Refreshdata(float scrollPositionFactor = 0)
        {
            m_ReloadData = false;
            RecycleAllCells();
            if (m_ElementEvents != null)
                Resize(false);
            if (m_ScrollRectTransform == null || m_Container == null)
            {
                ScrollPosition = 0f;
                return;
            }
            ScrollPosition = Mathf.Clamp(scrollPositionFactor * ScrollSize, 0, ScrollSize);
            if (vertical)
            {
                verticalNormalizedPosition = 1f - scrollPositionFactor;
            }
            else
            {
                horizontalNormalizedPosition = scrollPositionFactor;
            }
            RefreshActive();
            JumpToIndex(m_ScollValues.m_StartIndex);
        }

        public void RefreshActiveElements()
        {
            for (var i = 0; i < m_ActiveElements.Count; i++)
            {
                m_ActiveElements[i].RefreshScrollElement();
            }
        }
        /// <summary>
        /// Clear all data
        /// </summary>
        public void ClearAll()
        {
            ClearActive();
            ClearPooledElements();
        }
        /// <summary>
        /// Destroy visible element
        /// </summary>
        public void ClearActive()
        {
            for (var i = 0; i < m_ActiveElements.Count; i++)
            {
                DestroyImmediate(m_ActiveElements[i].gameObject);
            }
            m_ActiveElements.Clear();
        }
        /// <summary>
        /// Destroy Pool element
        /// </summary>
        public void ClearPooledElements()
        {
            for (var i = 0; i < m_PooledElements.Count; i++)
            {
                DestroyImmediate(m_PooledElements[i].gameObject);
            }
            m_PooledElements.Clear();
        }
        public void DeselecteActiveElement()
        {
            for (var i = 0; i < m_ActiveElements.Count; i++)
            {
                m_ActiveElements[i].DeselectElement();
            }
        }
        /// <summary>
        /// Scroll to perticular index element
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_ScollOffset"></param>
        /// <param name="_elementOffset"></param>
        /// <param name="_spacing"></param>
        /// <param name="_scrollTime"></param>
        /// <param name="_complete"></param>
        public void JumpToIndex(int _index, float _ScollOffset = 0, float _elementOffset = 0, bool _spacing = true, float _scrollTime = 0f, Action _complete = null
            )
        {
            var cellOffsetPosition = 0f;
            if (_elementOffset != 0)
            {
                var elementsize = (m_ElementEvents != null ? m_ElementEvents.GetRectSize(this, _index) : 0);
                if (_spacing)
                {
                    elementsize += m_ScollValues.ElementSpacing;
                    if (_index > 0 && _index < (NumberOfElements - 1)) elementsize += m_ScollValues.ElementSpacing;
                }
                cellOffsetPosition = elementsize * _elementOffset;
            }
            if (_ScollOffset == 1f)
            {
                cellOffsetPosition += m_ScollValues.m_Padding.bottom;
            }
            var offset = -(_ScollOffset * ScrollRectSize) + cellOffsetPosition;
            var newScrollPosition = 0f;
            newScrollPosition = GetScrollPositionForDataIndex(_index, ElementPosition.Before) + offset;
            newScrollPosition = Mathf.Clamp(newScrollPosition - (_spacing ? m_ScollValues.ElementSpacing : 0), 0, ScrollSize);
            if (newScrollPosition == ScrollPosition)
            {
                _complete?.Invoke();
                return;
            }
            StartCoroutine(LerpValue(_scrollTime, m_CurrentScrollPosition, newScrollPosition, _complete));
        }
        public void SnapElement()
        {
            if (NumberOfElements == 0) return;
            m_SnapJumping = true;
            m_LinearVelocity = 0;
            m_SnapInertia = inertia;
            inertia = false;
            var snapPosition = m_CurrentScrollPosition + (ScrollRectSize * Mathf.Clamp01(m_ScollValues.SnapWatchOffset));
            m_SnapElementIndex = GetElementIndexAtPosition(snapPosition);
            m_SnapDataIndex = m_SnapElementIndex % NumberOfElements;
            JumpToIndex(m_SnapDataIndex, m_ScollValues.SnapJumpToOffset, m_ScollValues.SnapCellCenterOffset, m_ScollValues.SnapUseCellSpacing, m_ScollValues.SnapTweenTime, SnapComplete);
        }

        /// <summary>
        /// Get Element position in scrollrect
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="insertPosition"></param>
        /// <returns></returns>
        public float GetElementPositioninScroll(int _index, ElementPosition insertPosition)
        {
            if (NumberOfElements == 0) return 0;
            if (_index < 0) _index = 0;
            if (_index == 0 && insertPosition == ElementPosition.Before)
            {
                return 0;
            }
            else
            {
                if (_index < m_ElementOffsetList.Count)
                {
                    if (insertPosition == ElementPosition.Before)
                    {
                        return m_ElementOffsetList[_index - 1] + m_ScollValues.ElementSpacing + (vertical ? m_ScollValues.m_Padding.top : m_ScollValues.m_Padding.left);
                    }
                    else
                    {
                        return m_ElementOffsetList[_index] + (vertical ? m_ScollValues.m_Padding.top : m_ScollValues.m_Padding.left);
                    }
                }
                else
                {
                    return m_ElementOffsetList[m_ElementOffsetList.Count - 2];
                }
            }
        }
        public float GetScrollPositionForDataIndex(int dataIndex, ElementPosition insertPosition)
        {
            return GetElementPositioninScroll(dataIndex, insertPosition);
        }
        public int GetElementIndexAtPosition(float position)
        {
            return ElementIndexAtPosition(position, 0, m_ElementOffsetList.Count - 1);
        }
        /// <summary>
        /// Return element at givent index
        /// </summary>
        /// <param name="dataIndex"></param>
        /// <returns></returns>
        public ElementBase ElementAtDataIndex(int dataIndex)
        {
            for (var i = 0; i < m_ActiveElements.Count; i++)
            {
                if (m_ActiveElements[i].ElementIndex == dataIndex)
                {
                    if (Selected.HasValue)
                    {
                        if (dataIndex == Selected.Value)
                        {
                            m_ActiveElements[i].Selected = true;
                        }
                    }
                    return m_ActiveElements[i];
                }
            }
            return null;
        }
        private void Resize(bool keepPosition)
        {
            var originalScrollPosition = ScrollPosition;
            m_ElementSizeList.Clear();
            var offset = AddElementSizes();
            CalculateElementOffsets();
            if (vertical)
                m_Container.sizeDelta = new Vector2(m_Container.sizeDelta.x, m_ElementOffsetList.Last() + m_ScollValues.m_Padding.top + m_ScollValues.m_Padding.bottom);
            else
                m_Container.sizeDelta = new Vector2(m_ElementOffsetList.Last() + m_ScollValues.m_Padding.left + m_ScollValues.m_Padding.right, m_Container.sizeDelta.y);

            ResetActiveElement();
            m_CurrentScrollPosition = keepPosition ? originalScrollPosition : 0;
        }
        /// <summary>
        /// Apply space between scroll elements
        /// </summary>
        /// <param name="spacing"></param>
        private void AddSpaceBetweenElements(float spacing)
        {
            m_UpdateSpacing = false;
            m_LayoutGroup.spacing = spacing;
            Refreshdata(m_NormalizedScrollPosition);
        }

        /// <summary>
        /// Get total Total offset value
        /// </summary>
        /// <returns></returns>
        private float AddElementSizes()
        {
            var offset = 0f;
            for (var i = 0; i < NumberOfElements; i++)
            {
                m_ElementSizeList.Add(m_ElementEvents.GetRectSize(this, i) + (i == 0 ? 0 : m_LayoutGroup.spacing));
                offset += m_ElementSizeList[m_ElementSizeList.Count - 1];
            }
            return offset;
        }
        private void CalculateElementOffsets()
        {
            m_ElementOffsetList.Clear();
            var offset = 0f;
            for (var i = 0; i < m_ElementSizeList.Count; i++)
            {
                offset += m_ElementSizeList[i];
                m_ElementOffsetList.Add(offset);
            }
        }
        private ElementBase GetPooledElement(ElementBase cellPrefab)
        {
            for (var i = 0; i < m_PooledElements.Count; i++)
            {
                if (m_PooledElements[i].ElementID == cellPrefab.ElementID)
                {
                    var element = m_PooledElements[i];
                    m_PooledElements.RemoveAt(i);
                    return element;
                }
            }
            return null;
        }
        /// <summary>
        /// Reset visible scroll elements 
        /// </summary>
        public void ResetActiveElement()
        {
            int startIndex; int endIndex;
            GetActiveElementRange(out startIndex, out endIndex);
            var i = 0;
            List<int> remainingElementIndex = new List<int>();
            while (i < m_ActiveElements.Count)
            {
                if (m_ActiveElements[i].CellIndex < startIndex || m_ActiveElements[i].CellIndex > endIndex)
                {
                    PoolElement(m_ActiveElements[i]);
                }
                else
                {
                    remainingElementIndex.Add(m_ActiveElements[i].CellIndex);
                    i++;
                }
            }
            if (remainingElementIndex.Count == 0)
            {
                for (i = startIndex; i <= endIndex; i++)
                {
                    AddElement(i, ListPosition.Last);
                }
            }
            else
            {
                for (i = endIndex; i >= startIndex; i--)
                {
                    if (i < remainingElementIndex.First())
                    {
                        AddElement(i, ListPosition.First);
                    }
                }
                for (i = startIndex; i <= endIndex; i++)
                {
                    if (i > remainingElementIndex.Last())
                    {
                        AddElement(i, ListPosition.Last);
                    }
                }
            }
            m_ActiveElementStartIndex = startIndex;
            m_ActiveElementEndIndex = endIndex;
            SetSpaceFiller();
        }
        private void RecycleAllCells()
        {
            while (m_ActiveElements.Count > 0) PoolElement(m_ActiveElements[0]);
            m_ActiveElementStartIndex = 0;
            m_ActiveElementEndIndex = 0;
        }
        private void PoolElement(ElementBase scrollElement)
        {
            if (OnElementWillPool != null) OnElementWillPool(scrollElement);
            m_ActiveElements.Remove(scrollElement);
            m_PooledElements.Add(scrollElement);
            scrollElement.transform.SetParent(m_PoolElementParent);
            scrollElement.ElementIndex = 0;
            scrollElement.CellIndex = 0;
            scrollElement.IsElementActive = false;
            scrollElement.Selected = false;
            if (OnActivityChange != null) OnActivityChange(scrollElement);
        }
        /// <summary>
        /// Add element to scrollrect
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="listPosition"></param>
        private void AddElement(int _index, ListPosition listPosition)
        {
            if (NumberOfElements == 0) return;
            var dataIndex = _index % NumberOfElements;
            var element = m_ElementEvents.GetScrollElement(this, dataIndex, _index);
            element.CellIndex = _index;
            element.Selected = false;
            element.ElementIndex = dataIndex;
            element.IsElementActive = true;
            element.transform.SetParent(m_Container, false);
            element.transform.localScale = Vector3.one;
            LayoutElement layoutElement = element.GetComponent<LayoutElement>();
            if (layoutElement == null) layoutElement = element.gameObject.AddComponent<LayoutElement>();
            if (vertical)
                layoutElement.minHeight = m_ElementSizeList[_index] - (_index > 0 ? m_LayoutGroup.spacing : 0);
            else
                layoutElement.minWidth = m_ElementSizeList[_index] - (_index > 0 ? m_LayoutGroup.spacing : 0);
            if (listPosition == ListPosition.First)
                m_ActiveElements.Insert(0, element);
            else
                m_ActiveElements.Add(element);
            if (listPosition == ListPosition.Last)
                element.transform.SetSiblingIndex(m_Container.childCount - 2);
            else if (listPosition == ListPosition.First)
                element.transform.SetSiblingIndex(1);
            if (OnActivityChange != null) OnActivityChange(element);
        }
        /// <summary>
        /// Update empty area with space filler 
        /// </summary>
        private void SetSpaceFiller()
        {
            if (NumberOfElements == 0) return;
            var firstSize = m_ElementOffsetList[m_ActiveElementStartIndex] - m_ElementSizeList[m_ActiveElementStartIndex];
            var lastSize = m_ElementOffsetList.Last() - m_ElementOffsetList[m_ActiveElementEndIndex];
            if (vertical)
            {
                m_StartSpaceFiller.minHeight = firstSize;
                m_StartSpaceFiller.gameObject.SetActive(m_StartSpaceFiller.minHeight > 0);
                m_EndSpaceFiller.minHeight = lastSize;
                m_EndSpaceFiller.gameObject.SetActive(m_EndSpaceFiller.minHeight > 0);
            }
            else
            {
                m_StartSpaceFiller.minWidth = firstSize;
                m_StartSpaceFiller.gameObject.SetActive(m_StartSpaceFiller.minWidth > 0);
                m_EndSpaceFiller.minWidth = lastSize;
                m_EndSpaceFiller.gameObject.SetActive(m_EndSpaceFiller.minWidth > 0);
            }
        }
        private void RefreshActive()
        {
            int startIndex; int endIndex;
            GetActiveElementRange(out startIndex, out endIndex);
            if (startIndex == m_ActiveElementStartIndex && endIndex == m_ActiveElementEndIndex) return;
            ResetActiveElement();
        }
        //Get visible element start and end index
        private void GetActiveElementRange(out int startIndex, out int endIndex)
        {
            startIndex = 0; endIndex = 0;
            var startPosition = ScrollPosition;
            var endPosition = ScrollPosition + (vertical ? m_ScrollRectTransform.rect.height : m_ScrollRectTransform.rect.width);
            startIndex = GetElementIndexAtPosition(startPosition);
            endIndex = GetElementIndexAtPosition(endPosition);
        }
        private int ElementIndexAtPosition(float position, int startIndex, int endIndex)
        {
            if (startIndex >= endIndex) return startIndex;
            var middleIndex = (startIndex + endIndex) / 2;
            var pad = vertical ? m_ScollValues.m_Padding.top : m_ScollValues.m_Padding.left;
            if ((m_ElementOffsetList[middleIndex] + pad) >= (position + (pad == 0 ? 0 : 1.00001f)))
                return ElementIndexAtPosition(position, startIndex, middleIndex);
            else
                return ElementIndexAtPosition(position, middleIndex + 1, endIndex);
        }
        protected override void OnDestroy()
        {
            Selected = null;
            base.OnDestroy();
            if (m_PoolElementParent != null)
            {
                DestroyImmediate(m_PoolElementParent.gameObject);
            }
        }
        public override void OnBeginDrag(PointerEventData data)
        {
            if (!horizontal && Math.Abs(data.delta.x) > Math.Abs(data.delta.y))
                m_RouteToParent = true;
            else if (!vertical && Math.Abs(data.delta.x) < Math.Abs(data.delta.y))
                m_RouteToParent = true;
            else
                m_RouteToParent = false;
            if (m_RouteToParent)
                DoForParents<IBeginDragHandler>((parent) => { parent.OnBeginDrag(data); });
            else
                base.OnBeginDrag(data);
            m_SnapBeforeDrag = m_Snapping;
            if (!m_ScollValues.SnapWhileDragging)
            {
                m_Snapping = false;
            }
        }
        public override void OnEndDrag(PointerEventData data)
        {
            if (m_RouteToParent)
                DoForParents<IEndDragHandler>((parent) => { parent.OnEndDrag(data); });
            else
                base.OnEndDrag(data);
            m_RouteToParent = false;
            m_Snapping = m_SnapBeforeDrag;
        }

#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        private void OnValidate()
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        {
            if (!Application.isPlaying) return;
            if (m_Initialized && m_ScollValues.ElementSpacing != m_LayoutGroup.spacing)
            {
                m_UpdateSpacing = true;
            }
        }
        private void ScrollValueChange(Vector2 val)
        {
            ScrollPosition = vertical ? (1f - val.y) * ScrollSize : val.x * ScrollSize;
            ScrollPosition = Mathf.Clamp(ScrollPosition, 0, ScrollSize);
            OnScrolled?.Invoke(this, val, ScrollPosition);
            if (m_Snapping && !m_SnapJumping)
            {
                if (Mathf.Abs(m_LinearVelocity) <= m_ScollValues.SnapVelocityThreshold && m_LinearVelocity != 0)
                {
                    var normalized = m_NormalizedScrollPosition;
                    if ((normalized > 0 && normalized < 1.0f))
                    {
                        SnapElement();
                    }
                }
            }
            RefreshActive();
        }
        // On Scoll snap complete
        private void SnapComplete()
        {
            m_SnapJumping = false;
            inertia = m_SnapInertia;
            ElementBase scrollElement = null;
            for (var i = 0; i < m_ActiveElements.Count; i++)
            {
                if (m_ActiveElements[i].ElementIndex == m_SnapDataIndex)
                {
                    scrollElement = m_ActiveElements[i];
                    break;
                }
            }
            if (OnScrollerSnapped != null) OnScrollerSnapped(this, m_SnapElementIndex, m_SnapDataIndex, scrollElement);
        }
        /// <summary>
        /// change value over time
        /// </summary>
        /// <param name="time"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="tweenComplete"></param>
        /// <returns></returns>
        IEnumerator LerpValue(float time, float start, float end, Action tweenComplete)
        {
            if (time != 0)
            {
                velocity = Vector2.zero;
                if (OnScrollValue != null) OnScrollValue(this, true);
                m_ScrollTimeRemaining = 0;
                var newPosition = 0f;
                while (m_ScrollTimeRemaining < time)
                {
                    newPosition = Mathf.Lerp(start, end, (m_ScrollTimeRemaining / time));
                    m_CurrentScrollPosition = newPosition;
                    m_ScrollTimeRemaining += Time.unscaledDeltaTime;
                    yield return null;
                }
            }
            m_CurrentScrollPosition = end;
            tweenComplete?.Invoke();
            if (OnScrollValue != null) OnScrollValue(this, false);
        }
        // Handles nested scrolling
        private void DoForParents<T>(Action<T> action) where T : IEventSystemHandler
        {
            Transform parent = transform.parent;
            while (parent != null)
            {
                foreach (var component in parent.GetComponents<Component>())
                {
                    if (component is T)
                        action((T)(IEventSystemHandler)component);
                }
                parent = parent.parent;
            }
        }
        // Handles nested scrolling
        public override void OnInitializePotentialDrag(PointerEventData eventData)
        {
            DoForParents<IInitializePotentialDragHandler>((parent) => { parent.OnInitializePotentialDrag(eventData); });
            base.OnInitializePotentialDrag(eventData);
        }
        // Handles nested scrolling
        public override void OnDrag(PointerEventData eventData)
        {
            if (m_RouteToParent)
                DoForParents<IDragHandler>((parent) => { parent.OnDrag(eventData); });
            else
                base.OnDrag(eventData);
        }
    }
}
