using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : UnitBase
{
    #region Serialized Fields
    #endregion

    #region private Fields
    PlayerController _playerController;
    #endregion

    public void Initialize(PlayerController playerController)
    {
        _playerController = playerController;
    }

    public override void UpdateUnit()
    {
        if(_throwTargetList.Count < 1) return;
        var target = _throwTargetList.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).First();
        if(_throwTarget != target){
            UIManager.Instance.TargetIcon.Show(target.transform);
            _unitAttackArea.HitEnemy(target);
        }
        _throwTarget = target;
    }

    public override void SetThrowTarget(UnitBase enemyUnit)
    {
        base.SetThrowTarget(enemyUnit);
        if(_throwTarget != null) UIManager.Instance.TargetIcon.Show(_throwTarget.transform);
    }

    public override void RemoveThrowTarget(UnitBase enemyUnit)
    {
        base.RemoveThrowTarget(enemyUnit);
        if(_throwTargetList.Count == 0) UIManager.Instance.TargetIcon.Hide();
    }


    protected override void Down(ThrowObject throwObject)
    {   
        base.Down(throwObject);
        if(!_isDown) return;
        UIManager.Instance.TargetIcon.Hide();
        UIManager.Instance.RespawnUI.StartTimer(RespawnTime);
    }


    protected override void CreateClone()
    {
        name = "Dummy";
        var clone  = Instantiate(gameObject, null).GetComponent<UnitBase>();
        clone.name = "PlayerUnit";
        clone.Reset(_unitScoreManager.Score);
        CameraManager.Instance.SetTarget(clone.transform);
        var playerUnit = (PlayerUnit)clone;
        playerUnit.Initialize(_playerController);
        _playerController.SetPlayerCharactor(playerUnit);
    }

}
