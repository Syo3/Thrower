using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStageSpawner : SingletonMonoBehaviour<RandomStageSpawner>
{
    #region Serialized Fields
    [SerializeField] private GameObject[] _unitPrefabs;
    [SerializeField] private GameObject[] _throwObjectPrefabs;
    [SerializeField] private Transform[] _centerPoints;

    [SerializeField] private int _unitCount;
    [SerializeField] private int _throwObjectCount;
    #endregion

    #region private Fields
    #endregion

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        for(var i = 0; i < _unitCount; i++)
        {
            var unit = Instantiate(_unitPrefabs[Random.Range(0, _unitPrefabs.Length)]);
            unit.name = "EnemyUnit" + (i+1);
            unit.transform.position = GetSpawnPosition() + Vector3.up * 2.5f;
        }

        for(var i = 0; i < _throwObjectCount; i++)
        {
            var throwObject = Instantiate(_throwObjectPrefabs[Random.Range(0, _throwObjectPrefabs.Length)]);
            throwObject.transform.position = GetSpawnPosition() + Vector3.up * 3.0f;
        }
    }

    public Vector3 GetSpawnPosition()
    {
        return Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0.0f) * Vector3.forward * Random.Range(2.0f, 10.0f) + _centerPoints[Random.Range(0, _centerPoints.Length)].position;
    }

}
