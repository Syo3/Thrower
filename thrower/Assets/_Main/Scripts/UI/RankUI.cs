using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankUI : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private List<RankUIContent> _rankUIContentList;
    #endregion


    public void UpdateRank(List<UnitScoreManager> unitScoreManagerList)
    {
        for(var i = 0; i < _rankUIContentList.Count; ++i)
        {
            _rankUIContentList[i].SetContent((i+1), unitScoreManagerList[i].name, unitScoreManagerList[i].Score);
        }
    }
}
