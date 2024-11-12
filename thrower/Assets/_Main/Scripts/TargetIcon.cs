using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Utilities.Extensions;

public class TargetIcon : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Image _iconImage;
    [SerializeField] private CanvasGroup _canvasGroup;
    #endregion

    #region private Fields
    private Sequence _sequence;
    private Transform _target;
    private Camera _camera;
    #endregion

    void Start()
    {
        Initialize();
    }


    public void Initialize()
    {
        _canvasGroup.alpha = 0;
        _camera            = Camera.main;
    }


    public void Show(Transform target)
    {
        this._target = target;
        _canvasGroup.alpha = 1;
        _iconImage.rectTransform.localRotation = Quaternion.identity;
        DOTweenUtil.CheckSequence(ref _sequence);
        _sequence.Append(_iconImage.rectTransform.DOLocalRotate(new Vector3(0, 0, 360), 6.0f, RotateMode.FastBeyond360).SetEase(Ease.Linear)).SetLoops(-1);
    }

    public void Hide()
    {
        _canvasGroup.alpha = 0;
    }

    void Update()
    {
        if(_canvasGroup.alpha < 1.0f) return;
        var screenPos = _camera.WorldToScreenPoint(_target.position + Vector3.up);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform,
            screenPos,
            _camera,
            out var uiLocalPos
        );
        _iconImage.rectTransform.localPosition = uiLocalPos;

    }
}
