using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SingletonMonoBehaviour<CameraManager>
{
    #region Serialized Fields
    [SerializeField] private Transform _target;
    #endregion

    #region private Fields
    private Vector3 _offset;
    #endregion

    void Start()
    {
        _offset = transform.position - _target.position;
    }

    void LateUpdate()
    {
        transform.position = _target.position + _offset;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

}
