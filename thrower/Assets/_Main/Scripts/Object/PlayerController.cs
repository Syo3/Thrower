using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private PlayerUnit _playerCharactor;
    #endregion

    #region private Fields
    private Vector3 _startTouchPosition;
    private bool _isDragging;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _playerCharactor.Initialize(this);
        _playerCharactor.Initialize();
        UIManager.Instance.InputUI.Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _startTouchPosition = Input.mousePosition;
            _isDragging         = true;
        }
        if(Input.GetMouseButton(0))
        {
            UIManager.Instance.InputUI.Show(_startTouchPosition);
            var direction = Input.mousePosition - _startTouchPosition;
            if(direction.magnitude < 1.0f) return;
            _playerCharactor.Move(new Vector3(direction.x, 0, direction.y));
        }
        if(Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            _playerCharactor.Stop();
            UIManager.Instance.InputUI.Hide();
        }
        if(!_playerCharactor.IsMove) return;
        _playerCharactor.UpdateUnit();
        _playerCharactor.CheckThrow();
    }

    public void SetPlayerCharactor(PlayerUnit playerCharactor)
    {
        _playerCharactor = playerCharactor;
    }
}
