using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Utilities.Extensions;
using DG.Tweening;

public class RespawnUI : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private TextMeshProUGUI _timerText;
    #endregion

    #region private Fields
    #endregion

    public void StartTimer(float time)
    {
        gameObject.SetActive(true);
        var sequence = DOTweenUtil.NewSequence(gameObject);
        sequence.Append(DOTween.To(() => time, x => _timerText.text = ((int)x + 1) + "", 0, time).SetEase(Ease.Linear));
        sequence.AppendCallback(() => {
            gameObject.SetActive(false);
        });
    }
}
