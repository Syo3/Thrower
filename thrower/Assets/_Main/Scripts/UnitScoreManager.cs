using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScoreManager : MonoBehaviour
{
    #region Serialized Fields
    #endregion

    #region private Fields
    private int _score;
    private string _name;
    #endregion

    #region Properties
    public int Score => _score;
    #endregion


    public void Initialize(string name)
    {
        _name = name;
        RankManager.Instance.SetScoreManager(this);
    }

    public void AddScore(int score)
    {
        _score += score;
        if(_score < 0) _score = 0;
    }
}
