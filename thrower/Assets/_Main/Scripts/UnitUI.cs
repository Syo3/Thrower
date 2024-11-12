using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Utilities.Extensions;
using TMPro;

public class UnitUI : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private List<Image> _hpIconList;
    #endregion

    #region private Fields
    #endregion


    public void Initialize(int score)
    {
        _scoreText.text = score + "";
    }

    public void SetScore(int score)
    {
        var beforeScore = int.Parse(_scoreText.text);
        var sequence = DOTweenUtil.NewSequence(gameObject);
        sequence.Append(_scoreText.transform.DOScale(1.5f, 0.3f).SetEase(Ease.OutQuad));
        sequence.Append(_scoreText.transform.DOScale(1.0f, 0.2f).SetEase(Ease.InQuad));
        DOTween.To(() => beforeScore, x => _scoreText.text = x.ToString(), score, 0.5f).SetEase(Ease.OutQuad);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    void Update()
    {
        transform.eulerAngles = new Vector3(Camera.main.transform.eulerAngles.x, 0.0f);
    }

    public void SetHP(int _hp)
    {
        for (int i = 0; i < _hpIconList.Count; ++i)
        {
            _hpIconList[i].enabled = (i < _hp);
        }
    }
}
