using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private EnemyUnit _enemyUnit;
    #endregion

    #region Properties
    private float _chargeTime = 3.0f;
    private float _timer;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _chargeTime             = Random.Range(0.5f, 4.0f);
        var course              = new Vector3(0, Random.Range(0, 180), 0);
        transform.localRotation = Quaternion.Euler(course);
        Initialize();
    }

    public void Initialize()
    {
        _enemyUnit.Initilize(this);
    }

    void Update()
    {
        if(!_enemyUnit.IsMove) return;
        _timer += Time.deltaTime;
        if(_timer > _chargeTime)
        {
            ChangeAngle();
        }
        _enemyUnit.Move(_enemyUnit.transform.forward);
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        var isHit = Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, out hit, 1.0f);
        if(!isHit || !hit.collider.CompareTag("Ground")) return;
        ChangeAngle();
    }

    private void ChangeAngle()
    {
        var course              = new Vector3(0, Random.Range(0, 360), 0);
        transform.localRotation = Quaternion.Euler(course);
        _timer                  = 0;
        _chargeTime             = Random.Range(0.5f, 4.0f);
    }
}
