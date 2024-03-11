using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bear : Entity
{
    [SerializeField]
    private Player PlayerEntity;

    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap waterTilemap;

    [SerializeField] private TilemapMovement tilemapMovement;

    private List<Direction> movesForConsideration;

    void Awake()
    {
        base.SetupOnAwake();
        movesForConsideration = new List<Direction>();
    }
    
    void FixedUpdate()
    {
        if (!tilemapMovement.CanMove) { return; }
        
        ConsiderMoves();

        if (movesForConsideration.Count == 0)
        {
            Debug.Log("No moves considered");
            return;
        }

        Direction chosenMove = movesForConsideration[Random.Range(0, movesForConsideration.Count)];
        tilemapMovement.Move(chosenMove);
    }

    void ConsiderMoves()
    {
        Debug.Log("Considering moves");
        movesForConsideration.Clear();
        
        Vector3 playerPosition = PlayerEntity.transform.position;
        Vector3 currentPosition = transform.position;
        Vector3Int tileCoordinates = Vector3Int.FloorToInt(currentPosition - _tileOffset);

        if (playerPosition.y > currentPosition.y)
        {
            if (tilemapMovement.ValidMove(Direction.Up, tileCoordinates + Vector3Int.up)) {
                movesForConsideration.Add(Direction.Up);
            }
        }
        else if (playerPosition.y < currentPosition.y)
        {
            if (tilemapMovement.ValidMove(Direction.Down, tileCoordinates + Vector3Int.down)) {
                movesForConsideration.Add(Direction.Down);
            }
        }
        
        if (playerPosition.x < currentPosition.x)
        {
            if (tilemapMovement.ValidMove(Direction.Left, tileCoordinates + Vector3Int.left)) {
                movesForConsideration.Add(Direction.Left);
            }
        }
        else if (playerPosition.x > currentPosition.x)
        {
            if (tilemapMovement.ValidMove(Direction.Right, tileCoordinates + Vector3Int.right)) {
                movesForConsideration.Add(Direction.Right);
            }
        }
    }

    public override void Die()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Player playerScript = other.GetComponent<Player>();
        if (playerScript != null)
        {
            playerScript.Die();
        }
    }
    
}
