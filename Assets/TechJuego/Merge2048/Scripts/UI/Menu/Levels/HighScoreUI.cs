using UnityEngine;
using TMPro;
using TechJuego.FruitSliceMerge.Utils;
using TechJuego.FruitSliceMerge.Integration;

namespace TechJuego.FruitSliceMerge
{
    public class HighScoreUI : ElementBase
    {
        [SerializeField] private TextMeshProUGUI m_Index;
        [SerializeField] private TextMeshProUGUI m_UserID;
        [SerializeField] private TextMeshProUGUI m_Score;
        [SerializeField] private TextMeshProUGUI m_You;
        public void SetGridData(int index)
        {
            SetGridData(TechCloundHandler.Instance.highScoreList[index]);
        }
        public void SetGridData(HighScoreData data)
        {
            m_You.text =string.Empty;
            if (TechCloundHandler.Instance.m_UserDetail.UserId == data.UserId)
            {
                m_You.text = "You";
            }
            m_Index.text = data.Index.ToString();
            m_UserID.text = data.UserId.ToString();
            m_Score.text = data.Score.ToString();
        }
    }
}