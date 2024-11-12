using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RankManager : SingletonMonoBehaviour<RankManager>
{
    #region Serialized Fields
    #endregion

    #region private Fields
    private List<UnitScoreManager> _unitScoreManagerList = new List<UnitScoreManager>();
    #endregion

    #region Properties
    #endregion

    public void SetScoreManager(UnitScoreManager unitScoreManager)
    {
        _unitScoreManagerList.Add(unitScoreManager);
    }

    public void RemoveScoreManager(UnitScoreManager unitScoreManager)
    {
        _unitScoreManagerList.Remove(unitScoreManager);
    }

    void Update()
    {
        UIManager.Instance.RankUI.UpdateRank(_unitScoreManagerList.OrderByDescending(x => x.Score).ToList());
    }
}
