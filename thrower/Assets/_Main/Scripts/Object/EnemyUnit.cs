using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using Utilities.Extensions;

public class EnemyUnit : UnitBase
{
    #region Serialized Fields
    [SerializeField] private Collider _collider;
    #endregion

    #region private Fields
    private EnemyController _enemyController;
    #endregion

    #region Properties
    #endregion

    public void Initilize(EnemyController enemyController)
    {
        _enemyController = enemyController;
        Initialize();
    }

    public override void Move(Vector3 direction)
    {
        if(_collectArea.IsThrowing) return;
        base.Move(direction);
    }

    public override void UpdateUnit()
    {
        if(_throwTargetList.Count < 1) return;
        _throwTarget = _throwTargetList.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).First();
        if(_collectArea.ThrowObjectCount == 0) return;
        Stop();
        CheckThrow();
    }
}
