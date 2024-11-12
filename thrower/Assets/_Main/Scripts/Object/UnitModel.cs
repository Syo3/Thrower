using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitModel : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private Animator _animator;
    #endregion

    #region private Fields
    private string _animationName;
    #endregion

    public void Play(string animationName, float duration)
    {
        if(_animationName == animationName) return;
        _animationName = animationName;
        _animator.Play(animationName, 0, duration);
    }

    public void Reset()
    {
        _animationName = "";
        transform.localRotation = Quaternion.identity;
        Play("Idle", 0.4f);
    }
}
