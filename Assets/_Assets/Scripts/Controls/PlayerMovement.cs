using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputReader input;
    [SerializeField] private TilemapMovement tilemapMovement;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [Tooltip("Up = 0, Down = 1, Left = 2, Right = 3")]
    [SerializeField] private List<Sprite> directionalSprites; 
    
    private Vector2 _moveDirection;
    private Direction _facingDirection;

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
        if (_moveDirection != Vector2.zero && tilemapMovement.CanMove)
        {
            Direction directionEnum = DetermineMoveDirection(_moveDirection);
            spriteRenderer.sprite = directionalSprites[(int) directionEnum];
            tilemapMovement.Move(directionEnum);
        }
    }
    
    private Direction DetermineMoveDirection(Vector2 direction)
    {
        Direction moveDirection = Direction.Up;
        if (direction.x > 0) { moveDirection = Direction.Right; }
        else if (direction.x < 0) { moveDirection = Direction.Left; }
        else if (direction.y < 0) { moveDirection = Direction.Down; }

        return moveDirection;
    }
}
