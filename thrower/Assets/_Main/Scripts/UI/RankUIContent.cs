using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankUIContent : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private TextMeshProUGUI _rankText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    #endregion

    public void SetContent(int rank, string name, int score)
    {
        _rankText.text = rank.ToString();
        _nameText.text = name;
        _scoreText.text = score.ToString();
    }
}
