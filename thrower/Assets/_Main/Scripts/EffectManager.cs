using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : SingletonMonoBehaviour<EffectManager>
{
    #region Serialized Fields
    [SerializeField] private List<EffectData> _effectDatas;
    #endregion

    public GameObject Pop(EffectType type)
    {
        var effectData = _effectDatas.Find(data => data.Type == type);
        if(effectData == null) return null;
        var effect = Instantiate(effectData.EffectPrefab, Vector3.zero, Quaternion.identity);
        return effect;
    }


    public enum EffectType
    {
        Hit,
        Smoke,
    }

    [System.Serializable]
    public class EffectData
    {
        public EffectType Type;
        public GameObject EffectPrefab;
    }
}
