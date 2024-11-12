using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    #region Serialized Fields
    [SerializeField] private TargetIcon _targetIcon;
    [SerializeField] private RespawnUI _respawnUI;
    [SerializeField] private RankUI _rankUI;
    [SerializeField] private InputUI _inputUI;
    #endregion

    #region Properties
    public TargetIcon TargetIcon => _targetIcon;
    public RespawnUI RespawnUI => _respawnUI;
    public RankUI RankUI => _rankUI;
    public InputUI InputUI => _inputUI;
    #endregion
}
