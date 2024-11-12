using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackArea : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] SpriteRenderer _spriteRenderer;
    #endregion

    #region private Fields
    private UnitBase _unitBase;
    #endregion

    public void Initialize(UnitBase unitBase)
    {
        _unitBase = unitBase;
        HitEnemy(null);
    }

    void OnTriggerEnter(Collider other)
    {
        if(_unitBase == null) return;
        if(other.tag != "Unit") return;
        var enemyUnit = other.GetComponent<UnitBase>();
        if(enemyUnit == null || !enemyUnit.IsMove) return;
        _unitBase.SetThrowTarget(enemyUnit);
    }

    void OnTriggerExit(Collider other)
    {
        if(_unitBase == null) return;
        if(other.tag != "Unit") return;
        var enemyUnit = other.GetComponent<UnitBase>();
        if(enemyUnit == null) return;
        _unitBase.RemoveThrowTarget(enemyUnit);
    }

    public void HitEnemy(UnitBase enemyUnit)
    {
        if(_spriteRenderer == null) return;
        var color = Color.red;
        if(enemyUnit == null) color = Color.white;
        color.a               = _spriteRenderer.color.a;
        _spriteRenderer.color = color;
    }
}
