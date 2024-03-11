using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class Player : Entity
{
    [SerializeField] private InputReader input;
    [SerializeField] private TilemapMovement tilemapMovement;
    
    private Vector2 _moveDirection;

    public static event Action<Direction> PlayerCharacterMove;

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
        if (_moveDirection == Vector2.zero || !tilemapMovement.CanMove) return;
        
        Direction directionEnum = DetermineMoveDirection(_moveDirection);
        spriteRenderer.sprite = directionalSprites[(int) directionEnum];
        tilemapMovement.Move(directionEnum);
        PlayerCharacterMove?.Invoke(directionEnum);
    }
    
    private Direction DetermineMoveDirection(Vector2 direction)
    {
        Direction moveDirection = Direction.Up;
        if (direction.x > 0) { moveDirection = Direction.Right; }
        else if (direction.x < 0) { moveDirection = Direction.Left; }
        else if (direction.y < 0) { moveDirection = Direction.Down; }

        return moveDirection;
    }
    
    public override void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Entity enemyScript = other.GetComponent<Entity>();
        if (enemyScript != null)
        {
            Die();
        }
    }
}
