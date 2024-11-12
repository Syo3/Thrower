using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utilities.Extensions;

public class UnitBase : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] protected UnitModel _unitModel;
    [SerializeField] protected CollectArea _collectArea;
    [SerializeField] protected UnitAttackArea _unitAttackArea;
    [SerializeField] protected UnitScoreManager _unitScoreManager;
    [SerializeField] protected UnitUI _unitUI;
    [SerializeField] protected ThrowObject _throwObject;
    [SerializeField] protected Rigidbody _rigidbody;
    [SerializeField] protected float _speed = 3.0f;
    #endregion

    #region private Fields
    protected int _hp;
    protected UnitBase _throwTarget;
    protected List<UnitBase> _throwTargetList = new List<UnitBase>();
    protected bool _isStop;
    protected bool _isDown;
    protected bool _isStun;
    protected float _throwWaitTime;
    protected static readonly float ThrowInterval = 0.5f;
    protected static readonly float RespawnTime = 5.0f;
    protected static readonly int MaxHP = 3;
    #endregion

    #region Properties
    public bool IsDown => _isDown;
    public bool IsMove => !_isDown && !_isStun;
    #endregion

    public virtual void Initialize()
    {
        _hp = MaxHP;
        _collectArea.Initialize(this);
        _unitAttackArea.Initialize(this);
        _unitScoreManager.Initialize(name);
        _unitUI.Initialize(_unitScoreManager.Score);
        _unitUI.SetHP(_hp);
    }

    public virtual void Move(Vector3 direction)
    {
        if(!IsMove) return;
        _rigidbody.MovePosition(_rigidbody.position + direction.normalized * Time.deltaTime * _speed);
        transform.forward = direction.normalized;
        _unitModel.Play("Run", 0.1f);
        _collectArea.Move();
        _isStop = false;
    }

    public virtual void Stop()
    {
        if(!IsMove) return;
        _unitModel.Play("Idle", 0.1f);
        _isStop = true;
    }

    protected void OnCollisionEnter(Collision other)
    {
        if(!IsMove) return;
        if(!other.collider.CompareTag("ThrowObject")) return;
        var throwObject = other.collider.GetComponent<ThrowObject>();
        if(!throwObject.IsThrowed || throwObject.ParentInstanceID == GetInstanceID()) return;

        var hitEffect = EffectManager.Instance.Pop(EffectManager.EffectType.Hit);
        hitEffect.transform.position   = transform.position + Vector3.up * 0.5f;
        hitEffect.transform.localScale = Vector3.one * 1.0f;
        hitEffect.GetComponent<ParticleSystem>().Play();
        Down(throwObject);
    }

    public virtual void SetThrowTarget(UnitBase unitBase)
    {
        if(_throwTargetList.Contains(unitBase)) return;
        _throwTargetList.Add(unitBase);
        for(var i = 0; i < _throwTargetList.Count; ++i)
        {
            if(_throwTargetList[i].IsDown) _throwTargetList.RemoveAt(i);
        }
        UpdateUnit();
    }

    public virtual void RemoveThrowTarget(UnitBase enemyUnit)
    {
        _throwTargetList.Remove(enemyUnit);
        if(_throwTargetList.Count == 0)
        {
            _throwTarget = null;
            _unitAttackArea.HitEnemy(_throwTarget);
            return;
        }
        _throwTarget = _throwTargetList.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).First();
    }

    public virtual void UpdateUnit(){}

    public virtual void SetThrowWaitAnim()
    {
        if(!IsMove) return;
        _unitModel.Play("ThrowWait", 0.1f);
    }

    public virtual void SetThrowAnim()
    {
        if(!IsMove) return;
        _unitModel.Play("Throw", 0.05f);
    }

    public virtual void CheckThrow()
    {
        if(!_isStop || _throwTarget == null) return;
        if(_throwTarget.IsDown){
            _throwTarget = null;
            return;
        }
        if(Time.time - _throwWaitTime < ThrowInterval) return;
        _throwWaitTime = Time.time;
        Throw();
    }

    public virtual void Throw()
    {
        if(_throwTarget == null) return;
        _collectArea.SetThrow(_throwTarget.transform);
    }

    protected virtual void Down(ThrowObject throwObject)
    {
        if(_isStun) return;
        _isStun = true;
        --_hp;
        _unitUI.SetHP(_hp);
        var direction = (throwObject.transform.position - transform.position).normalized;
        transform.forward = new Vector3(direction.x, 0.0f, direction.z);
        _unitModel.Play("Down", 0.0f);
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _rigidbody.AddForce(-transform.forward * 2.0f + Vector3.up * 1.0f, ForceMode.Impulse);
        throwObject.Hit(this);
        _collectArea.ReleaseThrowObject();
        if(_hp > 0)
        {
            DOTweenUtil.Delay(1.0f, () => {
                _isStun = false;
                _unitModel.Reset();
            });
            return;
        }

        _isDown = true;
        _unitUI.Hide();
        var minusScore = (int)(_unitScoreManager.Score * 0.2f);
        _throwObject.SetShippedScore(minusScore);
        _unitScoreManager.AddScore(-minusScore);

        _collectArea.gameObject.SetActive(false);
        _unitAttackArea.gameObject.SetActive(false);
        _throwObject.enabled = true;
        tag                  = "ThrowObject";
        _throwObject.SetOnCollect(OnCollect);
        DOTweenUtil.Delay(RespawnTime, CreateClone);
    }

    protected virtual void CreateClone()
    {
        var clone  = Instantiate(gameObject, null).GetComponent<UnitBase>();
        clone.name = name;
        clone.Reset(_unitScoreManager.Score);
        name = "Dummy";
        RankManager.Instance.RemoveScoreManager(_unitScoreManager);
    }

    /// <summary>
    /// リセット
    /// </summary>
    public void Reset(int score=0)
    {
        gameObject.SetActive(true);
        _throwObject.Release();
        _isDown = false;
        _isStun = false;
        _rigidbody.velocity    = Vector3.zero;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        transform.position   = RandomStageSpawner.Instance.GetSpawnPosition() + Vector3.up * 3.0f;
        transform.localScale = Vector3.one;
        _collectArea   .gameObject.SetActive(true);
        _unitAttackArea.gameObject.SetActive(true);
        _unitUI.Show();
        _throwObject.enabled = false;
        tag                  = "Unit";
        _unitModel.Reset();
        if(score != 0) _unitScoreManager.AddScore(score);
        Initialize();
    }

    protected void OnCollect()
    {
        _unitModel.Reset();
        _unitModel.Play("Downed", 0.1f);
    }

    public void OnShipping(ShippingArea shippingArea)
    {
        var addScore = _collectArea.OnShipping(shippingArea);
        if(addScore == 0) return;
        _unitScoreManager.AddScore(addScore);
        _unitUI.SetScore(_unitScoreManager.Score);
    }
}
