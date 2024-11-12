using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utilities.Extensions;

public class ThrowObject : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private Collider _collider;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private int _shippedScore = 1;
    #endregion

    #region private Fields
    private bool _isCollected;
    private bool _isThrowWait;
    private bool _isThrowed;
    private bool _isShipped;
    private Vector3 _targetPosition;
    private System.Action<UnitBase> _onHit;
    private System.Action _onCollect;
    private int _parentInstanceID;
    #endregion

    #region Properties
    public bool IsCollected => _isCollected || _isShipped;
    public bool IsThrowed    => _isThrowed;
    public int ParentInstanceID => _parentInstanceID;
    public int ShippedScore => _shippedScore;
    #endregion

    public void Collect(int parentInstanceID)
    {
        if(_isCollected) return;
        _parentInstanceID = parentInstanceID;
        _isCollected      = true;
        _rigidbody.isKinematic = true;
        _collider.enabled      = false;
        _onCollect?.Invoke();
    }

    public void ThrowWait(Transform unitTf, Transform targetTf, System.Action onThrow = null, System.Action<UnitBase> onHit = null)
    {
        _onHit = onHit;
        _isThrowWait = true;
        _trailRenderer.enabled = true;
        var waitPosition = unitTf.position + unitTf.forward * 0.2f + Vector3.up;
        var sequence = DOTweenUtil.NewSequence(gameObject);

        var direction = (targetTf.position - transform.position).normalized;
        sequence.Append(transform.DOMoveY(waitPosition.y + 0.5f, 0.1f).OnComplete(() => {
            transform.DOMoveY(waitPosition.y, 0.05f).SetEase(Ease.InQuad);
        }).SetEase(Ease.OutQuad));
        sequence.Join(transform.DOMoveX(waitPosition.x, 0.2f).SetEase(Ease.OutQuad));
        sequence.Join(transform.DOMoveZ(waitPosition.z, 0.2f).SetEase(Ease.OutQuad));
        // sequence.Append(transform.DOMoveY(waitPosition.y, 0.15f).SetEase(Ease.InQuad));
        sequence.AppendCallback(() => {
            _isThrowWait = false;
            onThrow?.Invoke();
            Throw(unitTf, targetTf);
        });
    }

    public void Throw(Transform unitTf, Transform targetTf)
    {
        var waitPosition = unitTf.position + unitTf.forward * 0.2f + Vector3.up;
        var vector       = unitTf.forward;
        vector.y         = 0.0f;
        var vector2      = (targetTf.position + Vector3.up * 1.0f) - waitPosition;
        vector.y         = vector2.y;
        _rigidbody.isKinematic = false;
        _collider.enabled      = true;
        _rigidbody.constraints = RigidbodyConstraints.None;
        _rigidbody.AddForce(vector.normalized * 7.5f, ForceMode.Impulse);
        _rigidbody.AddTorque(new Vector3(Random.Range(-180.0f, 180.0f), 0.0f) * 10.0f, ForceMode.Impulse);
        _isThrowed   = true;
        _isCollected = false;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        if(!_isCollected) return;
        _targetPosition = targetPosition;
    }

    void Update()
    {
        if(!_isCollected || _isThrowed || _isThrowWait || _isShipped) return;
        var vector = _targetPosition - transform.position;
        // vector.y   = 0.0f;
        if(vector.magnitude >= 1.0f) vector = vector.normalized;
        transform.position += vector * Time.deltaTime * 8.0f;
        if(vector.magnitude <= 0.01f) return;
        transform.forward = vector.normalized;
        var num = (int)(Time.time * 20.0f) % 2;
        // var angle = transform.localEulerAngles;
        // angle.z = num == 0 ? 30.0f : -30.0f;
        // transform.localEulerAngles = angle;
    }

    public void Hit(UnitBase enemyUnit)
    {
        _isThrowed = false;
        // _collider.enabled      = false;
        // _rigidbody.isKinematic = true;
        // transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
        // {
        //     Destroy(gameObject);
        // }).SetEase(Ease.OutQuad);
        _onHit?.Invoke(enemyUnit);
    }

    void OnCollisionEnter(Collision other)
    {
        if(!_isThrowed) return;
        if(!other.collider.CompareTag("Ground")) return;
        _isThrowed = false;
        _trailRenderer.enabled = false;
    }

    public void Release()
    {
        _isCollected = false;
        _isThrowed   = false;
        _isThrowWait = false;
        _collider.enabled = true;
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(Vector3.up * 3.0f, ForceMode.Impulse);
    }

    public void SetOnCollect(System.Action onCollect)
    {
        _onCollect = onCollect;
    }

    public void Shipping(ShippingArea shippingArea, float delay = 0.0f)
    {
        _isShipped             = true;
        _collider.enabled      = false;
        _rigidbody.isKinematic = true;
        _trailRenderer.enabled = false;
        var baseScale          = transform.localScale;
        var sequence = DOTweenUtil.NewSequence(gameObject);
        sequence.AppendInterval(delay);
        sequence.Append(transform.DOMove(shippingArea.transform.position, 0.3f).SetEase(Ease.OutQuad));
        sequence.Append(transform.DOScale(baseScale * 1.2f, 0.1f).SetEase(Ease.OutQuad));
        sequence.Append(transform.DOScale(0.0f, 0.2f).SetEase(Ease.InQuad));
        if(_shippedScore != 1 && GetComponent<UnitBase>() != null)
        {
            // リスポーン中に本体が削除されない対策
            sequence.AppendInterval(10.0f);
            sequence.AppendCallback(() => {Destroy(gameObject);});
            return;
        }
        sequence.AppendCallback(() =>
        {
            _isShipped = false;
            transform.position = RandomStageSpawner.Instance.GetSpawnPosition() + Vector3.up * 5.0f;
        });
        sequence.Append(transform.DOScale(baseScale * 1.2f, 0.2f).SetEase(Ease.OutQuad));
        sequence.Append(transform.DOScale(baseScale * 1.0f, 0.1f).SetEase(Ease.InQuad));
        sequence.AppendCallback(() =>
        {
            Release();
        });
    }

    public void SetShippedScore(int score)
    {
        _shippedScore = score;
    }
}
