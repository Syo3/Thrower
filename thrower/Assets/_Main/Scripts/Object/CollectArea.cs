using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CollectArea : MonoBehaviour
{
    #region private Fields
    private UnitBase _unitBase;
    private List<ThrowObject> _throwObjectList = new List<ThrowObject>();
    private List<Vector3> _posList = new List<Vector3>();
    private bool _isThrowing;
    #endregion

    #region Properties
    public int ThrowObjectCount => _throwObjectList.Count;
    public bool IsThrowing => _isThrowing;
    #endregion

    public void Initialize(UnitBase unitBase)
    {
        _unitBase = unitBase;
    }

    void OnTriggerEnter(Collider other)
    {
        if(_unitBase == null || !_unitBase.IsMove) return;
        if(other.gameObject.tag != "ThrowObject") return;
        var throwObject = other.gameObject.GetComponent<ThrowObject>();
        if(throwObject == null) return;
        if(throwObject.IsCollected || throwObject.IsThrowed) return;
        throwObject.Collect(_unitBase.GetInstanceID());
        _throwObjectList.Add(throwObject);
    }

    public void Move()
    {
        _posList.Insert(0, transform.position);
        for(var i = 0; i < _throwObjectList.Count; ++i)
        {
            var throwObject = _throwObjectList[i];
            var index       = (i + 1) * 8;
            if(index >= _posList.Count) break;
            var target = _posList[index];
            throwObject.SetTargetPosition(target);
        }
    }

    public void SetThrow(Transform target)
    {
        if(_throwObjectList.Count == 0) return;
        _isThrowing = true;
        var throwObject = _throwObjectList[0];
        _throwObjectList.RemoveAt(0);
        _unitBase.SetThrowWaitAnim();
        throwObject.ThrowWait(_unitBase.transform, target, () => {
            _unitBase.SetThrowAnim();
            _isThrowing = false;
        }, OnThrowObjectHit);
    }

    void OnThrowObjectHit(UnitBase enemyUnit)
    {
        _unitBase.RemoveThrowTarget(enemyUnit);
    }

    public void ReleaseThrowObject()
    {
        _throwObjectList.ForEach(x => x.Release());
        _throwObjectList.Clear();
    }

    public int OnShipping(ShippingArea shippingArea)
    {
        if(_throwObjectList.Count == 0) return 0;
        var count = 0;
        _throwObjectList.ForEach(x => {
            x.Shipping(shippingArea, 0.1f * count);
            ++count;
        });
        var sum = _throwObjectList.Sum(x => x.ShippedScore);
        _throwObjectList.Clear();
        return sum;
    }
}
