using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputReader input;
    [SerializeField] private TilemapMovement tilemapMovement;
    [SerializeField] [Range(.1f, 1f)] private float playerMoveDelayInSeconds;

    private Vector2 _moveDirection;
    private float _moveDelay;

    void Awake()
    {
        ResetControls();
    }

    void OnEnable()
    {
        input.MovePerformedEvent += OnMoveEvent;
        input.MoveCancelledEvent += OnMoveEvent;
        input.ConfirmEvent += OnConfirmEvent;
        input.DeclineEvent += OnDeclineEvent;
    }

    void OnDisable()
    {
        input.MovePerformedEvent -= OnMoveEvent;
        input.MoveCancelledEvent -= OnMoveEvent;
        input.ConfirmEvent -= OnConfirmEvent;
        input.DeclineEvent -= OnDeclineEvent;
    }

    void ResetControls()
    {
        _moveDirection = new Vector2(0.0f, 0.0f);
        _moveDelay = 0.0f;
    }
    
    void OnMoveEvent(Vector2 axis)
    {
        // move to different cell depending on direction
        _moveDirection = axis;
    }

    void OnConfirmEvent()
    {
        // continue game
    }

    void OnDeclineEvent()
    {
        // idk, pause?
    }

    void FixedUpdate()
    {
        if (_moveDirection != Vector2.zero && _moveDelay <= 0.0f)
        {
            tilemapMovement.Move(_moveDirection);
            _moveDelay = playerMoveDelayInSeconds;
        }
        else if (_moveDelay > 0.0f)
        {
            _moveDelay -= Time.deltaTime;            
        }
    }
}
