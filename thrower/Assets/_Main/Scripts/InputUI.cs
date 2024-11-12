using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputUI : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private RectTransform _bgCircle;
    [SerializeField] private RectTransform _stickCircle;
    #endregion


    public void Show(Vector2 inputStartPos)
    {
        gameObject.SetActive(true);
        var camera    = Camera.main;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform,
            inputStartPos,
            camera,
            out var uiLocalPos
        );
        _bgCircle.localPosition = uiLocalPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform,
            Input.mousePosition,
            camera,
            out var localPos
        );
        _stickCircle.localPosition = localPos;
        var direction = _stickCircle.localPosition - _bgCircle.localPosition;
        if(direction.magnitude > 150.0f)
        {
            direction = direction.normalized * 150.0f;
            _stickCircle.localPosition = _bgCircle.localPosition + direction;
        }

    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
