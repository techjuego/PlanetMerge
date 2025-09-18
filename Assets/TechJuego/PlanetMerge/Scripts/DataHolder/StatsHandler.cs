using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TechJuego.PlanetMerge
{
    [System.Serializable]
    public class StatClass
    {
        public int Value;
        public StatType statType;
    }
    [System.Serializable]
    public class StatsDetail
    {
        public List<StatClass> statClasses = new List<StatClass>();
    }
    public enum StatType
    {
        Retry,
        PlayCount,
        PlayTime,
        LeftTap,
        RightTap,
    }
    public class StatsHandler : Singleton<StatsHandler>
    {
        protected StatsHandler() { }
        private void Awake()
        {
            if (PlayerPrefs.HasKey("STAT"))
            {
                statsDetail = JsonUtility.FromJson<StatsDetail>(PlayerPrefs.GetString("STAT"));
            }
            else
            {
                statsDetail = new StatsDetail();
            }
        }
        public StatsDetail statsDetail = new StatsDetail();
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name == "Game")
            {
                StartCoroutine(StartTime());
            }
            if (arg0.name == "Menu")
            {
                StopAllCoroutines();
                SetStats(StatType.PlayTime, seconds);
                seconds = 0;
            }
        }
        private void OnApplicationQuit()
        {
            SetStats(StatType.PlayTime, seconds);
        }
        public int seconds;
        IEnumerator StartTime()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                seconds++;
            }
        }
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        public void SetReplayCount()
        {
            SetStats(StatType.Retry, 1);
        }
        public void SetPlayCount()
        {
            SetStats(StatType.PlayCount, 1);
        }

        public void SetStats(StatType statType, int value, bool isFixedvalue = false)
        {
            int index = -1;
            for (int i = 0; i < statsDetail.statClasses.Count; i++)
            {
                if (statsDetail.statClasses[i].statType == statType)
                {
                    index = i;
                    break;
                }
            }
            if (index > -1)
            {
                if (isFixedvalue)
                {
                    statsDetail.statClasses[index].Value = value;
                }
                else
                {
                    statsDetail.statClasses[index].Value += value;
                }
            }
            else
            {
                statsDetail.statClasses.Add(new StatClass() { Value = value, statType = statType });
            }
            PlayerPrefs.SetString("STAT", JsonUtility.ToJson(statsDetail));
        }
    }
}