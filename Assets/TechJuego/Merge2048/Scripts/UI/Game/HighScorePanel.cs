using System.Collections;
using System.Collections.Generic;
using TechJuego.FruitSliceMerge.Integration;
using TechJuego.FruitSliceMerge.Utils;
using UnityEngine;
namespace TechJuego.FruitSliceMerge
{
    public class HighScorePanel : MonoBehaviour, IUGetListDataInterface
    {
        [SerializeField] private RecycleScroll scroller;
        [SerializeField] private ElementBase m_LevelGrid;
        int itemCount;
        public HighScoreUI highScoreUI;
        private void OnEnable()
        {
            TechCloundHandler.Instance.GetHighScore(() =>
            {
                itemCount = TechCloundHandler.Instance.highScoreList.Count;
                transform.SetAsLastSibling();
                TechTween.DelayCall(gameObject, 0.1f, () =>
                {
                    scroller.ElementInterface = this;
                    if (itemCount > 0)
                    {
                        scroller.Refreshdata();
                    }
                });
                TechCloundHandler.Instance.GetOurScore((res) => 
                {
                    highScoreUI.SetGridData(res);
                });
            });
            
      
        }
        public ElementBase GetScrollElement(RecycleScroll scroller, int dataIndex, int cellIndex)
        {
            HighScoreUI cellView = scroller.GetElement(m_LevelGrid) as HighScoreUI;
            cellView.name = "Cell " + dataIndex;
            cellView.SetGridData(dataIndex );
            return cellView;
        }
        public int GetElementCount(RecycleScroll scroller)
        {
            return itemCount;
        }
        public float GetRectSize(RecycleScroll scroller, int dataIndex)
        {
            return 220;
        }
    }
}